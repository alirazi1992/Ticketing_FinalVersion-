using Ticketing.Api.Contracts.Requests;
using Ticketing.Api.Data;
using Ticketing.Api.Models;

namespace Ticketing.Api.Services;

public sealed class TicketService : ITicketService
{
    private readonly InMemoryTicketStore _store;

    public TicketService(InMemoryTicketStore store)
    {
        _store = store;
    }

    public IEnumerable<Ticket> GetTickets() => _store.Tickets;

    public Ticket? GetTicket(Guid id) => _store.Get(id);

    public Ticket CreateTicket(CreateTicketRequest request)
    {
        ValidateRequired(request.Title, nameof(request.Title));
        ValidateRequired(request.Description, nameof(request.Description));
        ValidateRequired(request.Category, nameof(request.Category));
        ValidateRequired(request.CreatedBy, nameof(request.CreatedBy));

        var ticket = new Ticket
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Subcategory = request.Subcategory,
            Priority = request.Priority,
            CreatedBy = request.CreatedBy
        };

        return _store.Add(ticket);
    }

    public Ticket? AssignTicket(Guid id, string assignedTo)
    {
        ValidateRequired(assignedTo, nameof(assignedTo));

        return _store.Update(id, ticket =>
        {
            ticket.AssignedTo = assignedTo;
            ticket.Status = ticket.Status == TicketStatus.New ? TicketStatus.InProgress : ticket.Status;
            ticket.UpdatedAt = DateTimeOffset.UtcNow;
            return ticket;
        });
    }

    public Ticket? AddReply(Guid id, AddReplyRequest request)
    {
        ValidateRequired(request.AuthorRole, nameof(request.AuthorRole));
        ValidateRequired(request.Content, nameof(request.Content));

        return _store.Update(id, ticket =>
        {
            ticket.Messages.Add(new TicketMessage
            {
                AuthorRole = request.AuthorRole,
                Content = request.Content,
                CreatedAt = DateTimeOffset.UtcNow
            });

            ticket.UpdatedAt = DateTimeOffset.UtcNow;
            return ticket;
        });
    }

    public Ticket? UpdateStatus(Guid id, TicketStatus status)
    {
        return _store.Update(id, ticket =>
        {
            ticket.Status = status;
            ticket.UpdatedAt = DateTimeOffset.UtcNow;
            return ticket;
        });
    }

    private static void ValidateRequired(string? value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        }
    }
}
