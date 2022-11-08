using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Linq.Expressions;
using SuperSocket.Common;

namespace ChessIO.ws
{
    public class WebChess : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            //Console.WriteLine("[Connected]: " + ID);
            var currentid = ID;
            Server.Players.Add(new Player(currentid));
            Server.SendMessage(ID, JsonConvert.SerializeObject(new Message() { Opcode=0, Playerid=ID}));

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Recieved message: " + e.Data);
            //SendToAll(e.Data);
            
            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            if (d.Opcode == 5)
            {
                Console.WriteLine("Client moved: message from client");
                Console.WriteLine("Fen: "+ d.Fen);
                Console.WriteLine("PlayerId: " + d.Playerid);
                Console.WriteLine("GameId: " + d.Gameid);
                foreach (var item in Server.Games)
                {
                    if (item.Id == d.Gameid)
                    {
                        Console.WriteLine(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = d.Fen });
                        item.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = d.Fen });
                    }
                }
            }
            
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
       

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("[Disconnected] :" + ID);
            Server.Players.Remove(Server.Players.FirstOrDefault(x => x.Id == ID));

        }

    }
}
