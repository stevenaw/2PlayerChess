using System;
using System.Collections.Generic;

namespace ChessGameLib
{
    public struct BoardSquare
    {
        #region Constants and Variables
        // Masks for extracting coordinate portions from the internal bit field
        //private const int MASK_Y = 0x00F0;
        //private const int MASK_X = 0x000F;

        // Values for bitwise shifting the internal bit field to extract coordinate portions
        //private const int BWSHIFT_X = 0;
        //private const int BWSHIFT_Y = 4;

        private const int EMPTY_INDICATOR = -1;

        /*
         * Provides the difference between a character representation of the X-coordinate of a square on
         * a board (as noted in chess notation as a, b, c, ... g, h). This is subtracted from the
         * appropriate character value, to get the desired integer value. This is 0-indexed (a value of
         * 'a' will yield an integer value of 0).
         */
        public const short XCOORD_OFFSET = (int)'a';

        /*
         * Provides the difference between a character representation of the Y-coordinate of a square on
         * a board (as noted in chess notation as 1, 2, 3, ... 7, 8). This is subtracted from the
         * appropriate character value, to get the desired integer value. This is set so that a value of
         * 1 will yield an integer value of 0, since 1 is the first file, and the game holds the pieces in
         * a 0-based array.
         */
        public const short YCOORD_OFFSET = (int)'1';

        // Internal storage of the coords
        private byte coords;
        
        public int X
        {
            get { return this.coords % Board.NUM_FILES; }
            set { this.coords = (byte)(this.Y * Board.NUM_ROWS + value); }

            //set { this.coords = (byte)(Math.Floor(this.coords / (double)Board.NUM_ROWS) + value); }
            //get { return (this.coords & MASK_X) >> BWSHIFT_X; }
            //set { this.coords |= (byte)((value << BWSHIFT_X) & MASK_X); }
        }

        public int Y
        {
            get { return ((short)this.coords) / Board.NUM_ROWS; }
            set { this.coords = (byte)(this.X + value * Board.NUM_ROWS); }

            //get { return (this.coords & MASK_Y) >> BWSHIFT_Y; }
            //set { this.coords |= (byte)((value << BWSHIFT_Y) & MASK_Y); }
        }

        public char AlphaX
        {
            get { return (char)(this.X + XCOORD_OFFSET); }
            //set { this.X = Char.ToLower(value) - XCOORD_OFFSET; }
        }

        public char AlphaY
        {
            get { return (char)(this.Y + YCOORD_OFFSET); }
            //set { this.Y = Char.ToLower(value) - YCOORD_OFFSET; }
        }

        public void setCoords(char rank, char file)
        {
            this.setCoords(rank - YCOORD_OFFSET, Char.ToLower(file) - XCOORD_OFFSET);
        }

        public void setCoords(int row, int column)
        {
            this.coords = (byte)(row * Board.NUM_ROWS + column);
        }

        public static BoardSquare Empty
        {
            get { return new BoardSquare(EMPTY_INDICATOR, EMPTY_INDICATOR); }
        }
        #endregion

        #region ctor

        public BoardSquare(int x, int y)
        {
            this.coords = 0;
            this.setCoords(y, x);
        }

        public BoardSquare(char x, char y)
        {
            this.coords = 0;
            this.setCoords(y, x);
        }

        #endregion

        #region Operators
        public static bool operator ==(BoardSquare obj1, BoardSquare obj2)
        {
            return obj1.coords == obj2.coords;
        }

        public static bool operator !=(BoardSquare obj1, BoardSquare obj2)
        {
            return (obj1.coords != obj2.coords);
        }
        #endregion

        #region Functions

        public bool IsBetweenPoints(BoardSquare endPointA, BoardSquare endPointB)
        {
            BoardSquare maxVals = BoardSquare.Empty, minVals = BoardSquare.Empty;

            maxVals.X = Math.Max(endPointA.X, endPointB.X);
            maxVals.Y = Math.Max(endPointA.Y, endPointB.Y);
            minVals.X = Math.Min(endPointA.X, endPointB.X);
            minVals.Y = Math.Min(endPointA.Y, endPointB.Y);

            /*if (endPointA.X > endPointB.X)
            {
                maxVals.X = endPointA.X;
                minVals.X = endPointB.X;
            }
            else
            {
                minVals.X = endPointA.X;
                maxVals.X = endPointB.X;
            }

            if (endPointA.Y > endPointB.Y)
            {
                maxVals.Y = endPointA.Y;
                minVals.Y = endPointB.Y;
            }
            else
            {
                minVals.Y = endPointA.Y;
                maxVals.Y = endPointB.Y;
            }*/

            if (this.X < minVals.X || this.X > maxVals.X || this.Y < minVals.Y || this.Y > maxVals.Y)
                return false;

            // Avoid divide by zero, the line goes straight up
            if (endPointB.X == endPointA.X)
            {
                return endPointB.X == endPointB.X;
            }
            else
            {
                double slope = (endPointB.Y - endPointA.Y) / (endPointB.X - endPointA.X);

                // The line is not a clear diagonal or straight across
                if (slope != 0 && Math.Abs(slope) != 1)
                    return false;
                else
                {
                    double yInt = endPointB.Y - slope * endPointB.X;
                    return (this.Y == slope * this.X + yInt);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BoardSquare))
                return false;
            return this == (BoardSquare)obj;
        }

        public override int GetHashCode()
        {
            return (int)this.coords;
        }

        #endregion
    }
}
