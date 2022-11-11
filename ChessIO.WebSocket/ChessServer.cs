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
    public class ChessServer : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            //Console.WriteLine("[Connected]: " + ID);
            var currentid = ID;
            Server.Players.Add(new Player(currentid));
            Server.SendMessage(ID, JsonConvert.SerializeObject(new Message() { Opcode = 0, Playerid = ID }));

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            //Movement comand from the client
            if (d.Opcode == 5)
            {
                //Have to add logic to check that the move vas legal                
                var currentgame = Server.Games.FirstOrDefault(x => x.Id == d.Gameid);
                if (d.Playerid == currentgame.ActivePlayerId)
                {
                    //Logic.IsCheck();
                    var oldpos = new Position(d.OldcoordX, d.OldcoordY);
                    var newpos = new Position(d.NewcoordX, d.NewcoordY);
                    var valid = Logic.IsValidMove(oldpos, newpos, currentgame);
                    Console.Clear();
                    if (currentgame.MovePiece(oldpos, newpos))
                    {
                        Console.WriteLine($"[Valid]: moving from [{oldpos.X},{oldpos.Y}] to [{newpos.X},{newpos.Y}]");
                        currentgame.TurnChanger();
                        currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                        if (currentgame.IsSomeoneLost())
                        {
                            //Sending message to the winning player
                            currentgame.SendMessage(new Message() { Opcode = 8, message = "You Won! Congratulation" }, currentgame.GetInactiveColor());
                            //Sending message to the loosing player
                            currentgame.SendMessage(new Message() { Opcode = 8, message = "You lost! Better luck next time" }, currentgame.ActiveColor);
                        }
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
