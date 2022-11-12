using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        //In Millisecnds
        public int TimerBlack { get; set; }
        //In Millisecnds
        public int TimerWhite { get; set; }

        public char[,] Board { get; set; }

        public string Fenstring { get; set; }

        public Playercolor ActiveColor { get; set; }
        public string ActivePlayerId { get; set; }

        #endregion
        [JsonConstructor]
        public Game(Player _p1, Player _p2, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            Board = new char[8,8]; 
            Id = Guid.NewGuid().ToString() ;
            //Fenstring = "rnbqkbnr/1pppppp1/p6p/8/2B5/4P3/PPPP1QPP/RNB1K1NR";
            //Fenstring = "q7/8/8/8/8/8/R7/K7";
            Fenstring = "k7/8/8/8/8/8/qq6/7K";
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
            ActiveColor = Playercolor.White;
            ActivePlayerId = White;
            
        }
        public Playercolor InactiveColor()
        { 
            if(ActiveColor == Playercolor.White)
                return Playercolor.Black;
            else
                return Playercolor.White;
        }
        public void StartGame()
        {
            var msg = new Message() { Opcode=4, Game=this};
            Server.SendMessage(White, JsonConvert.SerializeObject(msg));
            Server.SendMessage(Black, JsonConvert.SerializeObject(msg));

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
    }
}
