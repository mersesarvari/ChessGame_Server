using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws.Board
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsValid()
        {
            if (X == -1 || Y == -1)
                return false;
            else return true;
        }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
