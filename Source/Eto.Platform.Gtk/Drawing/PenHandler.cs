using System;
using Eto.Drawing;

namespace Eto.Platform.GtkSharp.Drawing
{
	public class PenHandler : IPenHandler
	{
		public Color Color { get; set; }

		public float Thickness { get; set; }

		public PenLineJoin LineJoin { get; set; }

		public PenLineCap LineCap { get; set; }

		public void Create (Color color, float thickness)
		{
			this.Color = color;
			this.Thickness = thickness;
		}

		public void Dispose ()
		{
		}

		public object ControlObject { get { return this; } }

		public void Apply (GraphicsHandler graphics)
		{
			graphics.Control.Color = Color.ToCairo ();
			graphics.Control.LineWidth = this.Thickness;
			graphics.Control.LineCap = Cairo.LineCap.Square;
			graphics.Control.LineJoin = LineJoin.ToCairo ();
		}
	}
}
