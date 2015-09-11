using System;
using System.Collections.Generic;
/* 
 * Since the movement of knights is not bound by pieces being in the way, many standard
 * checks throughout validation and detection involving scanning through lines of attack
 * ignore the movement of knights. This only happens in two methods, isPieceFreeToMove()
 * and isDiscoverCheck().
 * 
 * Since isPieceFreeToMove() checks for pieces in the way of an attempted movement, it
 * would always return true for a knight, where it is impossible to have pieces in the
 * way of movement.
 * 
 * Since discovery check involves the moving of a piece which is in the way of a clean
 * attack on a king, it is impossible for a knight to be the non-moved piece in a discovery
 * check.
 * 
 * Another key part of the checking knighyts are exempt from is path traversal (discussed
 * below), since their movement is not bound by pieces. This means all cases involving path
 * traversal (checking if pieces are in the way of moves, checking for movement uncovering
 * a line of attack) would never involve a knight.
 * 
 * Many times in this class, incrementers are used to traverse paths along the board. These
 * incrementers increment the X and Y coordinates appropriately, regardless of the direction
 * wished to go. These are set to 0 if the difference between the start and end points along
 * a given axis is 0, and otherwise they are set to the value of the difference divided by the
 * absolute value of the difference. In this case, they would be set to -1 if the difference
 * is negative, and 1 if the difference is positive.
 */
namespace ChessGameLib
{
    internal enum AttackTypes
    {
        regular = 0,
        discovery = 1
    }

    public struct GameResults
    {
        public const string WHITE_WIN = "1-0";
        public const string BLACK_WIN = "0-1";
        public const string DRAW = "1/2-1/2";
        public const string UNKNOWN = "";
    }

    public class Game
    {
        #region Variables and Constants
        /*
         * Boolean value determining whose turn it is.
         * 
         * A value of true means it is white's turn, a value of false means it is black's turn.
         * Its value is flipped every time the doTurn function is run. It is given readonly public
         * access.
         */
        private bool whiteTurn;
        public bool WhiteTurn
        {
            get { return whiteTurn; }
        }

        private string eventName;
        public string EventName
        {
            get { return this.eventName; }
            set { this.eventName = value; }
        }

        private string site;
        public string Site
        {
            get { return this.site; }
            set { this.site = value; }
        }

        /*
         * An 8x8 rectangular array of piece objects. Occupied squares hold their respective
         * pieces, while unoccupied squares are null.
         */
        internal Board board;
        public Board Layout
        {
            get { return this.board; }
            set
            {
                this.board = value;
                this.board.ActiveGame = this;
            }
        }

        

        /*
         * The last move made in the game at any given turn. The need for this variable is that
         * one move (en passant) requires the move prior to it to be a specific scenario. This is
         * stored every time the doTurn function is run. Publicly, it is given read-only status.
         */
        private ChessMove lastMove;
        public ChessMove LastMove
        {
            get { return this.lastMove; }
        }

        // A listing of all moves in the game, primarily for display purposes
        private List<ChessMove> allMoves;
        public List<ChessMove> Moves
        {
            get { return this.allMoves; }
        }

        /*
         * Collection of board positions.  These are kept for determining if a position has been
         * repeated n times, and it is cleared on pawn move or piece capture, since those movements
         * can not be undone.
         */
        private BoardPositions positions;

        /*
         * The starting and ending coordinates of the last attempted move. Its value is set every time
         * the isValidTurn function is run.
         */
        private BoardSquare startCoord;
        private BoardSquare endCoord;

        /*
         * A 2-D character array, storing the board coordinates of each side's kings. The position of
         * white's king are stored at index 0, and the coordinates of black's king are stored at an index
         * of 1.
         */
        private BoardSquare[] kingPos;

        /*
         * A 2-element array storing the X and Y coordinates relative to the board array (integer values)
         * of any piece forming a discovery check. It is set whenever it is discovered that discover check
         * has occured, and is cleared once the net turn occurs. This clearing point happens in the doTurn
         * funciton, between when it is checked that a move leaves a player in check and the checking for
         * attacks on the opponent that that move causes.
         */
        private BoardSquare[] attackingPieces;

