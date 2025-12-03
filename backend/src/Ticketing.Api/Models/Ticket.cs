namespace Ticketing.Api.Models;

public sealed class Ticket
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public string? Subcategory { get; set; }
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public required string CreatedBy { get; init; }
    public string? AssignedTo { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<TicketMessage> Messages { get; } = new();
}
