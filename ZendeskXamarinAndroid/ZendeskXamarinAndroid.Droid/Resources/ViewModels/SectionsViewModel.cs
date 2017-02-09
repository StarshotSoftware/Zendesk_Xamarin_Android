using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using ZendeskXamarinAndroid.Core.Entities;
using ZendeskXamarinAndroid.Core.Services;
using ZendeskXamarinAndroid.Utils;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public class SectionsViewModel : BaseViewModel
	{
		#region Fields
		readonly IDialogsService _dialogsService;

		long currentCategoryId;
		public string currentCategoryName { get; set;}

		MvxCommand _loadSections;
		public IMvxCommand LoadSectionsCommand => _loadSections;

		MvxCommand<ZendeskApi_v2.Models.Sections.Section> _sectionClickedCommand;

		public ICommand ItemSelectedCommand
		{
			get
			{
				return _sectionClickedCommand = _sectionClickedCommand ?? new MvxCommand<ZendeskApi_v2.Models.Sections.Section>(OpenArticles);
			}
		}

		private List<ZendeskApi_v2.Models.Sections.Section> _sections;
		public List<ZendeskApi_v2.Models.Sections.Section> Sections
		{
			get
			{
				return _sections;
			}

			set
			{
				_sections = value;
				RaisePropertyChanged(() => Sections);
			}
		}

		#endregion

		#region Constructor

		public SectionsViewModel(IDialogsService dialogsService)
		{
			_dialogsService = dialogsService;
		}

		public void Init(long catId,string catName)
		{
			currentCategoryId = catId;
			currentCategoryName = catName;
		}

		#endregion

		#region Implemented abstract members of BaseView

		protected override void ImplementCommands()
		{
			_loadSections = new MvxCommand(LoadSectionsByCategory);
			_sectionClickedCommand = new MvxCommand<ZendeskApi_v2.Models.Sections.Section>(OpenArticles);

		}


		#endregion

		#region commands implementation
		void OpenArticles(ZendeskApi_v2.Models.Sections.Section item)
		{
			ShowViewModel<ArticlesViewModel>(new { secId = item.Id.Value, secName = item.Name });
		}

		public async void LoadSectionsByCategory()
		{
			_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("loadSections"));

			var instance = FirstViewModel.Instance;
			var secs = await instance.HelpCenter.Sections.GetSectionsByCategoryIdAsync(currentCategoryId);
			var secList = new List<ZendeskApi_v2.Models.Sections.Section>();
			if (secs != null){

				foreach (var item in secs.Sections)
				{
					secList.Add(item);
				}
				Sections = secList;
			}
			_dialogsService.HideLoadingModal();

		}


		#endregion
	}
}
