using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.SignalR;
namespace ApiDeFilasDeAtendimento.Hubs
{
    public class QueueHub : Hub
    {
        public async Task TicketCalled(FilaSenha ticket, List<FilaSenha> lastCalled)
        {
            await Clients.All.SendAsync("TicketCalled", new
            {
                currentTicket = ticket,
                lastCalledTickets = lastCalled
            });
        }

        public async Task QueueUpdated(int waitingNormal, int waitingPriority)
        {
            await Clients.All.SendAsync("QueueUpdated", new
            {
                waitingNormal,
                waitingPriority
            });
        }
        public async Task TicketCreated(FilaSenha ticket)
        {
            await Clients.All.SendAsync("TicketCreated", ticket);
        }
    }

}
