using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using ZendeskXamarinAndroid.Core.Services;
using ZendeskXamarinAndroid.Utils;

namespace ZendeskXamarinAndroid.Droid
{
	public class DialogsService : IDialogsService
	{
		private AlertDialog loadingOverlay;

		public void DisplayDialog(string description, Action<DialogActionResult> onDialogDismissed)
		{
			DisplayDialog(null, description, null, null, onDialogDismissed);
		}

		public void DisplayDialog(string title, string description, Action<DialogActionResult> onDialogDismissed)
		{
			DisplayDialog(title, description, null, null, onDialogDismissed);
		}

		public void DisplayDialog(string title, string description, string okText, Action<DialogActionResult> onDialogDismissed)
		{
			DisplayDialog(title, description, okText, null, onDialogDismissed);
		}

		public void DisplayDialog(string title, string description, string okText, string cancelText, Action<DialogActionResult> onDialogDismissed)
		{
			if (string.IsNullOrEmpty(okText))
				okText = ResourceStrings.Instance.GetString("ok");
			
			var builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
			builder.SetCancelable(false);

			if (!string.IsNullOrEmpty(description))
				builder.SetMessage(description);
			
			if (!string.IsNullOrEmpty(title))
				builder.SetTitle(title);
			
			builder.SetPositiveButton(okText, (sender, e) => onDialogDismissed?.Invoke(DialogActionResult.Ok));

			if (!string.IsNullOrEmpty(cancelText))
				builder.SetNegativeButton(cancelText, (sender, e) => onDialogDismissed?.Invoke(DialogActionResult.Cancel));

			AlertDialog dialog = builder.Create();
			dialog.Show();
		}

		public void DisplayDialogWithTextbox(string title, string textboxPlaceholder, string okText, string cancelText, Action<DialogTextboxActionResult> onDialogDismissed)
		{
			if (string.IsNullOrEmpty(okText))
				okText = ResourceStrings.Instance.GetString("ok");

			var builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
			builder.SetCancelable(false);

			View layout = LayoutInflater.From(CrossCurrentActivity.Current.Activity).Inflate(Resource.Layout.DialogWithTextbox, null);
			EditText text = layout.FindViewById<EditText>(Resource.Id.dialogText);
			builder.SetView(layout);

			if (!string.IsNullOrEmpty(title))
				builder.SetTitle(title);

			builder.SetPositiveButton(okText, (sender, e) => onDialogDismissed?.Invoke(new DialogTextboxActionResult { Result = DialogActionResult.Ok, TextboxContent = text.Text }));

			if (!string.IsNullOrEmpty(cancelText))
				builder.SetNegativeButton(cancelText, (sender, e) => onDialogDismissed?.Invoke(new DialogTextboxActionResult { Result = DialogActionResult.Cancel, TextboxContent = null }));

			AlertDialog dialog = builder.Create();
			dialog.Show();
		}

		public void DisplayToast(string description)
		{
			DisplayToast(description, 2.0f);
		}

		public void DisplayToast(string description, float duration)
		{
			View layout = LayoutInflater.From(CrossCurrentActivity.Current.Activity).Inflate(Resource.Layout.ToastLayout, null);
			TextView text = layout.FindViewById<TextView>(Resource.Id.toastText);
			text.Text = description;

			var toast = new Toast(CrossCurrentActivity.Current.Activity);
			toast.SetGravity(GravityFlags.Top | GravityFlags.FillHorizontal, 0, 0);
			toast.Duration = ToastLength.Long;
			toast.SetMargin(0, 0);
			toast.View = layout;
			toast.Show();

			// The time can be shortened by the user. Remove the message before if the duration has elapsed
			var handler = new Handler();
			handler.PostDelayed(() => toast.Cancel(), (long)duration*1000);
		}

		public void DisplayDialogList(string description, string cancelText, string destroyText, string[] options, Action<DialogListActionResult> onDialogDismissed)
		{
			var builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
			builder.SetCancelable(false);

			if (!string.IsNullOrEmpty(description))
				builder.SetTitle(description);

			var optionsList = new List<string>(options);
			if (!string.IsNullOrEmpty(destroyText))
				optionsList.Add(destroyText);
			optionsList.Add(cancelText);

			builder.SetItems(optionsList.ToArray(), (object sender, Android.Content.DialogClickEventArgs e) =>
			{
				if (options != null && options.Length > 0 && e.Which < options.Length)
					onDialogDismissed?.Invoke(new DialogListActionResult { Cancel = false, Destroy = false, OptionChosen = e.Which });
				else if (!string.IsNullOrEmpty(destroyText) && e.Which == optionsList.Count - 2)
					onDialogDismissed?.Invoke(new DialogListActionResult { Cancel = false, Destroy = true, OptionChosen = -1 });
				else
					onDialogDismissed?.Invoke(new DialogListActionResult { Cancel = true, Destroy = false, OptionChosen = -1 });
			});

			AlertDialog dialog = builder.Create();
			dialog.Show();
		}

		public void ShowLoadingModal(string message)
		{
			// Only one message is allowed at a time
			if (loadingOverlay != null && loadingOverlay.IsShowing)
				return;

			if (string.IsNullOrEmpty(message))
				message = ResourceStrings.Instance.GetString("loadingPleaseWait");
			
			var builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
			builder.SetCancelable(false);
			View layout = LayoutInflater.From(CrossCurrentActivity.Current.Activity).Inflate(Resource.Layout.LoadingOverlay, null);
			TextView textMessage = layout.FindViewById<TextView>(Resource.Id.loadingText);
			textMessage.Text = message;
			builder.SetView(layout);

			loadingOverlay = builder.Create();
			loadingOverlay.Show();
		}

		public void HideLoadingModal()
		{
			loadingOverlay.Dismiss();
		}
	}
}

