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
            Server.Instance.AddWebSocketService<WebChess>("/chess");
            Server.MatchPlayers();

            //Tasting
            
            //Tester.TestBishop("8/8/2B5/3b4/4b3/8/8/8",3,3);
            Console.ReadKey();
            Server.Instance.Stop();
        }
    }
    
}




