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
            //stockfish.SetPosition(oldzonecode+""+ newzonecode);
            stockfish.SetPosition("e2e4");
            Console.WriteLine(stockfish.GetBoardVisual());
            
            /*
            Console.WriteLine("Board after th Player Moved");
            Console.WriteLine(stockfish.GetBoardVisual());
            var bestMove = stockfish.GetBestMove();
            var from = Game.GetZoneName(bestMove[0].ToString() + bestMove[1].ToString());
            var to = Game.GetZoneName(bestMove[2].ToString() + bestMove[3].ToString());
            stockfish.SetPosition(oldzonecode.ToLower() + "" + newzonecode.ToLower(), bestMove);
            Console.WriteLine("Board after the Bot Moved");
            Console.WriteLine(stockfish.GetBoardVisual());
            */
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
