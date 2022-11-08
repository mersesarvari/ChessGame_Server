using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ChessIO.ws
{
    public enum PlayerState
    {
        Lobby=0,
        //Search=1,
        Game=2,
    }
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public PlayerState PlayerState{get;set;}
        public string Color { get; set; }

        public string GameId { get; set; }

        public int Rating { get; set; }
        /*
        public Player(Socket client)
        {
            Id = client.UID.ToString();
            Username = client.Username;
            Color = client.Color;
        }
        */
        [JsonConstructor]
        public Player(string id, string username)
        {
            Id = id;
            Username = username;
        }
        [JsonConstructor]
        public Player(string id)
        {
            Id = id;
            Username = "default";
            PlayerState = PlayerState.Lobby;
            Rating = 300;
        }

        public void SearchGame() {
            this.PlayerState = PlayerState.Game;
        }

        public void SendMessage(string message) 
        {
            Server.SendMessage(this.Id, message);
        }

        public static Player FindPlayer(string id)
        {
            return Server.Players.FirstOrDefault(x => x.Id == id);
        }
    }
}
