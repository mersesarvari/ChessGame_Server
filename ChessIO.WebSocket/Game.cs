﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public enum GameState
    {
        None,
        Started,
        Ended
    }
    public enum Playercolor
    { 
        White,
        Black
    }
    public class Game
    {
        #region fields
        Random r = new Random();
        public string Id { get; set; }
        public GameState State { get; set; }
        //public List<Player> PlayerList { get; set; }
        public int Elosion { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public int TimerBlack { get; set; }
        public int TimerWhite { get; set; }
        public char[,] Board { get; set; }
        public string Fenstring { get; set; }
        public List<Possiblemoves> MovesForWhite { get; set; }
        public List<Possiblemoves> MovesForBlack { get; set; }


        public Playercolor ActiveColor { get; set; }
        public string ActivePlayerId { get; set; }

        #endregion
        [JsonConstructor]
        public Game(Player _p1, Player _p2, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            Board = new char[8,8]; 
            Id = Guid.NewGuid().ToString() ;
            //Checkmate situation
            //Fenstring = "kp6/1p6/2Q5/8/8/8/8/K7";
            //Fenstring = "8/pKP5/8/8/8/8/8/7k";
            //King test
            Fenstring = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            //Fenstring = "k7/8/8/8/8/8/qq6/7K";
            Board = Logic.ConvertFromFen(Fenstring);
            //PlayerList = new List<Player>();

            //Elosion = elosion;
            var whiteblack = r.Next(0, 99);
            if (whiteblack % 2 == 1)
            {
                White = _p1.Id;
                Black = _p2.Id;
            }
            else
            {
                White = _p2.Id;
                Black = _p1.Id;
            }
            //PlayerList.Add(_p1);
            //PlayerList.Add(_p2);
            TimerBlack = timer;
            TimerWhite = timer;
            State = GameState.None;
            ActiveColor = Playercolor.Black;
            ActivePlayerId = Black;
            MovesForWhite = new List<Possiblemoves>();
            MovesForBlack = new List<Possiblemoves>();
            
        }
        public Playercolor InactiveColor()
        { 
            if(ActiveColor == Playercolor.White)
                return Playercolor.Black;
            else
                return Playercolor.White;
        }
        public static Playercolor GetOppositeColor(Playercolor color)
        {
            if (color == Playercolor.White)
                return Playercolor.Black;
            else
                return Playercolor.White;
        }
        public void StartGame()
        {
            var msg = new Message() { Opcode=4, Game=this};
            Server.SendMessage(White, JsonConvert.SerializeObject(msg));
            Server.SendMessage(Black, JsonConvert.SerializeObject(msg));
            //Sending players the basic possible moves
            var whitemoves = GetPlayerMoves(Playercolor.White);
            var blackmoves = GetPlayerMoves(Playercolor.Black);
            var wmovemsg = new Message() { Opcode = 6, Custom=whitemoves };
            var bmovemsg = new Message() { Opcode = 6, Custom = blackmoves };
            Server.SendMessage(White, JsonConvert.SerializeObject(wmovemsg));                        
            Server.SendMessage(Black, JsonConvert.SerializeObject(bmovemsg));


        }
        public void BroadcastMessage(Message message) 
        {
            Server.SendMessage(White, JsonConvert.SerializeObject(message));
            Server.SendMessage(Black, JsonConvert.SerializeObject(message));
        }
        public void SendMessage(Message message, Playercolor color)
        {
            if (color == Playercolor.White)
            {
                Server.SendMessage(White, JsonConvert.SerializeObject(message));
            }
            else
            {
                Server.SendMessage(Black, JsonConvert.SerializeObject(message));
            }
        }
        public void MovePiece(Position oldpos, Position newpos)
        {
            var oldboard = CopyBoard(Board);
            Board[newpos.X, newpos.Y] = oldboard[oldpos.X, oldpos.Y];
            Board[oldpos.X, oldpos.Y] = '0';
            Fenstring = Logic.ConvertToFen(Board);
        }
        public static char[,] Simulatemove(Position oldpos, Position newpos, char[,]board)
        {
            var sboard = CopyBoard(board);
            sboard[newpos.X, newpos.Y] = sboard[oldpos.X, oldpos.Y];
            sboard[oldpos.X, oldpos.Y] = '0';
            return sboard;

        }
        public static void DrawBoard(char[,]Board)
        {
            Console.WriteLine("------------------ BOARD ------------------");
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    Console.Write(Board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public void TurnChanger()
        {
            if (ActiveColor == Playercolor.White)
            {
                ActiveColor = Playercolor.Black;
                ActivePlayerId = Black;
            }
            else
            {
                ActiveColor = Playercolor.White;
                ActivePlayerId = White;
            }
        }
        public static char[,] CopyBoard(char[,] oldboard)
        {
            char[,] newboard = new char[8, 8];            
            for (int i = 0; i < newboard.GetLength(0); i++)
            {
                for (int j = 0; j < newboard.GetLength(1); j++)
                {
                    newboard[i, j] = oldboard[i, j];
                }
            }
            return newboard;
        }
        public static Position GetKingPosition(char[,]board, Playercolor color)
        {
            if (Playercolor.White == color)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == 'K')
                        {
                            return new Position(i, j);
                        }
                    }
                }
                throw new Exception($"[Error]: {color} king not found");
            }
            else 
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == 'k')
                        {
                            return new Position(i, j);
                        }
                    }
                }
                throw new Exception($"[Error]: {color} king not found");
            }
        }
        public static bool TargetIsEnemy(char[,] board,int x,int y, Playercolor mycolor)
        {
            if (board[x, y] != '0')
            {
                if (mycolor == Playercolor.White && Char.IsLower(board[x,y]))
                {
                    return true;
                }
                else if (mycolor == Playercolor.Black && !Char.IsLower(board[x, y]))
                {
                    return true;
                }
                else return false;
            }
            else return true;
            
        }
        public static List<Position> GetFriendlyPiecesPos(char[,] board, Playercolor color)
        {
            var listlength = board.GetLength(0)*board.GetLength(1);
            List<Position> lista = new List<Position>();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (Playercolor.White == color && !Char.IsLower(board[i, j]))
                    {
                        lista.Add(new Position(i, j));
                    }
                    if (Playercolor.Black == color && Char.IsLower(board[i, j]))
                    {
                        lista.Add(new Position(i, j));
                    }
                    
                }
            }
            return lista;
        }

        public List<Possiblemoves> GetPlayerMoves(Playercolor color)
        {
            List<Position> whiteposs = GetFriendlyPiecesPos(this.Board, color);
            
            foreach (var item in whiteposs)
            {
                var validmoves = Logic.GetValidMoves(item, this.Board, color, true);
                var possiblemoves = new Possiblemoves(item);
                for (int i = 0; i < validmoves.Count(); i++)
                {
                    possiblemoves.AddMove(validmoves[i]);
                }
                if (color == Playercolor.White)
                {
                    if (possiblemoves.To.Count() > 0)
                    {
                        MovesForWhite.Add(possiblemoves);
                    }
                }
                else
                {
                    if (possiblemoves.To.Count() > 0)
                    {
                        MovesForBlack.Add(possiblemoves);
                    }
                }
                
            }
            if (color == Playercolor.White)
            {
                return MovesForWhite;
            }
            else
            {
                return MovesForBlack;
            }

        }
    }
}
