using Android.App;
using Android.OS;
using ZendeskXamarinAndroid.Core.Entities;
using ZendeskXamarinAndroid.Core.ViewModels;
namespace ZendeskXamarinAndroid.Droid.Views
{
	[Activity(Label = "Zendesk Categories"
	    , MainLauncher = true
		, Icon = "@drawable/icon")]
	public class FirstView : BaseView<FirstViewModel>
	{
		protected override int LayoutResource => Resource.Layout.FirstView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);

			var zendeskUser = new ZendeskUserAccess() { 
				user = "YOUR_USER_MAIL", 
				oAuthToken = "APP_OAUTH_TOKEN" 
			};

			//user = "angel.valera@starshotsoftware.com"
			//oAuthToken = "UAGJyEasLJbyviNxIJEAXRmH6p15x3UnjItiel0X"

			Vm.ZendeskInit(zendeskUser);
			Vm.LoadCategories();
		}
	}
}
