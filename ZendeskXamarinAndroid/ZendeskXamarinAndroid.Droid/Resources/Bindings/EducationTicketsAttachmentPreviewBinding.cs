using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Net;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using Plugin.CurrentActivity;

namespace ZendeskXamarinAndroid.Droid.Binding
{
	public class TicketsAttachmentPreviewBinding : MvxAndroidTargetBinding
	{
		private readonly ImageView _imagePreview;

		public TicketsAttachmentPreviewBinding(ImageView imagePreview) : base(imagePreview)
		{
			_imagePreview = imagePreview;
		}

		public override Type TargetType => typeof(ImageView);


		protected override void SetValueImpl(object target, object value)
		{
			var path = (string)value;

			if (path != null)
			{
				_imagePreview.Visibility = ViewStates.Visible;
				Bitmap bmp = null;

				bmp = BitmapFactory.DecodeFile(path);

				_imagePreview.SetImageBitmap(bmp);
			}
			else {
				_imagePreview.Visibility = ViewStates.Gone;
			}
				
		}

	public override MvxBindingMode DefaultMode => MvxBindingMode.Default;
	}
}