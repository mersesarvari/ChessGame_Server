using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ChessIO.ws
{
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public string Color { get; set; }
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
        }
    }
}
