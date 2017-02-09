using System;

namespace ZendeskXamarinAndroid.Core.Services
{
	public enum DialogActionResult
	{
		Cancel = -1,
		Ok = 0
	}

	public class DialogTextboxActionResult
	{
		public DialogActionResult Result { get; set;}
		public string TextboxContent { get; set; }
	}

	public class DialogListActionResult
	{
		public bool Cancel { get; set; }
		public bool Destroy { get; set; }
		public int OptionChosen { get; set; }
	}

	public interface IDialogsService
	{
		void DisplayDialog(string description, Action<DialogActionResult> onDialogDismissed);

		void DisplayDialog(string title, string description, Action<DialogActionResult> onDialogDismissed);

		void DisplayDialog(string title, string description, string okText, Action<DialogActionResult> onDialogDismissed);

		void DisplayDialog(string title, string description, string okText, string cancelText, Action<DialogActionResult> onDialogDismissed);

		void DisplayDialogWithTextbox(string title, string textboxPlaceholder, string okText, string cancelText, Action<DialogTextboxActionResult> onDialogDismissed);

		void DisplayToast(string description);

		void DisplayToast(string description, float duration);

		void DisplayDialogList(string description, string cancelText, string destroyText, string[] options, Action<DialogListActionResult> onDialogDismissed);

		void ShowLoadingModal(string message);

		void HideLoadingModal();
	}
}

