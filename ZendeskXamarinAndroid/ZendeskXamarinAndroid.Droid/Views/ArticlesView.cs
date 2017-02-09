using Android.App;
using Android.OS;
using ZendeskXamarinAndroid.Core.ViewModels;
namespace ZendeskXamarinAndroid.Droid.Views
{
	[Activity(Label = "Zendesk Articles")]
	public class ArticlesView : BaseView<ArticlesViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Articles;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);

			Vm.LoadArticlesBySection();
		}
	}
}
