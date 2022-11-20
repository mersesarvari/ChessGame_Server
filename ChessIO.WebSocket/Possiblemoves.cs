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
        public Position From { get; set; }
        public List<Position> To{ get; set; }

        public Possiblemoves(Position from)
        {
            From = from;
            To = new List<Position>();
        }
        public void AddMove(Position to)
        {
            To.Add(to);
        }
    }
}