        /*
         * An integer code that dictates the state of the entire game, such as whether a player is in check,
         * has been checkmated, whether a stalemate has occured, or wether a draw has occured. It is given
         * readonly public access
         */
        private GameStatusCodes gameStatus;
        public GameStatusCodes GameStatus
        {
            get { return gameStatus; }
        }

        /*
         * Players of white and black respectively in this game. Each is given readonly status,
         * as lpayers do not change mid-game
         */
        public readonly Player PlayerWhite;
        public readonly Player PlayerBlack;

        // Used in conjunction with GameStatusCode to determine who, if anyone, has won
        private bool blackWon;

        // Class constants
        private const short DRAW_LIMIT = 100;   // Number of plies before drawn
        #endregion
        #region Indexers
        /*
         * Get and Set the contents of a spot in the board array based on the 0-based row and
         * column number of the board. An IndexOutOfRangeException is thrown if the supplied values
         * are beyond allowable range.
         */
        public Piece this[int row, int col]
        {
            get
            {
                return board[row, col];
                //if (row < 0 || row >= Board.NUM_ROWS)
                //    throw new IndexOutOfRangeException("Row is outside the allowable range!");
                //else if (col < 0 || col > Board.NUM_FILES)
                //    throw new IndexOutOfRangeException("Column is outside the allowable range!");
                //else
                //    return board[row, col];
            }
            internal set
            {
                board[row, col] = value;
                //if (row < 0 || row >= Board.NUM_ROWS)
                //    throw new IndexOutOfRangeException("Row is outside the allowable range!");
                //else if (col < 0 || col > Board.NUM_FILES)
                //    throw new IndexOutOfRangeException("Column is outside the allowable range!");
                //else
                //    board[row, col] = value;
            }
        }

        public Piece this[char row, char col]
        {
            get
            {
                return board[row, col];
                //return this[Char.ToLower(row) - 'a', col - '1'];
            }
            internal set
            {
                board[row, col] = value;
                //this[Char.ToLower(row) - 'a', col - '0'] = value;
            }
        }
        #endregion
        #region Enums
        // Enumerated Constants indicating the status of the game
        public enum GameStatusCodes : short
        {
            normal = 0,
            check = 1,
            checkMate = 2,
            stalemate = 3,
            forcedDraw = 4,
            agreedDraw = 5,
            playerResign = 6,
            drawByMaterial = 7
        }

        private enum PieceCountIndices
        {
            pawn =0,
            knight=1,
            bishop=2,
            rook=3,
            queen=4
        }

        public enum Sides
        {
            white = 0,
            black = 1
        }

