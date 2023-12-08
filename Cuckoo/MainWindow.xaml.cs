using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cuckoo
{
	public partial class MainWindow : Window
	{
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Interval { get; set; }
        public int Adjustment { get; set; }
		public System.Timers.Timer SystemTimer { get; set; } = new System.Timers.Timer();
		public System.Timers.Timer IntervalTimer { get; set; } = new System.Timers.Timer();

        public MainWindow()
		{
			InitializeComponent();

			this.ContentRendered += MainWindow_ContentRendered;

			StartTime = DateTime.Parse(StartTimeTxt.Text);
			EndTime = DateTime.Parse(EndTimeTxt.Text);
			Adjustment = int.Parse(AdjustmentTxt.Text);
			Interval = int.Parse(IntervalTxt.Text);
        }

		private async void MainWindow_ContentRendered(object? sender, EventArgs e)
		{
			var now = DateTime.Now;
			if (now > EndTime) return;

			var seconds = now.TimeOfDay.Seconds + now.TimeOfDay.Minutes * 60;
			int remainder = Convert.ToInt32(seconds % (Interval * 60));
			var timeToStart = (now > StartTime ? now.AddSeconds(Interval * 60 - remainder) : StartTime).AddSeconds(Adjustment);

			var timeUntilEvent = timeToStart - now;

			await Task.Delay(timeUntilEvent);
			IntervalTimer.Interval = Interval * 60 * 1000;
			IntervalTimer.Elapsed += IntervalTimer_Elapsed;

			IntervalTimer.Start();
		}

		private void IntervalTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
		{
			if (DateTime.Now > EndTime) IntervalTimer.Stop();

			var mediaPlayer = new MediaPlayer();
			mediaPlayer.Open(new Uri("D:\\music\\Ringtones\\1sec-Notification-03.mp3"));
			mediaPlayer.Play();
		}
	}
}