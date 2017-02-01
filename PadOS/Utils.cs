using System.Windows;

namespace PadOS {
	public static class Utils {
		public static T FindParentOfType<T>(this FrameworkElement elm) where T : FrameworkElement {
			var searchElm = elm;
			while (searchElm.Parent != null && searchElm is T == false)
				searchElm = (FrameworkElement)searchElm.Parent;
			return searchElm as T;
		}

		public static System.Drawing.Bitmap BitmapSourceToBitmap(System.Windows.Media.Imaging.BitmapSource srs) {
			var width = srs.PixelWidth;
			var height = srs.PixelHeight;
			var stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
			var ptr = System.IntPtr.Zero;
			try {
				ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(height * stride);
				srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
				using (var btm = new System.Drawing.Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr)) {
					// Clone the bitmap so that we can dispose it and
					// release the unmanaged memory at ptr
					return new System.Drawing.Bitmap(btm);
				}
			}
			finally {
				if (ptr != System.IntPtr.Zero)
					System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
			}
		}

		public static System.Windows.Media.Imaging.BitmapImage BitmapToBitmapSource(System.Drawing.Image resBitmap) {
			var ms = new System.IO.MemoryStream();
			resBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			ms.Position = 0;
			var bitmap = new System.Windows.Media.Imaging.BitmapImage();
			bitmap.BeginInit();
			bitmap.StreamSource = ms;
			bitmap.EndInit();
			return bitmap;
		}

		public static T CreateInstance<T>(this System.Type type){
			return (T) System.Activator.CreateInstance(type);
		}
	}

}