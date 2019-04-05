using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DSTEd.UI
{
	public delegate void LoadingWorker(uint progress);
	public partial class Loadingv2 : Window
	{
		public List<LoadingWorker> workers;
		public Loadingv2()
		{
			//workers += (uint a) => { };
			InitializeComponent();
		}
		public double Progress
		{
			get
			{
				return progress.Value;
			}
			set =>
			Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render,
					new Action(() =>
					{
						progress.Value = Progress;
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


		}
	}
}
