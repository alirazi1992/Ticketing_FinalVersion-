using Ticketing.Api.Models;

namespace Ticketing.Api.Contracts.Requests;

public sealed record UpdateStatusRequest
{
    public TicketStatus Status { get; init; } = TicketStatus.New;
}
