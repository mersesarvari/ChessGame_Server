using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
            //Ha két player van akkor kiküldünk egy gamet
            while (true)
            {
                //Basing matching mechanish
                Console.WriteLine("Online players:");
                var playersinlobby = DataManager.players.FindAll(x => x.PlayerState == PlayerState.Lobby);
                if (playersinlobby.Count >= 2)
                {
                    //Matching two player
                    playersinlobby[0].PlayerState = PlayerState.Game;
                    playersinlobby[1].PlayerState = PlayerState.Game;
                }
                Thread.Sleep(3000);
            }
            
            server.Stop();
        }
    }
    public class ChessServer : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Console.WriteLine("[Connected]: "+ID);
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
            Console.WriteLine("[Disconnected] :"+ID);
            DataManager.players.Remove(DataManager.players.FirstOrDefault(x=>x.Id==ID));
            
        }
        
    }
}




