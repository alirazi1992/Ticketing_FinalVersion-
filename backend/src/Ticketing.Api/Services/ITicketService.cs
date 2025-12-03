using Ticketing.Api.Contracts.Requests;
using Ticketing.Api.Models;

namespace Ticketing.Api.Services;

public interface ITicketService
{
    IEnumerable<Ticket> GetTickets();
    Ticket? GetTicket(Guid id);
    Ticket CreateTicket(CreateTicketRequest request);
    Ticket? AssignTicket(Guid id, string assignedTo);
    Ticket? AddReply(Guid id, AddReplyRequest request);
    Ticket? UpdateStatus(Guid id, TicketStatus status);
}
