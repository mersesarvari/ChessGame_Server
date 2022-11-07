using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessIO.Server.Old.Models
{
    public class _Message
    {
        public string ownerid { get; set; }
        public string message { get; set; }

        public _Message(string ownerid, string message)
        {
            this.ownerid = ownerid;
            this.message = message;
        }
        //Executor is the lobby here
        public static void SendMessageToLobby(string executor, string command)
        {
            var currentlobby = Server.lobbies.Where(x => x.LobbyId == executor).FirstOrDefault();
            _Message message = JsonConvert.DeserializeObject<_Message>(command);
            currentlobby.Messages.Add(message);
            foreach (var item in currentlobby.Users)
            {
                var currentclient = Server.FindClient(item.Id);
                if (currentclient != null)
                {
                    Server.SendResponse(7, currentclient, JsonConvert.SerializeObject(message));
                    Console.WriteLine("Send message server answer");
                }
                else
                {
                    throw new Exception("curentclient was null");
                }
            }

        }
    }
}
