using Eto.Drawing;
using Eto.Forms;
using Eto.Platform.Wpf.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sw = System.Windows;

namespace Eto.Platform.Wpf.Forms
{
	public class ClipboardHandler : WidgetHandler<object, Clipboard>, IClipboard
	{
		public string[] Types
		{
			get { return sw.Clipboard.GetDataObject ().GetFormats (); }
		}

		public void SetString (string value, string type)
		{
			if (string.IsNullOrEmpty (type))
				sw.Clipboard.SetText (value);
			else
				sw.Clipboard.SetData (type, value);
		}

		public void SetData (byte[] value, string type)
		{
			sw.Clipboard.SetData (type, value);
		}

		public string GetString (string type)
		{
			if (string.IsNullOrEmpty (type))
				return sw.Clipboard.GetText ();
			else
				return Convert.ToString (sw.Clipboard.GetData (type));
		}

		public byte[] GetData (string type)
		{
			return sw.Clipboard.GetData (type) as byte[];
		}

		public string Text
		{
			get { return sw.Clipboard.GetText (); }
			set { sw.Clipboard.SetText (value); }
		}

		public string Html
		{
			get { return sw.Clipboard.GetText (sw.TextDataFormat.Html); }
			set { sw.Clipboard.SetText (value, sw.TextDataFormat.Html); }
		}

		public Eto.Drawing.Image Image
		{
			get { return new Bitmap (Widget.Generator, new BitmapHandler (sw.Clipboard.GetImage ())); }
			set { sw.Clipboard.SetImage (value.ToWpf ()); }
		}

		public void Clear ()
		{
			sw.Clipboard.Clear ();
		}
	}
}
