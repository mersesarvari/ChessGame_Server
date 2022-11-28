using ChessIO.ws.Board;
using ChessIO.ws.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIO.ws
{
    public class PieceMovementLogic
    {
        public Game game;
        public Logic logic;

        public PieceMovementLogic(Game game, Logic logic)
        {
            this.game = game;
            this.logic = logic;
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
                    !logic.KingIsInCheck(color))
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
                    !logic.KingIsInCheck(color))
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
                    !logic.KingIsInCheck(color))
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
                    !logic.KingIsInCheck(color))
                {
                    possiblemoves.Add(new Position(x, y + 2));
                }

            }
            #endregion
            return possiblemoves.ToArray();
        }
        #endregion
        #region PieceAttacks
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
    }
}
