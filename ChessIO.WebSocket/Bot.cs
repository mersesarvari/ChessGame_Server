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
        private Game game;
        private Playercolor color;

        public Bot(Game game, Playercolor color)
        {
            this.game = game;
            this.color = color;
        }
        public void Move()
        {
            var moves = game.GetPossibleMoves(color,false);
            Random random = new Random();
            var randommove=random.Next(moves.Count);
            var randommoveto = random.Next(moves[randommove].To.Count);
            game.MovePiece(moves[randommove].From, moves[randommove].To[randommoveto]);
        }
    }
}
