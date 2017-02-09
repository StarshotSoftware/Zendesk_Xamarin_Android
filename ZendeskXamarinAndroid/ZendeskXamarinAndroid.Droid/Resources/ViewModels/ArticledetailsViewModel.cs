using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using ZendeskApi_v2;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public class ArticledetailsViewModel : BaseViewModel
	{
		#region Fields


		MvxCommand _loadArticleDetails;
		public IMvxCommand LoadArticleDetailsCommand => _loadArticleDetails;

		long currentArticleId { get; set;}

		private string _articleTitle;
		public string ArticleTitle
		{
			get
			{
				return _articleTitle;
			}

			set
			{
				_articleTitle = value;
				RaisePropertyChanged(() => ArticleTitle);
			}
		}
		private string _articleBody;
		public string ArticleBody
		{
			get
			{
				return _articleBody;
			}

			set
			{
				_articleBody = value;
				RaisePropertyChanged(() => ArticleBody);
			}
		}
		private string _articleUrl;
		public string ArticleUrl
		{
			get
			{
				return _articleUrl;
			}

			set
			{
				_articleUrl = value;
				RaisePropertyChanged(() => ArticleUrl);
			}
		}
		private ZendeskApi_v2.Models.Articles.Article _article;
		public ZendeskApi_v2.Models.Articles.Article Article
		{
			get
			{
				return _article;
			}

			set
			{
				_article = value;
				ArticleBody = value.Body;
				ArticleTitle = value.Title;
				ArticleUrl = value.HtmlUrl;
				RaisePropertyChanged(() => Article);
			}
		}

		#endregion

		#region Constructor

		public ArticledetailsViewModel()
		{
		}
		public void Init(long artId)
		{
			currentArticleId = artId;
		}
		#endregion

		#region Implemented abstract members of BaseView

		protected override void ImplementCommands()
		{
		//	_loadArticleDetails = new MvxCommand(LoadArticleDetails);

		}


		#endregion

		#region commands implementation


		public async void LoadArticleDetails()
		{
			var ArticleResponse=await FirstViewModel.Instance.HelpCenter.Articles.GetArticleAsync(currentArticleId);
			Article= ArticleResponse.Article;
		}

		#endregion
	}
}
