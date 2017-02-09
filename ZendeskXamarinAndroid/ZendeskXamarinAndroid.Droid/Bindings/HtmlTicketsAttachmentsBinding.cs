using System;
using System.Threading;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using Plugin.CurrentActivity;
using Square.Picasso;

namespace ZendeskXamarinAndroid.Droid.Binding
{
	public class HtmlTicketsAttachmentsBinding : MvxAndroidTargetBinding
	{
		private readonly LinearLayout _avatarContainer;

		public HtmlTicketsAttachmentsBinding(LinearLayout avatarContainer) : base(avatarContainer)
		{
			_avatarContainer = avatarContainer;
		}

		public override Type TargetType => typeof(LinearLayout);


		protected override void SetValueImpl(object target, object value)
		{
			Thread thread = new Thread(() =>
		   {
			   if (value != null)
			   {
				   dynamic attachments = value;

				   if (attachments != null && attachments.Count > 0)
				   {
					   var progress = _avatarContainer.GetChildAt(0);
					   if (progress != null && progress.GetType() == typeof(ProgressBar))
					   {
							CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
							{
								progress.Visibility = ViewStates.Visible;
							});
					   }

					   LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
					   CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
						{
							_avatarContainer.RemoveAllViews();
						});
						foreach (var attachment in attachments)
					   {
							   var imageViewAvatar = new ImageView(CrossCurrentActivity.Current.Activity);
						   CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
					  {
						  Picasso.With(CrossCurrentActivity.Current.Activity)
								 .Load(attachment.ContentUrl)
								 .Into(imageViewAvatar);
					  });
						   imageViewAvatar.SetMinimumWidth(200);
						   imageViewAvatar.SetMinimumHeight(200);
							imageViewAvatar.SetMaxHeight(300);
						   imageViewAvatar.LayoutParameters = parameters;
						   CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
							{
								_avatarContainer.AddView(imageViewAvatar);
							});

					   }
					  
					   if (progress != null)
					   {
						   CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
							{
								progress.Visibility = ViewStates.Gone;
							});
					   }
				   }
				   else {
					   CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
						{
							_avatarContainer.Visibility = ViewStates.Gone;
						});
				   }
			   }
		   });
			thread.Start();
		}

		public override MvxBindingMode DefaultMode => MvxBindingMode.Default;
	}
}