        #endregion
        #region ctor
        /*
         * Initialize all game-driven variables who's values must be retained for the duration
         * of the game, and are not set with every move (startXCoord, for example).
         */
        public Game(Player p1, Player p2)
        {
            whiteTurn = true;
            kingPos = new BoardSquare[2] { new BoardSquare(4, 0), new BoardSquare(4, 7) };
            gameStatus = GameStatusCodes.normal;
            attackingPieces = new BoardSquare[2] {
                BoardSquare.Empty,
                BoardSquare.Empty,
            };

            allMoves = new List<ChessMove>();
            positions = new BoardPositions(Game.DRAW_LIMIT, BoardPositions.DEFAULT_START);
            PlayerWhite = p1;
            PlayerBlack = p2;

            board = new Board(this);
            board[0, 0] = new Rook(Piece.NOTATION_W);
            board[0, 1] = new Knight(Piece.NOTATION_W);
            board[0, 2] = new Bishop(Piece.NOTATION_W);
            board[0, 3] = new Queen(Piece.NOTATION_W);
            board[0, 4] = new King(Piece.NOTATION_W);
            board[0, 5] = new Bishop(Piece.NOTATION_W);
            board[0, 6] = new Knight(Piece.NOTATION_W);
            board[0, 7] = new Rook(Piece.NOTATION_W);
            board[1, 0] = new Pawn(Piece.NOTATION_W);
            board[1, 1] = new Pawn(Piece.NOTATION_W);
            board[1, 2] = new Pawn(Piece.NOTATION_W);
            board[1, 3] = new Pawn(Piece.NOTATION_W);
            board[1, 4] = new Pawn(Piece.NOTATION_W);
            board[1, 5] = new Pawn(Piece.NOTATION_W);
            board[1, 6] = new Pawn(Piece.NOTATION_W);
            board[1, 7] = new Pawn(Piece.NOTATION_W);

            board[7, 0] = new Rook(Piece.NOTATION_B);
            board[7, 1] = new Knight(Piece.NOTATION_B);
            board[7, 2] = new Bishop(Piece.NOTATION_B);
            board[7, 3] = new Queen(Piece.NOTATION_B);
            board[7, 4] = new King(Piece.NOTATION_B);
            board[7, 5] = new Bishop(Piece.NOTATION_B);
            board[7, 6] = new Knight(Piece.NOTATION_B);
            board[7, 7] = new Rook(Piece.NOTATION_B);
            board[6, 0] = new Pawn(Piece.NOTATION_B);
            board[6, 1] = new Pawn(Piece.NOTATION_B);
            board[6, 2] = new Pawn(Piece.NOTATION_B);
            board[6, 3] = new Pawn(Piece.NOTATION_B);
            board[6, 4] = new Pawn(Piece.NOTATION_B);
            board[6, 5] = new Pawn(Piece.NOTATION_B);
            board[6, 6] = new Pawn(Piece.NOTATION_B);
            board[6, 7] = new Pawn(Piece.NOTATION_B);
        }
        #endregion
        #region Public Methods
        public void SetPositionFromString(string s, SerializationTypes type)
        {
            ISerializer serializer = GameSerializerFactory.GetInstance().GetSerializer(type);
            this.Layout = (Board)serializer.Deserialize(s);

            // Update internal trackers
            foreach (BoardSquare sq in this.Layout) {
                Piece p = this[sq.Y, sq.X];
                if (p is King)
                    this.kingPos[Convert.ToInt32(p.Colour == Piece.NOTATION_B)] = sq;
            }

            this.allMoves.Clear();
            this.attackingPieces[0] = BoardSquare.Empty;
            this.attackingPieces[1] = BoardSquare.Empty;
        }

        internal bool IsInCheck()
        {
            return attackingPieces[(int)AttackTypes.discovery] != BoardSquare.Empty
                    || attackingPieces[(int)AttackTypes.regular] != BoardSquare.Empty;
        }

        public void SetPromotedValue(BoardSquare square, Piece newPiece)
        {
            if (square.Y == (whiteTurn ? 0 : Board.NUM_ROWS - 1))
            {
                Piece endSquare = board[square];
                if (endSquare != null && endSquare is Pawn)
                {
                    board[square] = newPiece;
                    IsGameOver(ChessMove.MoveStatusCodes.pawnPromote, this.endCoord);
                }
            }
        }

        /*
         * The actions that take place when a player is checkmated.
         * 
         * First, the game status is upated, followed by the ratings of each player.
         */
        public void CheckMate(bool blackWon, GameStatusCodes gameStatus)
        {
            this.gameStatus = gameStatus;
            int player1Rating = PlayerWhite.Rating;
            this.blackWon = blackWon;

            if (blackWon)
            {
                PlayerWhite.CalcNewELO(PlayerBlack.Rating, 0);
                PlayerBlack.CalcNewELO(player1Rating, 1);
            }
            else
            {
                PlayerWhite.CalcNewELO(PlayerBlack.Rating, 1);
                PlayerBlack.CalcNewELO(player1Rating, 0);
            }
        }

        /*
         * The actions that take place when a draw occurs
         * 
         * First, the game status is upated to the code passed through as a parameter.
         * Then, the player ratings are updated accordingly.
         */
        public void Draw(GameStatusCodes reasonForDraw)
        {
            gameStatus = reasonForDraw;
            int player1Rating = PlayerWhite.Rating;
            PlayerWhite.CalcNewELO(PlayerBlack.Rating, 0.5);
            PlayerBlack.CalcNewELO(player1Rating, 0.5);
        }

