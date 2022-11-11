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
            Server.MatchPlayers();
            var board = Logic.ConvertFromFen("8/8/8/3R4/3R4/8/8/8");
            //Logic.ConvertToFen();
            //Testing
            //Console.WriteLine($"Character pos:[{4},{3}]");
            //Tester.TestRook("8/8/8/3R4/3R4/8/8/8",4,3);
            Console.ReadKey();
            Server.Instance.Stop();
        }
    }
    
}




