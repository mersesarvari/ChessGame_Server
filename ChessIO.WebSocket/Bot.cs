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
        public static IStockfish stockfish= new Stockfish.NET.Stockfish(@"Stockfish\stockfish12.exe");

            

        public static void BotMovePiece(Position oldpos, Position newpos, Game game)
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
            stockfish.SetPosition(oldzonecode.ToLower()+""+ newzonecode.ToLower());
            ;
            Console.WriteLine("Board after th Player Moved");
            Console.WriteLine(stockfish.GetBoardVisual());
            var bestMove = stockfish.GetBestMove();
            var from = Game.GetZoneName(bestMove[0].ToString() + bestMove[1].ToString());
            var to = Game.GetZoneName(bestMove[2].ToString() + bestMove[3].ToString());
            game.MovePiece(from, to);
            stockfish.SetPosition(oldzonecode.ToLower() + "" + newzonecode.ToLower(), bestMove);
            Console.WriteLine("Board after the Bot Moved");
            Console.WriteLine(stockfish.GetBoardVisual());
        }

        public static void GetBot()
        {
            string currentDirName = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo current = new DirectoryInfo(currentDirName);
            var ggparent = current.Parent.Parent.Parent.FullName+@"\Stockfish\";
            if (!File.Exists(current + @"/Stockfish/stockfish12.exe"))
            {
                File.Copy(ggparent + @"/stockfish12.exe", current.FullName);
            }
            
        }
    }
}
