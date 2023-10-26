using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Media;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Alarms.UI;
public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly DispatcherTimer _timer;
    private readonly MainWindow _window;
    private int _minutes = 1;
    private int _seconds = 0;
    public event PropertyChangedEventHandler? PropertyChanged;
    [DllImport("user32")] public static extern int FlashWindowEx(FLASHWINFO info);

    public MainWindowViewModel(MainWindow window)
    {
        _window = window;
        _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };
        _timer.Tick += TimerEvent;
        _timer.Start();

        StartClockCommand = new CommonCommand(StartClock, () => GetTimeSpan() != TimeSpan.Zero);
        StartStopwatchCommand = new CommonCommand(() => StopwatchStartTime = DateTime.Now);
        StopStopwatchCommand = new CommonCommand(() => StopwatchStartTime = null);
        CloseCommand = new CommonCommand(() => _window.Close());
    }

    public int Minutes
    {
        get => _minutes;
        set
        {
            if (value < 0)
                value = 0;
            if (value > 999)
                value = 999;
            SetProperty(ref _minutes, value);
            CallPropertyChanged(nameof(StartClockCommand));
        }
    }

    public int Seconds
    {
        get => _seconds;
        set
        {
            if (value < 0)
                value = 0;
            if (value > 60)
                value = 60;
            SetProperty(ref _seconds, value);
            CallPropertyChanged(nameof(StartClockCommand));
        }
    }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public ICommand StartClockCommand { get; }
    public ICommand StartStopwatchCommand { get; }
    public ICommand StopStopwatchCommand { get; }
    public ICommand CloseCommand { get; }

    public TimeSpan TimeLeft
    {
        get
        {
            var time = (EndTime - DateTime.Now) ?? new TimeSpan(1);
            return time < TimeSpan.Zero ? TimeSpan.Zero : time;
        }
    }

    public DateTime? StopwatchStartTime { get; set; }
    public TimeSpan StopwatchTimePassed
    {
        get
        {
            if (StopwatchStartTime == null)
                return TimeSpan.Zero;

            return DateTime.Now - StopwatchStartTime.Value;
        }
    }

    private TimeSpan GetTimeSpan()
        => TimeSpan.FromSeconds(Minutes * 60 + Seconds);

    private void TimerEvent(object? sender, EventArgs e)
    {
        if (TimeLeft <= TimeSpan.Zero)
            TimesUp();

        CallPropertyChanged(nameof(TimeLeft));
        CallPropertyChanged(nameof(StopwatchTimePassed));
    }

    private bool SetProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue))
            return false;

        field = newValue;

        CallPropertyChanged(propertyName);

        return true;
    }

    private void CallPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void StartClock()
    {
        StartTime = DateTime.Now;
        EndTime = StartTime.Value.Add(GetTimeSpan().Add(TimeSpan.FromSeconds(0.9)));
        TimerEvent(this, EventArgs.Empty);
    }

    private void TimesUp()
    {
        using (var soundPlayer = new SoundPlayer(@"./alarm.wav"))
        {
            soundPlayer.Play();
        }

        _window.Activate();
        _window.BeginBackgroundBlink();

        WindowInteropHelper wih = new WindowInteropHelper(_window);
        FlashWindowEx(new FLASHWINFO(wih.Handle, 5));

        StartTime = null;
        EndTime = null;
    }
}
