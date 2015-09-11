using System;

namespace ChessGameLib
{
    public class Pawn : Piece
    {
        #region Constants
        private const int MAX_NUM_ADJ_SQ = 3;
        #endregion
        #region ctor
        public Pawn(char col)
            : base(col, (ushort)Piece.PieceValues.pawn, (char)Piece.PieceNotation.Pawn, MAX_NUM_ADJ_SQ, "Pawn")
        {
        }
        #endregion
        #region Public Methods
        /*
         * Pawns can move 1 space forward, 2 if they are in their starting positions.
         * Their traditional piece movement does not allow moving sideways in any way.
         * 
         * Check first if the x coordinate has changed. If it has, it is not a valid move.
         * If only the y-coordinate has changed, it has moved directly forward, which is
         * when range and directional checking occurs. White pawns have a higher number as
         * their new Y-coordinate, creating a negative difference. Black pawns have a
         * lower number as their new coordinate, creating a positive difference. The
         * starting row for a black pawn is 7, and it is 2 for a white pawn.
         */
        public override bool isValidMovement(short startX, short startY, short endX, short endY)
        {
            if (startX == endX)
            {
                int diff = startY - endY;
                if (colour == Piece.NOTATION_W && (diff == -1 || diff == -2 && startY == 1))
                    return true;
                else if (colour == Piece.NOTATION_B && (diff == 1 || diff == 2 && startY == 6))
                    return true;
            }
            return false;
        }

        /*
         * Pawns attack diagonally, 1 square to the side and 1 square in the direction 
         * their colour dictates as proper.
         */
        public override bool isValidCapture(short startX, short startY, short endX, short endY)
        {
            return (Math.Abs(startX - endX) == 1 && startY - endY == (this.colour == Piece.NOTATION_W ? -1 : 1));
        }

        /*
         * The immediate squares a pawn can move to are the one directly in it's path,
         * and the squares on either side.
         */
        /*public override BoardSquare[] getMoveableSquares(short xCoord, short yCoord)
        {
            possibleMoves[0] = new BoardSquare(xCoord, colour == Piece.NOTATION_W ? yCoord + 1 : yCoord - 1);
            possibleMoves[1] = new BoardSquare(possibleMoves[0].X - 1, possibleMoves[0].Y);
            possibleMoves[2] = new BoardSquare(possibleMoves[0].X - 1, possibleMoves[0].Y);

            return possibleMoves;
        }*/

        public int GetStartRow()
        {
            return this.colour == Piece.NOTATION_W ? 1 : Board.NUM_ROWS - 2;
        }

        public int GetLastRow()
        {
            return this.colour == Piece.NOTATION_W ? Board.NUM_ROWS - 1 : 0;
        }

        internal override BitBoard getAttackMoves(Board b, short xCoord, short yCoord)
        {
            BitBoard bit = BitBoard.Empty;
            int row = colour == Piece.NOTATION_W ? yCoord + 1 : yCoord - 1;
            bit.AddPosition(xCoord - 1, row);
            bit.AddPosition(xCoord + 1, row);
            return bit;
        }

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal override BitBoard getAllMoves(Board b, short xCoord, short yCoord)
        {
            BitBoard bit = BitBoard.Empty;

            BoardSquare ownKing = b.GetKingLocation(this.colour == Piece.NOTATION_W ? Game.Sides.white : Game.Sides.black);
            BoardSquare potentialPinner = b.ExtrapolateAttack(xCoord, yCoord, (short)ownKing.X, (short)ownKing.Y);
            int sign = this.colour == Piece.NOTATION_W ? 1 : -1;

            // if piece is not pinned, check normal movement
            if (potentialPinner == BoardSquare.Empty || potentialPinner.X == xCoord)
            {
                if (b[yCoord + sign, xCoord] == null) {
                    bit.AddPosition(xCoord, yCoord + sign);

                    if (yCoord == this.GetStartRow() && b[yCoord + sign * 2, xCoord] == null)
                        bit.AddPosition(xCoord, yCoord + sign * 2);
                }
            }

            // Check for attacks on either side
            if (potentialPinner == BoardSquare.Empty || new BoardSquare(xCoord + 1, yCoord + sign).IsBetweenPoints(potentialPinner, ownKing))
            {
                if (b.isValidSquare(xCoord + 1, yCoord +  sign) &&
                    (b[yCoord + sign, xCoord + 1] == null ? MayEnPassant(b, xCoord, yCoord, (short)(xCoord + 1), (short)(yCoord + sign)) : b[yCoord + sign, xCoord + 1].Colour != this.colour))
                    bit.AddPosition(xCoord + 1, yCoord + sign);
            }

            if (potentialPinner == BoardSquare.Empty || new BoardSquare(xCoord - 1, yCoord + sign).IsBetweenPoints(potentialPinner, ownKing))
            {
                if (b.isValidSquare(xCoord - 1, yCoord + sign) &&
                    (b[yCoord + sign, xCoord - 1] == null ? MayEnPassant(b, xCoord, yCoord, (short)(xCoord - 1), (short)(yCoord + sign)) : b[yCoord + sign, xCoord - 1].Colour != this.colour))
                    bit.AddPosition(xCoord - 1, yCoord + sign);
            }

            return bit;
        }

        /*
         * Determines if a pawn move adheres to the en Passant rules
         * 
         * Based the start and end coordinates and piece involved in the last move, determine if the
         * movement is a valid attempt at en Passant. A situation in en Passant is valid is when the
         * last move involved moving a pawn up 2 spaces from the starting rank, and having the final
         * position be right next to an enemy pawn. Can assume capturing square is empty if last pawn
         * movement is valid for en passant.
         */
        public bool MayEnPassant(Board board, short startX, short startY, short endX, short endY)
        {
            ChessMove lastMove = board.ActiveGame.LastMove;
            return (lastMove != null && lastMove.PieceMoved == (char)Piece.PieceNotation.Pawn && Math.Abs(lastMove.EndSquare.Y - lastMove.StartSquare.Y) == 2           // Last move was a pawn moving 2 spaces
                && board[startY, startX].Notational == (char)Piece.PieceNotation.Pawn && board[startY, startX].isValidCapture(startX, startY, endX, endY) // This move was a pawn doing capture movement
                && lastMove.EndSquare.Y == startY && endX == lastMove.EndSquare.X);                                                                      // Capture landed right behind other pawn
        }
        #endregion
    }
}
