using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ChessIO.ws
{
    public class Server
    {
        public static List<Player> players = new List<Player>();
        public static List<IWebSocketSession> sessions = new List<IWebSocketSession>();
        public static WebSocketServer instance = new WebSocketServer("ws://localhost:5000");

        public static void SendMessage(string id, string message)
        {
            foreach (var item in Server.instance.WebSocketServices.Hosts)
            {
                item.Sessions.SendTo(message, id);
            }
        }
    }
}
