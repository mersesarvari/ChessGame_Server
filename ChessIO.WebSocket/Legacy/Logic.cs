using ChessIO.ws.Board;
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
        public List<Position> GetValidMoves(Position oldpos, Playercolor color, bool ismyturn)
        {
            List<Position> validmoves = new List<Position>();

            var currentPiece = game.GetPieceByPos(oldpos);
            if (color == Playercolor.Black)
            {
                switch (currentPiece.Piece)
                {
                    case 'p':
                        validmoves = PawnMovement(oldpos.X, oldpos.Y, 'p', color).ToList();
                        break;
                    case 'r':
                        validmoves = RookMovement(oldpos.X, oldpos.Y, 'r', color).ToList();
                        break;
                    case 'n':
                        validmoves = KnightMovement(oldpos.X, oldpos.Y, 'n', color).ToList();
                        break;
                    case 'b':
                        validmoves = BishopMovement(oldpos.X, oldpos.Y,  color).ToList();
                        break;
                    case 'q':
                        validmoves = QueenMovement(oldpos.X, oldpos.Y, 'q',  color).ToList();
                        break;
                    case 'k':
                        validmoves = KingMovement(oldpos.X, oldpos.Y, Playercolor.Black).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (color == Playercolor.White)
            {
                switch (currentPiece.Piece)
                {
                    case 'P':
                        validmoves = PawnMovement(oldpos.X, oldpos.Y, 'P', color).ToList();
                        break;
                    case 'R':
                        validmoves = RookMovement(oldpos.X, oldpos.Y, 'R',  color).ToList();
                        break;
                    case 'N':
                        validmoves = KnightMovement(oldpos.X, oldpos.Y, 'N',  color).ToList();
                        break;
                    case 'B':
                        validmoves = BishopMovement(oldpos.X, oldpos.Y,  color).ToList();
                        break;
                    case 'Q':
                        validmoves = QueenMovement(oldpos.X, oldpos.Y, 'Q',  color).ToList();
                        break;
                    case 'K':
                        validmoves = KingMovement(oldpos.X, oldpos.Y, Playercolor.White).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (ismyturn)
            {
                List<Position> notcheckmoves = new List<Position>();
                foreach (var item in validmoves)
                {
                    var newboard = game.Simulatemove(oldpos, item);
                    List<Position> oppvalidmoves = new List<Position>();
                    oppvalidmoves = GetAllMoves(Game.GetOppositeColor(color), false);

                    Position kingpos = game.GetKingPosition(color);
                    var feltetel = oppvalidmoves.FirstOrDefault(x => x.X == kingpos.X && x.Y == kingpos.Y);
                    if (feltetel == null)
                    {
                        notcheckmoves.Add(item);
                    }
                }
                return notcheckmoves;

            }

            return validmoves;
        }
        /// <summary>
        /// Getting all the valid moves with a specific color and with the current board standing
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>        
        public List<Position> GetAllMoves(Playercolor color, bool repeat)
        {
            var allvalidmoves = new List<Position>();
            foreach (var item in game.PiecePositions)
            {
                allvalidmoves.AddRange(GetValidMoves(item.Position, color, repeat));
            }
            return allvalidmoves.Distinct().ToList();
        }
        public bool IsValidMove(Position oldpos, Position newpos, Playercolor color, bool checkifcheck)
        {
            var moves = GetValidMoves(oldpos, color, checkifcheck);
            ;
            var returnvalue = false;
            foreach (var item in moves)
            {
                if (newpos.X == item.X && newpos.Y == item.Y)
                {
                    returnvalue = true;
                }
            }
            return returnvalue;

        }
        public bool IsCheckMate(Playercolor color, bool checkifincheck)
        {
            var counter = GetAllMoves(color, checkifincheck).Count();
            if (counter == 0)
                return true;
            else return false;
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
                    if (board[i, j] == '0')
                    {
                        zerocounter++;
                    }
                    else if (board[i, j] != '0')
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
        //Working more or less
        public void ConvertFromFen()
        {
            char[] numbers = new[] { '1', '2', '3', '4', '5', '6', '7' };
            int posX = 0;
            int posY = 0;
            //Real Fen: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            var fen = game.Fenstring;
            //Getting the board
            string boardstr = fen.Split(' ')[0];
            for (int i = 0; i < boardstr.Split('/').Length; i++)
            {
                posX = i;
                
                var row = boardstr.Split('/');
                for (int j = 0; j < row[i].Length; j++)
                {
                    posY = j;
                    var item = boardstr.Split('/')[i][j];
                    if (item == '8')
                        break;
                    else if (numbers.Contains(item))
                    {
                        posY += int.Parse(item.ToString());
                        ;
                        if (j + 1 < 8)
                        {
                            var piece = new PiecePosition(new Position(posX, j+1), item, Playercolor.White);
                            game.PiecePositions.Add(piece);
                            j++;
                            break;
                        }
                    }
                    else
                    {
                        if (Char.IsUpper(item))
                        {
                            var piece = new PiecePosition(new Position(posX, posY), item, Playercolor.White);
                            game.PiecePositions.Add(piece);
                        }
                        else
                        {
                            var piece = new PiecePosition(new Position(posX, posY), item, Playercolor.Black);
                            game.PiecePositions.Add(piece);
                        }
                        
                    }
                }
                
            }
            game.DrawBoard();
            var _i= game.PiecePositions;
            ;

        }

        // Not fully working
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
        public Position[] BishopMovement(int x, int y, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;
            if (game.PiecePositions.FirstOrDefault(f => f.IsEquals(new Position(x,y)) && f.Color == color) != null)
            {
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
            else
            {
                throw new Exception("[ERROR]: This bishop doesnt exists");
            }
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
        public Position[] KnightMovement(int x, int y, char chartype, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            var originalX = x;
            var originalY = y;

            //Felfele mozgás
            if (x - 2 >= 0)
            {
                if (y - 1 >= 0)
                {
                    possiblemoves.Add(new Position(x - 2, y - 1));
                }
                if (y + 1 <= 7)
                {
                    possiblemoves.Add(new Position(x - 2, y + 1));
                }
            }
            // Lefele
            if (x + 2 <= 7)
            {
                if (y - 1 >= 0)
                {
                    possiblemoves.Add(new Position(x + 2, y - 1));
                }
                if (y + 1 <= 7)
                {
                    possiblemoves.Add(new Position(x + 2, y + 1));
                }
            }

            // Left side movement
            if (y - 2 >= 0)
            {
                if (x - 1 >= 0)
                {
                    possiblemoves.Add(new Position(x - 1, y - 2));
                }
                if (x + 1 <= 7)
                {
                    possiblemoves.Add(new Position(x + 1, y - 2));
                }
            }

            // Jobbra mozgás
            if (y + 2 <= 7)
            {
                if (x - 1 >= 0)
                {
                    possiblemoves.Add(new Position(x - 1, y + 2));
                }
                if (x + 1 <= 7)
                {
                    possiblemoves.Add(new Position(x + 1, y + 2));
                }
            }
            var p = possiblemoves;
            List<Position> filteredpossiblemoves = new List<Position>();
            foreach (var item in possiblemoves)
            {
                //Console.WriteLine(item.X+"|"+item.Y);
                if (
                    game.TargetIsEnemy(new Position(x, y), color))
                {
                    filteredpossiblemoves.Add(item);
                }
                if (game.GetPieceByPos(new Position(x, y)) == null)
                {
                    filteredpossiblemoves.Add(item);
                }
            }
            //Ki kell szedni még azokat a lépéseket amik sakkhoz vezetnének
            return filteredpossiblemoves.ToArray();


        }
        //Hiányzik a rosálás logika
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
            #endregion            
            return possiblemoves.ToArray();
        }
        #endregion
        public char[,] DrawBoard()
        {
            char[,] board = new char[8, 8];
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
                        board[i, j] = '0';
                    }

                }
            }
            return board;
        }
    }
}


