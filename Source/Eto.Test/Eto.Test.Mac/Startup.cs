using System;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Eto.Forms;
using Eto.Platform.Mac.Forms.Controls;
using Eto.Platform.Mac.Forms;
using Eto.Platform.Mac;
using Eto.Drawing;
using System.Diagnostics;
using MonoMac.CoreGraphics;

namespace Eto.Test.Mac
{
	class Startup
	{
		static void Main (string[] args)
		{
#if DEBUG
			Debug.Listeners.Add (new ConsoleTraceListener ());
#endif
			AddStyles ();
			
			var generator = new Eto.Platform.Mac.Generator ();
			
			var app = new TestApplication (generator);

			// use this to use your own app delegate:
			// ApplicationHandler.Instance.AppDelegate = new MyAppDelegate();

			app.Run (args);
		}

		static void AddStyles ()
		{
			// support full screen mode!
			Style.Add<FormHandler> ("main", handler => {
				handler.Control.CollectionBehavior |= NSWindowCollectionBehavior.FullScreenPrimary;
			});

			Style.Add<ApplicationHandler> ("application", handler => {
				handler.EnableFullScreen ();
			});

			// other styles
			Style.Add<TreeGridViewHandler> ("sectionList", handler => {
				handler.ScrollView.BorderType = NSBorderType.NoBorder;
				handler.Control.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.SourceList;
			});

			Style.Add<ToolBarButtonHandler> (null, handler => {
				// use standard textured/round buttons, and make the image grayscale
				handler.UseStandardButton (grayscale: true);
			});

			Style.Add<ToolBarHandler> (null, handler => { 
				// change display mode or other options
				//handler.Control.DisplayMode = NSToolbarDisplayMode.Icon;
			});
		}
	}
}	

