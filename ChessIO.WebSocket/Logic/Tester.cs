using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public class Tester
    {
        public void TestPawn(char[,] brd)
        {
            Console.WriteLine("--------------- Testing ---------------");
            var legitmovements = Logic.PawnMovement(1, 1, brd);
            foreach (var legitmovement in legitmovements)
            {
                Console.WriteLine("Move:" + legitmovement.X + "|" + legitmovement.Y);
            }
        }
    }
}
