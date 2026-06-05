using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;


namespace MyApp.Views;

public partial class MainWindow : Window
{
    string LayoutPoint = "TopLeft";
    List<ClassicTimerPreset> ClassicTimerPreset = new();

    public MainWindow()
    {
        InitializeComponent();

        PositionButton_Click(TopLeft, new RoutedEventArgs());
    }

    public void StartButton_Click(object sender, RoutedEventArgs e)
    {
  
        bool TimerSelected = false;
        //workTimers
        if (DeepWorkTimer.IsChecked == true)
        {
            TimerSelected = true;
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
            TimerSelected = true;
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
            TimerSelected = true;
            var preset = new ClassicTimerPreset(
                57 * 60,
                17 * 60,
                true,
                true,
                "57-17"
            );

            ClassicTimerPreset.Add(preset);
        }

        if (CustomeTimer.IsChecked == true)
        {
            TimerSelected = true;
            var preset = new ClassicTimerPreset(
                (int.TryParse(CustomeTimerWorkTime.Text, out var w) ? w : 1) * 60,
                (int.TryParse(CustomeTimerBreakTime.Text, out var b) ? b : 1) * 60,
                true,
                true,
                "Custome"
            );

            ClassicTimerPreset.Add(preset);


        }

        //Extra Timers
        if (TwentyTwentyTwentyTimer.IsChecked == true)
        {
            TimerSelected = true;
            var preset = new ClassicTimerPreset(
                20 * 60,
                25,
                true,
                false,
                "20-20-20"
            );

            ClassicTimerPreset.Add(preset);
        }

        if (_30_30rule.IsChecked == true)
        {
            TimerSelected = true;
            var preset = new ClassicTimerPreset(
                30 * 60 ,
                30,
                true,
                false,
                "30-30"
            );

            ClassicTimerPreset.Add(preset);
        }

        TimePanel timePanel = new TimePanel(ClassicTimerPreset, LayoutPoint);

        if (TimerSelected)
        {
            timePanel.Show();
            this.Close();
            
        }

    }

    public void CustomeTimer_CheckedChanged(object? sender, RoutedEventArgs e)
    {

        CustomeTimerGrid.IsVisible = CustomeTimer.IsChecked ?? false; 
        
    }

    private Button? _selectedButton;

    private void PositionButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button clickedButton)
            return;

        if (_selectedButton != null)
        {
            _selectedButton.ClearValue(Button.BackgroundProperty);
        }

        clickedButton.Background = Brushes.Gray;

        _selectedButton = clickedButton;
        LayoutPoint = _selectedButton.Name;
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