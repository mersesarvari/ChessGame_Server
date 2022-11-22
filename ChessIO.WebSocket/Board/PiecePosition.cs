using ChessIO.ws.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws.Board
{
    public class PiecePosition
    {
        public Position Position { get; set; }
        public string Piece { get; set; }
        public Playercolor Color { get; set; }

        public PiecePosition(Position position, string piece, Playercolor color)
        {
            Position = position;
            Piece = piece;
            Color = color;
        }

        public void ChangePosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public bool IsEquals(Position pos)
        {
            if (this.Position.X == pos.X && Position.Y == pos.Y)
                return true;
            else
            return false;
        }



        
    }
}
