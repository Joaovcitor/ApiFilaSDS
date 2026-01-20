using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.SignalR;
namespace ApiDeFilasDeAtendimento.Hubs
{
    public class QueueHub : Hub
    {
        public async Task TicketCalled(Guid unidadeId, FilaSenha ticket, List<FilaSenha> lastCalled)
        {
            await Clients.Group($"unidade-{unidadeId}")
                .SendAsync("TicketCalled", new
            {
                currentTicket = ticket,
                lastCalledTickets = lastCalled
            });
        }

        public async Task QueueUpdated(Guid unidadeId, int waitingNormal, int waitingPriority)
        {
            await Clients.Group($"unidade-{unidadeId}").SendAsync("QueueUpdated", new
            {
                waitingNormal,
                waitingPriority
            });
        }
        public async Task TicketCreated(Guid unidadeId, FilaSenha ticket)
        {
            await Clients.Group($"unidade-{unidadeId}").SendAsync("TicketCreated", ticket);
        }
        public async Task TicketChamado(FilaSenha ticket, Guid guicheId)
        {
            await Clients.Group($"guiche-{guicheId}").SendAsync("TicketChamado", ticket);
        }
        public async Task JoinUnit(Guid unidadeId)
        {
            var groupName = $"unidade-{unidadeId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task LeaveUnit(Guid unidadeId)
        {
            var groupName = $"unidade-{unidadeId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task JoinGuicheTickesInAtendimentos(Guid guicheId)
        {
            var groupName = $"guiche-{guicheId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGuicheTickesInAtendimentos(Guid guicheId)
        {
            var groupName = $"guiche-{guicheId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }

}
