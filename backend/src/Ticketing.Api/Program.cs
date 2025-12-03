using Ticketing.Api.Contracts.Requests;
using Ticketing.Api.Contracts.Responses;
using Ticketing.Api.Models;
using Ticketing.Api.Services;
using Ticketing.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<InMemoryTicketStore>();
builder.Services.AddSingleton<ITicketService, TicketService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/tickets", (ITicketService service) =>
{
    var tickets = service.GetTickets()
        .Select(TicketResponse.FromDomain);

    return Results.Ok(tickets);
});

app.MapGet("/api/tickets/{id:guid}", (Guid id, ITicketService service) =>
{
    var ticket = service.GetTicket(id);
    return ticket is null
        ? Results.NotFound()
        : Results.Ok(TicketResponse.FromDomain(ticket));
});

app.MapPost("/api/tickets", (CreateTicketRequest request, ITicketService service) =>
{
    var created = service.CreateTicket(request);
    return Results.Created($"/api/tickets/{created.Id}", TicketResponse.FromDomain(created));
});

app.MapPost("/api/tickets/{id:guid}/assign", (Guid id, AssignTicketRequest request, ITicketService service) =>
{
    var updated = service.AssignTicket(id, request.AssignedTo);
    return updated is null
        ? Results.NotFound()
        : Results.Ok(TicketResponse.FromDomain(updated));
});

app.MapPost("/api/tickets/{id:guid}/reply", (Guid id, AddReplyRequest request, ITicketService service) =>
{
    var updated = service.AddReply(id, request);
    return updated is null
        ? Results.NotFound()
        : Results.Ok(TicketResponse.FromDomain(updated));
});

app.MapPost("/api/tickets/{id:guid}/status", (Guid id, UpdateStatusRequest request, ITicketService service) =>
{
    var updated = service.UpdateStatus(id, request.Status);
    return updated is null
        ? Results.NotFound()
        : Results.Ok(TicketResponse.FromDomain(updated));
});

app.Run();
