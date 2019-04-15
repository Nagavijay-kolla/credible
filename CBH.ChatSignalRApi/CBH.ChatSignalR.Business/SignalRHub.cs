using CBH.ChatSignalR.Domain;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBH.ChatSignalR.Business
{
    public class SignalRHub : Hub
    {
        //TODO: IF needed add logger to hub class
        public async Task Subscribe(string ThreadID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ThreadID);
        }
		
        public void SubscribeList(List<string> ThreadIDs)
        {
            ThreadIDs.ForEach(async ThreadID =>
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, ThreadID);
            });

        }
		
        public async Task UnSubscribe(string ThreadID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ThreadID);
        }
		
        public void UnSubscribeList(List<string> ThreadIDs)
        {

            ThreadIDs.ForEach(async ThreadID =>
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, ThreadID);
            });
        }
		
        public async Task Publish(string ThreadID, ChatMessage chatMessage)
        {
            await Clients.Group(ThreadID).SendAsync("ReceiveMessage", chatMessage);
        }
		
        public async Task Broadcast(BroadcastMessage broadcastMessage)
        {
            await Clients.All.SendAsync("BroadcastMessage", broadcastMessage);
        }
    }
}