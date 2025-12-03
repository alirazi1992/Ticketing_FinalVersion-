namespace Ticketing.Api.Models;

public sealed record TicketMessage
{
    public required string AuthorRole { get; init; }
    public required string Content { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
