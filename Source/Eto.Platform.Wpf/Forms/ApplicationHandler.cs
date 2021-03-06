using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using System.Diagnostics;
using sw = System.Windows;
using swm = System.Windows.Media;
using System.Threading;

namespace Eto.Platform.Wpf.Forms
{
	public class ApplicationHandler : WidgetHandler<System.Windows.Application, Application>, IApplication
	{
		string badgeLabel;
		static ApplicationHandler instance;
		List<sw.Window> delayShownWindows;

		public static ApplicationHandler Instance
		{
			get { return instance; }
		}

		public static void InvokeIfNecessary (Action action)
		{
			if (Thread.CurrentThread == sw.Application.Current.Dispatcher.Thread)
				action ();
			else {
				Instance.Invoke(action);
			}
		}

		public List<sw.Window> DelayShownWindows
		{
			get
			{
				if (delayShownWindows == null)
					delayShownWindows = new List<sw.Window> ();
				return delayShownWindows;
			}
		}

		public bool IsStarted { get; private set; }

		public override sw.Application CreateControl ()
		{
			return new sw.Application ();
		}

		protected override void Initialize ()
		{
			base.Initialize ();
			instance = this;
			Control.Startup += HandleStartup;
		}

		void HandleStartup (object sender, sw.StartupEventArgs e)
		{
			IsActive = true;
			IsStarted = true;
			Control.Activated += (sender2, e2) => {
				IsActive = true;
			};
			Control.Deactivated += (sender2, e2) => {
				IsActive = false;
			};
			if (delayShownWindows != null)
			{
				foreach (var window in delayShownWindows)
				{
					window.Show ();
				}
				delayShownWindows = null;
			}
		}

		public bool IsActive { get; private set; }

		public string BadgeLabel
		{
			get { return badgeLabel; }
			set
			{
				badgeLabel = value;
				var mainWindow = sw.Application.Current.MainWindow;
				if (mainWindow != null)
				{
					if (mainWindow.TaskbarItemInfo == null)
						mainWindow.TaskbarItemInfo = new sw.Shell.TaskbarItemInfo ();
					if (!string.IsNullOrEmpty (badgeLabel))
					{
						var ctl = new CustomControls.OverlayIcon ();
						ctl.Content = badgeLabel;
						ctl.Measure (new sw.Size (16, 16));
						var size = ctl.DesiredSize;

						var m = sw.PresentationSource.FromVisual (mainWindow).CompositionTarget.TransformToDevice;

						var bmp = new swm.Imaging.RenderTargetBitmap ((int)size.Width, (int)size.Height, m.M22 * 96, m.M22 * 96, swm.PixelFormats.Default);
						ctl.Arrange (new sw.Rect (size));
						bmp.Render (ctl);
						mainWindow.TaskbarItemInfo.Overlay = bmp;
					}
					else
						mainWindow.TaskbarItemInfo.Overlay = null;
				}
			}
		}


		public void RunIteration()
		{
		}
		private bool shutdown;

		public void Quit()
		{
			bool cancel = false;
			foreach (sw.Window window in Control.Windows) {
				window.Close ();
				cancel |= window.IsVisible;
			}
			if (!cancel)
			{
				Control.Shutdown();
				shutdown = true;
			}
		}

		public void Invoke (Action action)
		{
			Control.Dispatcher.Invoke (action);
		}

		public void AsyncInvoke (Action action)
		{
			Control.Dispatcher.BeginInvoke (action);
		}

		public void GetSystemActions (GenerateActionArgs args, bool addStandardItems)
		{
		}

		public Key CommonModifier
		{
			get { return Key.Control; }
		}

		public Key AlternateModifier
		{
			get { return Key.Alt; }
		}

		public void Open(string url)
		{
			Process.Start(url);	
		}

		public void Run (string[] args)
		{
			Widget.OnInitialized (EventArgs.Empty);
			if (shutdown) return;
			if (Widget.MainForm != null)
				Control.Run ((System.Windows.Window)Widget.MainForm.ControlObject);
			else
				Control.Run ();
		}

		public void Restart ()
		{
			System.Diagnostics.Process.Start (System.Windows.Application.ResourceAssembly.Location);
			System.Windows.Application.Current.Shutdown ();
		}

		public override void AttachEvent (string handler)
		{
			switch (handler) {
				case Application.TerminatingEvent:
					// handled by WpfWindow
					break;
				default:
					base.AttachEvent (handler);
					break;
			}
		}
	}
}
