using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace DSTEd.UI
{
	public delegate void LoadingWorker();
	public class WorkUnit
	{
		public LoadingWorker worker;
		public bool MT = false;
		public bool f = false;
		public void ThreadFunc()
		{
			worker();
			f = true;
		}
	}
	public partial class Loadingv2 : Window
	{
		public WorkUnit[] WorkUnits;
		volatile private uint p = 0;
		public Loadingv2()
		{
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
			progress.Value = 0.0;
			Show();
			List<Thread> threads = new List<Thread>(5);
			foreach (var U in WorkUnits)
			{
				switch (U.MT)
				{
					case true:
						threads.Add(new Thread(U.ThreadFunc));
						break;
					case false:

						foreach (var t in threads)
						{
							t.IsBackground = true;
							t.Start(p);
						}
						while (p<threads.Count)
						{
							Progress = p / WorkUnits.Length;
							Thread.Sleep(300);
						}
						threads.Clear();

						U.worker();
						p++;
						break;
				}
			}
			foreach (var t in threads)
			{
				t.Start(p);
			}
			while (p < threads.Count)
			{
				Progress = p / WorkUnits.Length;
				Thread.Sleep(300);
			}
			threads.Clear();
			while (p<WorkUnits.Length)
			{
				Progress = p / WorkUnits.Length;
				Thread.Sleep(100);
			}
			Close();
		}
	}
}
