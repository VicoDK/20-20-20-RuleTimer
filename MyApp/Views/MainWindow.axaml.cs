using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System.Media;

namespace MyApp.Views;

public partial class MainWindow : Window
{
    public enum TimerState
    {
        work,
        workToBreak,
        breakTime,
        breakToWork
    }

    public TimerState timerState = TimerState.work;
    int remainingSeconds = 20 * 60; // 20 minutes in seconds
    DispatcherTimer timer;
    bool Aktiv = false;


    public MainWindow()
    {
        InitializeComponent();
        Position = new Avalonia.PixelPoint(0, 0);
        EyeBreak.IsVisible = false;
        
        //timer 
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, e) => Timer();
        timer.Start();

    }

    private void Timer()
    {

        if (timerState == TimerState.work)
        {
            timeUpdate();
            
            if (remainingSeconds < 0)
            {
                timerState = TimerState.workToBreak;
            }
        }
        else if (timerState == TimerState.workToBreak)
        {
            if (!Aktiv)            {
                PlaySound();
                EyeBreak.IsVisible = true;
                TimerTextBlock.IsVisible = false;
                Aktiv = true;
            }
        
        }
        else if (timerState == TimerState.breakTime)
        {
            timeUpdate();

            if (Aktiv)            {
      
                EyeBreak.IsVisible = false;
                TimerTextBlock.IsVisible = true;
                Aktiv = false;
            }


            if (remainingSeconds < 0)
            {
                timerState = TimerState.breakToWork;
            }
        }
        else if (timerState == TimerState.breakToWork)
        {
            System.Console.WriteLine("Break over, back to work!");
            remainingSeconds = 20 * 60; // Reset timer to 20 minutes
            PlaySound();
            UpdateTimerText(remainingSeconds);
            timerState = TimerState.work;
        }

        
    }

    public void PlaySound()
    {
        System.Console.Beep();
    }


    public void timeUpdate()
    {
            remainingSeconds -= 1;
            UpdateTimerText(remainingSeconds);
    }
    public void UpdateTimerText(int text)
    {
        int minutes = text / 60;
        int seconds = text % 60;
        TimerTextBlock.Text = $"20/20/20 Timer : {minutes:D2}:{seconds:D2}";

    }

    public void EyeBreakButton_Click(object sender, RoutedEventArgs e)
    {
            timerState = TimerState.breakTime;
            
            remainingSeconds = 25; 
        
    }





}