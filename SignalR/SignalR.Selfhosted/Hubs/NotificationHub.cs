﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Selfhosted.Hubs
{
    public class NotificationHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        #region Added For Testing Purpose

        /// <summary>
        /// Class to store connections.
        /// </summary>
        public static class ConnectionHandler
        {
            /// <summary>
            /// All the live connection IDs.
            /// </summary>
            public static HashSet<string> ConnectedIds = new HashSet<string>();
        }

        /// <summary>
        /// Automatically called when connected.
        /// </summary>
        /// <returns>A completed Task.</returns>
        public override Task OnConnectedAsync()
        {
            ConnectionHandler.ConnectedIds.Add(Context.ConnectionId);
            return Task.WhenAll(
                Clients.All.SendAsync("ConnectedClientUpdated", ConnectionHandler.ConnectedIds.Count),
                base.OnConnectedAsync());
        }

        /// <summary>
        /// Automatically called when disconnected.
        /// </summary>
        /// <param name="exception">Exception generated from connection.</param>
        /// <returns>A completed Task.</returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionHandler.ConnectedIds.Remove(Context.ConnectionId);
            return Task.WhenAll(
                Clients.All.SendAsync("ConnectedClientUpdated", ConnectionHandler.ConnectedIds.Count),
                Clients.All.SendAsync("ExceptionOccured", exception?.Message),
                base.OnDisconnectedAsync(exception));
        }
        #endregion

    }
}
