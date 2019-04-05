using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace DSTEd.UI
{
	public delegate void LoadingWorker(uint progress_ref);
	public class STWorkUnits
	{
		public LoadingWorker workers;
		public uint count;
		public static STWorkUnits operator+(STWorkUnits target, LoadingWorker worker)
		{
			target.workers += worker;
			target.count++;
			return target;
		}
	}
	public partial class Loadingv2 : Window
	{
		public List<STWorkUnits> WorkUnits;
		public Loadingv2()
		{
			progress.Value = 0.0;
			InitializeComponent();
		}
		public double Progress//0 to 1
		{
			get
			{
				return progress.Value;
			}
			set =>
			Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render,
					new Action(() =>
					{
						progress.Value = value * 100;
					})
			);
		}
		private void OnMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}
		public void Start()
		{
			Show();
			uint p = 0;
			foreach (var U in WorkUnits)
			{
				uint count = U.count;
				uint u_p = 0;
				U.workers.BeginInvoke(u_p, null, this);
				while (u_p<count)
				{
					Thread.Sleep(100);
				}
			}
			while (p<WorkUnits.Count)
			{
				Progress = p / WorkUnits.Count;
				Thread.Sleep(100);
			}
			Close();
		}
	}
}
