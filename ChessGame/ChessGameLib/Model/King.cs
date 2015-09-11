using System;

namespace ChessGameLib
{
    public class King : Piece
    {
        #region Variables and Constants
        /*
         * King-specific values related to castling.
         * 
         * hasNotMoved indicates if the King has moved yet in the game.  Castling is only
         * allowed if the king has not moved yet. IsInCheck indicates if the King is in check
         * or not. Castling is not allowed while in check.
         */
        private bool hasNotMoved = true;
        public bool HasNotMoved
        {
            get { return hasNotMoved; }
            set { hasNotMoved = value; }
        }

        public const int MAX_NUM_ADJ_SQ = 8;
        #endregion
        #region ctor
        public King(char col)
            : base(col, (ushort)Piece.PieceValues.king, (char)Piece.PieceNotation.King, MAX_NUM_ADJ_SQ, "King")
        {
        }
        #endregion
        #region Public Methods
        /*
         * A King can move 1 square in any direction.
         * 
         * This means that the absolute difference of both the X and Y coordinates
         * are less than 2 for any valid move
         */
        public override bool isValidMovement(short startX, short startY, short endX, short endY)
        {
            return (Math.Abs(startX - endX) < 2 && Math.Abs(startY - endY) < 2);
        }

        /*
         * A king's movement pattern is identical to the queen's, with the exception
         * that the king can only move 1 square, compared to the queen's many squares.
         * This means that the pattern of immediate squares that can be moved to is
         * identical to that of the king.
         */
        private BitBoard getMoveableSquares(short xCoord, short yCoord)
        {
            BitBoard bit = BitBoard.Empty;

            if (Board.isSquareValid(xCoord + 1, yCoord + 1))
                bit.AddPosition(xCoord + 1, yCoord + 1);
            if (Board.isSquareValid(xCoord + 1, yCoord - 1))
                bit.AddPosition(xCoord + 1, yCoord - 1);
            if (Board.isSquareValid(xCoord - 1, yCoord + 1))
                bit.AddPosition(xCoord - 1, yCoord + 1);
            if (Board.isSquareValid(xCoord - 1, yCoord - 1))
                bit.AddPosition(xCoord - 1, yCoord - 1);
            if (Board.isSquareValid(xCoord, yCoord + 1))
                bit.AddPosition(xCoord, yCoord + 1);
            if (Board.isSquareValid(xCoord, yCoord - 1))
                bit.AddPosition(xCoord, yCoord - 1);
            if (Board.isSquareValid(xCoord - 1, yCoord))
                bit.AddPosition(xCoord - 1, yCoord);
            if (Board.isSquareValid(xCoord - 1, yCoord))
                bit.AddPosition(xCoord - 1, yCoord);

            /*possibleMoves[0] = new BoardSquare(xCoord + 1, yCoord + 1);
            possibleMoves[1] = new BoardSquare(xCoord + 1, yCoord - 1);
            possibleMoves[2] = new BoardSquare(xCoord - 1, yCoord + 1);
            possibleMoves[3] = new BoardSquare(xCoord - 1, yCoord - 1);
            possibleMoves[4] = new BoardSquare(xCoord, yCoord + 1);
            possibleMoves[5] = new BoardSquare(xCoord, yCoord - 1);
            possibleMoves[6] = new BoardSquare(xCoord + 1, yCoord);
            possibleMoves[7] = new BoardSquare(xCoord + 1, yCoord);*/

            return bit;
        }

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal override BitBoard getAllMoves(Board b, short xCoord, short yCoord)
        {
            BitBoard bit = b.getMoves(xCoord, yCoord, this.colour, true, true, 1);

            if (this.hasNotMoved)
            {
                if (!b.ActiveGame.IsInCheck())
                {
                    // Castle checking
                    // Queenside
                    if (b.ActiveGame.IsPieceFreeToMove(xCoord, yCoord, 0, yCoord) && b[yCoord, 0] != null
                        && b[yCoord, 0] is Rook && b[yCoord, 0].Colour == this.colour
                        && !b.ActiveGame.IsAttacked((short)(xCoord - 1), (short)yCoord, this.Colour == Piece.NOTATION_B ? Piece.NOTATION_W : Piece.NOTATION_B))
                        bit.AddPosition(xCoord - 2, yCoord);

                    // Kingside
                    if (b.ActiveGame.IsPieceFreeToMove(xCoord, yCoord, Board.NUM_FILES - 2, yCoord)
                        && b[yCoord, Board.NUM_FILES - 1] != null
                        && b[yCoord, Board.NUM_FILES - 1] is Rook && b[yCoord, Board.NUM_FILES - 1].Colour == this.colour
                        && !b.ActiveGame.IsAttacked((short)(xCoord + 1), (short)yCoord, this.Colour == Piece.NOTATION_B ? Piece.NOTATION_W : Piece.NOTATION_B))
                        bit.AddPosition(xCoord + 2, yCoord);
                }
            }

            foreach (BoardSquare sq in b)
            {
                Piece p = b[sq];
                if (p.Colour != this.colour)
                {
                    // Generate all the attacking moves for enemy pieces, remove them from king's moves
                    if (p is King)
                        bit &= ~((King)p).getMoveableSquares((short)sq.X, (short)sq.Y);
                    else
                    {
                        // TODO: Pawns don't validate if square is occupied. For this to work well, we need to artificially move the king to the square before calling this function
                        //bit &= ~p.getAttackMoves(b, (short)sq.X, (short)sq.Y);

                        bit &= ~p.getAllMoves(b, (short)sq.X, (short)sq.Y);
                    }

                    if (bit == BitBoard.Empty)
                        break;
                }
            }

            /*foreach (BoardSquare sq in bit)
            {
                if (b.GetGame().IsSquareUnderAttack(xCoord, yCoord, this.colour))
                    bit.RemovePosition(sq.X, sq.Y);
            }*/

            return bit;
        }
        #endregion
    }
}
