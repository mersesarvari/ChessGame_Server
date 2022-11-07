using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessIO.Server.Old.Models
{
    public class _Lobby
    {
        public string LobbyId { get; set; }
        public List<_Player> Users { get; set; }
        public List<_Message> Messages { get; set; }

        public string Map;

        public _Lobby(string ownerid)
        {
            LobbyId = ownerid;
            Users = new List<_Player>();
            Messages = new List<_Message>();

        }
        //Constructor for the JSON Serialization
        [JsonConstructor]
        public _Lobby(string LobbyId, List<_Player> Users, List<_Message> Messages, string map)
        {
            this.LobbyId = LobbyId;
            this.Users = Users;
            this.Messages = Messages;
            Map = map;
        }

        public static void Create(string userid)
        {
            var alreadyexists = Server.lobbies.Where(x => x.LobbyId == userid.ToString()).FirstOrDefault();
            if (alreadyexists == null)
            {
                var newLobby = new _Lobby(userid);
                Server.lobbies.Add(newLobby);
                Console.WriteLine($"[LOBBY CREATED] : {userid}");

                foreach (var item in Server.players)
                {
                    var currentclient = Server.FindClient(item.Id);
                    if (currentclient != null)
                    {
                        Server.SendResponse(3, currentclient, JsonConvert.SerializeObject(newLobby));
                    }
                    else
                    {
                        throw new Exception("curentclient was null");
                    }
                }
            }

        }
        public static void Join(string userid, string lobbyid)
        {
            _Lobby currentlobby = Server.lobbies.Where(x => x.LobbyId.ToString() == lobbyid).FirstOrDefault();
            if (currentlobby != null)
            {

                bool alreadyadded = currentlobby.Users.Where(y => y.Id == userid).FirstOrDefault() != null;
                if (alreadyadded == false && currentlobby != null)
                {
                    currentlobby.Users.Add(Server.FindUserById(userid));
                    foreach (var item in currentlobby.Users)
                    {
                        Console.WriteLine($"[User in the current lobby]: {item.Username}");
                    }
                    Console.WriteLine($"[INFO]: {Server.FindUserById(userid).Username} Joined a lobby");
                    //SENDING RESPONSE TP ALL PLAYER IN THE LOBBY
                    foreach (var item in currentlobby.Users)
                    {
                        var currentclient = Server.FindClient(item.Id);
                        if (currentclient != null)
                        {
                            Server.SendResponse(2, currentclient, JsonConvert.SerializeObject(currentlobby));
                        }
                        else
                        {
                            throw new Exception("curentclient was null");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[Error]: User cannot join this lobby!");
                }
            }
        }
        public static void Leave(_Player user, string lobbyid)
        {
            //Ez most minden lobbyból törli az illetőt ami nem jó
            var currentlobby = Server.lobbies.Where(x => x.LobbyId.ToString() == lobbyid).FirstOrDefault();
            if (currentlobby != null)
            {
                if (currentlobby.Users.Contains(user))
                {
                    currentlobby.Users.Remove(user);
                }
                else
                {
                    throw new Exception("You cant delete user from this lobby because that user is not part of that lobby");
                }
                if (currentlobby.Users.Count == 0)
                {
                    Server.lobbies.Remove(currentlobby);
                    Console.WriteLine($"[Lobby Deleted]: ({currentlobby.LobbyId}) ");
                }
            }
            else
            {
                throw new Exception("You cant delete user from this lobby because this lobby doesnt exists");
            }
        }
        public void SetMap(string map)
        {
            Map = map;
        }
        public static void AddRealUsers(_Lobby lobby)
        {
            var old = Server.lobbies.FirstOrDefault(x => x.LobbyId == lobby.LobbyId);
            for (int i = 0; i < lobby.Users.Count; i++)
            {
                lobby.Users[i] = old.Users[i];
            }
        }
    }
}
