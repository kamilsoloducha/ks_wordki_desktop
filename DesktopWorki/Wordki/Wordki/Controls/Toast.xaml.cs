using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wordki.Controls {
  /// <summary>
  /// Interaction logic for Toast.xaml
  /// </summary>
  public partial class Toast : UserControl {

    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
      "ImageSource", typeof(ImageSource), typeof(Toast), new FrameworkPropertyMetadata(null, ((d, e) => {
        Toast lControl = d as Toast;
        if (lControl == null) return;
        ImageSource lImageSource = e.NewValue as ImageSource;
        lControl.Image.Source = lImageSource;
      })));

    public ImageSource ImageSource {
      get { return GetValue(ImageSourceProperty) as ImageSource; }
      set { SetValue(ImageSourceProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string),
      typeof(Toast), new FrameworkPropertyMetadata(null, ((d, e) => {
        Toast lControl = d as Toast;
        if (lControl == null) return;
        string lText = e.NewValue as string;
        lControl.TextBlock.Text = lText;
      })));

    public string Message {
      get { return GetValue(MessageProperty) as string; }
      set { SetValue(MessageProperty, value); }
    }

    private Thread _leaveThread;
    private const int LeaveThreadDelay = 2000;
    private readonly Storyboard _leaveStoryboard;

    public Toast() {
      InitializeComponent();
      _leaveStoryboard = FindResource("DisappearAnimation") as Storyboard;
      if (_leaveStoryboard != null) {
        Storyboard.SetTarget(_leaveStoryboard, this);
        _leaveStoryboard.Completed += delegate {
          Visibility = Visibility.Collapsed;
        };
      }
    }

    private Thread CreateLeaveThread() {
      Thread lThread = new Thread(() => {
        Thread.Sleep(LeaveThreadDelay);
        if (Dispatcher.Thread.ThreadState == ThreadState.Running)
          Dispatcher.Invoke(() => _leaveStoryboard.Begin());
      });
      return lThread;
    }

    private void Toast_OnLoaded(object sender, RoutedEventArgs e) {
      _leaveThread = CreateLeaveThread();
      _leaveThread.Start();
    }

    private void OnMouseEnter(object sender, MouseEventArgs e) {
      if (_leaveThread.IsAlive) {
        _leaveThread.Abort();
      }
      _leaveStoryboard.Stop();
      Opacity = 1;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e) {
      if (!_leaveThread.IsAlive) {
        _leaveThread = CreateLeaveThread();
        _leaveThread.Start();
      }
    }

    private void OnMouseDown(object sender, MouseEventArgs e) {
      Visibility = Visibility.Collapsed;
    }

    private void MakeDisappear() {
      Dispatcher.Invoke(() => {

      });
    }
  }

  public enum ToastLevel {
    Info,
    Alert,
    Error,
  }

  public class ToastProperties {
    public ImageSource ImageSource { get; set; }
    public string Message { get; set; }
  }
}
