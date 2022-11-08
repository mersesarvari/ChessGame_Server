using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChessIO.ws.Logic
{
    public class PieceLogic
    {
        //public string Fen { get; set; }
        public PieceLogic()
        {
        }

        public string ConvertToFen(string[] board)
        {
            var current = board;
            var fenstring = "";
            var numbercounter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    numbercounter = 0;

                    while (current[i][j] == '0')
                    {
                        numbercounter++;
                        j++;
                    }
                    if (numbercounter > 0)
                    {
                        fenstring += numbercounter;
                    }
                    if (current[i][j] != '0')
                    {
                        //console.log("Current: " + current[i][j])
                        fenstring += current[i][j];
                    }
                }
                if (i < 7)
                {
                    fenstring += "/";
                }
            }
            //this.Fen = fenstring;
            return fenstring;
        }

        public char[][] ConvertFromFEN(string fenstring)
        {
            //Többi beállítás hiányzik logic kell ide
            char[][] baseboard = new char[8][];
            //Alapvető állás konverzió
            fenstring = fenstring.Split(' ')[0];
            var _board = baseboard;
            //string[] line = new string[8];
            for (var i = 0; i < fenstring.Split('/').Length; i++)
            {
                string line = "";
                for (int chars = 0; chars < fenstring.Split('/')[i].Length; chars++)
                {
                    var current = fenstring.Split('/')[i][chars];
                    //CHARACTER IS NUMBER
                    //Checking empty row
                    if (current == '8')
                    {
                        line = "0" + "0" + "0" + "0" + "0" + "0" + "0" + "0";
                        break;
                    }
                    // Checking empty cells in the row
                    else if (current == '1' || current == '2' || current == '3' || current == '4' || current == '5' || current == '6' || current == '7')
                    {
                        for (int szorzo = 0; szorzo < int.Parse(current.ToString()); szorzo++)
                        {
                            line+=('0'.ToString());
                        }
                    }
                    //Checking valid piececodes
                    else if ("rnbqkpRNBQKP".Contains(current.ToString().ToLower()))
                    {
                        line+=(current.ToString());
                    }
                    else
                    {
                        throw new Exception("[Error]: cannot parse char: " + current);
                    }

                };
                //console.log("Line: " + line.toString());
                _board[i] = line.ToArray();
            };
            return _board;

        }
    }
}


