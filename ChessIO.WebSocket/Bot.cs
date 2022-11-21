using ChessIO.ws.Board;
using ChessIO.ws.Legacy;
using Stockfish.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public class Bot
    {
        public IStockfish stockfish;
        public Bot()
        {
            GetBot();
            stockfish = new Stockfish.NET.Stockfish(@"Stockfish\stockfish12.exe");
        }
        public void BotMovePiece(Position oldpos, Position newpos)
        {
            for (int i = 0; i < Game.Zones.GetLength(0); i++)
            {
                for (int j = 0; j < Game.Zones.GetLength(1); j++)
                {
                    Console.Write(Game.Zones[i,j]+" ");
                }
                Console.WriteLine();
            }
            var oldzonecode = Game.Zones[oldpos.X, oldpos.Y];
            var newzonecode = Game.Zones[newpos.X, newpos.Y];
        }

        public void GetBot()
        {
            string currentDirName = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo current = new DirectoryInfo(currentDirName);
            var ggparent = current.Parent.Parent.Parent.FullName+@"\Stockfish\stockfish12.exe";
            if (!File.Exists(current + @"/Stockfish/stockfish12.exe"))
            {
                File.Copy(ggparent.ToString(), current.FullName+@"\Stockfish\stockfish12.exe");
            }
            
        }
    }
}
