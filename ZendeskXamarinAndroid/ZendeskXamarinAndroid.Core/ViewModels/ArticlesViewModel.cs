using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using ZendeskXamarinAndroid.Core.Services;
using ZendeskXamarinAndroid.Utils;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public class ArticlesViewModel : BaseViewModel
	{
		#region Fields
		readonly IDialogsService _dialogsService;

	
		long currentSectionId;
		public string currentSectionName { get; set; }

		MvxCommand _loadArticles;
		public IMvxCommand LoadArticlesCommand => _loadArticles;


		MvxCommand<ZendeskApi_v2.Models.Articles.Article> _articleClickedCommand;

		public ICommand ItemSelectedCommand
		{
			get
			{
				return _articleClickedCommand = _articleClickedCommand ?? new MvxCommand<ZendeskApi_v2.Models.Articles.Article>(OpenArticleDetail);
			}
		}

		private List<ZendeskApi_v2.Models.Articles.Article> _articles;
		public List<ZendeskApi_v2.Models.Articles.Article> Articles
		{
			get
			{
				return _articles;
			}

			set
			{
				_articles = value;
				RaisePropertyChanged(() => Articles);
			}
		}

		#endregion

		#region Constructor

		public ArticlesViewModel(IDialogsService dialogsService)
		{
			_dialogsService = dialogsService;
		}
		public void Init(long secId, string secName)
		{
			currentSectionId = secId;
			currentSectionName = secName;
		}
		#endregion

		#region Implemented abstract members of BaseView

		protected override void ImplementCommands()
		{
			_loadArticles = new MvxCommand(LoadArticlesBySection);
			_articleClickedCommand = new MvxCommand<ZendeskApi_v2.Models.Articles.Article>(OpenArticleDetail);

		}


		#endregion

		#region commands implementation

		void OpenArticleDetail(ZendeskApi_v2.Models.Articles.Article item)
		{
			ShowViewModel<ArticledetailsViewModel>(new { artId = item.Id.Value });
		}

		public async void LoadArticlesBySection()
		{
			_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("loadArticles"));

			var arts = await FirstViewModel.Instance.HelpCenter.Articles.GetArticlesBySectionIdAsync(currentSectionId);
			var artList = new List<ZendeskApi_v2.Models.Articles.Article>();

			foreach (var item in arts.Articles)
			{
				artList.Add(item);

			}
			Articles = artList;
			_dialogsService.HideLoadingModal();

		}

		#endregion
	}
}
