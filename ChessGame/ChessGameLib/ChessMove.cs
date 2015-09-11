using System;
using System.Collections.Generic;

namespace ChessGameLib
{
    /* Move class
     * 
     * Encapsulates the results of a move. Internally-speaking, every instance of this class
     * is assumed to contain valid information upon instantiation.
     */
    public class ChessMove
    {
        #region Constants and Variables
        private const int BWSHIFT_PIECE = 16;
        private const int BWSHIFT_START = 0;
        private const int BWSHIFT_END = 8;
        public const int MAX_MOVE_CHARS = 8;

        private const string CASTLE_KING = "0-0";
        private const string CASTLE_QUEEN = "0-0-0";

        private BoardSquare startSquare;
        public BoardSquare StartSquare
        {
            get { return this.startSquare; }
            set { this.startSquare = value; }
        }

        private BoardSquare endSquare;
        public BoardSquare EndSquare
        {
            get { return this.endSquare; }
            set { this.endSquare = value; }
        }

        private bool isCapture;
        public bool IsCapture
        {
            get { return this.isCapture; }
        }

        private char pieceMoved;
        public char PieceMoved
        {
            get { return this.pieceMoved; }
        }

        private char checkIndicator;

        private string castleNotation;
        private char promotedPiece;
        #endregion

        #region Enums
        // Enumerated constants indicating the type or validity of a move
        public enum MoveStatusCodes : short
        {
            endedWhileCheck = 100,
            pawnPromote = 101,
            normalMovement = 102,
            enPassant = 103,
            queenCastle = 104,
            kingCastle = 105,
            invalidMove = 106
        }
        #endregion
        #region ctor
        public ChessMove(BoardSquare start, BoardSquare end, char pieceMoved, bool isCapture) : this (start, end, pieceMoved, isCapture, null)
        {
        }

        public ChessMove(BoardSquare start, BoardSquare end, char pieceMoved, bool isCapture, Piece promotedPiece)
        {
            this.startSquare = start;
            this.endSquare = end;
            this.pieceMoved = pieceMoved;
            this.checkIndicator = '\0';
            this.isCapture = isCapture;

            if (promotedPiece != null)
                this.promotedPiece = promotedPiece.Notational;

            // Determine if the move resulted in castling
            if (this.pieceMoved == (char)Piece.PieceNotation.King && start.X - end.X == 2)
                this.castleNotation = CASTLE_QUEEN;
            else if (this.pieceMoved == (char)Piece.PieceNotation.King && start.X - end.X == -2)
                this.castleNotation = CASTLE_KING;
            else
                this.castleNotation = String.Empty;
        }

        #endregion
        #region Methods
        public override bool Equals(object obj)
        {
            ChessMove m = obj as ChessMove;
            return m != null && m.startSquare == this.startSquare && m.endSquare == this.endSquare && this.pieceMoved == m.pieceMoved && this.checkIndicator == m.checkIndicator && this.isCapture == m.isCapture && this.promotedPiece == m.promotedPiece;
        }

        public override int GetHashCode()
        {
            return (this.pieceMoved << BWSHIFT_PIECE) & (this.startSquare.GetHashCode() << BWSHIFT_START) & (this.endSquare.GetHashCode() << BWSHIFT_END);
        }

        /*
         * Returns the last move as formatted in long-hand algebraic notation
         * 
         * In general it is simply the starting coordinate and ending coordinate delimited
         * by a dash (an 'x' if a capture occured), with the piece's symbol at the beginning.
         * Castling kingside returns a symbol of "0-0", while queenside castling returns a
         * symbol of "0-0-0".
         */
        public override string ToString()
        {
            if (castleNotation != String.Empty)
                return castleNotation;
            else
            {
                char[] chars = new char[MAX_MOVE_CHARS];
                int offset = 0;

                if (pieceMoved != '\0')
                    chars[offset++] = pieceMoved;

                chars[offset++] = this.startSquare.AlphaX;
                chars[offset++] = this.startSquare.AlphaY;
                chars[offset++] = this.IsCapture ? 'x' : '-';
                chars[offset++] = this.endSquare.AlphaX;
                chars[offset++] = this.endSquare.AlphaY;

                if(this.promotedPiece != '\0') {
                    chars[offset++] = '=';
                    chars[offset++] = this.promotedPiece;
                }

                if (checkIndicator != '\0')
                    chars[offset++] = this.checkIndicator;

                return (new string(chars)).TrimEnd('\0');
            }
        }
        #endregion
    }
}
