//dotnet publish -c Release -p:PublishSingleFile=true -p:SelfContained=true -p:PublishTrimmed=true -p:DebugSymbols=false -p:DebugType=none

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using System.Formats.Asn1;

namespace MyApp.Views;

public partial class TimePanel : Window
{
    DispatcherTimer timer;

    List<ClassicTimer> timers = new();   // FIXED (was null before)
    StackPanel timerPanel = new StackPanel();


    public TimePanel(List<ClassicTimerPreset> presets, string LayoutPoint)
    {
        InitializeComponent();

        this.Opened += (_, __) => SetPosition(LayoutPoint); 

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
        

        //ai made this
        // Create MenuFlyout
        var menuButton = new Button
        {
            Content = "Menu",
            Margin = new Thickness(10),
            HorizontalAlignment = HorizontalAlignment.Left
        };
        menuButton.Classes.Add("timer-button");

        // Create flyout
        var flyout = new MenuFlyout
        {
            Placement = PlacementMode.BottomEdgeAlignedLeft
        };

        // Pause item
        pauseItem = new MenuItem
        {
            Header = "Pause"
        };
        pauseItem.Classes.Add("timer-menu");
        pauseItem.Click += Pause_Click;

        


        // Back item
        var backItem = new MenuItem
        {
            Header = "Back"
        };
        backItem.Classes.Add("timer-menu");
        backItem.Click += Back_Click;

        // Add items to flyout
        flyout.Items.Add(pauseItem);
        flyout.Items.Add(backItem);

        // Attach flyout to button
        menuButton.Flyout = flyout;

        // Add button to your panel
        timerPanel.Children.Add(menuButton);
        //no longer Ai


    }
    private void SetPosition(string layoutPoint)
    {
        var screen = Screens.ScreenFromVisual(this) ?? Screens.Primary;
        var area = screen.WorkingArea;
        var w = (int)ClientSize.Width;
        var h = (int)ClientSize.Height;

        

        int x = 0;
        int y = 0;

        switch (layoutPoint)
        {
            case "TopLeft":
                x = area.X;
                y = area.Y;
                break;

            case "TopCenter":
                x = area.X + (area.Width - w) / 2;
                y = area.Y;
                break;

            case "TopRight":
                x = area.X + area.Width - w;
                y = area.Y;
                break;

            case "LeftCenter":
                x = area.X;
                y = area.Y + (area.Height - h) / 2;
                break;

            case "RightCenter":
                x = area.X + area.Width - w;
                y = area.Y + (area.Height - h) / 2;
                break;

            case "BottomLeft":
                x = area.X;
                y = area.Y + area.Height - h;
                break;

            case "BottomCenter":
                x = area.X + (area.Width - w) / 2;
                y = area.Y + area.Height - h;
                break;

            case "BottomRight":
                x = area.X + area.Width - w;
                y = area.Y + area.Height - h;
                break;
        }

        Position = new PixelPoint(x, y);
    }

    public void PlaySound()
    {
        Console.Beep();
    }

    public void Back_Click(object? sender, RoutedEventArgs e)
    {


        new MainWindow().Show();
        this.Close();
    }

    private MenuItem pauseItem;
    bool isPaused = true;

    public void Pause_Click(object? sender, RoutedEventArgs e)
    {
        isPaused = !isPaused;
        pauseItem.Header = isPaused ? "Paused" : "Resume";

        foreach (ClassicTimer timer in timers)
        {
            timer.Pause();
        }
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
    StackPanel panel = timerPanel;

    TimerTextBlock = new TextBlock
    {
        Text = $" {name} Timer : {workTimeSet}",
        
        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        Margin = new Thickness(4, 4, 10, 0)
       
    };
    WorkUpdateTimerText(workTimeSet);
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

    Button? ToBreak;
    Button? BackToWork;

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
                    if (ToBreak != null)
                    {
                        ToBreak.IsVisible = false;
                    }
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
        if (pauseBool)
        {
            Time -= 1;
            if (Time > -1)
            {
                WorkUpdateTimerText(Time);
            }
        }

    }
    bool pauseBool = true;
    public void Pause()
    {
        pauseBool = !pauseBool;
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
    public void BreakButton_Click(object? sender, RoutedEventArgs e)
    {

        BreakButton_Click();
    }

    public void AutopBreakButton_Click()
    {
        PlaySound();

        
    }

    public void BreakButton_Click()
    {
        WorkTimerState = ClassicTimerState.BreakTime;
        Time = BreakTimeSet;
    }



    public void BackToWork_Click(object? sender, RoutedEventArgs e)
    {
        BackToWork_Click();
    }

    public void BackToWork_Click()
    {   
        WorkTimerState = ClassicTimerState.Work;
        TimerTextBlock.IsVisible = true;
        Time = WorkTimeSet;
        WorkUpdateTimerText(Time);
        if (BackToWork != null)
        {
            BackToWork.IsVisible = false;
            
        }

    }

    public void PlaySound()
    {
        System.Console.Beep();
    }
}