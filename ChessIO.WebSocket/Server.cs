﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ChessIO.ws
{
    public class Server
    {
        public static WebSocketServer Instance = new WebSocketServer("ws://localhost:5000");
        public static List<Player> Players = new List<Player>();
        public static List<IWebSocketSession> Sessions = new List<IWebSocketSession>();
        public static List<Game> Games = new List<Game>();
        

        public static void SendMessage(string id, string message)
        {
            foreach (var item in Server.Instance.WebSocketServices.Hosts)
            {   if(id!="Bot")
                item.Sessions.SendTo(message, id);
            }
        }
        public static void Broadcast(string message)
        {
            foreach (var item in Server.Instance.WebSocketServices.Hosts)
            {
                item.Sessions.Broadcast(message);
            }
        }

        public static void MatchPlayers() 
        {
            while (true)
            {
                //Getting players in the lobby
                var playersinlobby = Server.Players.FindAll(x => x.PlayerState == PlayerState.Lobby);
                if (playersinlobby.Count >= 2)
                {
                    //Matching two player

                    playersinlobby[0].PlayerState = PlayerState.Game;
                    playersinlobby[1].PlayerState = PlayerState.Game;
                    //Basic 10minute game
                    Game currentgame = new Game(playersinlobby[0], playersinlobby[1], 600000);
                    
                    Console.WriteLine("[Game created]:" + currentgame.Id);
                    currentgame.StartGame();
                    Server.Games.Add(currentgame);
                }
                else
                {
                    //Console.WriteLine("Players in lobby:" + playersinlobby.Count);
                }
                Thread.Sleep(3000);
            }
        }
        public static Game CreateBotGame()
        {
            Console.WriteLine("Create Bot Game method is running");
            while (true)
            {
                var playersinlobby = Server.Players.FindAll(x => x.PlayerState == PlayerState.Lobby);
                if (playersinlobby.Count > 0)
                {
                    //Matching two player

                    playersinlobby[0].PlayerState = PlayerState.Game;
                    //Basic 10minute game
                    Game currentgame = new Game(playersinlobby[0], 600000);


                    Console.WriteLine("[Bot Game created]:" + currentgame.Id);
                    currentgame.StartGame();
                    Server.Games.Add(currentgame);
                }
                else
                {
                    //Console.WriteLine("Players in lobby:" + playersinlobby.Count);
                }
                Thread.Sleep(3000);
            }
            
        }
    }
}
