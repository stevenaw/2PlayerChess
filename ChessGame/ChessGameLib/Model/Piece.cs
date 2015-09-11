using System;

namespace ChessGameLib
{
    public abstract class Piece
    {
        #region Enums
        /* The values of the pieces, rounded, according to Larry Evans
         * 
         * Traditionally the King is not assigned a value (or has infinite value),
         * but here it's been assigned a value of 50, which is greater than the
         * combined total of all other pieces of the same color.
         */
        public enum PieceValues : ushort
        {
            pawn = 1,
            knight = 3,
            bishop = 4,
            rook = 5,
            queen = 10,
            king = 50
        }

        // ASCII-codes representative of any piece type in algebraic Notation
        public enum PieceNotation
        {
            Pawn = 80,      // P
            Knight = 78,    // N
            Bishop = 66,    // B
            Rook = 82,      // R
            Queen = 81,     // Q
            King = 75       // K
        }
        #endregion
        #region Constants and Variables
        public const char NOTATION_W = 'w';     // White piece colour notation
        public const char NOTATION_B = 'b';     // Black piece colour notation

        // The value for any specific instance of Piece
        protected readonly ushort value;
        public ushort Value
        {
            get { return value; }
        }

        // An ASCII character representation of the piece instance's color
        protected readonly char colour;
        public char Colour
        {
            get { return colour; }
        }

        // Algebraic notation symbol of this piece instance
        protected readonly char notational;
        public char Notational
        {
            get { return notational; }
        }

        // Algebraic notation symbol of this piece instance
        protected readonly string name;
        public string Name
        {
            get { return name; }
        }

        /*
         * Array of the immediately possible squares a piece can move to
         * 
         * At most, it will hold 8 sub-arrays (as is the case for the
         * Queen, King, and Knight), and at a minimum it will hold 3 (as is the
         * case of the Pawn). The rook and bishop's will each hold 4 sub-arrays.
         * 
         * It's length is set in the constructor, and it is populated in 
         * getMoveableSquares, where it is returned to the calling function. Each
         * sub-array will hold a 2-element coordinate set.
         */
        //protected BoardSquare[] possibleMoves;
        #endregion
        #region ctor
        public Piece(char col, ushort value, char notational, int maxNumMoves, string name)
        {
            this.colour = col;
            this.value = value;
            this.notational = notational;
            //this.possibleMoves = new BoardSquare[maxNumMoves];
            this.name = name;
        }
        #endregion
        #region Public Methods
        /*
         * Based on the new position and current possition of the piece, determines
         * if the coordinate change follows the piece's standard pattern of attack.
         * For all pieces except for the pawn, this pattern is identical to
         * isValidCapture(), and simply returns the result of that.
         */
        public virtual bool isValidCapture(short startX, short startY, short endX, short endY)
        {
            return isValidMovement(startX, startY, endX, endY);
        }

        /*
         * Based on the new position and current possition of the piece, determines
         * if the coordinate change follows the piece's standard pattern of movement.
         */
        public abstract bool isValidMovement(short startX, short startY, short endX, short endY);

        /*
         * Get the bitboard form of all the valid moves for the given board and start position.
         */
        internal abstract BitBoard getAllMoves(Board b, short xCoord, short yCoord);

        /*
         * Based on the position passed through, will return an array of 2-element
         * character arrays, each holding the coordinates of a square on the board.
         */
        //public abstract BoardSquare[] getMoveableSquares(short xCoord, short yCoord);

        internal virtual BitBoard getAttackMoves(Board b, short xCoord, short yCoord)
        {
            return getAllMoves(b, xCoord, yCoord);
        }

        public static bool AreEqual(Piece a, Piece b)
        {
            if (a == null && b == null)
                return true;
            else if (a == null ^ b == null)
                return false;
            else
                return a.colour == b.colour && a.GetType() == b.GetType(); 
        }

        #endregion
    }
}