        /*
         * Determines if the last move caused the game to end
         * 
         * First, check if the king is in check, and store it in a boolean variable.
         * 
         * If the king is in check, check if the attack is preventable. This is done
         * by seeing it the king can either move away safely or can neutralize all
         * attacks (both the primary attack and discoveryCheck if it occured). If the
         * king is in check and the conditions checking if the attempted capture
         * is preventable, checkmate has occured, otherwise the game is not over and the
         * King is simply in check.
         * 
         * If the king is not in check, determine if any of the various draw conditions are
         * met, including by repetition, redundancy (x moves without a capture or pawn movement),
         * stalemate (where a player can not make a legal move) and by material (where neither side
         * has enough material to checkmate).
         * 
         * If none of those conditions are met, the game can still be played, and is
         * considered to have normal status.
         */
        private void IsGameOver(ChessMove.MoveStatusCodes moveType, BoardSquare directAttacker)
        {
            // Check if king is in check (discovered, normal)
            // Be sure to account for rook when castled, removed pawn in en passant

            // If one or the other, check for blocks, capture, move out of way
            // If both, check move out of way
            // If neither, check other pieces can move (stalemate)
            // Check draw by repetition, inaction (no capture/movement), and material

            // Set gameStatus internal variable as appropriate

            BoardSquare currKingPos = this.kingPos[(int)(whiteTurn ? Sides.white : Sides.black)];
            BitBoard futureMoves = this.board[directAttacker].getAllMoves(this.board, (short)directAttacker.X, (short)directAttacker.Y);
            bool isDirectCheck = futureMoves.Contains(currKingPos);
            bool isDiscoverCheck;

            this.attackingPieces[(int)AttackTypes.discovery] = ExtrapolateAttack((short)this.startCoord.X, (short)this.startCoord.Y, (short)currKingPos.X, (short)currKingPos.Y);
            this.attackingPieces[(int)AttackTypes.regular] = isDirectCheck ? endCoord : BoardSquare.Empty;
            
            isDiscoverCheck = this.attackingPieces[(int)AttackTypes.discovery] != BoardSquare.Empty;

            if (!isDiscoverCheck)
            {
                BoardSquare lastKingPos = this.kingPos[(int)(whiteTurn ? Sides.black : Sides.white)];

                if (moveType == ChessMove.MoveStatusCodes.queenCastle)
                {
                    isDiscoverCheck = this[lastKingPos.Y, lastKingPos.X + 1].getAllMoves(this.board, (short)(this.endCoord.X + 1), (short)this.endCoord.Y).Contains(currKingPos);
                    if (isDiscoverCheck)
                        this.attackingPieces[(int)AttackTypes.discovery] = new BoardSquare(lastKingPos.X + 1, lastKingPos.Y);
                }
                else if (moveType == ChessMove.MoveStatusCodes.queenCastle)
                {
                    isDiscoverCheck = this[lastKingPos.Y, lastKingPos.X - 1].getAllMoves(this.board, (short)(this.endCoord.X - 1), (short)this.endCoord.Y).Contains(currKingPos);
                    if (isDiscoverCheck)
                        this.attackingPieces[(int)AttackTypes.discovery] = new BoardSquare(lastKingPos.X - 1, lastKingPos.Y);
                }
                else if (moveType == ChessMove.MoveStatusCodes.enPassant)
                { 
                    // Check captured square
                    this.attackingPieces[(int)AttackTypes.discovery] = ExtrapolateAttack((short)endCoord.X, (short)startCoord.Y, (short)currKingPos.X, (short)currKingPos.Y);
                    isDiscoverCheck = this.attackingPieces[(int)AttackTypes.discovery] != BoardSquare.Empty;
                }
            }
            
            if (isDirectCheck || isDiscoverCheck)
            {
                this.gameStatus = GameStatusCodes.check;

                // If king can't move safely, check alternatives
                if (this.board[currKingPos].getAllMoves(this.board, (short)currKingPos.X, (short)currKingPos.Y) == BitBoard.Empty)
                {
                    // Double attack, can only get out of it by moving
                    if (isDirectCheck && isDiscoverCheck)
                        this.gameStatus = GameStatusCodes.checkMate;
                    else
                    {
                        BoardSquare attackSquare = BoardSquare.Empty;

                        bool canBlockCap = false;

                        if (isDirectCheck) attackSquare = this.attackingPieces[(int)AttackTypes.regular];
                        else attackSquare = this.attackingPieces[(int)AttackTypes.discovery];

                        // Check for blocking, capture
                        foreach (BoardSquare sq in this.board)
                        {
                            Piece p = this.board[sq];

                            if (p.Colour == this.board[currKingPos].Colour)
                            {
                                BitBoard bbMoves = p.getAllMoves(this.board, (short)sq.X, (short)sq.Y);
                                foreach (BoardSquare sqMove in bbMoves)
                                {
                                    // Can block or capture
                                    if (sqMove == attackSquare || (!(board[sq] is King) && sqMove.IsBetweenPoints(currKingPos, attackSquare))
                                        || board[sq] is Pawn && ((Pawn)board[sq]).MayEnPassant(this.board, (short)sq.X, (short)sq.Y, (short)sqMove.X, (short)sqMove.Y))
                                    {
                                        canBlockCap = true;
                                        break;
                                    }
                                }

                                if (canBlockCap)
                                    break;
                            }
                        }

                        if (!canBlockCap)
                            this.gameStatus = GameStatusCodes.checkMate;
                    }
                }
            }
            else
            {
                gameStatus = GameStatusCodes.normal;

                //Check for draw conditions (stalemate, repetition, inactivity (no capture/pawn), material
                if (this.positions.isDrawByMoves())
                    this.gameStatus = GameStatusCodes.forcedDraw;
                else if (this.lastMove.IsCapture && IsDrawByMaterial())
                    this.gameStatus = GameStatusCodes.drawByMaterial;
                else
                {
                    bool isStalemate = true;
                    char nextMoveCol = whiteTurn ? Piece.NOTATION_W : Piece.NOTATION_B;

                    foreach (BoardSquare sq in this.board)
                        if (this.board[sq].Colour == nextMoveCol && this.board[sq].getAllMoves(this.board, (short)sq.X, (short)sq.Y) != BitBoard.Empty)
                        {
                            isStalemate = false;
                            break;
                        }

                    if (isStalemate)
                        this.gameStatus = GameStatusCodes.stalemate;
                }
            }
        }

