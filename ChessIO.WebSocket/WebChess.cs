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
            //Console.WriteLine("Recieved message: " + e.Data);
            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            if (d.Opcode == 5)
            {
                //Have to add logic to check that the move vas legal                
                var currentgame = Server.Games.FirstOrDefault(x => x.Id == d.Gameid);
                //Checking the current moves validity
                var oldpos = new Position(d.OldcoordX, d.OldcoordY);
                var newpos = new Position(d.NewcoordX, d.NewcoordY);
                var valid = Logic.IsValidMove(oldpos, newpos,currentgame.Board);
                //If move is valid we have to set the board
                if (valid)
                {
                    currentgame.MovePiece(oldpos, newpos);
                    currentgame.DrawBoard();
                    Console.WriteLine("Sending players this: "+currentgame.Fenstring);
                    currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                }
                else
                {
                    Console.WriteLine("Invalid movement happened");
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
