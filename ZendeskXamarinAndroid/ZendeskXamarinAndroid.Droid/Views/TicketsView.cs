using Android.App;
using Android.OS;
using ZendeskXamarinAndroid.Core.ViewModels;


namespace ZendeskXamarinAndroid.Droid.Views
{
	[Activity(Label="My Tickets")]
	public class TicketsView : BaseView<TicketsViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Tickets;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);

			Vm.LoadTickets();

		}
	}
}
