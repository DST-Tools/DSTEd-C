using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
namespace DSTEd.UI
{
	public delegate void LoadingWorker();
	
#pragma warning disable CS0660 
#pragma warning disable CS0661 //surpress these warnings
	struct refuint//it's waste......just for MultiThreading......
#pragma warning restore CS0661 
#pragma warning restore CS0660 
	{
		public uint v;

		public static bool operator== (refuint l,refuint r)
		{
			return l.v == r.v;
		}

		public static bool operator!= (refuint l,refuint r)
		{
			return l.v != r.v;
		}

		public static refuint operator+(refuint l, refuint r)
		{
			return new refuint { v = l.v + r.v };
		}
		public static refuint operator++(refuint t)
		{
			t.v++;
			return t;
		}
		public static refuint operator-(refuint l, refuint r)
		{
			return new refuint { v = l.v - r.v };
		}
	}
	public class WorkUnit
	{
		public LoadingWorker worker;
		public bool MT = false;
		public static bool ST = false;
		public WorkUnit(LoadingWorker w)
		{
			worker = w;
			this.MT = false;
		}
		public WorkUnit(LoadingWorker MTworker,ref uint mt_p)
		{
			worker = MTworker;
		}

		public void STFunc()
		{
			worker();
		}

		public void MTFunc(object refuint_p)
		{
			refuint p = (refuint)refuint_p;
			worker();
			p++;
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
			if (WorkUnits.Length == 0)
				Environment.Exit(10);
			Show();
			foreach (var U in WorkUnits)
			{
				switch (U.MT)
				{
				}
			}
			Close();
		}
	}
}
