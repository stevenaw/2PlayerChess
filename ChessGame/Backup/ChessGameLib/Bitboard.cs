using System;
using System.Collections.Generic;
using System.Collections;

namespace ChessGameLib
{
    public struct BitBoard : IEnumerable<BoardSquare>
    {
        private long positions;

        #region ctor
        public BitBoard(long b)
        {
            this.positions = b;
        }
        #endregion

        public static BitBoard Empty
        {
            get { return new BitBoard(0); }
        }

        #region Operations
        public void AddPosition(int x, int y)
        {
            this.positions |= (1L << (y * Board.NUM_ROWS + x));
        }

        public void RemovePosition(int x, int y)
        {
            this.positions &= ~(1L << (y * Board.NUM_ROWS + x));
        }

        public bool Contains(int x, int y)
        {
            return this.Contains(1L << (y * Board.NUM_ROWS + x));
        }

        public bool Contains(long pos)
        {
            return (this.positions & pos) != 0;
        }

        public bool Contains(BoardSquare b)
        {
            return this.Contains(b.X, b.Y);
        }
        #endregion

        #region Operators
        public static bool operator ==(BitBoard obj1, BitBoard obj2)
        {
            return obj1.positions == obj2.positions;
        }

        public static bool operator !=(BitBoard obj1, BitBoard obj2)
        {
            return !(obj1 == obj2);
        }

        public static BitBoard operator +(BitBoard obj1, BoardSquare square)
        {
            BitBoard b = new BitBoard(obj1.positions);
            b.AddPosition(square.X, square.Y);
            return b;
        }

        public static BitBoard operator -(BitBoard obj1, BoardSquare square)
        {
            BitBoard b = new BitBoard(obj1.positions);
            b.RemovePosition(square.X, square.Y);
            return b;
        }

        public static BitBoard operator |(BitBoard obj1, BitBoard obj2)
        {
            BitBoard b = new BitBoard(obj1.positions);
            b.positions |= obj2.positions;
            return b;
        }

        public static BitBoard operator &(BitBoard obj1, BitBoard obj2)
        {
            BitBoard b = new BitBoard(obj1.positions);
            b.positions &= obj2.positions;
            return b;
        }

        public static BitBoard operator ~(BitBoard obj1)
        {
            return new BitBoard(~obj1.positions);
        }
        #endregion

        #region Functions
        public int Count()
        {
            long remaining = this.positions;
            int count = 0;
            while (remaining != 0)
            {
                remaining &= (remaining - 1);
                count++;
            }
            return count;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BitBoard))
                return false;
            return this == (BitBoard)obj;
        }

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        public IEnumerator<BoardSquare> GetEnumerator()
        {
            long temp = this.positions;

            for (int i = 0; i < 64 && temp != 0; i++)
            {
                if ((temp & 1) != 0)
                    yield return new BoardSquare(i % Board.NUM_ROWS, i / Board.NUM_ROWS);

                temp >>= 1;
            }
        }

        public List<BoardSquare> Squares
        {
            get
            {
                long temp = this.positions;
                List<BoardSquare> squares = new List<BoardSquare>();

                for (int i = 0; i < 64 && temp != 0; i++)
                {
                    if ((temp & 1) != 0)
                        squares.Add(new BoardSquare(i % Board.NUM_ROWS, i / Board.NUM_ROWS));

                    temp >>= 1;
                }

                return squares;
            }
        }

        #endregion
    }
}
