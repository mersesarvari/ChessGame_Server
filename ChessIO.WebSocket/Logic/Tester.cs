using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public class Tester
    {
        //Working fine
        public static void TestBishop(string str,int x, int y)
        {
            var brd = Logic.ConvertFromFen(str);
            Console.WriteLine("--------------- Testing Bishop movement---------------");
            for (int i = 0; i < brd.GetLength(0); i++)
            {
                for (int j = 0; j < brd.GetLength(1); j++)
                {
                    Console.Write(brd[i, j]);
                }
                Console.WriteLine();
            }
            var legitmovements = Logic.BishopMovement(x, y, brd);
            foreach (var legitmovement in legitmovements)
            {
                Console.WriteLine("Move:" + legitmovement.X + "|" + legitmovement.Y);
            }
        }

        public static void TestNight(string str, int x, int y)
        {
            var brd = Logic.ConvertFromFen(str);
            Console.WriteLine("--------------- Testing Bishop movement---------------");
            for (int i = 0; i < brd.GetLength(0); i++)
            {
                for (int j = 0; j < brd.GetLength(1); j++)
                {
                    Console.Write(brd[i, j]);
                }
                Console.WriteLine();
            }
            var legitmovements = Logic.BishopMovement(x, y, brd);
            foreach (var legitmovement in legitmovements)
            {
                Console.WriteLine("Move:" + legitmovement.X + "|" + legitmovement.Y);
            }
        }
    }
}
