using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Drawing2D;
using System.Windows.Interop;
//using System.Threading;
using System.Timers;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Markup;
using System.Collections;
using System.Windows.Automation.Peers;
using System.ComponentModel;
using System.Threading;

namespace MEDIA_PLAYER
{
    public partial class MainWindow : Window
    {
        private OpenFileDialog _Dlg;
        private volatile string _FileName;
        private bool? _ResultDlg;
        private bool _CanLoop = false;
        private volatile bool _CanFlip = false;

        private IntPtr handle;

        public int countClick
        {
            set;
            get;
        }

        //public System.Threading.ThreadStart threadStart { set; get; }
        public System.Windows.Threading.Dispatcher DispacherTime { set; get; }

        public System.Timers.Timer timerCounter { set; get; } = new System.Timers.Timer(1000);

        public MainWindow()
        {
            InitializeComponent();

            mediaPlayerView.Margin = new Thickness(0D, 22D, 0D, 60D);
            mediaPlayerView.Initialized += MediaPlayerView_Initialized;

            // Dispacher timer
            DispacherTime = durationBar.Dispatcher;
        }

        private void MediaPlayerView_Initialized(object sender, EventArgs e)
        {
            return;
        }

        private void mi_Open_Click(object sender, RoutedEventArgs e)
        {
            _Dlg = new OpenFileDialog();
            _ResultDlg = _Dlg.ShowDialog();

            switch (_ResultDlg)
            {
                case false:
                    return;
                    break;

                case true:
                    foreach (var _change in _Dlg.FileNames)
                    {
                        _CanLoop = true;
                        _FileName = _change;
                        mediaPlayerView.Source = new Uri(_FileName);

                        playButton_Click(sender, e);
                    }
                    break;
            }
        }

        public uint delay_ms(long ms)
        {
            for (uint j = 0; j < 65000; j++)
                for (uint l = 0; l < ms; l++)
                    ;

            return (uint)ms;
        }

        private void MediaPlayerView_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_CanLoop == true)
            {
                if (loopButton.IsChecked == true)
                    foreach (var _change in _Dlg.FileNames)
                    {
                        _FileName = _change;
                        mediaPlayerView.Source = new Uri(_FileName);
                    }
            }
            else
                _CanLoop = false;

