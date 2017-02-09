using Android.App;
using Android.OS;
using ZendeskXamarinAndroid.Core.ViewModels;
namespace ZendeskXamarinAndroid.Droid.Views
{
	[Activity(Label = "Zendesk Sections")]
	public class SectionsView : BaseView<SectionsViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Sections;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);

			Vm.LoadSectionsByCategory();
		}
	}
}
