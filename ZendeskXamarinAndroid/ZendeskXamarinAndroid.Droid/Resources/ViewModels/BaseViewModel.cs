using MvvmCross.Core.ViewModels;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public abstract class BaseViewModel:MvxViewModel
	{
		public override void Start()
		{
			base.Start();
			ImplementCommands();
		}
		protected abstract void ImplementCommands();

		// Just a dummy string to be replaced with a custom text
		// For binding constraints, we need to provide a string before localizing
		public string ReplaceableString { get { return ""; } }
	}
}