using System;

namespace ChessGameLib
{
    public class Knight : Piece
    {
        #region Constants
        private const int MAX_NUM_ADJ_SQ = 8;
        #endregion
        #region ctor
        public Knight(char col)
            : base(col, (ushort)Piece.PieceValues.knight, (char)Piece.PieceNotation.Knight, MAX_NUM_ADJ_SQ, "Knight")
        {
        }
        #endregion
        #region Public Methods
        /*
         * Knights move in an 'L' shape, 2 sqaures high and 1 square long.
         * This means that the absolute difference between a knight's starting coordinates
         * and ending coordinates is always 2 for one coordinate and 1 for the other
         */
        public override bool isValidMovement(short startX, short startY, short endX, short endY)
        {
            int diffFile = Math.Abs(startX - endX);
            int diffRank = Math.Abs(startY - endY);
            return (diffFile == 2 && diffRank == 1 || diffFile == 1 && diffRank == 2);
        }

        /*
         * A valid move for a knight is 1 square along either the X or Y axes, and
         * 2 squares along the axis that wasn't moved 1 along. Every combination of
         * adding/subtracting 1 or 2 points along an axis and doing the opposite
         * mathematical operation to the other number is accounted for.
         */
        /*public override BoardSquare[] getMoveableSquares(short xCoord, short yCoord)
        {
            possibleMoves[0] = new BoardSquare(xCoord + 2, yCoord + 1);
            possibleMoves[1] = new BoardSquare(xCoord + 2, yCoord - 1);
            possibleMoves[2] = new BoardSquare(xCoord - 2, yCoord + 1);
            possibleMoves[3] = new BoardSquare(xCoord - 2, yCoord - 1);
            
            
            possibleMoves[4] = new BoardSquare(xCoord - 1, yCoord + 2);
            possibleMoves[5] = new BoardSquare(xCoord - 1, yCoord - 2);
            possibleMoves[6] = new BoardSquare(xCoord + 1, yCoord + 2);
            possibleMoves[7] = new BoardSquare(xCoord + 1, yCoord - 2);

            return possibleMoves;
        }*/

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal override BitBoard getAllMoves(Board b, short xCoord, short yCoord)
        {
            BitBoard bit = BitBoard.Empty;

            BoardSquare ownKing = b.GetKingLocation(this.colour == Piece.NOTATION_W ? Game.Sides.white : Game.Sides.black);
            BoardSquare potentialPinner = b.ExtrapolateAttack(xCoord, yCoord, (short)ownKing.X, (short)ownKing.Y);

            // if piece is not pinned
            if (potentialPinner == BoardSquare.Empty)
            {
                if (b.isValidSquare(xCoord+2, yCoord+1) &&
                        (b[yCoord + 1, xCoord + 2] == null || b[yCoord + 1, xCoord + 2].Colour != this.colour))
                    bit.AddPosition(xCoord + 2, yCoord + 1);

                if (b.isValidSquare(xCoord + 1, yCoord + 2) &&
                        (b[yCoord + 2, xCoord + 1] == null || b[yCoord + 2, xCoord + 1].Colour != this.colour))
                    bit.AddPosition(xCoord + 1, yCoord + 2);

                if (b.isValidSquare(xCoord - 2, yCoord - 1) &&
                        (b[yCoord - 1, xCoord - 2] == null || b[yCoord - 1, xCoord - 2].Colour != this.colour))
                    bit.AddPosition(xCoord - 2, yCoord - 1);

                if (b.isValidSquare(xCoord - 1, yCoord - 2) &&
                        (b[yCoord - 2, xCoord - 1] == null || b[yCoord - 2, xCoord - 1].Colour != this.colour))
                    bit.AddPosition(xCoord - 1, yCoord - 2);

                if (b.isValidSquare(xCoord + 2, yCoord - 1) &&
                        (b[yCoord - 1, xCoord + 2] == null || b[yCoord - 1, xCoord + 2].Colour != this.colour))
                    bit.AddPosition(xCoord +2, yCoord - 1);

                if (b.isValidSquare(xCoord + 1, yCoord - 2) &&
                        (b[yCoord - 2, xCoord + 1] == null || b[yCoord - 2, xCoord + 1].Colour != this.colour))
                    bit.AddPosition(xCoord + 1, yCoord - 2);

                if (b.isValidSquare(xCoord - 2, yCoord + 1) &&
                        (b[yCoord + 1, xCoord - 2] == null || b[yCoord + 1, xCoord - 2].Colour != this.colour))
                    bit.AddPosition(xCoord - 2, yCoord + 1);

                if (b.isValidSquare(xCoord - 1, yCoord + 2) &&
                        (b[yCoord + 2, xCoord - 1] == null || b[yCoord + 2, xCoord - 1].Colour != this.colour))
                    bit.AddPosition(xCoord - 1, yCoord + 2);
            }

            return bit;
        }
        #endregion
    }
}
