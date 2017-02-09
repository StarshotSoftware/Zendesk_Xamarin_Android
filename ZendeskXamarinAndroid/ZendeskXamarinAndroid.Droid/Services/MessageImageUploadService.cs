using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using MvvmCross.Platform;
using MvvmCross.Plugins.PictureChooser;
using ZendeskXamarinAndroid.Core.Services;

namespace ZendeskXamarinAndroid.Droid
{
	public class MessageImageUploadService : IMessageImageUploadService
	{
		public async Task<Stream> GetImageFromCamera()
		{
			var task = Mvx.Resolve<IMvxPictureChooserTask>();
			return await task.TakePictureAsync(2048, 100);
		}

		public async Task<Stream> GetImageFromLibrary()
		{
			var task = Mvx.Resolve<IMvxPictureChooserTask>();
			return await task.ChoosePictureFromLibraryAsync(2048, 100);
		}

		public string ResizeImage(Stream imageDataStream, float width, float height)
		{
			// Load the bitmap

			var imageData = ReadFully(imageDataStream);

			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

			float maxWidth = 1024;
			float maxHeight =768;
		
			var sourceWidth = originalImage.Width;
			var sourceHeight = originalImage.Height;
			var maxResizeFactor = Math.Max(maxWidth / sourceWidth, maxHeight / sourceHeight);
			var newWidth = maxResizeFactor * sourceWidth;
			var newHeight = maxResizeFactor * sourceHeight;


			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);


			var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "tmp.jpg");
			var stream = new FileStream(filePath, FileMode.Create);
			resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
			stream.Close();

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
				return filePath;
			}
		}

		private static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}

