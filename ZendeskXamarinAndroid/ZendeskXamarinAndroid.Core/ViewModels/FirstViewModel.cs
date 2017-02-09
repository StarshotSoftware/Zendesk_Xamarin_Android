using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using ZendeskApi_v2;
using ZendeskXamarinAndroid.Core.Entities;
using ZendeskXamarinAndroid.Core.Services;
using ZendeskXamarinAndroid.Utils;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public class FirstViewModel: BaseViewModel
	{
		
		public static ZendeskApi Instance { get; set; }
		readonly IDialogsService _dialogsService;
		MvxCommand _openTickets;
		public IMvxCommand OpenTicketsCommand => _openTickets;

		MvxCommand _loadCategories;
		public IMvxCommand LoadCategoriesCommand => _loadCategories;

		MvxCommand<ZendeskApi_v2.Models.HelpCenter.Categories.Category> _categoryClickedCommand;

		public ICommand ItemSelectedCommand
		{
			get
			{
				return _categoryClickedCommand = _categoryClickedCommand ?? new MvxCommand<ZendeskApi_v2.Models.HelpCenter.Categories.Category>(OpenSections);
			}
		}

		private List<ZendeskApi_v2.Models.HelpCenter.Categories.Category> _categories;
		public List<ZendeskApi_v2.Models.HelpCenter.Categories.Category> Categories
		{
			get
			{
				return _categories;
			}

			set
			{
				_categories = value;
				RaisePropertyChanged(() => Categories);
			}
		}


		public FirstViewModel(IDialogsService dialogsService)
		{
			_dialogsService = dialogsService;
		}

		public void ZendeskInit(ZendeskUserAccess userData)
		{
			var ZendeskUrl = "https://narustestsupport.zendesk.com";
			Instance = new ZendeskApi(ZendeskUrl, userData.user, userData.password, userData.oAuthToken, userData.locale, null);
		}

		#region Implemented abstract members of BaseView

			protected override void ImplementCommands()
			{
				_loadCategories = new MvxCommand(LoadCategories);
				_categoryClickedCommand = new MvxCommand<ZendeskApi_v2.Models.HelpCenter.Categories.Category>(OpenSections);
				_openTickets = new MvxCommand(OpenTickets);
			}

			void OpenSections(ZendeskApi_v2.Models.HelpCenter.Categories.Category item)
			{
				ShowViewModel<SectionsViewModel>(new {  catId = item.Id.Value, catName = item.Name });
			}
			void OpenTickets()
			{
				ShowViewModel<TicketsViewModel>();
			}

			#endregion

			#region commands implementation


			public async void LoadCategories() {
				_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("loadCategories"));

				var cats = await Instance.HelpCenter.Categories.GetCategoriesAsync();
				var catList = new List<ZendeskApi_v2.Models.HelpCenter.Categories.Category>();

				foreach (var item in cats.Categories)
				{
					catList.Add(item);
				}
				Categories = catList;
				_dialogsService.HideLoadingModal();

			}
	
			#endregion
	
	}
}
