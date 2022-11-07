using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Runtime.CompilerServices;

namespace ChessIO.ws
{
    public class WebChess : WebSocketBehavior
    {
        public static void CreateGames()
        {/*
            while (true)
            {
                //Basing matching mechanish
                //Console.WriteLine("Online players:");
                var playersinlobby = DataManager.players.FindAll(x => x.PlayerState == PlayerState.Lobby);
                if (playersinlobby.Count >= 2)
                {
                    //Matching two player
                    playersinlobby[0].PlayerState = PlayerState.Game;
                    playersinlobby[1].PlayerState = PlayerState.Game;


                }
                else
                {
                    Console.WriteLine("Players in lobby:" + playersinlobby.Count);
                }
                Thread.Sleep(3000);
            }
            */
        }
        protected override void OnOpen()
        {
            Console.WriteLine("[Connected]: " + ID);
            var currentid = ID;
            Server.players.Add(new Player(currentid));
            Server.SendMessage(currentid, "Üdv:" + currentid);

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Recieved message: " + e.Data);
            SendToAll(e.Data);
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
            Server.players.Remove(Server.players.FirstOrDefault(x => x.Id == ID));

        }

    }
}
