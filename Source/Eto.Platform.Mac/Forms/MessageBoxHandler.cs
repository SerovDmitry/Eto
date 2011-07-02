using System;
using Eto.Forms;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace Eto.Platform.Mac
{
    internal class MacModalDelegate : NSObject
    {
        [Export("alertDidEnd:returnCode:contextInfo:")]
         public void AlertDidEnd (NSAlert alert, int returnCode, IntPtr contextInfo)
        {
            NSApplication.SharedApplication.StopModalWithCode (returnCode);
        }
        
        public static int Run(NSAlert view, Control parent)
        {
            int ret;
            if (parent != null) {
                var window = parent.ControlObject as NSWindow;
                if (window == null && parent.ControlObject is NSView)
                    window = ((NSView)parent.ControlObject).Window;
                if (window == null || !view.RespondsToSelector(new Selector("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")))
                    ret = view.RunModal ();
                else {
                    view.BeginSheet (window, new MacModalDelegate (), new Selector ("alertDidEnd:returnCode:contextInfo:"), IntPtr.Zero);
                    ret = NSApplication.SharedApplication.RunModalForWindow (window);
                }
            } else
                ret = view.RunModal ();
            return ret;
        }
        
        public static int Run(NSSavePanel view, Control parent)
        {
            int ret;
            if (parent != null) {
                var window = parent.ControlObject as NSWindow;
                if (window == null && parent.ControlObject is NSView)
                    window = ((NSView)parent.ControlObject).Window;
                if (window == null || !view.RespondsToSelector(new Selector("beginSheetModalForWindow:completionHandler:")))
                    ret = view.RunModal ();
                else {
                    view.BeginSheet (window, delegate(int returnCode) { NSApplication.SharedApplication.StopModalWithCode (returnCode); });
                    ret = NSApplication.SharedApplication.RunModalForWindow (window);
                }
            } else
                ret = view.RunModal ();
            return ret;
        }
    }
    
    public class MessageBoxHandler : IMessageBox
    {
        NSAlert alert;
     
        public string Text { get; set; }

        public string Caption { get; set; }

        public IWidget Handler { get; set; }
     
        public void Initialize ()
        {
        }
     
        void Ended ()
        {
            NSApplication.SharedApplication.StopModal ();
        }

        public DialogResult ShowDialog (Control parent)
        {
            alert = new NSAlert ();
            RunDialog (parent, alert);
            return DialogResult.Ok;
        }
     
        int RunDialog (Control parent, NSAlert alert)
        {
            if (Text != null)
                alert.MessageText = Text;
            if (Caption != null)
                alert.InformativeText = Caption;
            return MacModalDelegate.Run(alert, parent);
        }

        public DialogResult ShowDialog (Control parent, MessageBoxButtons buttons)
        {
            var alert = new NSAlert ();
         
            var OkButton = "OK";
            var CancelButton = "Cancel";
            var YesButton = "Yes";
            var NoButton = "No";
         
            switch (buttons) {
            case MessageBoxButtons.OK:
                alert.AddButton (OkButton);
                break;
            case MessageBoxButtons.OKCancel:
                alert.AddButton (OkButton);
                alert.AddButton (CancelButton);
                break;
            case MessageBoxButtons.YesNo:
                alert.AddButton (YesButton);
                alert.AddButton (NoButton);
                break;
            case MessageBoxButtons.YesNoCancel:
                alert.AddButton (YesButton);
                alert.AddButton (CancelButton);
                alert.AddButton (NoButton);
                break;
            }
            int ret = RunDialog (parent, alert);
            switch (buttons) {
            default:
            case MessageBoxButtons.OK:
                return DialogResult.Ok;
            case MessageBoxButtons.OKCancel:
                return (ret == 1000) ? DialogResult.Ok : DialogResult.Cancel;
            case MessageBoxButtons.YesNo:
                return (ret == 1000) ? DialogResult.Yes : DialogResult.No;
            case MessageBoxButtons.YesNoCancel:
                return (ret == 1000) ? DialogResult.Yes : (ret == 1001) ? DialogResult.Cancel : DialogResult.No;
            }
        }
    }
}