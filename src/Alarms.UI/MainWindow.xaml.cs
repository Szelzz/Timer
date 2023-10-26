using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Alarms.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        private Storyboard _showInputStoryboard;
        private Storyboard _hideInputStoryboard;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel(this);
            DataContext = _viewModel;
            MouseDown += Window_MouseDown;

            _showInputStoryboard = (Storyboard)FindResource("ShowInput");
            _hideInputStoryboard = (Storyboard)FindResource("HideInput");
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        public void BeginBackgroundBlink()
        {
            if (FindResource("BackgroundBlink") is Storyboard storyboard)
                storyboard.Begin();
        }
        private void MinutesTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _viewModel.Minutes++;
            }
            else
            {
                _viewModel.Minutes--;
            }
        }

        private void SecondsTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _viewModel.Seconds++;
            }
            else
            {
                _viewModel.Seconds--;
            }
        }

        private void NumberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var value = ((TextBox)e.Source).Text + e.Text;
            if (value.Length > 3 || !uint.TryParse(value, out _))
            {
                e.Handled = true;
            }
        }

        private void InputPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            _hideInputStoryboard.Stop();
            _showInputStoryboard.Begin();
        }

        private void InputPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            _showInputStoryboard.Stop();
            _hideInputStoryboard.Begin();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            _hideInputStoryboard.Begin();
        }
    }

}
