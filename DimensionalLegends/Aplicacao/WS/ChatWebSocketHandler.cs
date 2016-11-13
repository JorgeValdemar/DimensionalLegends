using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.WebSockets;

namespace WebSocketTurnoAcao
{
    public class ChatWebSocketHandler : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();
        private Chat chatContent;

        public override void OnOpen()
        {
            //this.name = this.WebSocketContext.QueryString["name"];
            clients.Add(this);
            clients.Broadcast(" has connected.");
        }

        public override void OnMessage(string message)
        {
            clients.Broadcast(string.Format("{0} said: {1}", "nome", message));
        }

        public override void OnClose()
        {
            clients.Remove(this);
            clients.Broadcast(string.Format("{0} has gone away.", "nome"));
        }

        public override void OnError()
        {
            base.OnError();
        }

    }
}
