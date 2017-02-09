using Android.App;
using Android.OS;
using ZendeskXamarinAndroid.Core.ViewModels;


namespace ZendeskXamarinAndroid.Droid.Views
{
	[Activity(Label="")]
	public class ArticledetailsView : BaseView<ArticledetailsViewModel>
	{
		protected override int LayoutResource => Resource.Layout.ArticleDetails;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);

			Vm.LoadArticleDetails();

		}
	}
}
