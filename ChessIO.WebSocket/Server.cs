﻿using ChessIO.ws.Logic;
using System;
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
        public static List<ChessGame> Games = new List<ChessGame>();
        

        public static void SendMessage(string id, string message)
        {
            foreach (var item in Server.Instance.WebSocketServices.Hosts)
            {
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
            //Testing pahse
            PieceLogic pl = new PieceLogic();
            char[][] brd = pl.ConvertFromFEN("rnbqkbnr/pppppppp/8/8/1P6/P4N2/2PPPPPP/RNBQKB1R");
            for (int i = 0; i < brd.Length; i++)
            {
                for (int j = 0; j < brd[i].Length; j++)
                {
                    Console.Write(brd[i][j] + " ");
                }
                Console.WriteLine();
            }
            while (true)
            {
                //Getting players in the lobby
                var playersinlobby = Server.Players.FindAll(x => x.PlayerState == PlayerState.Lobby);
                if (playersinlobby.Count >= 2)
                {
                    //Matching two player

                    playersinlobby[0].PlayerState = PlayerState.Game;
                    playersinlobby[1].PlayerState = PlayerState.Game;
                    //Server.SendMessage(playersinlobby[0].Id, "You found a game");
                    //Server.SendMessage(playersinlobby[1].Id, "You found a game");
                    //Server.Broadcast("Welcome");
                    //Basic 10minute game
                    ChessGame currentgame = new ChessGame(playersinlobby[0], playersinlobby[1], 600000);
                    Server.Games.Add(currentgame);
                    currentgame.StartGame();
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
