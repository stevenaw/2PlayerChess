using System;
using System.Collections.Generic;
using System.Collections;

namespace ChessGameLib
{
    public class Board : IEnumerable<BoardSquare>
    {
        public const short NUM_ROWS = 8;        // Number of rows on the board
        public const short NUM_FILES = 8;       // Number of columns on the board
        public const int NUM_SQUARES = Board.NUM_ROWS * Board.NUM_FILES; // Number of squares on the board

        private Piece[,] pieces = new Piece[Board.NUM_ROWS, Board.NUM_FILES];
        private Game g = null;

        public Game ActiveGame
        {
            internal set { this.g = value; }
            get { return this.g; }
        }

        /*public Game GetGame()
        {
            return this.g;
        }*/

        public Board(Game g)
        {
            this.g = g;
        }

        /*
         * Get and Set the contents of a spot in the board array based on the character coordinates
         * passed through. Parameters are the board coordinates as expected by algebraic notation. That
         * is to say, lowercase alpha as the first and numeric alpha as the second. An
         * IndexOutOfRangeException is thrown if this is not the case.
         */
        public Piece this[char row, char col]
        {
            get
            {
                return this[row - BoardSquare.YCOORD_OFFSET, Char.ToLower(col) - BoardSquare.XCOORD_OFFSET];
            }
            set
            {
                this[row - BoardSquare.YCOORD_OFFSET, Char.ToLower(col) - BoardSquare.XCOORD_OFFSET] = value;
            }
        }

        /*
         * Get and Set the contents of a spot in the board array based on the 0-based row and
         * column number of the board. An IndexOutOfRangeException is thrown if the supplied values
         * are beyond allowable range.
         */
        public Piece this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= Board.NUM_ROWS)
                    throw new IndexOutOfRangeException("Row is outside the allowable range!");
                else if (col < 0 || col >= Board.NUM_FILES)
                    throw new IndexOutOfRangeException("Column is outside the allowable range!");
                else
                    return pieces[row, col];
            }
            set
            {
                if (row < 0 || row >= Board.NUM_ROWS)
                    throw new IndexOutOfRangeException("Row is outside the allowable range!");
                else if (col < 0 || col >= Board.NUM_FILES)
                    throw new IndexOutOfRangeException("Column is outside the allowable range!");
                else
                    pieces[row, col] = value;
            }
        }

        public void movePiece(int startX, int startY, int endX, int endY)
        {
            this[endY, endX] = this[startY, startX];
            this[startY, startX] = null;
        }

        public BoardSquare GetKingLocation(Game.Sides side)
        {
            return g.GetKingLocation(side);
        }

        public BoardSquare ExtrapolateAttack(short testXCoord, short testYCoord, short targetXCoord, short targetYCoord)
        {
            return g.ExtrapolateAttack(testXCoord, testYCoord, targetXCoord, targetYCoord);
        }

        public bool isValidSquare(int col, int row)
        {
            return row >= 0 && row < Board.NUM_ROWS && col >= 0 && col < Board.NUM_FILES;
        }

        public static bool isSquareValid(int col, int row)
        {
            return row >= 0 && row < Board.NUM_ROWS && col >= 0 && col < Board.NUM_FILES;
        }

        internal BitBoard extrapolateMoves(int x, int y, char colour, int incrX, int incrY, int maxSquares)
        {
            BitBoard bit = BitBoard.Empty;

            int testX = x + incrX;
            int testY = y + incrY;

            for (int i = maxSquares; i > 0; i--)
            {
                if (this.isValidSquare(testX, testY)
                    && (this[testY, testX] == null || this[testY, testX].Colour != this[y, x].Colour))
                    bit.AddPosition(testX, testY);
                else
                    break;

                if (this[testY, testX] != null)
                    break;

                testX += incrX;
                testY += incrY;
            }

            return bit;
        }

        internal BitBoard getMoves(short xCoord, short yCoord, char colour, bool moveDiag, bool moveParallel, int maxSquares)
        {
            BitBoard bit = BitBoard.Empty;

            BoardSquare ownKing = GetKingLocation(colour == Piece.NOTATION_W ? Game.Sides.white : Game.Sides.black);
            BoardSquare potentialPinner = ExtrapolateAttack(xCoord, yCoord, (short)ownKing.X, (short)ownKing.Y);

            // if piece is not pinned
            if (potentialPinner == BoardSquare.Empty)
            {
                if (moveDiag)
                {
                    bit |= extrapolateMoves(xCoord, yCoord, colour, 1, 1, maxSquares);
                    bit |= extrapolateMoves(xCoord, yCoord, colour, -1, -1, maxSquares);
                    bit |= extrapolateMoves(xCoord, yCoord, colour, 1, -1, maxSquares);
                    bit |= extrapolateMoves(xCoord, yCoord, colour, -1, 1, maxSquares);
                }

                if (moveParallel)
                {
                    bit |= extrapolateMoves(xCoord, yCoord, colour, 1, 0, maxSquares);
                    bit |= extrapolateMoves(xCoord, yCoord, colour, -1, 0, maxSquares);
                    bit |= extrapolateMoves(xCoord, yCoord, colour, 0, 1, maxSquares);
                    bit |= extrapolateMoves(xCoord, yCoord, colour, 0, -1, maxSquares);
                }
            }
            else
            {
                int incrX = 0;
                int incrY = 0;
                
                if (ownKing.X != potentialPinner.X)
                    incrX = potentialPinner.X > ownKing.X ? 1 : -1;

                if (ownKing.Y != potentialPinner.Y)
                    incrY = potentialPinner.Y > ownKing.Y ? 1 : -1;

                bit |= extrapolateMoves(xCoord, yCoord, colour, incrX, incrY, maxSquares);
                bit |= extrapolateMoves(xCoord, yCoord, colour, incrX * -1, incrY * -1, maxSquares);
            }

            return bit;
        }

        public Piece this[BoardSquare square]
        {
            get
            {
                return this[square.Y, square.X];
            }
            set
            {
                this[square.Y, square.X] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        public IEnumerator<BoardSquare> GetEnumerator()
        {
            for (int i = 0; i < NUM_ROWS; i++)
                for (int j = 0; j < NUM_FILES; j++)
                    if (this.pieces[i, j] != null)
                        yield return new BoardSquare(j, i);
        }

        public override string ToString()
        {
            return new FENSerializer().Serialize(this);
        }
    }
}