        /*
         * Based on the current position and attempted new position of a piece, determines if the move
         * is valid. Owing to View-Layer checking, it is known that the starting coordinate holds a piece
         * pf the colour who's turn it is to move, and the ending coordinate does not hold a piece of the
         * same colour.
         * 
         * Checking starts off by taking the two board coordinates, passed in as 2 character arrays, and
         * converting them to integers so they can be accurately used to reference the 2-D board array
         * holding all the pieces.
         * 
         * First to be checked is if performing the move would leave the player in check. Since a cardinal
         * rule of chess involves not ending your turn while you yourself are in check, if true is returned by
         * this function, it is an invalid move, all other checking is ignored, and the appropriate flag is
         * returned.
         * 
         * Next check to see if the move is a traditional movement of a piece by checking if the square it's moving
         * to is unoccupied, if the movement follows that piece's traditional movement pattern, and that no pieces
         * lie in the way of the movement. If it is, it is a valid movement, and some piece-specific checking occurs.
         * If it was a king , indicate that he has moved (meaning that he can no longer castle). If it was a pawn, reset
         * the draw counters for draw by repetition and movement, and check and see if the pawn has reached the far side
         * of the board. If it did, perform the turn and return the appropriate flag. If not, piece specific checking
         * ends, the turn is performed, and the movement is a typical movement.
         * 
         * If the movement is not a typical movement, checking occurs to see if it is a traditional capture. This means
         * that the square the piece is moving to is occupied, the movement follows the piece's traditional capture
         * movement pattern, and that there are no pieces in the way. If it is, the same actions occur here as do if
         * a regular movement has occured, with the exception that the draw counters are always reset, instead of just
         * if a pawn was moved.
         * 
         * If none of the above conditions are met, exception checking occurs.
         * First, if the piece moved was a pawn, checking for en Passant is dealt with. If the movement was valid as
         * being en Passant, reset the draw counters, set the square contents of the captured piece to null manually
         * (since the square was never clicked on, doTurn can not handle doing that), perform the turn, and return
         * the appropriate flag.
         * Second, checking for castling occurs, by seeing if the square moved to is null and castling is allowed for
         * that piece. If it is, the current rank and file of the involved rook is calculated, as is the ending
         * X-coordinate of the rook. Then it is tested if the involved rook is actually on the square that it
         * needs to be on, that there is nothing in between the rook and king, and that the square the
         * king is moving over on it's way to it's ending position is not under attack. If all of these conditions
         * are met, it is a valid castling, the turn is performed, and the appropriate value is returned.
         * 
         * If none of the conditions are met, it is an invalid move, and a flag is returned indicating this.
         */
        public ChessMove.MoveStatusCodes IsValidTurn(char[] currPos, char[] newPos)
        {
            startCoord = new BoardSquare(currPos[0], currPos[1]);
            endCoord = new BoardSquare(newPos[0], newPos[1]);
            Piece startPiece = board[startCoord];
            Piece endPiece = board[endCoord];
            ChessMove.MoveStatusCodes resultant = ChessMove.MoveStatusCodes.invalidMove;

            // Clicked on own piece, nothing in way of desired move
            if (startPiece != null && startPiece.Colour == (whiteTurn ? Piece.NOTATION_W : Piece.NOTATION_B) && (endPiece == null || endPiece.Colour != startPiece.Colour) /* && IsPieceFreeToMove((short)startCoord.X, (short)startCoord.Y, (short)endCoord.X, (short)endCoord.Y) */)
            {
                BitBoard b = startPiece.getAllMoves(this.board, (short)startCoord.X, (short)startCoord.Y);

                if (b.Contains(endCoord))
                {
                    resultant = ChessMove.MoveStatusCodes.normalMovement;

                    if (startPiece is King)
                    {
                        if (startCoord.X - endCoord.X == -2)
                            resultant = ChessMove.MoveStatusCodes.kingCastle;
                        else if (startCoord.X - endCoord.X == 2)
                            resultant = ChessMove.MoveStatusCodes.queenCastle;
                    }
                    else if (startPiece is Pawn)
                    {
                        if (endCoord.Y == ((Pawn)startPiece).GetLastRow())
                            resultant = ChessMove.MoveStatusCodes.pawnPromote;
                        else if (endCoord.X - startCoord.X != 0 && this[endCoord.Y, endCoord.X] == null)
                            resultant = ChessMove.MoveStatusCodes.enPassant;
                    }

                    DoTurn(resultant);
                }
            }

            return resultant;
        }

