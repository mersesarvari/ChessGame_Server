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
            stockfish.SetPosition(oldzonecode+ newzonecode);
            ;
            var bestMove = stockfish.GetBestMove();
            var from = Game.GetZoneName(bestMove[0].ToString() + bestMove[1].ToString());
            var to = Game.GetZoneName(bestMove[2].ToString() + bestMove[3].ToString());
            game.MovePiece(from, to);
            stockfish.SetPosition(bestMove);
            Console.WriteLine(stockfish.GetBoardVisual());
        }
    }
}
