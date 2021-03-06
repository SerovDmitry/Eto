using System;
using System.Reflection;
using SWF = System.Windows.Forms;
using SD = System.Drawing;
using Eto.Forms;
using Eto.Drawing;
using Eto.Platform.Windows.Drawing;

namespace Eto.Platform.Windows
{
	public interface IToolBarItemHandler
	{
		void CreateControl(ToolBarHandler handler);
		
	}
	
	public abstract class ToolBarItemHandler<T, W> : WidgetHandler<T, W>, IToolBarItem, IToolBarItemHandler
		where T: SWF.ToolStripItem
		where W: ToolBarItem
	{
        Image image;
		int imageSize = 16;

		public abstract void CreateControl(ToolBarHandler handler);

		public virtual void InvokeButton()
		{
		}

		public int ImageSize
		{
			get { return imageSize; }
			set
			{
				imageSize = value;
				Control.Image = image.ToSD (imageSize);
			}
		}
		
		public string Text
		{
			get { return Control.Text; }
			set { Control.Text = value; }
		}
		
		public string ToolTip
		{
			get { return Control.ToolTipText; }
			set { Control.ToolTipText = value; }
		}
		
		public Image Image
		{
			get { return image; }
			set
			{
				this.image = value;
				Control.Image = image.ToSD (imageSize);
			}
		}

		public abstract bool Enabled { get; set; }
	}
}
