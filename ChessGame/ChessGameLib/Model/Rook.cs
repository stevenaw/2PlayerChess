using System;

namespace ChessGameLib
{
    public class Rook : Piece
    {
        #region Variables and Constants
        private const int MAX_NUM_ADJ_SQ = 4;
        #endregion
        #region ctor
        public Rook(char col)
            : base(col, (ushort)Piece.PieceValues.rook, (char)Piece.PieceNotation.Rook, MAX_NUM_ADJ_SQ, "Rook")
        {
        }
        #endregion
        #region Public Methods
        /*
         * When a rook moves, it moves either horizontally or vertically, so one
         * of those coordinates will have changed, but not both
         */
        public override bool isValidMovement(short startX, short startY, short endX, short endY)
        {
            return (startX == endX ^ startY == endY);
        }

        /*
         * The immediate squares a rook can move to have an X-coordinate difference of one
         * or a Y-coordinate difference of 1, but not both.
         */
        /*public override BoardSquare[] getMoveableSquares(short xCoord, short yCoord)
        {
            possibleMoves[0] = new BoardSquare(xCoord, yCoord + 1);
            possibleMoves[1] = new BoardSquare(xCoord, yCoord - 1);
            possibleMoves[2] = new BoardSquare(xCoord + 1, yCoord);
            possibleMoves[3] = new BoardSquare(xCoord + 1, yCoord);

            return possibleMoves;
        }*/

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal override BitBoard getAllMoves(Board b, short xCoord, short yCoord)
        {
            return b.getMoves(xCoord, yCoord, this.colour, false, true, Board.NUM_FILES);
        }
        #endregion
    }
}
