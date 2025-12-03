using Ticketing.Api.Models;

namespace Ticketing.Api.Contracts.Responses;

public sealed record TicketResponse
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public string? Subcategory { get; init; }
    public TicketPriority Priority { get; init; }
    public TicketStatus Status { get; init; }
    public required string CreatedBy { get; init; }
    public string? AssignedTo { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public required IReadOnlyCollection<TicketMessage> Messages { get; init; }

    public static TicketResponse FromDomain(Ticket ticket) => new()
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
        Messages = ticket.Messages.AsReadOnly()
    };
}