        /*
         * Get the result of this game as a string decision. The first number is for white, the second
         * is for black, and they follow the expected format of a win being 1, a loss being 0, and a
         * draw being 1/2. Therefore, a win for white is "1-0", a win for black is "0-1", and a draw
         * is "1/2-1/2". Unknown or ongoing status is registered as an empty string.
         */
        public string GetResultString()
        {
            string returnStr;

            switch (this.gameStatus)
            {
                case GameStatusCodes.checkMate:
                case GameStatusCodes.playerResign:
                    returnStr = this.blackWon ? GameResults.BLACK_WIN : GameResults.WHITE_WIN;
                    break;
                case GameStatusCodes.stalemate:
                case GameStatusCodes.forcedDraw:
                case GameStatusCodes.drawByMaterial:
                case GameStatusCodes.agreedDraw:
                    returnStr = GameResults.DRAW;
                    break;
                default:
                    returnStr = GameResults.UNKNOWN;
                    break;
            }

            return returnStr;
        }

        public BoardSquare GetKingLocation(Game.Sides side)
        {
            return this.kingPos[(int)side];
        }

        public BoardSquare[] GetAttackers()
        {
            return this.attackingPieces;
        }

        #endregion
        #region Private Methods
        /*
         * Standard set of actions that occur when a valid turn takes takes place.
         * 
         * The two arguments, currPos and newPos, are character representations
         * of the original and new positions of the moved piece respectively
         * 
         * First, the board array is updated.
         * Second, switch internal track of move, and record the last move.
         * Third, update the array of repeatable board positions, clear discoverCheckPiece, and increment the move count for draws
         * Fourth of all, if the piece that moved was the king, update his board position in the array
         * Lastly, check to see if this move causes the game to end
         */
        private void DoTurn(ChessMove.MoveStatusCodes moveType)
        {
            BoardSquare directAttacker = endCoord;

            switch (moveType)
            {
                case ChessMove.MoveStatusCodes.enPassant:
                    board[lastMove.EndSquare.Y, lastMove.EndSquare.X] = null;
                    break;
                case ChessMove.MoveStatusCodes.kingCastle:
                    board.movePiece(Board.NUM_FILES-1, startCoord.Y, startCoord.X + 1, startCoord.Y);
                    directAttacker = new BoardSquare(startCoord.X + 1, startCoord.Y);
                    break;
                case ChessMove.MoveStatusCodes.queenCastle:
                    board.movePiece(0, startCoord.Y, startCoord.X - 1, startCoord.Y);
                    directAttacker = new BoardSquare(startCoord.X - 1, startCoord.Y);
                    break;
            }

            // Update internal board position and move trackers
            lastMove = new ChessMove(startCoord, endCoord, board[startCoord].Notational, board[endCoord] != null);
            allMoves.Add(lastMove);

            board.movePiece(startCoord.X, startCoord.Y, endCoord.X, endCoord.Y);

            // Switch who's move it is and record move
            whiteTurn = !whiteTurn;

            // Update internal move-by-move trackers
            positions.add(lastMove, board);

            // Update position of king if king moved
            if (lastMove.PieceMoved == (char)Piece.PieceNotation.King)
            {
                kingPos[Convert.ToInt32(whiteTurn)] = endCoord;
                ((King)board[endCoord]).HasNotMoved = false;
            }

            if (moveType != ChessMove.MoveStatusCodes.pawnPromote)
                IsGameOver(moveType, directAttacker);
        }

