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

        try
        {
            if (TwentyTwentyTwentyTimer.IsChecked == true)
            {
                var preset = new ClassicTimerPreset(
                    5 ,
                    5,
                    true,
                    false,
                    "20-20-20"
                );

                ClassicTimerPreset.Add(preset);
            }

            System.Diagnostics.Debug.WriteLine("2");

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

            System.Diagnostics.Debug.WriteLine("3");

            TimePanel timePanel = new TimePanel(ClassicTimerPreset);

            timePanel.Show();
            this.Close();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
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