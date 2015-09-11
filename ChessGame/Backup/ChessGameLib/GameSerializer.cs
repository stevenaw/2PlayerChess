using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ChessGameLib
{
    public enum SerializationModes
    {
        Input,
        Output
    }

    public enum SerializationTypes
    {
        PGN,
        FEN
    }

    public interface ISerializer
    {
        string Serialize(Object obj);
        Object Deserialize(string s);
        bool IsValidSerialization(string s);
    }

    public interface ISerializer<T>
    {
        string Serialize(T obj);
        T Deserialize(string s);
        bool IsValidSerialization(string s);
    }

    public class GameSerializerFactory
    {
        private static volatile GameSerializerFactory _instance = null;
        private GameSerializerFactory() { }

        public static GameSerializerFactory GetInstance() {
            if (_instance == null)
                _instance = new GameSerializerFactory();
            return _instance;
        }

        public ISerializer GetSerializer(SerializationTypes type)
        {
            switch (type)
            {
                case SerializationTypes.PGN:
                    return new PGNSerializer();
                case SerializationTypes.FEN:
                default:
                    return new FENSerializer();
            }
        }
    }

    public class FENSerializer : ISerializer<Board>, ISerializer
    {
        private const int MAX_LEN_FEN = 71;
        private const char FEN_DELIM = '/';
        public const string FEN_START = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

        /*
         * Generates a string representation of the board position following FEN Diagram rules
         * 
         * These rules involve having a slash to indicate a new row, and rows are processed from high
         * to low (black's home row to white's home row). Each piece is represented by its algebraic
         * notational character, with case indicative of color (white is uppercase, black is lowercase)
         */
        public string Serialize(Board obj)
        {
            char[] endResult = new char[MAX_LEN_FEN];
            int currIndex = 0;
            int numBlanks = 0;
            Piece p;

            for (int row = Board.NUM_ROWS - 1; row >= 0; row--)
            {
                for (int col = 0; col < Board.NUM_FILES; col++)
                {
                    p = obj.ActiveGame[row, col];

                    // If not piece, increment numBlanks
                    if (p == null)
                        numBlanks++;
                    else
                    {
                        // Output blanks before piece if necessary
                        if (numBlanks != 0)
                        {
                            endResult[currIndex++] = ((char)(numBlanks + BoardSquare.YCOORD_OFFSET - 1));
                            numBlanks = 0;
                        }

                        // Output piece, black as lower case
                        if (p.Colour == Piece.NOTATION_B)
                            endResult[currIndex++] = Char.ToLower(p.Notational);
                        else
                            endResult[currIndex++] = Char.ToUpper(p.Notational);
                    }
                }

                // If blanks to output at end of line, output
                if (numBlanks != 0)
                {
                    endResult[currIndex++] = ((char)(numBlanks + BoardSquare.YCOORD_OFFSET - 1));
                    numBlanks = 0;
                }

                // Add line delimiter if not last row
                if (row != 0)
                    endResult[currIndex++] = FEN_DELIM;
            }

            // Return as a string
            return (new String(endResult, 0, currIndex));
        }
        public string Serialize(Object o) { return this.Serialize(o as Board); }

        Object ISerializer.Deserialize(string s) { return this.Deserialize(s); }
        public Board Deserialize(string s)
        {
            Board g = new Board(null);
            int currRow = Board.NUM_ROWS - 1;
            int currFile = 0;
            int currPiece = 0;
            char currPieceCol = '\0';
            char lastSymbol = '-';  // Initialize to unexpected symbol, similar to null for objects
            // Checks are based on assuming it is valid, allows bypass for
            // first round

            if (String.IsNullOrEmpty(s))
                throw new FENException("String can not be empty!", -1);

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == FEN_DELIM)
                {
                    if (currFile != Board.NUM_FILES)
                        throw new FENException("Unanticipated new row, columns to fill!", i);
                    else if (--currRow < 0)
                        throw new FENException("Too many rows!", i);

                    currFile = 0;
                }
                else if (Char.IsDigit(s[i]))
                {
                    int gap = s[i] - '0';

                    if (Char.IsDigit(lastSymbol))
                        throw new FENException("Can not specify two consecutive blank spots!", i);
                    else if (currFile + gap > Board.NUM_FILES)
                        throw new FENException("Blank spot exceeds column count!", i);
                    else if (gap == 0)
                        throw new FENException("Can not have a 0 in a FEN diagram!", i);

                    while (gap-- > 0)
                        g[currRow, currFile++] = null;
                }
                else if (Char.IsLetter(s[i]))
                {
                    currPiece = Convert.ToInt32(Char.ToUpper(s[i]));
                    currPieceCol = Char.IsUpper(s[i]) ? Piece.NOTATION_W : Piece.NOTATION_B;

                    if (currFile >= Board.NUM_FILES)
                        throw new FENException("Too many pieces specified in row!", i);

                    switch (currPiece)
                    {
                        case (int)Piece.PieceNotation.Bishop:
                            g[currRow, currFile++] = new Bishop(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Pawn:
                            g[currRow, currFile++] = new Pawn(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.King:
                            g[currRow, currFile++] = new King(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Knight:
                            g[currRow, currFile++] = new Knight(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Queen:
                            g[currRow, currFile++] = new Queen(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Rook:
                            g[currRow, currFile++] = new Rook(currPieceCol);
                            break;
                        default:
                            throw new FENException("Unsupported piece notation: " + s[i], i);
                    }
                }
                else
                    throw new FENException("Unsupported symbol: " + s[i], i);

                lastSymbol = s[i];
            }

            if (currRow != 0)
                throw new FENException("Not enough rows specified!", -1);
            else if (currFile != 8)
                throw new FENException("Not enough columns specified!", -1);
            else
                return g;
        }

        public bool IsValidSerialization(string s)
        {
            try
            {
                Deserialize(s);
                return true;
            }
            catch (FENException)
            {
                return false;
            }
        }
    }

    public class PGNSerializer : ISerializer<Game>, ISerializer
    {
        private const int MAX_PGN_LINE = 80;
        private const string PGN_DATE_FMT = "yyyy.MM.dd";

        public string Serialize(Game g)
        {
            // currMove can handle a 4-digit move number, followed by a period and a space
            char[] currMove = new char[ChessMove.MAX_MOVE_CHARS + 6];
            char[] line = new char[MAX_PGN_LINE];
            StringBuilder s = new StringBuilder();

            int currIndex = 0;
            int moveNum = 1;
            bool isFirstPly = true;

            // Write out event-specific things
            s.AppendLine("[Event \"" + g.EventName + "\"]");
            s.AppendLine("[Site \"" + g.Site + "\"]");
            s.AppendLine("[Date \"" + DateTime.Now.ToString(PGN_DATE_FMT) + "\"]");
            s.AppendLine("[Round \"\"]");

            s.AppendLine("[White \"" + g.PlayerWhite.Name + "\"]");
            s.AppendLine("[Black \"" + g.PlayerBlack.Name + "\"]");
            s.AppendLine("[Result \"" + g.GetResultString() + "\"]");

            // Write out moves
            foreach (ChessMove c in g.Moves)
            {
                // Create move notation
                if (isFirstPly)
                {
                    currMove = (moveNum.ToString() + ". " + c.ToString()).ToCharArray();
                    moveNum++;
                }
                else
                    currMove = c.ToString().ToCharArray();

                // No more room in line to prepend, write out line
                if (currMove.Length + currIndex > line.Length)
                {
                    s.AppendLine(new string(line, 0, currIndex));
                    line = new char[MAX_PGN_LINE];
                    currIndex = 0;
                }

                // Add to line
                currMove.CopyTo(line, currIndex);
                currIndex += currMove.Length;
                line[currIndex++] = ' ';
            }

            s.AppendLine(new string(line, 0, currIndex));

            // Clear buffer and write out last of moves
            return s.ToString();
        }
        public string Serialize(Object o) { return this.Serialize(o as Game); }

        Object ISerializer.Deserialize(string s) { return this.Deserialize(s); }
        public Game Deserialize(string s)
        {
            return null;
        }

        public bool IsValidSerialization(string s)
        {
            return false;
        }
    }

    
    public class GameSerializer
    {/*
        #region Constants
        private const int MAX_LEN_FEN = 71;
        private const int MAX_PGN_LINE = 80;
        private const char FEN_DELIM = '/';
        private const string PGN_DATE_FMT = "yyyy.MM.dd";
        public const string FEN_START = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        #endregion*/
        #region Methods

        public void ToStream(ISerializer ser, Object o, TextWriter writer)
        {
            try
            {
                // Format, write and close
                writer.Write(ser.Serialize(o));
                writer.Flush();
            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }
        }

        public Object FromStream(ISerializer ser, TextReader reader)
        {
            string result = String.Empty;

            try
            {
                // Generate string and write to file
                result = reader.ReadLine();
            }
            finally
            {
                reader.Close();
                reader.Dispose();
            }

            return ser.Deserialize(result);
        }

        /*
         * Generates a string representation of the board position following FEN Diagram rules
         * 
         * These rules involve having a slash to indicate a new row, and rows are processed from high
         * to low (black's home row to white's home row). Each piece is represented by its algebraic
         * notational character, with case indicative of color (white is uppercase, black is lowercase)
         */
        /*
        public string ToFEN(Game g)
        {
            char[] endResult = new char[MAX_LEN_FEN];
            int currIndex = 0;
            int numBlanks = 0;
            Piece p;

            for (int row = Board.NUM_ROWS - 1; row >= 0; row--)
            {
                for (int col = 0; col < Board.NUM_FILES; col++)
                {
                    p = g[row, col];

                    // If not piece, increment numBlanks
                    if (p == null)
                        numBlanks++;
                    else
                    {
                        // Output blanks before piece if necessary
                        if (numBlanks != 0)
                        {
                            endResult[currIndex++] = ((char)(numBlanks + BoardSquare.YCOORD_OFFSET - 1));
                            numBlanks = 0;
                        }

                        // Output piece, black as lower case
                        if (p.Colour == Piece.NOTATION_B)
                            endResult[currIndex++] = Char.ToLower(p.Notational);
                        else
                            endResult[currIndex++] = Char.ToUpper(p.Notational);
                    }
                }

                // If blanks to output at end of line, output
                if (numBlanks != 0)
                {
                    endResult[currIndex++] = ((char)(numBlanks + BoardSquare.YCOORD_OFFSET - 1));
                    numBlanks = 0;
                }

                // Add line delimiter if not last row
                if (row != 0)
                    endResult[currIndex++] = FEN_DELIM;
            }

            // Return as a string
            return (new String(endResult, 0, currIndex));
        }

        public string IsValidFEN(string fenString)
        {
            int currRow = Board.NUM_ROWS - 1;
            int currFile = 0;
            int currPiece = 0;
            char lastSymbol = '-';  // Initialize to unexpected symbol, similar to null for objects
            // Checks are based on assuming it is valid, allows bypass for
            // first round

            for (int i = 0; i < fenString.Length; i++)
            {
                if (fenString[i] == GameSerializer.FEN_DELIM)
                {
                    if (currFile != Board.NUM_FILES)
                        return "Unanticipated new row, columns to fill!";
                    else if (currRow < 0)
                        return "Too many rows!";

                    currRow--;
                    currFile = 0;
                }
                else if (Char.IsDigit(fenString[i]))
                {
                    int gap = fenString[i] - '0';

                    if (Char.IsDigit(lastSymbol))
                        return "Can not specify two consecutive blank spots!";
                    else if (currFile + gap > Board.NUM_FILES)
                        return "Blank spot exceeds column count!";
                    else if (gap == 0)
                        return "Can not have a 0 in a FEN diagram!";

                    currFile += gap;
                }
                else if (Char.IsLetter(fenString[i]))
                {
                    currPiece = Convert.ToInt32(Char.ToUpper(fenString[i]));

                    switch (currPiece)
                    {
                        case (int)Piece.PieceNotation.Bishop:
                            break;
                        case (int)Piece.PieceNotation.Pawn:
                            break;
                        case (int)Piece.PieceNotation.King:
                            break;
                        case (int)Piece.PieceNotation.Knight:
                            break;
                        case (int)Piece.PieceNotation.Queen:
                            break;
                        case (int)Piece.PieceNotation.Rook:
                            break;
                        default:
                            return "Invalid piece :" + fenString[i];
                    }

                    currFile++;
                }
                else
                    return "Invalid token :" + fenString[i];
            }

            if (currRow != 0)
                return "Not enough rows specified!";
            else if (currFile != 8)
                return "Not enough columns specified!";
            else
                return "";
        }

        // Interpret a board position from a FEN string and populate board accordingly
        public void FromFEN(string fen, ref Game g)
        {
            int currRow = Board.NUM_ROWS - 1;
            int currFile = 0;
            int currPiece = 0;
            char currPieceCol = '\0';
            char lastSymbol = '-';  // Initialize to unexpected symbol, similar to null for objects
                                    // Checks are based on assuming it is valid, allows bypass for
                                    // first round

            if (g == null)
                throw new FENException("Game can not be null!");

            for (int i = 0; i < fen.Length; i++)
            {
                if (fen[i] == GameSerializer.FEN_DELIM)
                {
                    if (currFile != Board.NUM_FILES)
                        throw new FENException("Unanticipated new row, columns to fill!");
                    else if (currRow < 0)
                        throw new FENException("Too many rows!");

                    currRow--;
                    currFile = 0;
                }
                else if (Char.IsDigit(fen[i]))
                {
                    int gap = fen[i] - '0';

                    if (Char.IsDigit(lastSymbol))
                        throw new FENException("Can not specify two consecutive blank spots!");
                    else if (currFile + gap > Board.NUM_FILES)
                        throw new FENException("Blank spot exceeds column count!");
                    else if (gap == 0)
                        throw new FENException("Can not have a 0 in a FEN diagram!");

                    while (gap-- > 0)
                        g[currRow, currFile++] = null;
                }
                else if (Char.IsLetter(fen[i]))
                {
                    currPiece = Convert.ToInt32(Char.ToUpper(fen[i]));
                    currPieceCol = Char.IsUpper(fen[i]) ? Piece.NOTATION_W : Piece.NOTATION_B;

                    switch (currPiece)
                    {
                        case (int)Piece.PieceNotation.Bishop:
                            g[currRow, currFile++] = new Bishop(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Pawn:
                            g[currRow, currFile++] = new Pawn(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.King:
                            g[currRow, currFile++] = new King(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Knight:
                            g[currRow, currFile++] = new Knight(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Queen:
                            g[currRow, currFile++] = new Queen(currPieceCol);
                            break;
                        case (int)Piece.PieceNotation.Rook:
                            g[currRow, currFile++] = new Rook(currPieceCol);
                            break;
                        default:
                            throw new FENException(fen[i]);
                    }
                }
                else
                    throw new FENException(fen[i]);

                lastSymbol = fen[i];
            }

            if (currRow != 0)
                throw new FENException( "Not enough rows specified!");
            else if (currFile != 8)
                throw new FENException("Not enough columns specified!");
        }

        
        public string ToPGN(Game g, string eventName, string siteName)
        {
            // currMove can handle a 4-digit move number, followed by a period and a space
            char[] currMove = new char[ChessMove.MAX_MOVE_CHARS + 6];
            char[] line = new char[MAX_PGN_LINE];
            StringBuilder s = new StringBuilder();

            int currIndex = 0;
            int moveNum = 1;
            bool isFirstPly = true;

            // Write out event-specific things
            s.AppendLine("[Event \"" + eventName + "\"]");
            s.AppendLine("[Site \"" + siteName + "\"]");
            s.AppendLine("[Date \"" + DateTime.Now.ToString(PGN_DATE_FMT) + "\"]");
            s.AppendLine("[Round \"\"]");

            s.AppendLine("[White \"" + g.PlayerWhite.Name + "\"]");
            s.AppendLine("[Black \"" + g.PlayerBlack.Name + "\"]");
            s.AppendLine("[Result \"" + g.GetResultString() + "\"]");

            // Write out moves
            foreach (ChessMove c in g.Moves)
            {
                // Create move notation
                if (isFirstPly)
                {
                    currMove = (moveNum.ToString() + ". " + c.ToString()).ToCharArray();
                    moveNum++;
                }
                else
                    currMove = c.ToString().ToCharArray();

                // No more room in line to prepend, write out line
                if (currMove.Length + currIndex > line.Length)
                {
                    s.AppendLine(new string(line, 0, currIndex));
                    line = new char[MAX_PGN_LINE];
                    currIndex = 0;
                }

                // Add to line
                currMove.CopyTo(line, currIndex);
                currIndex += currMove.Length;
                line[currIndex++] = ' ';
            }

            s.AppendLine(new string(line, 0, currIndex));

            // Clear buffer and write out last of moves
            return s.ToString();
        }*/
        #endregion
    }
}
