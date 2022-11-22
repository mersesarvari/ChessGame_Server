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
        public Logic(Game game)
        {
            this.game = game;
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
                        validmoves = PawnMovement(oldpos.X, oldpos.Y, 'p', color).ToList();
                        break;
                    case "r":
                        validmoves = RookMovement(oldpos.X, oldpos.Y, 'r', color).ToList();
                        break;
                    case "n":
                        validmoves = KnightMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "b":
                        validmoves = BishopMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "q":
                        validmoves = QueenMovement(oldpos.X, oldpos.Y, 'q', color).ToList();
                        break;
                    case "k":
                        validmoves = KingMovement(oldpos.X, oldpos.Y, color).ToList();
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
                        validmoves = PawnMovement(oldpos.X, oldpos.Y, 'P', color).ToList();
                        break;
                    case "R":
                        validmoves = RookMovement(oldpos.X, oldpos.Y, 'R', color).ToList();
                        break;
                    case "N":
                        validmoves = KnightMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "B":
                        validmoves = BishopMovement(oldpos.X, oldpos.Y, color).ToList();
                        break;
                    case "Q":
                        validmoves = QueenMovement(oldpos.X, oldpos.Y, 'Q', color).ToList();
                        break;
                    case "K":
                        validmoves = KingMovement(oldpos.X, oldpos.Y, color).ToList();
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
                            validmoves.AddRange(PawnAttack(item.Position.X, item.Position.Y, 'P', color).ToList());
                            break;
                        case "R":
                            validmoves.AddRange(RookAttack(item.Position.X, item.Position.Y, 'R', color).ToList());
                            break;
                        case "N":
                            validmoves.AddRange(KnightAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "B":
                            validmoves.AddRange(BishopAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "Q":
                            validmoves.AddRange(QueenAttack(item.Position.X, item.Position.Y, 'Q', color).ToList());
                            break;
                        case "K":
                            validmoves.AddRange(KingAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        default:
                            break;
                    }
                }
                if (color == Playercolor.Black)
                {
                    switch (item.Piece)
                    {
                        case "p":
                            validmoves.AddRange(PawnAttack(item.Position.X, item.Position.Y, 'p', color).ToList());
                            break;
                        case "r":
                            validmoves.AddRange(RookAttack(item.Position.X, item.Position.Y, 'r', color).ToList());
                            break;
                        case "n":
                            validmoves.AddRange(KnightAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "b":
                            validmoves.AddRange(BishopAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                        case "q":
                            validmoves.AddRange(QueenAttack(item.Position.X, item.Position.Y, 'q', color).ToList());
                            break;
                        case "k":
                            validmoves.AddRange(KingAttack(item.Position.X, item.Position.Y, color).ToList());
                            break;
                    }
                }
            }
            Console.WriteLine($"there is {validmoves.Distinct().ToList().Count()} attacked positions by {color}");
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
                var possiblemoves = new Possiblemoves(item.Position);
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
            ;
            
            //Get the real moves for white
            if (color == Playercolor.White)
            {
                game.MovesForBlack = new List<Possiblemoves>();
                game.MovesForWhite = new List<Possiblemoves>();
                Position kingpos = game.GetKingPosition(color);
                foreach (var item in whitemoves.Distinct().ToList())
                {
                    foreach (var moveto in item.To)
                    {
                        var newboard = game.Simulatemove(item.From, moveto);
                        var opponentmoves = GetAttackedPositions(newboard, Playercolor.Black);
                        foreach (var oppattacks in opponentmoves)
                        {
                            if (oppattacks.X != kingpos.X && oppattacks.Y != kingpos.Y)
                            {
                                game.MovesForWhite.Add(item);
                            }
                        }
                    }
                    
                }
                return game.MovesForWhite.Distinct().ToList();
            }
            else
            {
                game.MovesForBlack = new List<Possiblemoves>();
                game.MovesForWhite = new List<Possiblemoves>();
                Position kingpos = game.GetKingPosition(color);
                
                foreach (var item in blackmoves.Distinct().ToList())
                {
                    foreach (var moveto in item.To)
                    {
                        var newboard = game.Simulatemove(item.From, moveto);
                        var opponentmoves = GetAttackedPositions(newboard, Playercolor.White);
                        foreach (var oppattacks in opponentmoves)
                        {
                            if (oppattacks.X != kingpos.X && oppattacks.Y != kingpos.Y)
                            {
                                game.MovesForBlack.Add(item);
                            }
                        }
                    }

                }
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
        #region piecemovement
        public Position[] PawnMovement(int x, int y, char chartype, Playercolor color)
        {
            var possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            if (color == Playercolor.White)
            {
                possiblemoves = new List<Position>();
                //Ha nincsenek előtte
                if (x - 1 >= 0 && game.GetPieceByPos(new Position(x - 1, y)) == null)
                {
                    possiblemoves.Add(new Position(x - 1, y));
                    //Ha kezdőhelyen van, kettőt léphet, ha üres az adott mező
                    if (x == 6 && game.GetPieceByPos(new Position(x - 2, y)) == null)
                    {
                        possiblemoves.Add(new Position(x - 2, y));
                    }
                }

                //Ha Balra lehet ütni ellenséges babút
                if (y != 0 && x != 0 && game.GetPieceByPos(new Position(x - 1, y - 1)) != null && game.TargetIsEnemy(new Position(x - 1, y - 1), color))
                {
                    possiblemoves.Add(new Position(x - 1, y - 1));
                }
                //Ha Jobbra lehet ütni
                if (y != 7 && x != 0 && game.GetPieceByPos(new Position(x - 1, y + 1)) != null && game.TargetIsEnemy(new Position(x - 1, y + 1), color))
                {
                    possiblemoves.Add(new Position(x - 1, y + 1));
                }
                //Ha mellé lépett egy paraszt
                if (true)
                {

                }
            }
            else if (color == Playercolor.Black)
            {
                //Ha a király sakkban van akkor nem történhet semmi

                //Ha nincsenek előtte
                if (x != 7 && game.GetPieceByPos(new Position(x + 1, y)) == null)
                {
                    possiblemoves.Add(new Position(x + 1, y));
                    //Ha kezdőhelyen van, kettőt léphet, ha üres az adott mező
                    if (x == 1 && game.GetPieceByPos(new Position(x + 2, y)) == null)
                    {
                        possiblemoves.Add(new Position(x + 2, y));
                    }
                }

                //Ha Balra lehet ütni ellenséges babút
                if (x != 7 && y != 0 && game.GetPieceByPos(new Position(x + 1, y - 1)) != null && game.TargetIsEnemy(new Position(x + 1, y - 1), color))
                {
                    possiblemoves.Add(new Position(x + 1, y - 1));
                }
                //Ha Jobbra lehet ütni
                if (y != 7 && x != 7 && game.GetPieceByPos(new Position(x + 1, y + 1)) != null && game.TargetIsEnemy(new Position(x + 1, y + 1), color))
                {
                    possiblemoves.Add(new Position(x + 1, y + 1));
                }
                //En Passant
                if (true)
                {

                }
            }
            else
            {
                throw new Exception("[ERROR]: This pawn doesnt exists");
            }
            return possiblemoves.ToArray();


        }
        public Position[] PawnAttack(int x, int y, char chartype, Playercolor color)
        {
            var attack = new List<Position>();
            //Check that the coordinate is valid
            if (color == Playercolor.White)
            {
                attack = new List<Position>();

                //Ha Balra lehet ütni ellenséges babút
                if (y != 0 && x != 0 && game.GetPieceByPos(new Position(x - 1, y - 1)) != null && game.TargetIsEnemy(new Position(x - 1, y - 1), color))
                {
                    attack.Add(new Position(x - 1, y - 1));
                }
                //Ha Jobbra lehet ütni
                if (y != 7 && x != 0 && game.GetPieceByPos(new Position(x - 1, y + 1)) != null && game.TargetIsEnemy(new Position(x - 1, y + 1), color))
                {
                    attack.Add(new Position(x - 1, y + 1));
                }
                //Ha mellé lépett egy paraszt
                if (true)
                {

                }
            }
            else if (color == Playercolor.Black)
            {

                //Ha Balra lehet ütni ellenséges babút
                if (x != 7 && y != 0 && game.GetPieceByPos(new Position(x + 1, y - 1)) != null && game.TargetIsEnemy(new Position(x + 1, y - 1), color))
                {
                    attack.Add(new Position(x + 1, y - 1));
                }
                //Ha Jobbra lehet ütni
                if (y != 7 && x != 7 && game.GetPieceByPos(new Position(x + 1, y + 1)) != null && game.TargetIsEnemy(new Position(x + 1, y + 1), color))
                {
                    attack.Add(new Position(x + 1, y + 1));
                }
                //En Passant
                if (true)
                {

                }
            }
            else
            {
                throw new Exception("[ERROR]: This pawn doesnt exists");
            }
            return attack.ToArray();


        }
        public Position[] BishopMovement(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;

            /* X+ Y+ -Vagy ha barátságos karakter jön */
            while (x < 7 && y < 7)
            {
                x++;
                y++;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }
            // Resetting X and Y data
            x = originalX;
            y = originalY;
            while (x > 0 && y > 0)
            {
                x--;
                y--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
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
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }
            }
            // Resetting X and Y data
            x = originalX;
            y = originalY;
            while (x < 7 && y > 0)
            {
                x++;
                y--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }
            }

            return possiblemoves.ToArray();

        }
        public Position[] BishopAttack(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;

            /* X+ Y+ -Vagy ha barátságos karakter jön */
            while (x < 7 && y < 7)
            {
                x++;
                y++;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }
            // Resetting X and Y data
            x = originalX;
            y = originalY;
            while (x > 0 && y > 0)
            {
                x--;
                y--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
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
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }
            }
            // Resetting X and Y data
            x = originalX;
            y = originalY;
            while (x < 7 && y > 0)
            {
                x++;
                y--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }
            }

            return possiblemoves.ToArray();

        }
        public Position[] RookMovement(int x, int y, char chartype, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;
            //Felfele mozgás
            while (x > 0)
            {
                x--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }
            // Lefele
            x = originalX;
            y = originalY;
            while (x < 7)
            {
                x++;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }
            }
            // Left side movement
            x = originalX;
            y = originalY;
            while (y > 0)
            {
                y--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }
            // Jobbra mozgás
            x = originalX;
            y = originalY;
            while (y < 7)
            {
                y++;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }

            return possiblemoves.ToArray();
        }
        public Position[] RookAttack(int x, int y, char chartype, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;
            //Felfele mozgás
            while (x > 0)
            {
                x--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }
            // Lefele
            x = originalX;
            y = originalY;
            while (x < 7)
            {
                x++;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }
            }
            // Left side movement
            x = originalX;
            y = originalY;
            while (y > 0)
            {
                y--;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }
            // Jobbra mozgás
            x = originalX;
            y = originalY;
            while (y < 7)
            {
                y++;
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    possiblemoves.Add(new Position(x, y));
                }
                else if (
                    //Ha mindkét karakter azonos színű
                    !game.TargetIsEnemy(new Position(x, y), color))
                {
                    break;
                }
                else if (
                    //Ha Mindkét karakter különböző színű
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    possiblemoves.Add(new Position(x, y));
                    break;
                }

            }

            return possiblemoves.ToArray();
        }
        public Position[] QueenMovement(int x, int y, char chartype, Playercolor color)
        {
            List<Position> possiblemovesall = new List<Position>();
            List<Position> rookmoves = new List<Position>();
            rookmoves = RookMovement(x, y, chartype, color).ToList();
            List<Position> bishopmoves = new List<Position>();
            bishopmoves = BishopMovement(x, y, color).ToList();
            foreach (var item in rookmoves)
            {
                possiblemovesall.Add(item);
            }
            foreach (var item in bishopmoves)
            {
                possiblemovesall.Add(item);
            }
            return possiblemovesall.ToArray();
        }
        public Position[] QueenAttack(int x, int y, char chartype, Playercolor color)
        {
            List<Position> possiblemovesall = new List<Position>();
            List<Position> rookmoves = new List<Position>();
            rookmoves = RookAttack(x, y, chartype, color).ToList();
            List<Position> bishopmoves = new List<Position>();
            bishopmoves = BishopAttack(x, y, color).ToList();
            foreach (var item in rookmoves)
            {
                possiblemovesall.Add(item);
            }
            foreach (var item in bishopmoves)
            {
                possiblemovesall.Add(item);
            }
            return possiblemovesall.ToArray();
        }
        public Position[] KnightMovement(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            var originalX = x;
            var originalY = y;

            //Felfele mozgás
            if (x - 2 >= 0)
            {
                if (y - 1 >= 0 && game.TargetIsEnemy(new Position(x - 2, y - 1), color))
                {
                    possiblemoves.Add(new Position(x - 2, y - 1));
                }
                if (y + 1 <= 7 && game.TargetIsEnemy(new Position(x - 2, y + 1), color))
                {
                    possiblemoves.Add(new Position(x - 2, y + 1));
                }
            }
            // Lefele
            if (x + 2 <= 7)
            {
                if (y - 1 >= 0 && game.TargetIsEnemy(new Position(x + 2, y - 1), color))
                {
                    possiblemoves.Add(new Position(x + 2, y - 1));
                }
                if (y + 1 <= 7 && game.TargetIsEnemy(new Position(x + 2, y + 1), color))
                {
                    possiblemoves.Add(new Position(x + 2, y + 1));
                }
            }

            // Left side movement
            if (y - 2 >= 0)
            {
                if (x - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y - 2), color))
                {
                    possiblemoves.Add(new Position(x - 1, y - 2));
                }
                if (x + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y - 2), color))
                {
                    possiblemoves.Add(new Position(x + 1, y - 2));
                }
            }

            // Jobbra mozgás
            if (y + 2 <= 7)
            {
                if (x - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y + 2), color))
                {
                    possiblemoves.Add(new Position(x - 1, y + 2));
                }
                if (x + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y + 2), color))
                {
                    possiblemoves.Add(new Position(x + 1, y + 2));
                }
            }
            return possiblemoves.ToArray();


        }
        public Position[] KnightAttack(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            var originalX = x;
            var originalY = y;

            //Felfele mozgás
            if (x - 2 >= 0)
            {
                if (y - 1 >= 0 && game.TargetIsEnemy(new Position(x - 2, y - 1), color))
                {
                    possiblemoves.Add(new Position(x - 2, y - 1));
                }
                if (y + 1 <= 7 && game.TargetIsEnemy(new Position(x - 2, y + 1), color))
                {
                    possiblemoves.Add(new Position(x - 2, y + 1));
                }
            }
            // Lefele
            if (x + 2 <= 7)
            {
                if (y - 1 >= 0 && game.TargetIsEnemy(new Position(x + 2, y - 1), color))
                {
                    possiblemoves.Add(new Position(x + 2, y - 1));
                }
                if (y + 1 <= 7 && game.TargetIsEnemy(new Position(x + 2, y + 1), color))
                {
                    possiblemoves.Add(new Position(x + 2, y + 1));
                }
            }

            // Left side movement
            if (y - 2 >= 0)
            {
                if (x - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y - 2), color))
                {
                    possiblemoves.Add(new Position(x - 1, y - 2));
                }
                if (x + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y - 2), color))
                {
                    possiblemoves.Add(new Position(x + 1, y - 2));
                }
            }

            // Jobbra mozgás
            if (y + 2 <= 7)
            {
                if (x - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y + 2), color))
                {
                    possiblemoves.Add(new Position(x - 1, y + 2));
                }
                if (x + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y + 2), color))
                {
                    possiblemoves.Add(new Position(x + 1, y + 2));
                }
            }
            return possiblemoves.ToArray();


        }
        public Position[] KingMovement(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Kilépünk kettőt x vagy y irányba és a cellának mindkét x vagy y menti szomszédos oldala jó
            var originalX = x;
            var originalY = y;
            #region Adding original moves
            //-x, -y
            if (x - 1 >= 0 && y - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y - 1), color))
            {
                possiblemoves.Add(new Position(x - 1, y - 1));
            }
            //-x, y
            if (x - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y), color))
            {
                possiblemoves.Add(new Position(x - 1, y));
            }
            // -x, +y
            if (x - 1 >= 0 && y + 1 <= 7 && game.TargetIsEnemy(new Position(x - 1, y + 1), color))
            {
                possiblemoves.Add(new Position(x - 1, y + 1));
            }
            //x, y-
            if (y - 1 >= 0 && game.TargetIsEnemy(new Position(x, y - 1), color))
            {
                possiblemoves.Add(new Position(x, y - 1));
            }
            //x, +y
            if (y + 1 <= 7 && game.TargetIsEnemy(new Position(x, y + 1), color))
            {
                possiblemoves.Add(new Position(x, y + 1));
            }
            //x+ y-
            if (x + 1 <= 7 && y - 1 >= 0 && game.TargetIsEnemy(new Position(x + 1, y - 1), color))
            {
                possiblemoves.Add(new Position(x + 1, y - 1));
            }
            //x+, y
            if (x + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y), color))
            {
                possiblemoves.Add(new Position(x + 1, y));
            }
            //x+ , y+
            if (x + 1 <= 7 && y + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y + 1), color))
            {
                possiblemoves.Add(new Position(x + 1, y + 1));
            }
            //Castle movement for white
            if (game.whiteCastleQueenSide)
            {
                if (game.GetPieceByPos(new Position(x, y - 2)) == null &&
                    game.GetPieceByPos(new Position(x, y - 1)) == null &&
                    game.GetPieceByPos(new Position(x, y - 3)) == null &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y - 1), color) &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y - 2), color) &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y - 3), color) &&
                    !KingIsInCheck(color))
                {
                    possiblemoves.Add(new Position(x, y - 2));
                }

            }
            if (game.whiteCastleKingSide)
            {
                if (game.GetPieceByPos(new Position(x, y + 2)) == null &&
                    game.GetPieceByPos(new Position(x, y + 1)) == null &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y + 1), color) &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y + 2), color) &&
                    !KingIsInCheck(color))
                {
                    possiblemoves.Add(new Position(x, y + 2));
                }

            }
            //Castle movement for black
            if (game.blackCastleQueenSide)
            {
                if (game.GetPieceByPos(new Position(x, y - 2)) == null &&
                    game.GetPieceByPos(new Position(x, y - 1)) == null &&
                    game.GetPieceByPos(new Position(x, y - 3)) == null &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y - 1), color) &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y - 2), color) &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y - 3), color) &&
                    !KingIsInCheck(color))
                {
                    possiblemoves.Add(new Position(x, y - 2));
                }

            }
            if (game.blackCastleKingSide)
            {
                if (game.GetPieceByPos(new Position(x, y + 2)) == null &&
                    game.GetPieceByPos(new Position(x, y + 1)) == null &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y + 1), color) &&
                    !game.ZoneIsAttackedByEnemy(new Position(x, y + 2), color) &&
                    !KingIsInCheck(color))
                {
                    possiblemoves.Add(new Position(x, y + 2));
                }

            }
            #endregion
            return possiblemoves.ToArray();
        }
        public Position[] KingAttack(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Kilépünk kettőt x vagy y irányba és a cellának mindkét x vagy y menti szomszédos oldala jó
            var originalX = x;
            var originalY = y;
            //-x, -y
            if (x - 1 >= 0 && y - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y - 1), color))
            {
                possiblemoves.Add(new Position(x - 1, y - 1));
            }
            //-x, y
            if (x - 1 >= 0 && game.TargetIsEnemy(new Position(x - 1, y), color))
            {
                possiblemoves.Add(new Position(x - 1, y));
            }
            // -x, +y
            if (x - 1 >= 0 && y + 1 <= 7 && game.TargetIsEnemy(new Position(x - 1, y + 1), color))
            {
                possiblemoves.Add(new Position(x - 1, y + 1));
            }
            //x, y-
            if (y - 1 >= 0 && game.TargetIsEnemy(new Position(x, y - 1), color))
            {
                possiblemoves.Add(new Position(x, y - 1));
            }
            //x, +y
            if (y + 1 <= 7 && game.TargetIsEnemy(new Position(x, y + 1), color))
            {
                possiblemoves.Add(new Position(x, y + 1));
            }
            //x+ y-
            if (x + 1 <= 7 && y - 1 >= 0 && game.TargetIsEnemy(new Position(x + 1, y - 1), color))
            {
                possiblemoves.Add(new Position(x + 1, y - 1));
            }
            //x+, y
            if (x + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y), color))
            {
                possiblemoves.Add(new Position(x + 1, y));
            }
            //x+ , y+
            if (x + 1 <= 7 && y + 1 <= 7 && game.TargetIsEnemy(new Position(x + 1, y + 1), color))
            {
                possiblemoves.Add(new Position(x + 1, y + 1));
            }
            return possiblemoves.ToArray();
        }
        #endregion
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


