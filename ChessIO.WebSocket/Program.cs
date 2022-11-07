using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ChessIO.ws
{
    internal class Program
    {        

        static void Main(string[] args)
        {            
            WebSocketServer server = new WebSocketServer("ws://localhost:5000");
            server.AddWebSocketService<ChessServer>("/chess");
            server.Start();
            Console.WriteLine("Server started on ws://localhost:5000");
            Console.ReadKey();
            server.Stop();
        }
    }
    public class ChessServer : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Console.WriteLine("Client connected");
            Console.WriteLine("Active clients:");
            var currentid = ID;
            DataManager.players.Add(new Player(currentid));
            SendMessage(currentid, "Üdv:"+currentid);
            
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Recieved message: " + e.Data);
            SendToAll(e.Data);
        }
        public void SendToAll(string message)
        {
            Sessions.Broadcast(message);
        }
        public void SendToClient(string message)
        {
            Send(message);
        }
        public void SendToSpecificClients()
        {
            foreach (var item in Sessions.ActiveIDs)
            {
                Console.WriteLine(item);
            }
        }

        public void SendMessage(string id, string message)
        {
            var current = Sessions.Sessions.FirstOrDefault(x => x.ID == id.ToString());
            Sessions.SendTo(message,current?.ID);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Active clients:");
            foreach (var item in Sessions.Sessions)
            {
                Console.WriteLine(item.ID + ": " + item.State);
            }
            
        }
        
    }
}




