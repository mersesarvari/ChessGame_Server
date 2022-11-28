using ChessIO.ws.Board;
using ChessIO.ws.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public class Possiblemoves
    {
        public string FromChar { get; set; }
        public Position From { get; set; }
        public List<Position> To{ get; set; }

        public Possiblemoves(Position from, string fromChar)
        {
            From = from;
            To = new List<Position>();
            FromChar = fromChar;
        }

        public Possiblemoves(Position from, string fromChar,List<Position> To)
        {
            From = from;
            this.To = To;
            FromChar = fromChar;
        }
    }
}
