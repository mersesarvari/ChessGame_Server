using ChessIO.ws.Board;
using ChessIO.ws.Helper;
using Newtonsoft.Json;

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
        public Bot bot;
        Random r = new Random();
        public string Id { get; set; }
        public GameState State { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public int TimerBlack { get; set; }
        public int TimerWhite { get; set; }
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

        //Sáncolás változók
        public bool whiteCastleKingSide { get; set; }
        public bool whiteCastleQueenSide { get; set; }
        public bool blackCastleKingSide { get; set; }
        public bool blackCastleQueenSide { get; set; }


        #endregion

        public Game(Player _p1, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            bot = new Bot(this, Playercolor.Black);
            Id = Guid.NewGuid().ToString();
            Gametype = GameType.Singleplayer;
            Fenstring = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            //Fenstring = "rnbqk1nr/1p1ppp1p/2p3pb/p7/2BP4/4PQ2/PPP2PPP/RNB1K1NR";
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
            
            whiteCastleKingSide = true;
            whiteCastleQueenSide = true;
            blackCastleKingSide = true;
            blackCastleQueenSide = true;
            logic = new Logic(this);
            logic.ConvertFromFen();

        }
        public Game(Player _p1, Player _p2, int timer)
        {
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            Id = Guid.NewGuid().ToString();
            Gametype = GameType.Multiplayer;
            Fenstring = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
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
            
            whiteCastleKingSide = true;
            whiteCastleQueenSide = true;
            blackCastleKingSide = true;
            blackCastleQueenSide = true;
            logic = new Logic(this);
            logic.ConvertFromFen();
        }
        public Playercolor InactiveColor()
        {
            if (ActiveColor == Playercolor.White)
                return Playercolor.Black;
            else
                return Playercolor.White;
        }
        public Playercolor GetOppositeColor(Playercolor color)
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
                var whitemoves = logic.GetValidMoves(Playercolor.White, true);
                var blackmoves = logic.GetValidMoves(Playercolor.Black, true);
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
                    var blackmoves = logic.GetValidMoves(Playercolor.Black, true);
                    var bmovemsg = new Message() { Opcode = 6, Custom = blackmoves };
                    Server.SendMessage(Black, JsonConvert.SerializeObject(bmovemsg));
                }
                else
                {
                    var gamedataWhite = new Message() { Opcode = 4, Gameid = Id, Fen = Fenstring, Playerid = White, Color = Playercolor.Black };
                    Server.SendMessage(White, JsonConvert.SerializeObject(gamedataWhite));
                    var whitemoves = logic.GetValidMoves(Playercolor.White, true);
                    var wmovemsg = new Message() { Opcode = 6, Custom = whitemoves };
                    Server.SendMessage(White, JsonConvert.SerializeObject(wmovemsg));
                }
            }
            Console.WriteLine("Game starting:");
            foreach (var item in logic.game.PiecePositions)
            {
                Console.WriteLine(item.Piece + "--" + item.Position.X + "|" + item.Position.Y);
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
        public bool ZoneIsAttackedByEnemy(Position pos, Playercolor mycolor)
        {
            var opponentmoves = logic.GetValidMoves(GetOppositeColor(mycolor),false);
            foreach (var moveto in opponentmoves)
            {
                foreach (var item in moveto.To)
                {
                    if (item.X == pos.X && item.Y == pos.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void CastleMove(Position oldpos, Position newpos, PiecePosition oldpiece)
        {
            if (oldpiece.Piece == 'K'.ToString())
            {
                if (oldpos.X == newpos.X && whiteCastleQueenSide)
                {
                    if (oldpos.Y - 2 == newpos.Y)
                    {
                        //removing old rook from left
                        var rook = GetPieceByPos(new Position(7, 0));
                        var king = GetPieceByPos(oldpos);
                        king.ChangePosition(oldpos.X, oldpos.Y - 2);
                        rook.ChangePosition(newpos.X, newpos.Y + 1);

                    }
                    if (oldpos.Y + 2 == newpos.Y && whiteCastleKingSide)
                    {
                        var rook = GetPieceByPos(new Position(7, 7));
                        var king = GetPieceByPos(oldpos);
                        king.ChangePosition(oldpos.X, oldpos.Y + 2);
                        rook.ChangePosition(newpos.X, newpos.Y - 1);

                    }
                }
                whiteCastleQueenSide = false;
                whiteCastleKingSide = false;
                logic.ConvertToFen();
                return;
            }
            if (oldpiece.Piece == 'k'.ToString())
            {

                if (oldpos.X == newpos.X)
                {
                    if (oldpos.Y - 2 == newpos.Y && blackCastleQueenSide)
                    {
                        //removing old rook from left
                        var rook = GetPieceByPos(new Position(0, 0));
                        var king = GetPieceByPos(oldpos);
                        king.ChangePosition(oldpos.X, oldpos.Y - 2);
                        rook.ChangePosition(newpos.X, newpos.Y + 1);

                    }
                    if (oldpos.Y + 2 == newpos.Y && blackCastleKingSide)
                    {
                        var rook = GetPieceByPos(new Position(0, 7));
                        var king = GetPieceByPos(oldpos);
                        king.ChangePosition(oldpos.X, oldpos.Y + 2);
                        rook.ChangePosition(newpos.X, newpos.Y - 1);

                    }
                }
                blackCastleKingSide = false;
                blackCastleQueenSide = false;
                logic.ConvertToFen();
                return;
            }
            //Queenside black rook moved
            if (oldpiece.Piece == 'r'.ToString() && oldpos.X == 0 && oldpos.Y == 0)
            {
                blackCastleQueenSide = false;
            }
            //Kingside black rook moved
            if (oldpiece.Piece == 'r'.ToString() && oldpos.X == 0 && oldpos.Y == 7)
            {
                blackCastleKingSide = false;
            }
            //Queenside white rook moved
            if (oldpiece.Piece == 'R'.ToString() && oldpos.X == 7 && oldpos.Y == 0)
            {
                whiteCastleQueenSide = false;
            }
            //Kingside white rook moved
            if (oldpiece.Piece == 'R'.ToString() && oldpos.X == 7 && oldpos.Y == 7)
            {
                whiteCastleKingSide = false;
            }
        }
        public void MovePiece(Position oldpos, Position newpos)
        {
            var oldpiece = GetPieceByPos(oldpos);

            CastleMove(oldpos, newpos, oldpiece);

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
            logic.ConvertToFen();
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
        public void DrawBoard()
        {
            Console.WriteLine("------------------ BOARD ------------------");
            for (int i = 0; i < Zones.GetLength(0); i++)
            {
                for (int j = 0; j < Zones.GetLength(1); j++)
                {
                    if (PiecePositions.FirstOrDefault(f => f.Position.X == i && f.Position.Y == j) != null)
                    {
                        var piece = PiecePositions.FirstOrDefault(f => f.Position.X == i && f.Position.Y == j);
                        Console.Write(piece.Piece + " ");
                    }
                    else
                    {
                        Console.Write("0" + " ");
                    }

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
            List<PiecePosition> positions = new();
            foreach (var item in PiecePositions)
            {
                positions.Add(item);
            }
            return positions;
        }
        public Position GetKingPosition(Playercolor color)
        {
            if (color == Playercolor.Black)
                return PiecePositions.FirstOrDefault(f => f.Piece == 'k'.ToString()).Position;
            else
                return PiecePositions.FirstOrDefault(f => f.Piece == 'K'.ToString()).Position;
        }
        public bool TargetIsEnemy(Position target, Playercolor mycolor)
        {
            //Ez itt teljeen rossz
            foreach (var item in PiecePositions)
            {
                if (item.IsEquals(target))
                {
                    if (mycolor == item.Color)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;
        }
        public PiecePosition GetPieceByPos(Position pos)
        {
            return PiecePositions.FirstOrDefault(x => x.Position.X == pos.X && x.Position.Y == pos.Y);
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
