using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
namespace DSTEd.UI
{	
	public partial class Loadingv2 : Window
	{
		private int task;
		public int Progress = 0;

		public Loadingv2()
		{
			InitializeComponent();
		}
		private void OnMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}
		public void Start(params Action[] queue)
		{
			task = queue.Length;
			Show();
			Progress = 0;
			Parallel.ForEach(queue, (Action work) =>
			{
				work();
				Progress++;
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
				{
					progress.Value = (Progress == task) ? 100 : (100 / task) * Progress;
				}));
			});
			Close();
		}
	}
}
