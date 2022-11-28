using ChessIO.ws.Board;
using Chesster.Chess;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace ChessIO.ws.Legacy
{
    public class Logic
    {
        public Game game { get; set; }
        public PieceMovementLogic piecelogic;
        public Logic(Game game)
        {
            this.game = game;
            piecelogic = new PieceMovementLogic(game, this);
        }
        private List<Position> GetValidMovesByPiece(Position oldpos, Playercolor color)
        {
            List<Position> validmoves = new List<Position>();
            var currentPiece = game.GetPieceByPos(oldpos);
            if (color == Playercolor.Black)
            {
                switch (currentPiece.Piece)
                {
                    case "p":
                        validmoves = piecelogic.PawnMovement(oldpos.X, oldpos.Y, 'p', color).ToList();
                        break;
                    case "r":
                        validmoves = piecelogic.RookMovement(oldpos.X, oldpos.Y, 'r', color).ToList();
                        break;
                    case "n":
                        validmoves = piecelogic.KnightMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "b":
                        validmoves = piecelogic.BishopMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "q":
                        validmoves = piecelogic.QueenMovement(oldpos.X, oldpos.Y, 'q', color).ToList();
                        break;
                    case "k":
                        validmoves = piecelogic.KingMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (color == Playercolor.White)
            {
                switch (currentPiece.Piece)
                {
                    case "P":
                        validmoves = piecelogic.PawnMovement(oldpos.X, oldpos.Y, 'P', color).ToList();
                        break;
                    case "R":
                        validmoves = piecelogic.RookMovement(oldpos.X, oldpos.Y, 'R', color).ToList();
                        break;
                    case "N":
                        validmoves = piecelogic.KnightMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "B":
                        validmoves = piecelogic.BishopMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "Q":
                        validmoves = piecelogic.QueenMovement(oldpos.X, oldpos.Y, 'Q', color).ToList();
                        break;
                    case "K":
                        validmoves = piecelogic.KingMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    default:
                        break;
                }
            }
            return validmoves;

        }
        private List<Position> GetAttackedPositions(List<PiecePosition> piecepositions, Playercolor color)
        {
            List<Position> validmoves = new List<Position>();

            foreach (var item in piecepositions)
            {
                if (color == Playercolor.White)
                {
                    switch (item.Piece)
                    {
                        case "P":
                            validmoves.AddRange(piecelogic.PawnAttack(item.Position.X, item.Position.Y, 'P', color).ToList());
                            break;
                        case "R":
                            validmoves.AddRange(piecelogic.RookAttack(item.Position.X, item.Position.Y, 'R', color).ToList());
                            break;
                        case "N":
                            validmoves.AddRange(piecelogic.KnightAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "B":
                            validmoves.AddRange(piecelogic.BishopAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "Q":
                            validmoves.AddRange(piecelogic.QueenAttack(item.Position.X, item.Position.Y, 'Q', color).ToList());
                            break;
                        case "K":
                            validmoves.AddRange(piecelogic.KingAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                    }
                }
                if (color == Playercolor.Black)
                {
                    switch (item.Piece)
                    {
                        case "p":
                            validmoves.AddRange(piecelogic.PawnAttack(item.Position.X, item.Position.Y, 'p', color).ToList());
                            break;
                        case "r":
                            validmoves.AddRange(piecelogic.RookAttack(item.Position.X, item.Position.Y, 'r', color).ToList());
                            break;
                        case "n":
                            validmoves.AddRange(piecelogic.KnightAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "b":
                            validmoves.AddRange(piecelogic.BishopAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "q":
                            validmoves.AddRange(piecelogic.QueenAttack(item.Position.X, item.Position.Y, 'q', color).ToList());
                            break;
                        case "k":
                            validmoves.AddRange(piecelogic.KingAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                    }
                }
            }
            //Console.WriteLine($"there is {validmoves.Distinct().ToList().Count()} attacked positions by {color}");
            return validmoves.Distinct().ToList();
        }
        public bool IsValidMove(Position oldpos, Position newpos, Playercolor color)
        {
            var validmoves = GetValidMoves(color);
            var isvalid = false;
            foreach (var item in validmoves)
            {
                if (item.From.X == oldpos.X && item.From.Y == oldpos.Y)
                {
                    foreach (var tomove in item.To)
                    {
                        if (tomove.X == newpos.X && tomove.Y == newpos.Y)
                        {
                            isvalid = true;
                        }
                    }
                }

            }
            return isvalid;
        }
        public List<Possiblemoves> GetValidMoves(Playercolor color)
        {
            List<PiecePosition> whiteposs = (game.PiecePositions.FindAll(x => x.Color == color));
            var whitemoves = new List<Possiblemoves>();
            var blackmoves = new List<Possiblemoves>();
            foreach (var item in whiteposs)
            {
                //Megnézem az adott pozícióról az összes valid lépést
                var valid_moves_from_pos = GetValidMovesByPiece(item.Position, color);
                var possiblemoves = new Possiblemoves(item.Position, game.GetPieceByPos(item.Position).Piece);
                possiblemoves.To = valid_moves_from_pos;
                if (color == Playercolor.White)
                {
                    if (possiblemoves.To.Count() > 0)
                    {
                        whitemoves.Add(possiblemoves);
                    }
                }
                else
                {
                    if (possiblemoves.To.Count() > 0)
                    {
                        blackmoves.Add(possiblemoves);
                    }
                }

            }
            //Get the real moves for white
            if (color == Playercolor.White)
            {
                game.MovesForBlack = new List<Possiblemoves>();
                game.MovesForWhite = new List<Possiblemoves>();
                Position kingpos = game.GetKingPosition(color);
                foreach (var item in whitemoves.Distinct().ToList())
                {
                    List<Position> possiblemovesfromPosition = new List<Position>();
                    foreach (var moveto in item.To)
                    {
                        var newboard = game.Simulatemove(item.From, moveto);
                        var opponentmoves = GetAttackedPositions(newboard, Playercolor.Black);
                        kingpos = game.GetKingPosition(newboard, color);
                        if (!KingIsInCheck(newboard, color))
                        {
                            //Itt ha egy lépés valid akkor az TO-ból az összeset hozzáadom ami hiba . Csak az adott To-t kéne hozzáadni és nem az összeset
                            //game.MovesForWhite.Add(item);
                            possiblemovesfromPosition.Add(moveto);
                        }
                    }
                    //Ha van possible move akko fel kell tölteni a MovesFor XY-t
                    if (possiblemovesfromPosition.Count > 0)
                    {
                        game.MovesForWhite.Add(new Possiblemoves(item.From,item.FromChar, possiblemovesfromPosition));
                    }
                    item.To = possiblemovesfromPosition;
                    ;
                }
                var r = game.MovesForWhite.Distinct().ToList();
                Console.WriteLine("Valid moves for White:");
                foreach (var item in r)
                {
                    Console.Write(item.From.X +":"+item.From.Y+"---> ");
                    foreach (var tolist in item.To)
                    {
                        Console.Write(tolist.X + "" + tolist.Y+",");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                return game.MovesForWhite.Distinct().ToList();
            }
            else
            {
                game.MovesForBlack = new List<Possiblemoves>();
                game.MovesForWhite = new List<Possiblemoves>();
                Position kingpos = game.GetKingPosition(color);
                ;
                foreach (var item in blackmoves.Distinct().ToList())
                {
                    List<Position> possiblemovesfromPosition = new List<Position>();
                    foreach (var moveto in item.To)
                    {
                        var newboard = game.Simulatemove(item.From, moveto);
                        var opponentmoves = GetAttackedPositions(newboard, Playercolor.White);
                        Console.WriteLine("King position:"+kingpos.X+kingpos.Y);
                        game.DrawAttackedBoard(opponentmoves);
                        kingpos = game.GetKingPosition(newboard, color);
                        if (!KingIsInCheck(newboard, color))
                        {
                            //Itt ha egy lépés valid akkor az TO-ból az összeset hozzáadom ami hiba . Csak az adott To-t kéne hozzáadni és nem az összeset
                            //game.MovesForWhite.Add(item);
                            possiblemovesfromPosition.Add(moveto);
                            ;
                        }
                    }
                    //Ha van possible move akko fel kell tölteni a MovesFor XY-t
                    if (possiblemovesfromPosition.Count > 0)
                    {
                        game.MovesForBlack.Add(new Possiblemoves(item.From, item.FromChar, possiblemovesfromPosition));
                    }
                    var a = item;
                    ;

                }
                var r = game.MovesForBlack.Distinct().ToList();
                ;
                Console.WriteLine("Valid moves for Black:"+color);
                foreach (var item in r)
                {
                    Console.Write(item.From.X + ":" + item.From.Y + "---> ");
                    foreach (var tolist in item.To)
                    {
                        Console.Write(tolist.X + "" + tolist.Y + ",");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                return game.MovesForBlack.Distinct().ToList();
            }
        }
        public bool IsCheckMate(Playercolor color)
        {
            var counter = GetValidMoves(color).Count();
            if (counter == 0)
                return true;
            else return false;
        }
        public bool KingIsInCheck(Playercolor color)
        {
            var kingpos = game.GetKingPosition(color);
            var enemymoves = GetAttackedPositions(game.PiecePositions,game.GetOppositeColor(color));
            bool check = false;
            foreach (var item in enemymoves)
            {
                if (item.X == kingpos.X && item.Y == kingpos.Y)
                {
                    check = true;
                }
            }
            return check;
        }
        public bool KingIsInCheck(List<PiecePosition> _board,Playercolor color)
        {
            var kingpos = game.GetKingPosition(_board,color);
            var enemymoves = GetAttackedPositions(_board, game.GetOppositeColor(color));
            bool check = false;
            foreach (var item in enemymoves)
            {
                if (item.X == kingpos.X && item.Y == kingpos.Y)
                {
                    check = true;
                }
            }
            return check;
        }
        public void ConvertToFen()
        {
            var board = DrawBoard();
            var fenstring = "";
            int zerocounter = 0;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                //Kezdődik a sor elemeinek megnézése
                zerocounter = 0;
                bool emptyrow = true;
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    //Ha 0 jön akkor nő a változó
                    if (board[i, j] == "0")
                    {
                        zerocounter++;
                    }
                    else if (board[i, j] != "0")
                    {
                        emptyrow = false;
                        if (zerocounter > 0)
                        {
                            fenstring += zerocounter;
                            zerocounter = 0;
                        }
                        fenstring += board[i, j];
                    }
                    //checking at the end of the row
                    if (j == 7 && emptyrow)
                    {
                        fenstring += zerocounter;
                    }
                    emptyrow = true;
                }
                if (i < 7)
                {
                    fenstring += "/";
                }
            }
            //this.Fen = fenstring;
            game.Fenstring = fenstring;

        }
        public void ConvertFromFen()
        {
            game.PiecePositions = new List<PiecePosition>();
            DrawBoard();
            var matrix = GetMatrixFromFen(game.Fenstring);
            ;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != "0")
                    {
                        if (Char.IsUpper(matrix[i, j][0]))
                        {
                            game.PiecePositions.Add(new PiecePosition(new Position(i, j), matrix[i, j], Playercolor.White));
                        }
                        else
                        {
                            game.PiecePositions.Add(new PiecePosition(new Position(i, j), matrix[i, j], Playercolor.Black));
                        }
                    }
                }
            }
            var l = game.PiecePositions;
            ;
        }        
        public string[,] DrawBoard()
        {
            string[,] board = new string[8, 8];
            for (int i = 0; i < Game.Zones.GetLength(0); i++)
            {
                for (int j = 0; j < Game.Zones.GetLength(1); j++)
                {
                    if (game.PiecePositions.FirstOrDefault(f => f.Position.X == i && f.Position.Y == j) != null)
                    {
                        var piece = game.PiecePositions.FirstOrDefault(f => f.Position.X == i && f.Position.Y == j);
                        board[i, j] = piece.Piece;
                    }
                    else
                    {
                        board[i, j] = "0";
                    }

                }
            }
            return board;
        }       
        public string[,] GetMatrixFromFen(string fen)
        {
            //Többi beállítás hiányzik logic kell ide
            string[,] baseboard = new string[8, 8];
            //Alapvető állás konverzió
            var fenstring = fen.Split(" ")[0];
            var _board = baseboard;
            //string[] line = new string[8];
            for (var i = 0; i < fenstring.Split("/").Length; i++)
            {
                string line = "";
                for (int chars = 0; chars < fenstring.Split("/")[i].Length; chars++)
                {
                    var current = fenstring.Split("/")[i][chars].ToString();
                    //CHARACTER IS NUMBER
                    //Checking empty row
                    if (current == "8")
                    {
                        line = "0" + "0" + "0" + "0" + "0" + "0" + "0" + "0";
                        break;
                    }
                    // Checking empty cells in the row
                    else if (current == "1" || current == "2" || current == "3" || current == "4" || current == "5" || current == "6" || current == "7")
                    {
                        for (int szorzo = 0; szorzo < int.Parse(current.ToString()); szorzo++)
                        {
                            line += ("0".ToString());
                        }
                    }
                    //Checking valid piececodes
                    else if ("rnbqkpRNBQKP".Contains(current.ToString().ToLower()))
                    {
                        line += (current.ToString());
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
                    _board[i, k] = line[k].ToString();
                }
            };
            return _board;
        }
    }
}


