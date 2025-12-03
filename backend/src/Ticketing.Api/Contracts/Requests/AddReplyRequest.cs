namespace Ticketing.Api.Contracts.Requests;

public sealed record AddReplyRequest
{
    public required string AuthorRole { get; init; }
    public required string Content { get; init; }
}
