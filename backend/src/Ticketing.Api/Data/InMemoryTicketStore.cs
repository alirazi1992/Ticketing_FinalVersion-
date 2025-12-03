using Ticketing.Api.Models;

namespace Ticketing.Api.Data;

public sealed class InMemoryTicketStore
{
    private readonly List<Ticket> _tickets = new();
    private readonly object _lock = new();

    public InMemoryTicketStore()
    {
        Seed();
    }

    public IReadOnlyCollection<Ticket> Tickets
    {
        get
        {
            lock (_lock)
            {
                return _tickets
                    .Select(ticket => Clone(ticket))
                    .ToList();
            }
        }
    }

    public Ticket Add(Ticket ticket)
    {
        lock (_lock)
        {
            _tickets.Add(ticket);
            return Clone(ticket);
        }
    }

    public Ticket? Update(Guid id, Func<Ticket, Ticket> update)
    {
        lock (_lock)
        {
            var ticket = _tickets.SingleOrDefault(t => t.Id == id);
            if (ticket is null)
            {
                return null;
            }

            var updatedTicket = update(ticket);
            return Clone(updatedTicket);
        }
    }

    public Ticket? Get(Guid id)
    {
        lock (_lock)
        {
            var ticket = _tickets.SingleOrDefault(t => t.Id == id);
            return ticket is null ? null : Clone(ticket);
        }
    }

    private static Ticket Clone(Ticket ticket) => new()
    {
        Id = ticket.Id,
        Title = ticket.Title,
        Description = ticket.Description,
        Category = ticket.Category,
        Subcategory = ticket.Subcategory,
        Priority = ticket.Priority,
        Status = ticket.Status,
        CreatedBy = ticket.CreatedBy,
        AssignedTo = ticket.AssignedTo,
        CreatedAt = ticket.CreatedAt,
        UpdatedAt = ticket.UpdatedAt,
        Messages = ticket.Messages.Select(message => new TicketMessage
        {
            AuthorRole = message.AuthorRole,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        }).ToList()
    };

    private void Seed()
    {
        _tickets.AddRange(new[]
        {
            new Ticket
            {
                Title = "VPN connection is unstable",
                Description = "Intermittent disconnects when connecting from home office.",
                Category = "Networking",
                Subcategory = "VPN",
                Priority = TicketPriority.High,
                Status = TicketStatus.InProgress,
                CreatedBy = "client.mina",
                AssignedTo = "tech.omid",
                Messages =
                {
                    new TicketMessage
                    {
                        AuthorRole = "Client",
                        Content = "VPN drops every 15 minutes, need stable access.",
                        CreatedAt = DateTimeOffset.UtcNow.AddHours(-5)
                    },
                    new TicketMessage
                    {
                        AuthorRole = "Technician",
                        Content = "Investigating server logs, please share ISP details.",
                        CreatedAt = DateTimeOffset.UtcNow.AddHours(-3)
                    }
                }
            },
            new Ticket
            {
                Title = "Email inbox is full",
                Description = "Unable to receive new messages due to storage limit reached.",
                Category = "Productivity",
                Subcategory = "Email",
                Priority = TicketPriority.Medium,
                Status = TicketStatus.WaitingForCustomer,
                CreatedBy = "client.kourosh",
                AssignedTo = "tech.sara",
                Messages =
                {
                    new TicketMessage
                    {
                        AuthorRole = "Technician",
                        Content = "Removed outdated attachments. Please confirm if space is sufficient now.",
                        CreatedAt = DateTimeOffset.UtcNow.AddHours(-2)
                    }
                }
            },
            new Ticket
            {
                Title = "Laptop fan noise",
                Description = "Device is overheating after the latest update.",
                Category = "Hardware",
                Subcategory = "Laptop",
                Priority = TicketPriority.Low,
                Status = TicketStatus.New,
                CreatedBy = "client.reza"
            }
        });
    }
}
