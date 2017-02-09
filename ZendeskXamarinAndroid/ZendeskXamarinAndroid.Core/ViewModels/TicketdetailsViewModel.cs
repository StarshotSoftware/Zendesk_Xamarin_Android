using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using ZendeskApi_v2.Models.Shared;
using ZendeskXamarinAndroid.Core.Services;
using ZendeskXamarinAndroid.Utils;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public class TicketdetailsViewModel : BaseViewModel
	{
		#region Fields

		readonly IDialogsService _dialogsService;
		readonly IMessageImageUploadService _messageImageUploadService;
		readonly IFileSystem _fileSystem;

		MvxCommand _loadArticleDetails;
		public IMvxCommand LoadArticleDetailsCommand => _loadArticleDetails;

		long currentTicketId { get; set; }

		private string _ticketSubject;
		public string TicketSubject
		{
			get
			{
				return _ticketSubject;
			}

			set
			{
				_ticketSubject = value;
				RaisePropertyChanged(() => TicketSubject);
			}
		}

		private List<ZendeskApi_v2.Models.Tickets.Comment> _comments;
		public List<ZendeskApi_v2.Models.Tickets.Comment> Comments

		{
			get
			{
				return _comments;
			}

			set
			{
				_comments = value;
				RaisePropertyChanged(() => Comments);
			}
		}

		string _textMessage;
		public string TextMessage
		{
			get { return _textMessage; }
			set
			{
				_textMessage = value;
				RaisePropertyChanged(() => TextMessage);
				RaisePropertyChanged(() => IsPublishEnabled);
			}
		}

	
		string _imagePath;
		public string ImagePath
		{ 
			get { return _imagePath; }
			set
			{
				_imagePath = value;
				RaisePropertyChanged(() => ImagePath);
			}
		}


		public bool IsPublishEnabled => !string.IsNullOrEmpty(TextMessage) || ImagePath!=null;
		public bool IsNewMessage => currentTicketId==0;


		MvxCommand _publishMessageCommand;
		public IMvxCommand PublishMessageCommand => _publishMessageCommand;

		MvxCommand _uploadPhotoCommand;
		public IMvxCommand UploadPhotoCommand => _uploadPhotoCommand;


		#endregion

		#region Constructor

		public TicketdetailsViewModel(IDialogsService dialogsService, IMessageImageUploadService messageService, IFileSystem filesystem)
		{
			_dialogsService = dialogsService;
			_messageImageUploadService = messageService;
			_fileSystem = filesystem;
	
		}
		public void Init(long ticketId, string ticketSubject)
		{
			currentTicketId = ticketId;
			TicketSubject = ticketSubject;
			RaisePropertyChanged(() => IsNewMessage);
		}
		#endregion

		#region Implemented abstract members of BaseView

		protected override void ImplementCommands()
		{
		//	_loadArticleDetails = new MvxCommand(LoadArticleDetails);
			_publishMessageCommand = new MvxCommand(async () => await PublishMessage());
			_uploadPhotoCommand = new MvxCommand(async () => await UploadPicture());
		}

		#endregion

		#region commands implementation

		public async void LoadTicketDetails()
		{
			if (currentTicketId != 0) //is not a new ticket
			{
				
				_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("loadComments"));

				var comments = await FirstViewModel.Instance.Tickets.GetTicketCommentsAsync(currentTicketId);
				var commentList = new List<ZendeskApi_v2.Models.Tickets.Comment>();
				var attachmentsList = new List<Tuple<long, string>>();
				foreach (var comment in comments.Comments)
				{
					
					commentList.Add(comment);
					foreach (var attachment in comment.Attachments)
					{
						attachmentsList.Add(new Tuple<long, string>(comment.Id.Value, attachment.ContentUrl));
					}
				}
				Comments = commentList;
				_dialogsService.HideLoadingModal();
			}
		}

		async Task PublishMessage()
		{
			RaisePropertyChanged(() => IsPublishEnabled);
			Upload res =null;
			if (ImagePath != null)
			{
				_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("sendComment"));

			
				try
				{
					// Resize the image before sending

					//upload attachment to zendesk
					res = await FirstViewModel.Instance.Attachments.UploadAttachmentAsync(new ZenFile()
					{
						ContentType = "image/jpg",
						FileName = DateTime.Now.ToString()+".jpg",
						FileData = _fileSystem.ReadAllByteS(ImagePath)
					});
				}
				catch (Exception ex)
				{
					_dialogsService.DisplayToast(ResourceStrings.Instance.GetString("imageNotUploaded") + ex.Message);
				}
				finally
				{

					_dialogsService.HideLoadingModal();
				}
			}


			try
			{

				var comment = new ZendeskApi_v2.Models.Tickets.Comment();
				comment.Body = TextMessage;
				comment.Public = true;
				if (res != null)
				{
					comment.Uploads = new List<string>() { res.Token };

				}

				if (currentTicketId != 0) //new Ticket
				{
					var ticket = await FirstViewModel.Instance.Tickets.GetTicketAsync(currentTicketId);
					var updateResponse = await FirstViewModel.Instance.Tickets.UpdateTicketAsync(ticket.Ticket, comment);

				}
				else {

					var ticket = new ZendeskApi_v2.Models.Tickets.Ticket()
					{
						Subject = TextMessage,
						Priority = ZendeskApi_v2.Models.Constants.TicketPriorities.Normal,
						Comment = comment
						                        
					};

		
					var newTicketResponse = await FirstViewModel.Instance.Tickets.CreateTicketAsync(ticket);
					currentTicketId = newTicketResponse.Ticket.Id.Value;
					TicketSubject = newTicketResponse.Ticket.Subject;
				}

				LoadTicketDetails();

				TextMessage = "";
				ImagePath = null;
				RaisePropertyChanged(() => IsNewMessage);
			}
			catch (Exception ex)
			{
				_dialogsService.DisplayToast("Ticket not sended " + ex.Message);
			}
			finally
			{
				RaisePropertyChanged(() => IsPublishEnabled);
				_dialogsService.HideLoadingModal();
			}
		}

		public async Task UploadPicture()
		{
			var tcs = new TaskCompletionSource<Stream>();
			var options = new string[] { ResourceStrings.Instance.GetString("takePhoto"), ResourceStrings.Instance.GetString("chooseExisting") };
			_dialogsService.DisplayDialogList(ResourceStrings.Instance.GetString("chooseShare"), ResourceStrings.Instance.GetString("cancel"), null, options, async (DialogListActionResult obj) =>
			{
				if (obj.Cancel)
					tcs.SetResult(null);
				else if (obj.OptionChosen == 0)
					tcs.SetResult(await _messageImageUploadService.GetImageFromCamera());
				else
					tcs.SetResult(await _messageImageUploadService.GetImageFromLibrary());
			});

				var RawImage = await tcs.Task;
			_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("sendImage"));


				ImagePath = await Task.Run(() => _messageImageUploadService.ResizeImage(RawImage, 800, 800));
				_dialogsService.HideLoadingModal();

		}
		#endregion
	}
}
