﻿using Stockfish.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public class Bot
    {
        public IStockfish stockfish = new Stockfish.NET.Stockfish(@"Stockfish\stockfish12.exe");
        public Game currentGame { get; set; }

        public Bot(Game game)
        {
            currentGame= game;
        }


        public void AddMoveToList(Position oldpos, Position newpos) {
            var oldzone = Game.Zones[oldpos.X, oldpos.Y].ToLower();
            var newzone = Game.Zones[newpos.X, newpos.Y].ToLower();
            Console.WriteLine(oldzone + "" + newzone+" added to the list.");
            currentGame.moveList.Add(oldzone+""+newzone);
        }
        public void AddMoveToList(string zone)
        {
            Console.WriteLine(zone + " added to the list.");
            currentGame.moveList.Add(zone);
        }

        //public void BotMovePiece(Position oldpos, Position newpos, Game game)
        //{
        //    for (int i = 0; i < Game.Zones.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < Game.Zones.GetLength(1); j++)
        //        {
        //            Console.Write(Game.Zones[i,j]+" ");
        //        }
        //        Console.WriteLine();
        //    }
        //    var oldzonecode = Game.Zones[oldpos.X, oldpos.Y];
        //    var newzonecode = Game.Zones[newpos.X, newpos.Y];
        //    this.stockfish.SetPosition(oldzonecode.ToLower()+""+ newzonecode.ToLower());
        //    var bestMove = stockfish.GetBestMove();
        //    var from = Game.GetZoneName(bestMove[0].ToString() + bestMove[1].ToString());
        //    var to = Game.GetZoneName(bestMove[2].ToString() + bestMove[3].ToString());
        //    game.MovePiece(from, to);
        //    stockfish.SetPosition(oldzonecode.ToLower() + "" + newzonecode.ToLower(), bestMove);
        //}
        public void MoveBot()
        {
            var a = testgame();

            var move = Logic.ConvertPositionToMatrix(a);
            currentGame.MovePiece(move.Item1, move.Item2);
            currentGame.TurnChanger();
            currentGame.BroadcastMessage(new Message() { Opcode = 5, Gameid = currentGame.Id, Playerid = currentGame.ActivePlayerId, Fen = currentGame.Fenstring });
        }
        public async static void GetBot()
        {
            string currentDirName = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo current = new DirectoryInfo(currentDirName);
            var ggparent = current.Parent.Parent.Parent.FullName+@"\Stockfish\";
            if (!Directory.Exists(@"Stockfish"))
                Directory.CreateDirectory(@"Stockfish");
            if (!File.Exists(current + @"/Stockfish/stockfish12.exe"))
            {
                File.Copy(ggparent + @"/stockfish12.exe", current.FullName+ @"/Stockfish/stockfish12.exe");
            }
        }


        public string testgame()
        {
            stockfish.SetPosition(currentGame.moveList.ToArray());
            string bestMove = stockfish.GetBestMove();
            currentGame.moveList.Add(bestMove);
            return bestMove;
        }
    }
}
