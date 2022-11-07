using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ChessIO.ws
{
    internal class Program
    {
        public static readonly WebSocketServer server = new WebSocketServer("ws://localhost:5000");
        public static bool disconnectRequested = false;
        static void Main(string[] args)
        {
            server.Start();
            Console.WriteLine("Server started on ws://localhost:5000");
            server.AddWebSocketService<WebChess>("/chess");
            while (true)
            {
                //Basing matching mechanish
                //Console.WriteLine("Online players:");
                var playersinlobby = DataManager.players.FindAll(x => x.PlayerState == PlayerState.Lobby);
                if (playersinlobby.Count >= 2)
                {
                    //Matching two player
                    playersinlobby[0].PlayerState = PlayerState.Game;
                    playersinlobby[1].PlayerState = PlayerState.Game;
                    //chess.SendMessage(playersinlobby[0].Id,"You found a game");
                    //chess.SendMessage(playersinlobby[1].Id, "You found a game");

                }
                else
                {
                    Console.WriteLine("Players in lobby:" + playersinlobby.Count);
                }
                Thread.Sleep(3000);
            }
            server.Stop();
        }
    }
    
}




