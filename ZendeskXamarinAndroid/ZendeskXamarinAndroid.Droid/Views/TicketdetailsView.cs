using Android.App;
using Android.OS;
using ZendeskXamarinAndroid.Core.ViewModels;


namespace ZendeskXamarinAndroid.Droid.Views
{
	[Activity(Label="My Tickets")]
	public class TicketdetailsView : BaseView<TicketdetailsViewModel>
	{
		protected override int LayoutResource => Resource.Layout.TicketsDetails;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);

			Vm.LoadTicketDetails();

		}
	}
}
