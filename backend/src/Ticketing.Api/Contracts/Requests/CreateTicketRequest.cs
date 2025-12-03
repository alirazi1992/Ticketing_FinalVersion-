using Ticketing.Api.Models;

namespace Ticketing.Api.Contracts.Requests;

public sealed record CreateTicketRequest
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public string? Subcategory { get; init; }
    public TicketPriority Priority { get; init; } = TicketPriority.Medium;
    public required string CreatedBy { get; init; }
}
