using System;

namespace ChessGameLib
{
    public class Bishop : Piece
    {
        #region Constants
        private const int MAX_NUM_ADJ_SQ = 4;
        #endregion
        #region ctor
        public Bishop(char col)
            : base(col, (ushort)Piece.PieceValues.bishop, (char)Piece.PieceNotation.Bishop, MAX_NUM_ADJ_SQ, "Bishop")
        {
        }
        #endregion
        #region Inherited Methods
        /*
         * Bishops move diagonally at 45 degree angle
         * 
         * For each point in the coordinate, subtract the larger of them (comparing the
         * new position to the current position) from the smaller one, and then compare
         * them. If the differences are equal, then it is a valid move.
         * 
         * If a coordinate on a grid moves the same number of points up and over, the
         * differences of the starting coordinates are the same. Moving the same number
         * of points on both the x and y axes forms a 45 degree angle.
         */
        public override bool isValidMovement(short startX, short startY, short endX, short endY)
        {
            int diffFile = Math.Abs(startX - endX);
            return (diffFile != 0 && diffFile == Math.Abs(startY - endY));
        }

        /*
         * The immediate squares a bishop can move to have a difference of 1 or -1
         * for each the X and Y coordinates.
         */
        /*public override BoardSquare[] getMoveableSquares(short xCoord, short yCoord)
        {
            possibleMoves[0] = new BoardSquare(xCoord + 1, yCoord + 1);
            possibleMoves[1] = new BoardSquare(xCoord + 1, yCoord - 1);
            possibleMoves[2] = new BoardSquare(xCoord - 1, yCoord + 1);
            possibleMoves[3] = new BoardSquare(xCoord - 1, yCoord - 1);

            return possibleMoves;
        }*/

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal override BitBoard getAllMoves(Board b, short xCoord, short yCoord)
        {
            return b.getMoves(xCoord, yCoord, this.colour, true, false, Board.NUM_FILES);
        }
        #endregion
    }
}
