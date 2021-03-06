using ElmSharp;
using System.Collections.Generic;

namespace System.Maui.Platform.Tizen
{
	public class ImageCellRenderer : TextCellRenderer
	{
		const int _defaultHeight = 55;

		public ImageCellRenderer() : this(Device.Idiom == TargetIdiom.Watch ? "1icon_2text" : (Device.Idiom == TargetIdiom.Phone ? "double_label" : "default"))
		{
			ImagePart = "elm.swallow.icon";
		}

		protected ImageCellRenderer(string style) : base(style)
		{
		}

		protected string ImagePart { get; set; }

		protected override EvasObject OnGetContent(Cell cell, string part)
		{
			if (part == ImagePart)
			{
				var imgCell = cell as ImageCell;
				int pixelSize = System.Maui.Maui.ConvertToScaledPixel(imgCell.RenderHeight);
				if (pixelSize <= 0)
				{
					pixelSize = System.Maui.Maui.ConvertToPixel(_defaultHeight);
				}

				var image = new Native.Image(System.Maui.Maui.NativeParent)
				{
					MinimumWidth = pixelSize,
					MinimumHeight = pixelSize
				};
				image.SetAlignment(-1.0, -1.0); // fill
				image.SetWeight(1.0, 1.0); // expand

				var task = image.LoadFromImageSourceAsync(imgCell.ImageSource);
				return image;
			}
			else
			{
				return null;
			}
		}

		protected override bool OnCellPropertyChanged(Cell cell, string property, Dictionary<string, EvasObject> realizedView)
		{
			if (property == ImageCell.ImageSourceProperty.PropertyName)
			{
				EvasObject image;
				realizedView.TryGetValue(ImagePart, out image);
				(image as Native.Image)?.LoadFromImageSourceAsync((cell as ImageCell)?.ImageSource);
				return false;
			}
			return base.OnCellPropertyChanged(cell, property, realizedView);
		}
	}
}
