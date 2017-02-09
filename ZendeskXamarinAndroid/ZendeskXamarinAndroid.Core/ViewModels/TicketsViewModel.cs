using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using ZendeskXamarinAndroid.Core.Services;
using ZendeskXamarinAndroid.Utils;

namespace ZendeskXamarinAndroid.Core.ViewModels
{
	public class TicketsViewModel : BaseViewModel
	{
		#region Fields

		readonly IDialogsService _dialogsService;
		MvxCommand _loadTickets;
		public IMvxCommand LoadTicketsCommand => _loadTickets;

		MvxCommand _newTicket;
		public IMvxCommand NewTicketCommand => _newTicket;


		MvxCommand<ZendeskApi_v2.Models.Tickets.Ticket> _ticketClickedCommand;

		public ICommand ItemSelectedCommand
		{
			get
			{
				return _ticketClickedCommand = _ticketClickedCommand ?? new MvxCommand<ZendeskApi_v2.Models.Tickets.Ticket>(OpenTicket);
			}
		}

		private List<ZendeskApi_v2.Models.Tickets.Ticket> _tickets;
		public List<ZendeskApi_v2.Models.Tickets.Ticket> Tickets
		{
			get
			{
				return _tickets;
			}

			set
			{
				_tickets = value;
				RaisePropertyChanged(() => Tickets);
			}
		}

		#endregion

		#region Constructor

		public TicketsViewModel(IDialogsService dialogsService)
		{
			_dialogsService = dialogsService;

		}

		#endregion

		#region Implemented abstract members of BaseView

		protected override void ImplementCommands()
		{
			_loadTickets = new MvxCommand(LoadTickets);
			_ticketClickedCommand = new MvxCommand<ZendeskApi_v2.Models.Tickets.Ticket>(OpenTicket);
			_newTicket = new MvxCommand(NewTicket);

		}
		void NewTicket()
		{
			ShowViewModel<TicketdetailsViewModel>(new { ticketSubject ="New Ticket" });
		}

		void OpenTicket(ZendeskApi_v2.Models.Tickets.Ticket item)
		{
			ShowViewModel<TicketdetailsViewModel>(new {  ticketId = item.Id.Value, ticketSubject = item.Subject });
		}
		#endregion

		#region commands implementation


	
		public async void LoadTickets()
		{
			_dialogsService.ShowLoadingModal(ResourceStrings.Instance.GetString("loadTickets"));
			var identity = await FirstViewModel.Instance.Users.GetCurrentUserAsync();
			var id = identity.User.Id.Value;

			var tickets = await FirstViewModel.Instance.Tickets.GetTicketsByUserIDAsync(id);
			var ticList = new List<ZendeskApi_v2.Models.Tickets.Ticket>();

			foreach (var item in tickets.Tickets)
			{
				ticList.Add(item);
			}
			Tickets = ticList;
			_dialogsService.HideLoadingModal();

		}


		#endregion
	}
}
