using Android.OS;
using Android.Support.V7.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using ZendeskXamarinAndroid.Core.ViewModels;

namespace ZendeskXamarinAndroid.Droid.Views
{
	public abstract class BaseView<T> : MvxAppCompatActivity where T : MvxViewModel
	{
		T _viewModel;
		internal T Vm
		{
			get
			{
				if (_viewModel == null)
					_viewModel = base.ViewModel as T;
				return _viewModel;
			}
		}

		protected Toolbar Toolbar { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(LayoutResource);

			Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			if (Toolbar != null)
			{
				SetSupportActionBar(Toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				SupportActionBar.SetHomeButtonEnabled(true);
			}
		}

		protected abstract int LayoutResource { get; }
	}
}
