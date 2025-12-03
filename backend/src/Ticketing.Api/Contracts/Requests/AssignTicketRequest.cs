namespace Ticketing.Api.Contracts.Requests;

public sealed record AssignTicketRequest
{
    public required string AssignedTo { get; init; }
}