        /*
         * Checks if a given square is under attack by an opposing piece
         * 
         * Takes the colour of the piece who's turn it is to move (as pulled from the kingPos array)
         * and checks every square in every row for a piece that is the opposite colour that has a
         * clean line of attack on testSquare.
         */
        internal bool IsAttacked(short xCoord, short yCoord, char colourOfAttacker)
        {
            for (short y = 0; y < Board.NUM_ROWS; y++)
                for (short x = 0; x < Board.NUM_ROWS; x++)
                    if (board[y, x] != null && board[y, x].Colour == colourOfAttacker && board[y, x].isValidCapture(x, y, xCoord, yCoord) && IsPieceFreeToMove(x, y, xCoord, yCoord))
                        return true;

            return false;
        }

        /*
         * Determines if either side has insufficient material to force checkmate
         * 
         * This function cycles through the board array, returning true if it comes
         * to any material which could be used in conjunction with the King to
         * checkmate. Since the King will always be on the board, it is not included
         * in the checking.
         * 
         * Individual pieces which are viable to checkmate with include a pawn (pawn
         * promotion), rook, and queen. It is also possible to force a checkmate with
         * 2 bishops.
         */
        private bool IsDrawByMaterial()
        {
            bool[] bishopsFound = new bool[] { false, false };

            for (short y = 0; y < Board.NUM_ROWS; y++)
                for (short x = 0; x < Board.NUM_FILES; x++)
                    if (board[y, x] != null)
                    {
                        if (board[y, x] is Pawn || board[y, x] is Rook || board[y, x] is Queen)
                            return false;
                        else if (board[y, x] is Bishop)
                        {
                            if (bishopsFound[Convert.ToInt32(board[y, x].Colour != Piece.NOTATION_W)])
                                return false;
                            else
                                bishopsFound[Convert.ToInt32(board[y, x].Colour != Piece.NOTATION_W)] = true;
                        }
                    }

            return true;
        }

        /*
         * Checks if the movement of a piece potentially uncovers an attack on the king.
         * 
         * The toCheckIndex argument is the key in the array at which to check
         * if they are a victim of discovery check. This will be the king of the
         * opposite colour of that which just moved. The next four parameters are
         * simply the X and Y coordinates of the piece which just moved. The final
         * parameter indicates whether to store the attacking piece or not.
         * 
         * This can be used to check for pins or discover checks by altering the toCheckIndex
         * parameter to indicate either the black or white King dependant on the turn. If the
         * indicated value is for the colour who has just attempted a move, it checks for pins.
         * Otherwise, it checks for Discovery checks.
         */
        /*private bool ExtrapolateCheck(int toCheckIndex, short startXCoord, short startYCoord, short endXCoord, short endYCoord, bool storeAttacker)
        {
            BoardSquare attacker = ExtrapolateAttack(startXCoord, startYCoord, (short)kingPos[toCheckIndex].X, (short)kingPos[toCheckIndex].Y);
            bool returnVal = attacker != BoardSquare.Empty && board[attacker].Colour != board[new BoardSquare(startXCoord, startYCoord)].Colour;

            if (returnVal && storeAttacker)
            {
                attackingPieces[(int)AttackTypes.discovery] = attacker;
            }

            return returnVal;
        }*/

