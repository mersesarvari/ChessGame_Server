using System;
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


        
        public static List<Position> GetValidMoves(Position oldpos, char[,] board)
        {
            List<Position> validmoves = new List<Position>();
            //Have to check here the color

            var currentPiece = board[oldpos.X, oldpos.Y];
            if (currentPiece != '0')
            {
                if (Char.IsLower(currentPiece))
                {
                    switch (currentPiece)
                    {
                        case 'p':
                            validmoves = PawnMovement(oldpos.X, oldpos.Y, 'p', board).ToList();
                            break;
                        case 'r':
                            validmoves = RookMovement(oldpos.X, oldpos.Y, 'r', board).ToList();
                            break;
                        case 'n':
                            validmoves = KnightMovement(oldpos.X, oldpos.Y, 'n', board).ToList();
                            break;
                        case 'b':
                            validmoves = BishopMovement(oldpos.X, oldpos.Y, 'b', board).ToList();
                            break;
                        case 'q':
                            validmoves = QueenMovement(oldpos.X, oldpos.Y, 'q', board).ToList();
                            break;
                        case 'k':
                            validmoves = KingMovement(oldpos.X, oldpos.Y, 'q', board).ToList();
                            break;
                        default:
                            throw new Exception("Error when try to get the logic of the current character");
                    }
                }
                //Possible moves for the white
                else
                {
                    switch (currentPiece)
                    {
                        case 'P':
                            validmoves = PawnMovement(oldpos.X, oldpos.Y, 'P', board).ToList();
                            break;
                        case 'R':
                            validmoves = RookMovement(oldpos.X, oldpos.Y, 'R', board).ToList();
                            break;
                        case 'N':
                            validmoves = KnightMovement(oldpos.X, oldpos.Y, 'N', board).ToList();
                            break;
                        case 'B':
                            validmoves = BishopMovement(oldpos.X, oldpos.Y, 'B', board).ToList();
                            break;
                        case 'Q':
                            validmoves = QueenMovement(oldpos.X, oldpos.Y, 'Q', board).ToList();
                            break;
                        case 'K':
                            validmoves = KingMovement(oldpos.X, oldpos.Y, 'K', board).ToList();
                            break;
                        default:
                            throw new Exception("Error when try to get the logic of the current character");
                    }
                }
            }
            
            return validmoves;
        }

        public static List<Position> GetAllValidMoves(Game game)
        {
            var validmoveswhatarenotcheck = new List<Position>();
            List<Position> validmoves = new List<Position>();
            //Adding all the valid moves to the validmoves list
            for (int i = 0; i < game.Board.GetLength(0); i++)
            {
                for (int j = 0; j < game.Board.GetLength(1); j++)
                {
                    if (game.Board[i, j] != '0')
                    {
                        if (game.ActiveColor == Playercolor.White && Char.IsLower(game.Board[i, j]))
                        {
                            validmoves = GetValidMoves(new Position(i,j),game.Board);
                            foreach (var item in validmoves)
                            {
                                if (IsValidMove(new Position(i, j), new Position(item.X, item.Y), game.Board, game.ActiveColor, game))
                                { 
                                    validmoveswhatarenotcheck.Add(item);
                                }
                            }
                            
                        }
                        else if(game.ActiveColor == Playercolor.White && Char.IsLower(game.Board[i, j]))
                        {
                            validmoves = GetValidMoves(new Position(i, j), game.Board);
                            foreach (var item in validmoves)
                            {
                                if (IsValidMove(new Position(i, j), new Position(item.X, item.Y), game.Board, game.ActiveColor, game))
                                {
                                    validmoveswhatarenotcheck.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            //If 
            foreach (var item in validmoveswhatarenotcheck)
            {
                Logic.IsKingInChess(game.Board, game.ActiveColor);
            }
            return validmoveswhatarenotcheck;
        }
        public static bool IsValidMove(Position oldpos, Position newpos, char[,] board, Playercolor color, Game game)
        {
            List<Position> validmoves = GetValidMoves(oldpos, board);
            if (validmoves.FirstOrDefault(x => x.X == newpos.X && x.Y == newpos.Y) != null)
            {
                var simboard = game.Simulatemove(oldpos, newpos);
                if (Logic.IsKingInChess(simboard, color))
                {
                    return false;
                }
                else 
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public static string ConvertToFen(char[,] board)
        {
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
            return fenstring;
        }
        public static char[,] ConvertFromFen(string fenstring)
        {
            //Többi beállítás hiányzik logic kell ide
            char[,] baseboard = new char[8, 8];
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
                            line += ('0'.ToString());
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
                    _board[i, k] = line[k];
                }
            };
            return _board;

        }
        // Not fully working
        public static Position[] PawnMovement(int x, int y, char chartype,char[,] board)
        {
            var possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            if (board[x, y] == chartype.ToString().ToUpper()[0])
            {
                //Ha a király sakkban van akkor nem történhet semmi
                possiblemoves = new List<Position>();
                //Ha nincsenek előtte
                if (board[x - 1, y] == '0' && x - 1 >= 0)
                {
                    possiblemoves.Add(new Position(x - 1, y));
                    //Ha kezdőhelyen van, kettőt léphet, ha üres az adott mező
                    if (x == 6 && board[x - 2, y] == '0')
                    {
                        possiblemoves.Add(new Position(x - 2, y));
                    }
                }

                //Ha Balra lehet ütni ellenséges babút
                if (y != 0 && x != 0 && board[x - 1, y - 1] != '0' && Char.IsLower(board[x - 1, y - 1]))
                {
                    possiblemoves.Add(new Position(x - 1, y - 1));
                }
                //Ha Jobbra lehet ütni
                if (y != 7 && x != 0 && board[x - 1, y + 1] != '0' && Char.IsLower(board[x - 1, y + 1]))
                {
                    possiblemoves.Add(new Position(x - 1, y + 1));
                }
                //Ha mellé lépett egy paraszt
                if (true)
                {

                }
            }
            else if (board[x, y] == chartype.ToString().ToLower()[0])
            {
                //Ha a király sakkban van akkor nem történhet semmi

                //Ha nincsenek előtte
                if (x != 7 && board[x + 1, y] == '0')
                {
                    possiblemoves.Add(new Position(x + 1, y));
                    //Ha kezdőhelyen van, kettőt léphet, ha üres az adott mező
                    if (x == 1 && board[x + 2, y] == '0')
                    {
                        possiblemoves.Add(new Position(x + 2, y));
                    }
                }

                //Ha Balra lehet ütni ellenséges babút
                if (x != 7 && y != 0 && board[x + 1, y - 1] != '0' && Char.IsLower(board[x + 1, y - 1]))
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
        public static Position[] BishopMovement(int x, int y,char chartype, char[,] board)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;
            if (board[x, y] == chartype.ToString().ToLower()[0] || board[x, y] == chartype.ToString().ToUpper()[0])
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
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
        public static Position[] RookMovement(int x, int y,char chartype, char[,] board)
        {
            List<Position> possiblemoves = new List<Position>();
            //Check that the coordinate is valid
            var originalX = x;
            var originalY = y;
            if (board[x, y] == chartype.ToString().ToLower()[0] || board[x, y] == chartype.ToString().ToUpper()[0])
            {
                //Felfele mozgás
                while (x > 0)
                {
                    x--;
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
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
                    if (board[x, y] == '0')
                    {
                        possiblemoves.Add(new Position(x, y));
                    }
                    else if (
                        //Ha mindkét karakter azonos színű
                        (Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY])) ||
                        (!Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY]))
                        )
                    {
                        break;
                    }
                    else if (
                        //Ha Mindkét karakter különböző színű
                        !Char.IsLower(board[x, y]) && Char.IsLower(board[originalX, originalY]) ||
                        Char.IsLower(board[x, y]) && !Char.IsLower(board[originalX, originalY])
                        )
                    {
                        possiblemoves.Add(new Position(x, y));
                        break;
                    }

                }

                return possiblemoves.ToArray();
            }
            else
            {
                throw new Exception("[ERROR]: This rook doesnt exists");
            }
        }
        public static Position[] QueenMovement(int x, int y,char chartype, char[,] board) 
        {
            List<Position> possiblemovesall = new List<Position>();
            List<Position> rookmoves = new List<Position>();
            rookmoves = RookMovement(x, y,chartype, board).ToList();
            List<Position> bishopmoves = new List<Position>();
            bishopmoves = BishopMovement(x, y,chartype, board).ToList();
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
        public static Position[] KnightMovement(int x, int y, char chartype, char[,] board)
        {
            List<Position> possiblemoves = new List<Position>();
            //Kilépünk kettőt x vagy y irányba és a cellának mindkét x vagy y menti szomszédos oldala jó
            var originalX = x;
            var originalY = y;
            if (board[x, y] == chartype.ToString().ToLower()[0] || board[x, y] == chartype.ToString().ToUpper()[0])
            {
                //Felfele mozgás
                if (x - 2 >= 0)
                {
                    if (y - 1 >= 0)
                    {
                        possiblemoves.Add(new Position(x - 2, y-1));
                    }
                    if (y + 1 <= 7)
                    {
                        possiblemoves.Add(new Position(x - 2, y+1));
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
                if (y - 2 >=0)
                {
                    if (x - 1 >= 0)
                    {
                        possiblemoves.Add(new Position(x -1, y - 2));
                    }
                    if (x + 1 <= 7)
                    {
                        possiblemoves.Add(new Position(x + 1, y -2));
                    }
                }

                // Jobbra mozgás
                if (y + 2 <=7)
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
                //filteredpossiblemoves = possiblemoves;
                foreach (var item in possiblemoves)
                {
                    //Console.WriteLine(item.X+"|"+item.Y);
                    if (
                        Char.IsLower(board[item.X, item.Y]) && 
                        !Char.IsLower(board[originalX, originalY]))
                    {
                        filteredpossiblemoves.Add(item);
                    }
                    else if (
                        !Char.IsLower(board[item.X, item.Y]) && 
                        Char.IsLower(board[originalX, originalY]))                    
                    {
                        filteredpossiblemoves.Add(item);
                    }
                    if(board[item.X, item.Y] == '0'){
                        filteredpossiblemoves.Add(item);
                    }
                }
                return filteredpossiblemoves.ToArray();
            }
            else
            {
                throw new Exception("[ERROR]: This rook doesnt exists");
            }

        }
        //Hiányzik a rosálás logika
        public static Position[] KingMovement(int x, int y, char chartype, char[,] board)
        {
            List<Position> possiblemoves = new List<Position>();
            //Kilépünk kettőt x vagy y irányba és a cellának mindkét x vagy y menti szomszédos oldala jó
            var originalX = x;
            var originalY = y;
            #region Adding original moves
            //-x, -y
            if (x-1>=0 && y-1 >=0)
            { 
                possiblemoves.Add(new Position(x-1, y-1));
            }
            //-x, y
            if (x-1>=0)
            {
                possiblemoves.Add(new Position(x - 1, y));
            }
            // -x, +y
            if (x-1>=0 && y+1<=7)
            {
                possiblemoves.Add(new Position(x - 1, y+1));
            }
            //x, y-
            if (y - 1 >= 0)
            {
                possiblemoves.Add(new Position(x, y-1));
            }
            //x, +y
            if (y + 1 <= 7)
            {
                possiblemoves.Add(new Position(x, y+1));
            }
            //x+ y-
            if (x + 1 <= 7 && y - 1 >= 0)
            {
                possiblemoves.Add(new Position(x + 1, y-1));
            }
            //x+, y
            if (x + 1 <= 7)
            {
                possiblemoves.Add(new Position(x + 1, y));
            }
            //x+ , y+
            if (x + 1 <= 7 && y + 1 <= 7)
            {
                possiblemoves.Add(new Position(x + 1, y+1));
            }
            #endregion
            
            //Getting all the possible next movement for all the pieces on the board. === Invalid moves for the king
            List<Position> invalidmoves = new List<Position>();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != 'K' && board[i, j] != 'k')
                    {
                        if (Char.IsLower(chartype) && !Char.IsLower(board[i,j]))
                        {
                            foreach (var item in GetValidMoves(new Position(i, j), board))
                            {
                                if (!invalidmoves.Contains(item))
                                    invalidmoves.Add(item);
                            }
                        }
                        else if (!Char.IsLower(chartype) && Char.IsLower(board[i, j]))
                        {
                            foreach (var item in GetValidMoves(new Position(i, j), board))
                            {
                                if (!invalidmoves.Contains(item))
                                    invalidmoves.Add(item);
                            }
                        }

                    }
                }
            }
            //Getting the possible moves
            List<Position> filteredpossiblemoves = new List<Position>();            
            foreach (var item in possiblemoves)
            {
                //Checking if the move is a check for the current king
                if (invalidmoves.FirstOrDefault(k=>k.X==item.X && k.Y==item.Y)==null)
                {
                    if (
                        Char.IsLower(board[item.X, item.Y]) &&
                        !Char.IsLower(board[originalX, originalY]))
                    {
                        filteredpossiblemoves.Add(item);
                    }
                    else if (
                        !Char.IsLower(board[item.X, item.Y]) &&
                        Char.IsLower(board[originalX, originalY]))
                    {
                        filteredpossiblemoves.Add(item);
                    }
                    if (board[item.X, item.Y] == '0')
                    {
                        filteredpossiblemoves.Add(item);
                    }
                }
            }
            return filteredpossiblemoves.ToArray();
        }

        //Check if king is in chess after the move
        public static bool IsKingInChess(char[,]board, Playercolor color)
        {
            List<Position> possiblemoves = new List<Position>();
            //Getting the possible movements of all the other pieces (what are not kings)
            Position king=new Position(-1,-1);
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    //Fehér királyt nézzük - Pozíció megszerzése
                    if (board[i, j] == 'K'&& color==Playercolor.White)
                    { 
                        king = new Position(i, j);
                    }
                    //Fekete királyt nézzük - Pozíció megszerzése
                    if (board[i, j] == 'k' && color == Playercolor.White)
                    {
                        king = new Position(i, j);
                    }
                    //Összes lehetséges lépés letárolása
                    possiblemoves.AddRange(GetValidMoves(new Position(i, j), board));
                }
            }
            if (king.X != -1 && king.Y != -1)
            {
                possiblemoves = possiblemoves.Distinct().ToList();
                if (possiblemoves.FirstOrDefault(k => k.X == king.X && k.Y == king.Y) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //throw new Exception("King was null" + king);
                return false;
            }
            
        }
        
    }
}


