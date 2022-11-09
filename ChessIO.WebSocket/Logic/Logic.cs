﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace ChessIO.ws
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    // White Pawn is tested and working
    // Black Pawn is tested and working
    public class Logic
    {
        
        public static readonly char[,] startingboard = Logic.ConvertFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        //public string Fen { get; set; }
        public Logic()
        {
        }
        public static bool IsValidMove(Position oldpos, Position newpos, char[,] board)
        {
            List<Position> validmoves = new List<Position>();
            //if the current element is a pawn
            var currentchar = board[oldpos.X, oldpos.Y].ToString();
            switch (currentchar.ToLower())
            {
                case "p":
                    validmoves = PawnMovement(oldpos.X, oldpos.Y, board).ToList();
                    break;
                case "r":
                    break;
                case "n":
                    break;
                case "b":
                    break;
                case "q":
                    break;
                case "k":
                    break;
                default:
                    throw new Exception("Error when try to get the logic of the current character");
            }
            #region printvalidmoves
            //Checking the valid moves
            //Console.WriteLine($"Valid moves from {oldpos.X},{oldpos.Y}");
            /*
            foreach (var item in validmoves)
            {
                Console.WriteLine($"{item.X},{item.Y}");
            }
            */
            #endregion
            if (validmoves.FirstOrDefault(x=>x.X== newpos.X && x.Y == newpos.Y)!=null)
            {
                Console.WriteLine($"[Valid-Move] : From [{oldpos.X}|{oldpos.Y}] to [{newpos.X}|{newpos.Y}]");
                return true;
            }
            else 
            {
                Console.WriteLine($"[Invalid-Move] : From [{oldpos.X}|{oldpos.Y}] to [{newpos.X}|{newpos.Y}]");
                return false; 
            }
        }
        public static string ConvertToFen(char[,] board)
        {
            var fenstring = "";
            int zerocounter = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                zerocounter = 0;
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    //Ha 0 jön akkor nő a változó
                    if (board[i, j] == '0')
                    {
                        zerocounter++;
                    }
                    if (zerocounter == 8)
                    {
                        fenstring += zerocounter;
                    }
                    if (board[i, j] != '0')
                    {
                        if (zerocounter > 0)
                        {
                            fenstring += zerocounter;
                            zerocounter = 0;
                        }

                        fenstring += board[i, j];
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

        public static char[,] ConvertFromFen(string fenstring)
        {
            //Többi beállítás hiányzik logic kell ide
            char[,] baseboard = new char[8,8];
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
                //_board[i] = line.ToArray();
                for (int k = 0; k < line.Length; k++)
                {
                    _board[i, k] = line[k];
                }
            };
            return _board;

        }

        public static Position[] PawnMovement(int x, int y, char[,]board)
        {
            var possiblemoves = new List<Position>();
            //Console.WriteLine($"Current character:  [{board[x,y]}] on coordinate [{x},{y}]");
            //Check that the coordinate is valid
            if (board[x, y] == 'P')
            {   
                //Ha a király sakkban van akkor nem történhet semmi
                possiblemoves = new List<Position>();
                //Ha nincsenek előtte
                if (board[x - 1, y] == '0' && x-1 >=0)
                {
                    possiblemoves.Add(new Position(x-1, y));
                    //Ha kezdőhelyen van, kettőt léphet, ha üres az adott mező
                    if (x == 6 && board[x-2,y]=='0')
                    {
                        possiblemoves.Add(new Position(x - 2, y));
                    }
                }
                
                //Ha Balra lehet ütni ellenséges babút
                if (y!=0 && x!=0  && board[x - 1, y - 1] != '0' && Char.IsLower(board[x - 1, y - 1])) 
                {
                    possiblemoves.Add(new Position(x-1, y -1));
                }
                //Ha Jobbra lehet ütni
                if (y!=7 && x!=0 && board[x - 1, y + 1] != '0' && Char.IsLower(board[x - 1, y + 1]) )
                {
                    possiblemoves.Add(new Position(x -1, y +1));
                }
                //Ha mellé lépett egy paraszt
                if (true)
                {

                }
            }
            else if (board[x, y] == 'p')
            {
                //Ha a király sakkban van akkor nem történhet semmi

                //Ha nincsenek előtte
                if (x!= 7 && board[x + 1, y] == '0')
                {
                    possiblemoves.Add(new Position(x + 1, y));
                    //Ha kezdőhelyen van, kettőt léphet, ha üres az adott mező
                    if (x == 1 && board[x + 2, y] == '0')
                    {
                        possiblemoves.Add(new Position(x + 2, y));
                    }
                }

                //Ha Balra lehet ütni ellenséges babút
                if (x!=7 && y!=0 && board[x + 1, y -1] != '0' && Char.IsLower(board[x + 1, y - 1]))
                {
                    possiblemoves.Add(new Position(x + 1, y - 1));
                }
                //Ha Jobbra lehet ütni
                if (y != 7 && x != 7 && board[x + 1, y + 1] != '0' && Char.IsLower(board[x + 1, y + 1]))
                {
                    possiblemoves.Add(new Position(x + 1, y + 1));
                }
                //Ha mellé lépett egy paraszt
                if (true)
                {

                }
            }
            else {
                throw new Exception("[ERROR]: This pawn doesnt exists");
            }
            return possiblemoves.ToArray();


        }
        
        public static Position[] BishopMovement(int x, int y, char[,] board)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;
            if (board[x, y] == 'b' || board[x, y] == 'B')
            {
                /* X+ Y+ -Vagy ha barátságos karakter jön */ 
                while (x < 7 && y < 7)
                {
                    x++;
                    y++;
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b')||
                        (!Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                        )
                    {
                        break;
                    }

                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b'||
                        Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                    {
                        possiblemoves.Add(new Position(x, y));
                        break;
                    }
                    
                }
                // Resetting X and Y data
                x = originalX;
                y = originalY;
                while (x >0 && y > 0)
                {
                    x--;
                    y--;
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b') ||
                        (!Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                        )
                    {
                        break;
                    }

                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b' ||
                        Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                    {
                        possiblemoves.Add(new Position(x, y));
                        break;
                    }
                }
                // Resetting X and Y data
                x = originalX;
                y = originalY;
                while (x > 0 && y < 7)
                {
                    x--;
                    y++;
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b') ||
                        (!Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                        )
                    {
                        break;
                    }

                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b' ||
                        Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                    {
                        possiblemoves.Add(new Position(x, y));
                        break;
                    }
                }
                // Resetting X and Y data
                x = originalX;
                y = originalY;
                while (x <7 && y > 0)
                {
                    x++;
                    y--;
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b') ||
                        (!Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                        )
                    {
                        break;
                    }

                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && board[originalX, originalY] == 'b' ||
                        Char.IsLower(board[x, y]) && board[originalX, originalY] == 'B')
                    {
                        possiblemoves.Add(new Position(x, y));
                        break;
                    }
                }

                return possiblemoves.ToArray();
            }
            else
            {
                throw new Exception("[ERROR]: This bishop doesnt exists");
            }
        }
        
    }
}

