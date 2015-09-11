using System;

namespace ChessGameLib
{
    public class Queen : Piece
    {
        #region Constants
        private const int MAX_NUM_ADJ_SQ = 8;
        #endregion
        #region ctor
        public Queen(char col)
            : base(col, (ushort)Piece.PieceValues.queen, (char)Piece.PieceNotation.Queen, MAX_NUM_ADJ_SQ, "Queen")
        {
        }
        #endregion
        #region Public Methods
        /*
         * A Queen can move in any direction in a straight line. Horizontal and vertical
         * moves share the same pattern as a rook, and diagonal moves share the same
         * pattern as a bishop, so their move patterns are mirrored here in the order they
         * were mentioned above.
         */
        public override bool isValidMovement(short startX, short startY, short endX, short endY)
        {
            int diffFile = Math.Abs(startX - endX);

            return (startX == endX ^ startY == endY) || (diffFile != 0 && diffFile == Math.Abs(startY - endY));
        }

        /*
         * The immediate squares a queen can move to are a combination of what the rook
         * and bishop can move to.
         */
        /*public override BoardSquare[] getMoveableSquares(short xCoord, short yCoord)
        {
            possibleMoves[0] = new BoardSquare(xCoord + 1, yCoord + 1);
            possibleMoves[1] = new BoardSquare(xCoord + 1, yCoord - 1);
            possibleMoves[2] = new BoardSquare(xCoord - 1, yCoord + 1);
            possibleMoves[3] = new BoardSquare(xCoord - 1, yCoord - 1);
            possibleMoves[4] = new BoardSquare(xCoord, yCoord + 1);
            possibleMoves[5] = new BoardSquare(xCoord, yCoord - 1);
            possibleMoves[6] = new BoardSquare(xCoord + 1, yCoord);
            possibleMoves[7] = new BoardSquare(xCoord + 1, yCoord);

            return possibleMoves;
        }*/

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal override BitBoard getAllMoves(Board b, short xCoord, short yCoord)
        {
            return b.getMoves(xCoord, yCoord, this.colour, true, true, Board.NUM_FILES);
        }
        #endregion
    }
}
