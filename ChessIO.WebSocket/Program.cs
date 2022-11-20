using Stockfish.NET;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ChessIO.ws
{
    internal class Program
    {
        public static bool disconnectRequested = false;

        [Obsolete]
        static void Main(string[] args)
        {
            
            Server.Instance.Start();
            Console.WriteLine("Server started on ws://localhost:5000");
            Server.Instance.AddWebSocketService<ChessServer>("/chess");
            // Create online chess machmaker
            //Thread t = new Thread(()=>Server.MatchPlayers());
            Thread t = new Thread(() => Server.CreateBotGame());
            t.Start();     
            Console.ReadKey();
            Server.Instance.Stop();
        }
    }
    
}




