using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public class Game
    {
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


        [JsonConstructor]
        public Game(Player _p1, Player _p2, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            Board = new char[8,8]; 
            Id = Guid.NewGuid().ToString() ;
            Fenstring = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
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

        public void MovePiece(Position oldpos, Position newpos)
        {
            //Checking logic
            Board[newpos.X, newpos.Y] = Board[oldpos.X, oldpos.Y];
            Board[oldpos.X, oldpos.Y] = '0';
            Fenstring = Logic.ConvertToFen(Board);
        }
        public void DrawBoard()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    Console.Write(Board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