            timerCounter.Stop();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerView.Play();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerView.Pause();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerView.Stop();
        }

        private void muteButton_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayerView.IsMuted == false)
            {
                mediaPlayerView.IsMuted = true;
            }
            else
                mediaPlayerView.IsMuted = false;
        }

        private Thread newThread()
        {
            timerCounter.Interval = 1000;
            durationBar.Value += mediaPlayerView.Position.Seconds / timerCounter.Interval;
            timerCounter.Start();

            return null;
        }

        private void durationBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            durationBar.Maximum = mediaPlayerView.NaturalDuration.TimeSpan.TotalSeconds;
            mediaPlayerView.Position = TimeSpan.FromSeconds(durationBar.Value);

        }

        private void TimerCounter_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
            timerCounter = sender as Timer;

            timerCounter.Enabled = true;
            timerCounter.Interval = 1000;
            timerCounter.Start(); */
        }

        private void volumeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayerView.Volume = volumeBar.Value / 100;
        }

        private void fullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }

            if (this.WindowState == WindowState.Maximized)
            {
                if (this.WindowStyle == WindowStyle.SingleBorderWindow)
                {
                    this.WindowStyle = WindowStyle.None;
                }

                if (m_MainMenu.IsVisible == true)
                {
                    m_MainMenu.Visibility = Visibility.Hidden;
                }

                if (playButton.IsVisible == true)
                {
                    playButton.Visibility = Visibility.Hidden;
                }

                if (pauseButton.IsVisible == true)
                {
                    pauseButton.Visibility = Visibility.Hidden;
                }

                if (stopButton.IsVisible == true)
                {
                    stopButton.Visibility = Visibility.Hidden;
                }

                if (loopButton.IsVisible == true)
                {
                    loopButton.Visibility = Visibility.Hidden;
                }

                if (fullscreenButton.IsVisible == true)
                {
                    fullscreenButton.Visibility = Visibility.Hidden;
                }

                if (muteButton.IsVisible == true)
                {
                    muteButton.Visibility = Visibility.Hidden;
                }

                if (durationBar.IsVisible == true)
                {
                    durationBar.Visibility = Visibility.Hidden;
                }

                if (volumeBar.IsVisible == true)
                {
                    volumeBar.Visibility = Visibility.Hidden;
                }

                Taskbar.WindowFullscreenTaskbar.IsMaximize = true;

                mediaPlayerView.Stretch = Stretch.Fill;
                mediaPlayerView.Margin = new Thickness(0D, 0D, 0D, 0D);

                if (Taskbar.WindowFullscreenTaskbar.IsMaximize == true)
                {
                    handle = new WindowInteropHelper(this).Handle;
                    Taskbar.SetWinFullScreen(handle);
                }
                else
                    return;

                this.Topmost = true;
            }
            else
            {
                return;
            }
        }

        private void OnKeyDown_Form(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (this.WindowStyle == WindowStyle.None)
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                }

                if (this.WindowState == WindowState.Maximized)
                {
                    if (m_MainMenu.IsVisible == false)
                    {
                        m_MainMenu.Visibility = Visibility.Visible;
                    }

                    if (playButton.IsVisible == false)
                    {
                        playButton.Visibility = Visibility.Visible;
                    }

                    if (pauseButton.IsVisible == false)
                    {
                        pauseButton.Visibility = Visibility.Visible;
                    }

                    if (stopButton.IsVisible == false)
                    {
                        stopButton.Visibility = Visibility.Visible;
                    }

                    if (loopButton.IsVisible == false)
                    {
                        loopButton.Visibility = Visibility.Visible;
                    }

                    if (fullscreenButton.IsVisible == false)
                    {
                        fullscreenButton.Visibility = Visibility.Visible;
                    }

                    if (muteButton.IsVisible == false)
                    {
                        muteButton.Visibility = Visibility.Visible;
                    }

                    if (durationBar.IsVisible == false)
                    {
                        durationBar.Visibility = Visibility.Visible;
                    }

                    if (volumeBar.IsVisible == false)
                    {
                        volumeBar.Visibility = Visibility.Visible;
                    }

                    Taskbar.WindowFullscreenTaskbar.IsMaximize = false;

                    mediaPlayerView.Stretch = Stretch.Uniform;
                    mediaPlayerView.Margin = new Thickness(0D, 22D, 0D, 60D);

                    this.WindowState = WindowState.Normal;
                    this.Width = 800;
                    this.Height = 450;
                    this.Topmost = true;
                }
            }
            else
            {
                if (e.Key == Key.Space && countClick == 0)
                {
                    mediaPlayerView.Pause();

                    countClick = 1;
                }
                else
                    if (e.Key == Key.Space && countClick == 1)
                {
                    mediaPlayerView.Play();

                    countClick = 0;
                }
                return;
            }
        }

        private void loopButton_Click(object sender, RoutedEventArgs e)
        {
            loopButton.Checked += LoopButton_Checked;

            if (_CanLoop == false)
                return;
            else
            {
                if (loopButton.IsChecked == true)
                    mediaPlayerView.MediaEnded += MediaPlayerView_MediaEnded;
                else
                    if (loopButton.IsChecked == false)
                    mediaPlayerView.MediaOpened += MediaPlayerView_MediaOpened;
                return;
            }
        }

        private void LoopButton_Checked(object sender, RoutedEventArgs e)
        {
            if (loopButton.IsChecked == true)
            {
                MediaPlayerView_MediaEnded(sender, e);
            }
            else
                MediaPlayerView_MediaOpened(sender, e);
        }

        private void MediaPlayerView_MediaOpened(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void mi_FlipView_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayerView.HasVideo)
                if (mi_FlipView.IsChecked == true)
                {
                    _CanFlip = true;
                    delay_ms(1000);
                    mediaPlayerView.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    if (mi_FlipView.IsChecked == false)
                    {
                        _CanFlip = false;
                        delay_ms(1000);
                        mediaPlayerView.FlowDirection = FlowDirection.LeftToRight;
                    }
                }
            else
                return;

            mi_FlipView.Checked += Mi_FlipView_Checked;
        }

        private void Mi_FlipView_Checked(object sender, RoutedEventArgs e)
        {
            if (mi_FlipView.IsChecked == true)
                mi_FlipView_Click(sender, e);
            else
                if (_CanFlip == false)
                return;
        }

        private void mi_Playlist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_FlipViewTop_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayerView.HasVideo)
            {
                if (mi_FlipViewTop.IsChecked == true)
                {
                    _CanFlip = true;
                    delay_ms(1000);

                    try
                    {
                        mediaPlayerView.FlowDirection = (System.Windows.FlowDirection)flow_diration.TopDown;
                    }
                    catch (System.ArgumentException evc)
                    {
                        MessageBox.Show(evc.Message);
                    }
                }
                else if (mi_FlipViewTop.IsChecked == false)
                {
                    _CanFlip = false;
                    delay_ms(1000);
                    mediaPlayerView.FlowDirection = (System.Windows.FlowDirection)flow_diration.LeftToRight;
                }
            }
        }

        private void mi_FlipViewBottom_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_FlipViewRight_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_FlipViewLeft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mediaPlayerView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int counter = 0;

            if (mediaPlayerView.HasVideo)
            {
                if (e.ClickCount == 2 && counter == 0 && (Taskbar.WindowFullscreenTaskbar.IsMaximize == false))
                {
                    counter = 1;

                    if (this.WindowState == WindowState.Normal)
                    {
                        this.WindowState = WindowState.Maximized;
                    }

                    if (this.WindowState == WindowState.Maximized)
                    {
                        if (this.WindowStyle == WindowStyle.SingleBorderWindow)
                        {
                            this.WindowStyle = WindowStyle.None;
                        }

                        if (m_MainMenu.IsVisible == true)
                        {
                            m_MainMenu.Visibility = Visibility.Hidden;
                        }

                        if (playButton.IsVisible == true)
                        {
                            playButton.Visibility = Visibility.Hidden;
                        }

                        if (pauseButton.IsVisible == true)
                        {
                            pauseButton.Visibility = Visibility.Hidden;
                        }

                        if (stopButton.IsVisible == true)
                        {
                            stopButton.Visibility = Visibility.Hidden;
                        }

                        if (loopButton.IsVisible == true)
                        {
                            loopButton.Visibility = Visibility.Hidden;
                        }

                        if (fullscreenButton.IsVisible == true)
                        {
                            fullscreenButton.Visibility = Visibility.Hidden;
                        }

                        if (muteButton.IsVisible == true)
                        {
                            muteButton.Visibility = Visibility.Hidden;
                        }

                        if (durationBar.IsVisible == true)
                        {
                            durationBar.Visibility = Visibility.Hidden;
                        }

                        if (volumeBar.IsVisible == true)
                        {
                            volumeBar.Visibility = Visibility.Hidden;
                        }

                        Taskbar.WindowFullscreenTaskbar.IsMaximize = true;

                        mediaPlayerView.Stretch = Stretch.Fill;
                        mediaPlayerView.Margin = new Thickness(0D, 0D, 0D, 0D);

                        if (Taskbar.WindowFullscreenTaskbar.IsMaximize == true)
                        {
                            handle = new WindowInteropHelper(this).Handle;
                            Taskbar.SetWinFullScreen(handle);
                        }
                        else
                            return;

                        this.Topmost = true;
                    }
                }
                else if (e.ClickCount == 2)
                {
                    {
                        Taskbar.WindowFullscreenTaskbar.IsMaximize = false;

                        if (Taskbar.WindowFullscreenTaskbar.IsMaximize == false)
                        {
                            handle = new WindowInteropHelper(this).Handle;
                            Taskbar.SetWinFullScreen(handle);
                        }
                    }

                    if (this.WindowStyle == WindowStyle.None)
                    {
                        this.WindowStyle = WindowStyle.SingleBorderWindow;
                    }

                    if (this.WindowState == WindowState.Maximized)
                    {
                        if (m_MainMenu.IsVisible == false)
                        {
                            m_MainMenu.Visibility = Visibility.Visible;
                        }

                        if (playButton.IsVisible == false)
                        {
                            playButton.Visibility = Visibility.Visible;
                        }

                        if (pauseButton.IsVisible == false)
                        {
                            pauseButton.Visibility = Visibility.Visible;
                        }

                        if (stopButton.IsVisible == false)
                        {
                            stopButton.Visibility = Visibility.Visible;
                        }

                        if (loopButton.IsVisible == false)
                        {
                            loopButton.Visibility = Visibility.Visible;
                        }

                        if (fullscreenButton.IsVisible == false)
                        {
                            fullscreenButton.Visibility = Visibility.Visible;
                        }

                        if (muteButton.IsVisible == false)
                        {
                            muteButton.Visibility = Visibility.Visible;
                        }

                        if (durationBar.IsVisible == false)
                        {
                            durationBar.Visibility = Visibility.Visible;
                        }

                        if (volumeBar.IsVisible == false)
                        {
                            volumeBar.Visibility = Visibility.Visible;
                        }

                        Taskbar.WindowFullscreenTaskbar.IsMaximize = false;

                        mediaPlayerView.Stretch = Stretch.Uniform;
                        mediaPlayerView.Margin = new Thickness(0D, 22D, 0D, 60D);

                        if (Taskbar.WindowFullscreenTaskbar.IsMaximize == false)
                        {
                            handle = new WindowInteropHelper(this).Handle;
                            Taskbar.SetWinFullScreen(handle);
                        }

                        this.WindowState = WindowState.Normal;
                        this.Width = 800;
                        this.Height = 450;
                        this.Topmost = true;
                    }

                    counter = 0;
                }
            }
        }
    }
}