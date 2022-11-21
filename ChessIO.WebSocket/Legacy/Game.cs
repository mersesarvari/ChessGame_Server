using ChessIO.ws.Board;
using ChessIO.ws.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws.Legacy
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
    public enum GameType
    {
        Multiplayer,
        Singleplayer,
    }
    public class Game
    {
        #region fields
        Random r = new Random();
        public string Id { get; set; }
        public GameState State { get; set; }
        public int Elosion { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public int TimerBlack { get; set; }
        public int TimerWhite { get; set; }
        //public char[,] Board { get; set; }
        public string Fenstring { get; set; }
        public Logic logic;
        public List<Possiblemoves> MovesForWhite { get; set; }
        public List<Possiblemoves> MovesForBlack { get; set; }
        public List<PiecePosition> PiecePositions = new List<PiecePosition>();
        public static string[,] Zones = new string[8, 8] {
                { "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8" },
                { "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7" },
                { "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6" },
                { "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5" },
                { "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4" },
                { "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3" },
                { "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2" },
                { "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1" },
            };
        public GameType Gametype { get; set; }
        public Playercolor ActiveColor { get; set; }
        public string ActivePlayerId { get; set; }
        public Bot CurrentBot { get; set; }
        #endregion

        public Game(Player _p1, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            //Board = new char[8, 8];
            Id = Guid.NewGuid().ToString();
            Gametype = GameType.Singleplayer;
            /*  Original */
            Fenstring = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            //Board = Logic.ConvertFromFen(Fenstring);
            White = _p1.Id;
            Black = "Bot";
            TimerBlack = timer;
            TimerWhite = timer;
            State = GameState.None;
            ActiveColor = Playercolor.White;
            ActivePlayerId = White;
            MovesForWhite = new List<Possiblemoves>();
            MovesForBlack = new List<Possiblemoves>();
            PiecePositions = new List<PiecePosition>();
            logic = new Logic(this);

        }
        [JsonConstructor]
        public Game(Player _p1, Player _p2, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            Id = Guid.NewGuid().ToString();
            Gametype = GameType.Multiplayer;
            Fenstring = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            //Board = Logic.ConvertFromFen(Fenstring);
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
            TimerBlack = timer;
            TimerWhite = timer;
            State = GameState.None;
            ActiveColor = Playercolor.White;
            ActivePlayerId = White;
            MovesForWhite = new List<Possiblemoves>();
            MovesForBlack = new List<Possiblemoves>();
            PiecePositions = new List<PiecePosition>();
            logic = new Logic(this);

        }
        public Playercolor InactiveColor()
        {
            if (ActiveColor == Playercolor.White)
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
            if (Gametype == GameType.Multiplayer)
            {
                //Fel kellett cserélni itt a színeket valamiért. tuti valami bug...
                var gamedataWhite = new Message() { Opcode = 4, Gameid = Id, Fen = Fenstring, Playerid = White, Color = Playercolor.Black };
                var gamedataBlack = new Message() { Opcode = 4, Gameid = Id, Fen = Fenstring, Playerid = Black, Color = Playercolor.White };
                Server.SendMessage(White, JsonConvert.SerializeObject(gamedataWhite));
                Server.SendMessage(Black, JsonConvert.SerializeObject(gamedataBlack));
                //Sending players the basic possible moves
                var whitemoves = GetPlayerMoves(Playercolor.White, true);
                var blackmoves = GetPlayerMoves(Playercolor.Black, true);
                ;
                var wmovemsg = new Message() { Opcode = 6, Custom = whitemoves };
                var bmovemsg = new Message() { Opcode = 6, Custom = blackmoves };
                Server.SendMessage(White, JsonConvert.SerializeObject(wmovemsg));
                Server.SendMessage(Black, JsonConvert.SerializeObject(bmovemsg));
            }
            else
            {
                Console.WriteLine("Player1:" + White);
                Console.WriteLine("Player2:" + Black);
                if (White == "Bot")
                {
                    var gamedataBlack = new Message() { Opcode = 4, Gameid = Id, Fen = Fenstring, Playerid = Black, Color = Playercolor.White };
                    Server.SendMessage(Black, JsonConvert.SerializeObject(gamedataBlack));
                    var blackmoves = GetPlayerMoves(Playercolor.Black, true);
                    var bmovemsg = new Message() { Opcode = 6, Custom = blackmoves };
                    Server.SendMessage(Black, JsonConvert.SerializeObject(bmovemsg));
                }
                else
                {
                    var gamedataWhite = new Message() { Opcode = 4, Gameid = Id, Fen = Fenstring, Playerid = White, Color = Playercolor.Black };
                    Server.SendMessage(White, JsonConvert.SerializeObject(gamedataWhite));
                    var whitemoves = GetPlayerMoves(Playercolor.White, true);
                    var wmovemsg = new Message() { Opcode = 6, Custom = whitemoves };
                    Server.SendMessage(White, JsonConvert.SerializeObject(wmovemsg));
                }
            }



        }
        public void BroadcastMessage(Message message)
        {
            if (White != "Bot")
            {
                Server.SendMessage(White, JsonConvert.SerializeObject(message));
            }
            if (Black != "Bot")
            {
                Server.SendMessage(Black, JsonConvert.SerializeObject(message));
            }


        }
        public void SendMessage(Message message, Playercolor color)
        {
            if (color == Playercolor.White)
            {
                if (White != "Bot")
                    Server.SendMessage(White, JsonConvert.SerializeObject(message));
            }
            else
            {
                if (Black != "Bot")
                    Server.SendMessage(Black, JsonConvert.SerializeObject(message));
            }
        }
        public void MovePiece(Position oldpos, Position newpos)
        {
            var oldpiece = GetPieceByPos(oldpos);
            // Checking is the new position is an enemy character or not
            if (GetPieceByPos(newpos) != null)
            {
                PiecePositions.Remove(GetPieceByPos(newpos));
                PiecePositions.Remove(oldpiece);
                PiecePositions.Add(new PiecePosition(newpos, oldpiece.Piece, oldpiece.Color));
            }
            else
            {
                
                PiecePositions.Remove(oldpiece);
                PiecePositions.Add(new PiecePosition(newpos, oldpiece.Piece, oldpiece.Color));
            }
            
        }
        public List<PiecePosition> Simulatemove(Position oldpos, Position newpos)
        {
            var simboard = CopyBoard();
            var oldpiece = GetPieceByPos(oldpos);
            // Checking is the new position is an enemy character or not
            if (GetPieceByPos(newpos) != null)
            {
                simboard.Remove(GetPieceByPos(newpos));
                simboard.Remove(oldpiece);
                simboard.Add(new PiecePosition(newpos, oldpiece.Piece, oldpiece.Color));
            }
            else
            {

                simboard.Remove(oldpiece);
                simboard.Add(new PiecePosition(newpos, oldpiece.Piece, oldpiece.Color));
            }
            return simboard;

        }
        public static void DrawBoard(char[,] Board)
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
        public List<PiecePosition> CopyBoard()
        {
            List<PiecePosition> positions= new();
            foreach (var item in PiecePositions)
            {
                positions.Add(item);
            }
            return positions;
        }
        public Position GetKingPosition(Playercolor color)
        {
            return PiecePositions.FirstOrDefault(x => x.Piece == 'K'&& x.Color==color).Position;
        }
        public bool TargetIsEnemy(Position target, Playercolor mycolor)
        {
            if (PiecePositions.FirstOrDefault(x => x.Position == target && x.Color != mycolor) != null)
                return true;
            else return false;
        }
        public List<Possiblemoves> GetPlayerMoves(Playercolor color, bool ismyturn)
        {
            List<PiecePosition> whiteposs = (PiecePositions.FindAll(x=>x.Color == color));
            ;
            foreach (var item in whiteposs)
            {
                //Megnézem az adott pozícióról az összes valid lépést
                var valid_moves_from_pos = logic.GetValidMoves(item.Position, color, ismyturn);
                var possiblemoves = new Possiblemoves(item.Position);
                possiblemoves.To = valid_moves_from_pos;
                if (color == Playercolor.White)
                {
                    if (possiblemoves.To.Count() > 0)
                    {
                        MovesForWhite.Add(possiblemoves);
                        MovesForBlack = new List<Possiblemoves>();
                    }
                }
                else
                {
                    if (possiblemoves.To.Count() > 0)
                    {
                        MovesForBlack.Add(possiblemoves);
                        MovesForWhite = new List<Possiblemoves>();
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
        public PiecePosition GetPieceByPos(Position pos)
        {
            return PiecePositions.FirstOrDefault(x=>x.Position==pos);
        }
        public static Position GetZoneName(string pos)
        {
            Position _pos = new Position(-1, -1);
            for (int i = 0; i < Zones.GetLength(0); i++)
            {
                for (int j = 0; j < Zones.GetLength(1); j++)
                {
                    if (pos.ToLower() == Zones[i, j].ToLower())
                    {
                        _pos = new Position(i, j);
                    }

                }
            }
            if (_pos.X != -1 && _pos.Y != -1)
                return _pos;
            else throw new Exception("Position was not found");
        }
    }
}
