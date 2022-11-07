using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessIO.Server.Old.Models
{
    public class _Game
    {
        public static void Move(string executor, string command)
        {
            /*
            Player data = JsonConvert.DeserializeObject<Player>(command);
            foreach (var item in Server.players)
            {
                var currentclient = Server.FindClient(item.Id);
                if (currentclient != null)
                {
                    Server.SendResponse(4, currentclient, JsonConvert.SerializeObject(data));
                }
                else
                {
                    throw new Exception("curentclient was null");
                }
            }
            */
        }
        public static void Start(string executor, string command)
        {
            /*
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(command);
            Console.WriteLine("Current players in game:");
            foreach (var item in lobby.Users)
            {
                Console.WriteLine($"{item.Id}");
            }
            foreach (var item in lobby.Users)
            {
                var currentclient = Server.FindClient(item.Id);
                if (currentclient != null)
                {
                    Server.SendResponse(5, currentclient, JsonConvert.SerializeObject(lobby));
                }
                else
                {
                    throw new Exception("curentclient was null");
                }
            }
            */
        }
        //executor is clientID and command is LobbyID
        public static void LeaveGame(string executor, string command)
        {
            /*
            var currentclient = Server.FindClient(executor);
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(command);
            if (lobby != null)
            {
                ;
                if (lobby.Users.Count <= 1)
                {
                    Server.lobbies.RemoveAt(0);
                    Console.WriteLine($"[Delete Lobby] : {lobby.LobbyId}");
                    Server.SendResponse(8, currentclient, lobby.LobbyId);
                    Console.WriteLine($"Current Lobby Count: {Server.lobbies.Count}");
                }
                lobby.Users.Remove(lobby.Users.FirstOrDefault(x => x.Id == currentclient.UID.ToString()));
                Server.SendResponse(6, currentclient, JsonConvert.SerializeObject(lobby));
                Console.WriteLine($"[Leave Game]: {executor} Users: {lobby.Users.Count}");

            }
            */
        }
        public static void Finished(string executor, string command)
        {
            /*
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(command);
            foreach (var item in lobby.Users)
            {
                var currentclient = Server.FindClient(item.Id);
                if (currentclient != null)
                {
                    Server.SendResponse(9, currentclient, executor);
                }
                else
                {
                    throw new Exception("curentclient was null");
                }
            }
            */
        }
    }
}
