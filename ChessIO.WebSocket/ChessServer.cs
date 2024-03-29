﻿using System;
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
using System.Collections;
using System.ComponentModel;
using System.Security;

namespace ChessIO.ws
{
    public class ChessServer : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            //Console.WriteLine("[Connected]: " + ID);
            var currentid = ID;
            Server.Players.Add(new Player(currentid));
            Console.WriteLine($"[connected]: {currentid}");
            Server.SendMessage(ID, JsonConvert.SerializeObject(new Message() { Opcode = 0, Playerid = ID }));

        }
        protected override void OnMessage(MessageEventArgs e)
        {

            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            //Movement comand from the client in an Online Game
            if (d.Opcode == 5)
            {
                
                //Have to add logic to check that the move vas legal                
                var currentgame = Server.Games.FirstOrDefault(x => x.Id == d.Gameid);
                if (d.Playerid == currentgame.ActivePlayerId && currentgame.Gametype == GameType.Multiplayer)
                {
                    //Logic.IsCheck();
                    var oldpos = new Position(d.OldcoordX, d.OldcoordY);
                    var newpos = new Position(d.NewcoordX, d.NewcoordY);
                    //Console.Clear();
                    //var ischeck = Logic.IsMoveCheck(oldpos, newpos, currentgame.Board, currentgame.ActiveColor);
                    var isvalid = Logic.IsValidMove(oldpos, newpos, currentgame.Board, currentgame.ActiveColor,true);
                    if (isvalid)
                    {
                        //Moving on the board
                        currentgame.MovePiece(oldpos, newpos);                        
                        currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                        currentgame.TurnChanger();
                        //Checking checkmates
                        var cm = Logic.IsCheckMate(currentgame.Board, currentgame.ActiveColor, true);
                        if(cm)
                        {
                            //Sending message to the winning player
                            currentgame.SendMessage(new Message() { Opcode = 8, message = "You Won! Congratulation" }, currentgame.InactiveColor());
                            //Sending message to the loosing player
                            currentgame.SendMessage(new Message() { Opcode = 8, message = "You lost! Better luck next time" }, currentgame.ActiveColor);
                        }
                        else
                        {

                            //Sending list of possible moves to the next player
                            var playermoves = currentgame.GetPlayerMoves(currentgame.ActiveColor, true);
                            var wmovemsg = new Message() { Opcode = 6, Custom = playermoves };
                            Server.SendMessage(currentgame.ActivePlayerId, JsonConvert.SerializeObject(wmovemsg));
                        }
                    }
                    else 
                    {
                        currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                    }
                }
                if (d.Playerid == currentgame.ActivePlayerId && currentgame.Gametype == GameType.Singleplayer)
                {
                    Console.WriteLine("Server recieved a move command");
                    //Logic.IsCheck();
                    var oldpos = new Position(d.OldcoordX, d.OldcoordY);
                    var newpos = new Position(d.NewcoordX, d.NewcoordY);
                    //Console.Clear();
                    //var ischeck = Logic.IsMoveCheck(oldpos, newpos, currentgame.Board, currentgame.ActiveColor);
                    var isvalid = Logic.IsValidMove(oldpos, newpos, currentgame.Board, currentgame.ActiveColor, true);
                    if (isvalid)
                    {
                        //Moving on the board
                        currentgame.bot.AddMoveToList(oldpos, newpos);
                        currentgame.MovePiece(oldpos, newpos);
                        //Adding Bot logic to here
                        currentgame.TurnChanger();
                        currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                        Console.Clear();
                        Game.DrawBoard(currentgame.Board);

                        if (currentgame.ActivePlayerId == "Bot")
                        {
                            var botmove = currentgame.bot.testgame();
                            var a = currentgame.moveList;

                            var move = Logic.ConvertPositionToMatrix(botmove);
                            currentgame.MovePiece(move.Item1, move.Item2);
                            currentgame.TurnChanger();
                            currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                            Console.Clear();
                            Game.DrawBoard(currentgame.Board);
                        }
                        

                        //Checking checkmates
                        var cm = Logic.IsCheckMate(currentgame.Board, currentgame.ActiveColor, true);
                        if (cm)
                        {
                            //Sending message to the winning player
                            currentgame.SendMessage(new Message() { Opcode = 8, message = "You Won! Congratulation" }, currentgame.InactiveColor());
                            //Sending message to the loosing player
                            currentgame.SendMessage(new Message() { Opcode = 8, message = "You lost! Better luck next time" }, currentgame.ActiveColor);
                        }
                        else
                        {

                            //Sending list of possible moves to the next player
                            var playermoves = currentgame.GetPlayerMoves(currentgame.ActiveColor, true);
                            var wmovemsg = new Message() { Opcode = 6, Custom = playermoves };
                            Server.SendMessage(currentgame.ActivePlayerId, JsonConvert.SerializeObject(wmovemsg));
                        }
                    }
                    else
                    {
                        currentgame.BroadcastMessage(new Message() { Opcode = 5, Gameid = d.Gameid, Playerid = d.Playerid, Fen = currentgame.Fenstring });
                    }
                }


            }
            //Moving in an offline game
            if (d.Opcode == 15)
            {

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
