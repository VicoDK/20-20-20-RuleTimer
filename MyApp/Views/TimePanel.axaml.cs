//to do 
// plays sounds when needed 
// fixe timings
//check deep work





using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Interactivity;

namespace MyApp.Views;

public partial class TimePanel : Window
{
    DispatcherTimer timer;

    List<ClassicTimer> timers = new();   // FIXED (was null before)
    StackPanel timerPanel = new StackPanel();

    public TimePanel(List<ClassicTimerPreset> presets)
    {
        InitializeComponent();

        Position = new PixelPoint(0, 0);

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);

        foreach (var preset in presets)
        {
            timers.Add(new ClassicTimer(
                preset.WorkTime,
                preset.BreakTime,
                preset.ToBreakButton,
                preset.BackToWorkButton,
                preset.Name,
                timerPanel
            ));
        }

        foreach (var t in timers)
        {
            timer.Tick += (s, e) => t.ClassicWorkUpdateSection();
        }

        Content = timerPanel; // FIXED (removed self-add bug)

        timer.Start();
    }

    public void PlaySound()
    {
        Console.Beep();
    }
}
public class ClassicTimer
{

    public string Name;
    public ClassicTimer(int workTimeSet, int breakTimeSet, bool needToBreakButton, bool needBackToWorkButton, string name, StackPanel timerPanel)
{
    WorkTimeSet = workTimeSet;
    Time = WorkTimeSet;
    BreakTimeSet = breakTimeSet;
    this.Name = name;
    var panel = timerPanel;

    TimerTextBlock = new TextBlock
    {
        Text = $" {name} Timer : ",
        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        Margin = new Thickness(10, 2, 10, 2)
    };

    panel.Children.Add(TimerTextBlock);

    if (needToBreakButton)
    {
        ToBreak = new Button
        {
            Content = $" {name} Break",
            Margin = new Thickness(10),
            IsVisible = false
        };

        ToBreak.Click += BreakButton_Click;
        panel.Children.Add(ToBreak);
    }

    if (needBackToWorkButton)
    {
        BackToWork = new Button
        {
            Content = "Back to Work",
            Margin = new Thickness(10),
            IsVisible = false
        };

        BackToWork.Click += BackToWork_Click;
        panel.Children.Add(BackToWork);
    }
}







    

    int WorkTimeSet; 
    int BreakTimeSet; 
    TextBlock TimerTextBlock;

    Button ToBreak;
    Button BackToWork;

    int Time;

    public enum ClassicTimerState //Enum for Time states to DeepWork timer
    {
        Work,
        DeepWorkToBreak,
        BreakTime,
        BreakToDeepWork
    }

    ClassicTimerState WorkTimerState = ClassicTimerState.Work; // DeepWork timer state variable
    bool ClassicAktiv = false; // Variable to track if the deep work break is active
    

    public void ClassicWorkUpdateSection()
    {
        switch (WorkTimerState)
        {
            case ClassicTimerState.Work:
                WorktimeUpdate();
                if (Time < 0 || Time == 0)
                {
                    WorkTimerState = ClassicTimerState.DeepWorkToBreak;
                }
                break;
            case ClassicTimerState.DeepWorkToBreak:
                if (!ClassicAktiv)
                {
                    PlaySound();
                    if (ToBreak != null)
                    {
                        ToBreak.IsVisible = true;
                        
                    }
                    else if (ToBreak == null)
                    {
                        BreakButton_Click();
                    }
                    
                    TimerTextBlock.IsVisible = false;
                    ClassicAktiv = true;
                }
                break;
            case ClassicTimerState.BreakTime:
                WorktimeUpdate();
                if (ClassicAktiv)
                {
                    ToBreak.IsVisible = false;
                    TimerTextBlock.IsVisible = true;
                    ClassicAktiv = false;
                }
                if (Time < 0 || Time == 0)
                {
                    WorkTimerState = ClassicTimerState.BreakToDeepWork;
                }
                break;
            case ClassicTimerState.BreakToDeepWork:
                if (BackToWork != null)
                {
                    BackToWork.IsVisible = true;

                    
                }
                TimerTextBlock.IsVisible = false;

                if (BackToWork == null)
                {
                    BackToWork_Click();
                    
                }
                PlaySound();
                break;
        }
        
    }
    
    
    /// <summary>
    /// function that updates the DeepWork timer by decreasing the remaining seconds and updating the timer text on the UI
    /// </summary>
    public void WorktimeUpdate() 
    {
            Time -= 1;
            if (Time > -1)
            {
                WorkUpdateTimerText(Time);
            }

    }

    /// <summary>
    /// updates the DeepWork timer text on the UI with the remaining time in hours, minutes and seconds format
    /// </summary>
    public void WorkUpdateTimerText(int text)
    {
        int hours = text / 3600;
        int minutes = (text % 3600) / 60;
        int seconds = text % 60;
        if (WorkTimerState == ClassicTimerState.Work)
        {
            if (hours > 0)
            {
                TimerTextBlock.Text = $" {Name} Timer: {hours:D2}:{minutes:D2}";
            }
            else
            {
                TimerTextBlock.Text = $" {Name} Timer: {minutes:D2}:{seconds:D2}";
            }
        }
        else if (WorkTimerState == ClassicTimerState.BreakTime)
        {
            if (hours > 0)
            {
                TimerTextBlock.Text = $" {Name} Break Timer : {hours:D2}:{minutes:D2}";
            }
            else
            {
                TimerTextBlock.Text = $" {Name} Break Timer : {minutes:D2}:{seconds:D2}";
            }
        }
        
    }

    /// <summary>
    /// function used to manually start the eye break by clicking the eye break button
    /// </summary>
    public void BreakButton_Click(object sender, RoutedEventArgs e)
    {
        PlaySound();
        BreakButton_Click();
    }

    public void BreakButton_Click()
    {
        WorkTimerState = ClassicTimerState.BreakTime;
        Time = BreakTimeSet;
        
    }

    public void BackToWork_Click(object sender, RoutedEventArgs e)
    {
        BackToWork_Click();
    }

    public void BackToWork_Click()
    {   
        if (BackToWork != null)
        {
            BackToWork.IsVisible = false;
            
        }
        TimerTextBlock.IsVisible = true;

        Time = WorkTimeSet;
        WorkUpdateTimerText(Time);
        WorkTimerState = ClassicTimerState.Work;
    }

    public void PlaySound()
    {
        System.Console.Beep();
    }
}