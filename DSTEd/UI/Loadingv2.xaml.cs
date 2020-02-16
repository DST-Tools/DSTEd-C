using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace DSTEd.UI
{	
	//loading will be jammed, this is caused by operating UI while loading.[Akarinnnnn(Fa_Ge)]
	public partial class Loadingv2 : Window
	{
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
		public void Start(BackgroundWorker work, double workerCount)
		{
			progress.Maximum = workerCount;
			Show();
			
			{
				work.RunWorkerCompleted += (object _, RunWorkerCompletedEventArgs _1) => { 
					Close();
					Boot.Core.IDE.Show();
				};
				work.ProgressChanged += OnProgressChanged;
				work.RunWorkerAsync();
				/*progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
				{
					progress.Value++;
				}));*/
			};
			//Close();
			/*new Thread(delegate () {
					while (!work.IsCompleted)
						Thread.Sleep(100);
				}).Start();*/
		}

		private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progress.Value++;
		}
	}
}