        /*
         * Checks if the movement of a piece is in between another piece and a king
         * 
         * The startXCoord and statYCoord arguments are the argument for the piece in the
         * middle. The targetXCoord and targetYCoord arguments are for the king in question to check.
         * The return value is a Bitbord representation holding the attacking piece.
         * 
         * This function makes no checks to see if the piece found is of the same colour as the test piece,
         * nor does it compare the colours of the target and test pieces. BoardSquare.Empty is returned
         * if no attackis found
         * 
         * This can be used to check for pins or discover checks by altering the toCheckIndex
         * parameter to indicate either the black or white King dependant on the turn. If the
         * indicated value is for the colour who has just attempted a move, it checks for pins.
         * Otherwise, it checks for Discovery checks.
         */
        internal BoardSquare ExtrapolateAttack(short testXCoord, short testYCoord, short targetXCoord, short targetYCoord)
        {
            BoardSquare attackSquare = BoardSquare.Empty;

            // Starting position and target position form a straight, unimpeded line
            if ((testXCoord != targetXCoord || testYCoord != targetYCoord) && IsPieceFreeToMove(testXCoord, testYCoord, targetXCoord, targetYCoord))
            {
                // Each will be 0, 1, or -1
                short incrX = (short)(testXCoord == targetXCoord ? 0 : ((testXCoord - targetXCoord) / Math.Abs(testXCoord - targetXCoord)));
                short incrY = (short)(testYCoord == targetYCoord ? 0 : ((testYCoord - targetYCoord) / Math.Abs(testYCoord - targetYCoord)));

                // Current extrapolated square is inside the board and is not the piece's end square
                while (attackSquare == BoardSquare.Empty && board.isValidSquare(testXCoord += incrX, testYCoord += incrY))
                {
                    // Has come across a different piece
                    if (this[testYCoord, testXCoord] != null)
                    {
                        // Piece is a different colour from the King and can attack
                        if (this[testYCoord, testXCoord].isValidCapture(testXCoord, testYCoord, targetXCoord, targetYCoord) && this[testYCoord, testXCoord].Colour != this[targetYCoord, targetXCoord].Colour)
                            attackSquare.setCoords(testYCoord, testXCoord);

                        break;
                    }
                }
            }

            return attackSquare;
        }

        /*
         * Determines if a piece can perform an unimpeded movement
         * 
         * Traverse the path from the starting coordinate to the ending coordinate
         * for all piece movements involving diagonal, horizontal, and vertical
         * movement patterns. That is to say, any non-knight movements.
         * 
         * For each movement type, cycle through the squares, incrementing the appropriate
         * variable each iteration through. Note that it only determines if a piece is in the way
         * for general movement. To determine if movement conforms to a specific piece, the
         * appropriate isValidMovement or isValidCapture function must be called.
         */
        internal bool IsPieceFreeToMove(short startXCoord, short startYCoord, short endXCoord, short endYCoord)
        {
            short diffX = (short)(endXCoord - startXCoord);
            short diffY = (short)(endYCoord - startYCoord);
            short incrX = (short)(diffX == 0 ? 0 : diffX / Math.Abs(diffX));
            short incrY = (short)(diffY == 0 ? 0 : diffY / Math.Abs(diffY));
            bool isValid = true;

            if (Math.Abs(diffX) == Math.Abs(diffY)) // Check for diagonal movement
                while (isValid && (startXCoord += incrX) != endXCoord && (startYCoord += incrY) != endYCoord)
                    isValid = board[startYCoord, startXCoord] == null;
            else if (diffY == 0) // Check for horizontal movement
                while (isValid && (startXCoord += incrX) != endXCoord)
                    isValid = board[startYCoord, startXCoord] == null;
            else if (diffX == 0) // Check for vertical movement
                while (isValid && (startYCoord += incrY) != endYCoord)
                    isValid = board[startYCoord, startXCoord] == null;
            else
                isValid = board[startYCoord, startXCoord] is Knight;

            return isValid;
        }
        #endregion
    }
}
