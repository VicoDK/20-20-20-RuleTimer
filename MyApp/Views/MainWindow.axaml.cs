using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyApp.Views;

public partial class MainWindow : Window
{
    List<ClassicTimerPreset> ClassicTimerPreset = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    public void StartButton_Click(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("0");





        //workTimers
        if (DeepWorkTimer.IsChecked == true)
        {
            var preset = new ClassicTimerPreset(
                90 * 60,
                20 * 60,
                true,
                true,
                "DeepWork"
            );

            ClassicTimerPreset.Add(preset);
        }


        if (PomodoroTimer.IsChecked == true)
        {
            var preset = new ClassicTimerPreset(
                25 * 60,
                5 * 60,
                true,
                true,
                "Pomodoro"
            );

            ClassicTimerPreset.Add(preset);
        }

        if (_52_17Timer.IsChecked == true)
        {
            var preset = new ClassicTimerPreset(
                57 * 60,
                17 * 60,
                true,
                true,
                "57-17"
            );

            ClassicTimerPreset.Add(preset);
        }

        //Extra Timers
        if (TwentyTwentyTwentyTimer.IsChecked == true)
        {
            var preset = new ClassicTimerPreset(
                20 * 60 ,
                25,
                true,
                false,
                "20-20-20"
            );

            ClassicTimerPreset.Add(preset);
        }

        if (_30_30rule.IsChecked == true)
        {
            var preset = new ClassicTimerPreset(
                30 * 60 ,
                30,
                true,
                false,
                "30-30"
            );

            ClassicTimerPreset.Add(preset);
        }

        TimePanel timePanel = new TimePanel(ClassicTimerPreset);

        timePanel.Show();
        this.Close();

    }
}

public class ClassicTimerPreset
{
    public int WorkTime;
    public int BreakTime;
    public bool ToBreakButton;
    public bool BackToWorkButton;
    public string Name;

    public ClassicTimerPreset(
        int workTime,
        int breakTime,
        bool toBreakButton,
        bool backToWorkButton,
        string name)
    {
        WorkTime = workTime;
        BreakTime = breakTime;
        ToBreakButton = toBreakButton;
        BackToWorkButton = backToWorkButton;
        Name = name;
    }
}