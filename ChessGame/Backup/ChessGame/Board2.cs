using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ChessGameLib;

namespace ChessGame
{
    /*
     * This class handles the View-Layer logic for the main game interface.
     */
    public partial class Board2 : Form
    {
        /*
         * 2-D character array of the board coordinates for the starting and
         * ending coordinates of an attempt at a move.
         */
        private char[][] move = { new char[2], new char[2] };

        /*
         * Boolean value indicating if a given click is an attempt at starting
         * a move or finishing a move.
         */
        private bool startMove = true;

        /*
         * Since the same event (mousedown) handles all piece interaction, a
         * copy of whichever panel is clicked on is stored internally to be
         * referenced later, once another panel is clicked.
         */
        private Panel startSquare;

        // Players of white and black respectively
        private Player player1;
        private Player player2;

        /*
         * The game object storing all the back-end logic for movement validation
         */
        private Game game;

        /*
         * A rectangular array holding the controls for each square on the board
         */
        private Panel[,] squares = new Panel[8, 8];

        /*
         * A dialog-box which is used to save game data (PGN or FEN format)
         */
        FileDialog saveDiag = new SaveFileDialog();
        FileDialog openDiag = new OpenFileDialog();

        public Board2(Game g) : this(g, FENSerializer.FEN_START)
        {
        }

        public Board2(Game g, string fenString)
        {
            InitializeComponent();
            game = g;
            player1 = g.PlayerWhite;
            player2 = g.PlayerBlack;

            squares[0, 0] = pnlA1;
            squares[1, 0] = pnlB1;
            squares[2, 0] = pnlC1;
            squares[3, 0] = pnlD1;
            squares[4, 0] = pnlE1;
            squares[5, 0] = pnlF1;
            squares[6, 0] = pnlG1;
            squares[7, 0] = pnlH1;

            squares[0, 1] = pnlA2;
            squares[1, 1] = pnlB2;
            squares[2, 1] = pnlC2;
            squares[3, 1] = pnlD2;
            squares[4, 1] = pnlE2;
            squares[5, 1] = pnlF2;
            squares[6, 1] = pnlG2;
            squares[7, 1] = pnlH2;

            squares[0, 2] = pnlA3;
            squares[1, 2] = pnlB3;
            squares[2, 2] = pnlC3;
            squares[3, 2] = pnlD3;
            squares[4, 2] = pnlE3;
            squares[5, 2] = pnlF3;
            squares[6, 2] = pnlG3;
            squares[7, 2] = pnlH3;

            squares[0, 3] = pnlA4;
            squares[1, 3] = pnlB4;
            squares[2, 3] = pnlC4;
            squares[3, 3] = pnlD4;
            squares[4, 3] = pnlE4;
            squares[5, 3] = pnlF4;
            squares[6, 3] = pnlG4;
            squares[7, 3] = pnlH4;

            squares[0, 4] = pnlA5;
            squares[1, 4] = pnlB5;
            squares[2, 4] = pnlC5;
            squares[3, 4] = pnlD5;
            squares[4, 4] = pnlE5;
            squares[5, 4] = pnlF5;
            squares[6, 4] = pnlG5;
            squares[7, 4] = pnlH5;

            squares[0, 5] = pnlA6;
            squares[1, 5] = pnlB6;
            squares[2, 5] = pnlC6;
            squares[3, 5] = pnlD6;
            squares[4, 5] = pnlE6;
            squares[5, 5] = pnlF6;
            squares[6, 5] = pnlG6;
            squares[7, 5] = pnlH6;

            squares[0, 6] = pnlA7;
            squares[1, 6] = pnlB7;
            squares[2, 6] = pnlC7;
            squares[3, 6] = pnlD7;
            squares[4, 6] = pnlE7;
            squares[5, 6] = pnlF7;
            squares[6, 6] = pnlG7;
            squares[7, 6] = pnlH7;
            
            squares[0, 7] = pnlA8;
            squares[1, 7] = pnlB8;
            squares[2, 7] = pnlC8;
            squares[3, 7] = pnlD8;
            squares[4, 7] = pnlE8;
            squares[5, 7] = pnlF8;
            squares[6, 7] = pnlG8;
            squares[7, 7] = pnlH8;

            SetPositionFromFEN(fenString);
        }

        public Board2(Player p1, Player p2)
            : this(new Game(p1, p2), FENSerializer.FEN_START)
        {
        }

        public Board2(Player p1, Player p2, string fenString)
            : this(new Game(p1, p2), fenString)
        {
        }

        // Instantiates a new game and sets the name and rating labels
        private void Board2_Load(object sender, EventArgs e)
        {
            lblWhiteName.Text = player1.Name;
            lblBlackName.Text = player2.Name;
            lblWhiteRating.Text = player1.Rating.ToString();
            lblBlackRating.Text = player2.Rating.ToString();
            saveDiag.AddExtension = true;
        }

        public void SetPositionFromFEN(string fenString)
        {
            Piece currPiece;

            const char bishopSymbol = (char)Piece.PieceNotation.Bishop;
            const char kingSymbol = (char)Piece.PieceNotation.King;
            const char knightSymbol = (char)Piece.PieceNotation.Knight;
            const char queenSymbol = (char)Piece.PieceNotation.Queen;
            const char pawnSymbol = (char)Piece.PieceNotation.Pawn;
            const char rookSymbol = (char)Piece.PieceNotation.Rook;

            game.SetPositionFromString(fenString, SerializationTypes.FEN);
            //serializer.FromFEN(fenString, ref game);

            for (int i = 0; i < Board.NUM_ROWS; i++)
            {
                for (int j = 0; j < Board.NUM_FILES; j++)
                {
                    currPiece = game[i, j];

                    if (currPiece == null)
                        squares[j,i].BackgroundImage = null;
                    else if (currPiece.Colour == Piece.NOTATION_B)
                    {
                        switch (currPiece.Notational)
                        {
                            case bishopSymbol:
                                squares[j,i].BackgroundImage = Properties.Resources.bishopB;
                                break;
                            case kingSymbol:
                                squares[j,i].BackgroundImage = Properties.Resources.kingB;
                                break;
                            case knightSymbol:
                                squares[j,i].BackgroundImage = Properties.Resources.knightB;
                                break;
                            case pawnSymbol:
                                squares[j,i].BackgroundImage = Properties.Resources.pawnB;
                                break;
                            case queenSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.queenB;
                                break;
                            case rookSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.rookB;
                                break;
                        }
                    }
                    else
                    {
                        switch (currPiece.Notational)
                        {
                            case bishopSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.bishopW;
                                break;
                            case kingSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.kingW;
                                break;
                            case knightSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.knightW;
                                break;
                            case pawnSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.pawnW;
                                break;
                            case queenSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.queenW;
                                break;
                            case rookSymbol:
                                squares[j, i].BackgroundImage = Properties.Resources.rookW;
                                break;
                        }
                    }
                }
            }
        }

        /*
         * Handles any and all involvement with making a move
         * 
         * If the status of the game is currently that of an active game, begin
         * checking for if it's the first or second panel to be clicked on for a
         * move attempt. If the game status is not that of an active game, the
         * player is notified that a game is not being played.
         * 
         * Assuming that the game is active, the coordinates of the panel are first
         * grabbed and stored locally.
         * 
         * If it's the first panel they've clicked on, and they click on a panel
         * holding an image of their own piece, store the neccessary information in
         * class variables and update the class counters that a panel has been
         * clicked on. This includes changing the background and storing a copy
         * of the Panel in memory.
         * 
         * If it's the second panel they've clicked on, store the coordinates of
         * it in the second element of the move array and begin subchecking.
         * 
         * If they've clicked on the same square again, change the background of
         * the panel back and update the counter. If they clicked on a different
         * panel, one that hold another of their own pieces, treat that panel
         * like it's the first panel they clicked on, effectively deselcting the
         * old panel and selecting this new panel.
         * 
         * If neither of the above two cases are true, it is an attempt at a movement,
         * and the game engine handles the attempt, returning a flag of the success of
         * of move, as well as the type of move it was, and the visuals are updated
         * accordingly. Normal movement just involves typical updating, castling involves
         * the updating of the visual location of the involved rook. En Passant involves
         * the clearing of the visual of the captured pawn. If pawn promotion occured,
         * display the piece choice box, update the game engine with the correct piece,
         * and update the visuals. If the move attempt was invalid, display the appropriate
         * message box to the user.
         * 
         * After movement attempts are handled, the game status is then reviewed, and the
         * visuals updated accordingly.
         */
        private void squareClick(object sender, MouseEventArgs e)
        {
            // The game is active
            if (game.GameStatus == Game.GameStatusCodes.normal || game.GameStatus == Game.GameStatusCodes.check)
            {
                char[] coords = ((Panel)sender).Name.ToCharArray(3, 2);
                Piece clickedPiece = game[coords[1], coords[0]];

                // Clicked their first click on a piece who's move it is
                if (startMove && clickedPiece != null && (game.WhiteTurn ^ clickedPiece.Colour == Piece.NOTATION_B))
                {
                    startSquare = ((Panel)sender);
                    move[0] = coords;
                    startSquare.BackColor = Color.Goldenrod;
                    startMove = !startMove;
                }
                // Clicked on a panel the second time
                else if (!startMove)
                {
                    move[1] = coords;
                    Piece startPiece = game[move[0][1], move[0][0]];

                    // Clicked on same square again
                    if (move[0][0] == move[1][0] && move[0][1] == move[1][1])
                    {
                        startSquare.BackColor = ((short)move[0][0] % 2 == (short)move[0][1] % 2 ? Color.Black : Color.White);
                        startMove = !startMove;
                    }
                    // Clicked different square, still on own colour piece
                    else if (clickedPiece != null && clickedPiece.Colour == startPiece.Colour)
                    {
                        startSquare.BackColor = ((short)move[0][0] % 2 == (short)move[0][1] % 2 ? Color.Black : Color.White);
                        startSquare = ((Panel)sender);
                        move[0] = move[1];
                        startSquare.BackColor = Color.Goldenrod;
                    }
                    else // Attempt at complete move
                    {
                        ChessMove.MoveStatusCodes result = game.IsValidTurn(move[0], move[1]);
                        switch (result) // Sorting out the movementStatus codes
                        {
                            case ChessMove.MoveStatusCodes.normalMovement:
                                completeMove(((Panel)sender));
                                break;
                            case ChessMove.MoveStatusCodes.kingCastle: // Castling
                            case ChessMove.MoveStatusCodes.queenCastle: // Castling
                                completeMove(((Panel)sender));

                                if (result == ChessMove.MoveStatusCodes.kingCastle)
                                {
                                    if (!game.WhiteTurn) // White moved
                                    {
                                        pnlF1.BackgroundImage = pnlH1.BackgroundImage;
                                        pnlH1.BackgroundImage = null;
                                    }
                                    else
                                    {
                                        pnlF8.BackgroundImage = pnlH8.BackgroundImage;
                                        pnlH8.BackgroundImage = null;
                                    }
                                }
                                else if (result == ChessMove.MoveStatusCodes.queenCastle) // Queenside
                                {
                                    if (!game.WhiteTurn) // White
                                    {
                                        pnlD1.BackgroundImage = pnlA1.BackgroundImage;
                                        pnlA1.BackgroundImage = null;
                                    }
                                    else
                                    {
                                        pnlD8.BackgroundImage = pnlA8.BackgroundImage;
                                        pnlA8.BackgroundImage = null;
                                    }
                                }
                                break;
                            case ChessMove.MoveStatusCodes.enPassant: // En Passant
                                completeMove(((Panel)sender));

                                // Dynamically generate name of panel holding the captured pawn, and clear the image on it
                                System.Reflection.FieldInfo field = this.GetType().GetField("pnl" + move[1][0].ToString() + move[0][1].ToString(), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                ((Panel)field.GetValue(this)).BackgroundImage = null;
                                break;
                            case ChessMove.MoveStatusCodes.pawnPromote: // Pawn Promotion
                                completeMove(((Panel)sender));
                                ChoosePiece form = new ChoosePiece(game.WhiteTurn ? Piece.NOTATION_B : Piece.NOTATION_W);
                                Piece newPiece = form.getPromotion();

                                if (newPiece.Colour == Piece.NOTATION_B)
                                {
                                    if (newPiece.Value == (ushort)Piece.PieceValues.queen)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.queenB;
                                    else if (newPiece.Value == (ushort)Piece.PieceValues.rook)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.rookB;
                                    else if (newPiece.Value == (ushort)Piece.PieceValues.bishop)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.bishopB;
                                    else if (newPiece.Value == (ushort)Piece.PieceValues.knight)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.knightB;
                                }
                                else
                                {
                                    if (newPiece.Value == (ushort)Piece.PieceValues.queen)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.queenW;
                                    else if (newPiece.Value == (ushort)Piece.PieceValues.rook)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.rookW;
                                    else if (newPiece.Value == (ushort)Piece.PieceValues.bishop)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.bishopW;
                                    else if (newPiece.Value == (ushort)Piece.PieceValues.knight)
                                        ((Panel)sender).BackgroundImage = Properties.Resources.knightW;
                                }

                                game.SetPromotedValue(new BoardSquare(move[1][0], move[1][1]), newPiece);
                                break;
                            case ChessMove.MoveStatusCodes.endedWhileCheck: // Ended turn while in check
                                MessageBox.Show("You can not be in check at the end of your move!");
                                break;
                            case ChessMove.MoveStatusCodes.invalidMove: // Invalid move
                                MessageBox.Show("That's an invalid move!");
                                break;
                        }

                        if (result != ChessMove.MoveStatusCodes.endedWhileCheck && result != ChessMove.MoveStatusCodes.invalidMove)
                        {
                            IPlayerDB db = Main.GetPlayerIO();

                            switch (game.GameStatus)
                            {
                                case Game.GameStatusCodes.check:
                                    MessageBox.Show("Check!");
                                    break;
                                case Game.GameStatusCodes.checkMate:
                                    db.updatePlayerRatings(player1, player2);
                                    updateDisplayedRatings();
                                    MessageBox.Show("Checkmate!");
                                    break;
                                case Game.GameStatusCodes.stalemate:
                                    db.updatePlayerRatings(player1, player2);
                                    updateDisplayedRatings();
                                    MessageBox.Show("Stalemate!");
                                    break;
                                case Game.GameStatusCodes.forcedDraw:
                                    db.updatePlayerRatings(player1, player2);
                                    updateDisplayedRatings();
                                    MessageBox.Show("Draw by repetition!");
                                    break;
                                case Game.GameStatusCodes.drawByMaterial:
                                    db.updatePlayerRatings(player1, player2);
                                    updateDisplayedRatings();
                                    MessageBox.Show("Draw, not enough material to checkmate.");
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please start a new game to play");
            }
        }

        /*
         * Reset the background colour of the panel to it's original color, and set
         * the stored reference of it as being clicked on to null if it's been set.
         * Regardless of whether or not it has been, set startMove to true.
         */
        private void resetMove(object sender, MouseEventArgs e)
        {
            if (startSquare != null)
            {
                startSquare.BackColor = ((short)move[0][0] % 2 == (short)move[0][1] % 2 ? Color.Black : Color.White);
                startSquare = null;
            }
            startMove = true;
        }

        /*
         * Called when a valid move has been tried, this method updates the
         * graphics of the involved squares.
         * 
         * This includes setting the background colour of the start square
         * back to it's original colour, updating the background images
         * of each involved square, updating the move counter, and updating
         * the visual of who's turn it is to go.
         * 
         * Based on the predictable position of black squares on the board, the correct
         * background colour to set the panel to is ascertained by checking if both the
         * X and Y coordinates of the panel, when converted to an integral type, are both
         * either divisible by 2 or not. Black squares will have both divisible by 2, white
         * squares will not.
         */
        private void completeMove(Panel endSquare)
        {
            startSquare.BackColor = ((short)move[0][0] % 2 == (short)move[0][1] % 2 ? Color.Black : Color.White);
            endSquare.BackgroundImage = startSquare.BackgroundImage;
            startSquare.BackgroundImage = null;
            startSquare = null;
            startMove = !startMove;

            if (!game.WhiteTurn)
                lstMoves.Items.Add(Convert.ToInt32(game.Moves.Count >> 1)+1 + ". " + game.LastMove.ToString().PadRight(12));
            else
                lstMoves.Items[lstMoves.Items.Count-1] += game.LastMove.ToString();

            picToGo.Image = game.WhiteTurn ? Properties.Resources.pawnW : Properties.Resources.pawnB;
        }

        /*
         * This method is run when a game ends
         * 
         * In it, the display of each player's ratings are updated, and the New Game button
         * is enabled.
         */
        private void updateDisplayedRatings()
        {
            lblWhiteRating.Text = player1.Rating.ToString();
            lblBlackRating.Text = player2.Rating.ToString();
            btnNewGame.Enabled = true;
        }

        /*
         * This method is called when a new game is started. In it, the images are reverted
         * to the positions they would be in at the beginning of a game, and the New Game
         * button is disabled.
         */
        private void StartNewGame()
        {
            pnlA1.BackgroundImage = Properties.Resources.rookW;
            pnlA2.BackgroundImage = Properties.Resources.pawnW;
            pnlA3.BackgroundImage = null;
            pnlA4.BackgroundImage = null;
            pnlA5.BackgroundImage = null;
            pnlA6.BackgroundImage = null;
            pnlA7.BackgroundImage = Properties.Resources.pawnB;
            pnlA8.BackgroundImage = Properties.Resources.rookB;
            pnlB1.BackgroundImage = Properties.Resources.knightW;
            pnlB2.BackgroundImage = Properties.Resources.pawnW;
            pnlB3.BackgroundImage = null;
            pnlB4.BackgroundImage = null;
            pnlB5.BackgroundImage = null;
            pnlB6.BackgroundImage = null;
            pnlB7.BackgroundImage = Properties.Resources.pawnB;
            pnlB8.BackgroundImage = Properties.Resources.knightB;
            pnlC1.BackgroundImage = Properties.Resources.bishopW;
            pnlC2.BackgroundImage = Properties.Resources.pawnW;
            pnlC3.BackgroundImage = null;
            pnlC4.BackgroundImage = null;
            pnlC5.BackgroundImage = null;
            pnlC6.BackgroundImage = null;
            pnlC7.BackgroundImage = Properties.Resources.pawnB;
            pnlC8.BackgroundImage = Properties.Resources.bishopB;
            pnlD1.BackgroundImage = Properties.Resources.queenW;
            pnlD2.BackgroundImage = Properties.Resources.pawnW;
            pnlD3.BackgroundImage = null;
            pnlD4.BackgroundImage = null;
            pnlD5.BackgroundImage = null;
            pnlD6.BackgroundImage = null;
            pnlD7.BackgroundImage = Properties.Resources.pawnB;
            pnlD8.BackgroundImage = Properties.Resources.queenB;
            pnlE1.BackgroundImage = Properties.Resources.kingW;
            pnlE2.BackgroundImage = Properties.Resources.pawnW;
            pnlE3.BackgroundImage = null;
            pnlE4.BackgroundImage = null;
            pnlE5.BackgroundImage = null;
            pnlE6.BackgroundImage = null;
            pnlE7.BackgroundImage = Properties.Resources.pawnB;
            pnlE8.BackgroundImage = Properties.Resources.kingB;
            pnlF1.BackgroundImage = Properties.Resources.bishopW;
            pnlF2.BackgroundImage = Properties.Resources.pawnW;
            pnlF3.BackgroundImage = null;
            pnlF4.BackgroundImage = null;
            pnlF5.BackgroundImage = null;
            pnlF6.BackgroundImage = null;
            pnlF7.BackgroundImage = Properties.Resources.pawnB;
            pnlF8.BackgroundImage = Properties.Resources.bishopB;
            pnlG1.BackgroundImage = Properties.Resources.knightW;
            pnlG2.BackgroundImage = Properties.Resources.pawnW;
            pnlG3.BackgroundImage = null;
            pnlG4.BackgroundImage = null;
            pnlG5.BackgroundImage = null;
            pnlG6.BackgroundImage = null;
            pnlG7.BackgroundImage = Properties.Resources.pawnB;
            pnlG8.BackgroundImage = Properties.Resources.knightB;
            pnlH1.BackgroundImage = Properties.Resources.rookW;
            pnlH2.BackgroundImage = Properties.Resources.pawnW;
            pnlH3.BackgroundImage = null;
            pnlH4.BackgroundImage = null;
            pnlH5.BackgroundImage = null;
            pnlH6.BackgroundImage = null;
            pnlH7.BackgroundImage = Properties.Resources.pawnB;
            pnlH8.BackgroundImage = Properties.Resources.rookB;

            lstMoves.Items.Clear();
            btnNewGame.Enabled = false;
            picToGo.Image = Properties.Resources.pawnW;
        }

        /*
         * Handles draw offers.
         * 
         * If both players click on their draw checkbox, the visuals and
         * game object are updated.
         */
        private void OfferedDraw(object sender, EventArgs e)
        {
            if (chkWhiteDraw.Checked && chkBlackDraw.Checked && game.GameStatus == Game.GameStatusCodes.normal || game.GameStatus == Game.GameStatusCodes.check)
            {
                game.Draw(Game.GameStatusCodes.agreedDraw);
                updateDisplayedRatings();
            }
        }

        /* Perform all actions for resignation attempts
         * 
         * Supplied parameters indicate which side wants to resign and whether to update the
         * GUI as a result of the potential resignation.
         */
        private bool CheckResign(bool isWhite, bool updateGUI)
        {
            string playerName = isWhite ? player1.Name : player2.Name;
            bool didResign = (game.GameStatus != Game.GameStatusCodes.normal && game.GameStatus != Game.GameStatusCodes.check) || ConfirmationBox.ShowDialog("You are currently in a game.\n\nWould you like to resign this game, " + playerName + "?", "Would you like to resign?");

            if (didResign)
            {
                game.CheckMate(isWhite, Game.GameStatusCodes.playerResign);

                if (updateGUI)
                {
                    updateDisplayedRatings();
                    MessageBox.Show(playerName + " has resigned");
                }
            }

            return didResign;
        }

        /*
         * Sets the focus on the control object passed through
         * 
         * This method is called for any object which should have the focus set on it.
         */
        private void SetFocus(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        /*
         * Triggered by a MouseMove event for any of the Panels forming the board, this
         * method will set the focus to that panel, detect the contents of it as far as
         * the game engine is concerned, and set the help text appropriately.
         */
        private void SetBoardHelp(object sender, MouseEventArgs e)
        {
            SetFocus(sender, e);
            char[] coords = ((Panel)sender).Name.ToCharArray(3, 2);
            Piece boardSquare = game[coords[1], coords[0]];
            string displayText = "";

            if (boardSquare == null)
                displayText = "An empty square";
            else
            {
                displayText = boardSquare.Colour == Piece.NOTATION_W ? "A white " : "A black ";
                displayText += boardSquare.Name;
            }

            helpProvider1.SetHelpString((Panel)sender, displayText);
        }

        /*
         * Handles when a resignation attempt is tried via the menu.
         * 
         * If the game is currently being played, a Confirmation box is displayed,
         * asking for confirmation of the resignation. If OK is clicked on it, both
         * the View Layer and game engine are notified.
         */
        private void resignToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.CheckResign(game.WhiteTurn, true);
        }

        /*
         * Handles exiting the game
         * 
         * If a game is currently going on, the clicker is prompted of such, and asked
         * if they wish to resign. Clicking Cancel resumes the game, clicking okay ends
         * it and closes the window.
         * 
         * If no game is going on, the window is simply closed.
         */
        private void exitToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * Triggered when the user wants to learn how to play chess, that form is shown
         * in a Dialog box
         */
        private void howToPlayChessToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpPlay helpPlay = new HelpPlay();
            helpPlay.ShowDialog();
        }

        /*
         * Triggered when the user wants to learn about the program, that form is shown
         * in a Dialog box
         */
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpAbout about = new HelpAbout();
            about.ShowDialog();
        }

        /*
         * Handles when a New Game attempt is tried.
         * 
         * If the game is currently being played, a Confirmation box is displayed,
         * asking for confirmation of the resignation. If OK is clicked on it, both
         * the View Layer and game engine are updated accordingly.
         */
        private void NewGame(object sender, EventArgs e)
        {
            if ((game.GameStatus != Game.GameStatusCodes.normal && game.GameStatus != Game.GameStatusCodes.check) || ConfirmationBox.ShowDialog("You are currently in a game.\n\nWould you like to resign this game, " + (game.WhiteTurn ? player1.Name : player2.Name) + "?", "Would you like to resign?"))
            {
                game = new Game(player1, player2);
                StartNewGame();
            }
        }

        /*
         * Handles when a resignation attempt is tried via button.
         * 
         * If the game is currently being played, a Confirmation box is displayed,
         * asking for confirmation of the resignation. If OK is clicked on it, both
         * the View Layer and game engine are notified.
         */
        private void btnPlayerResigned(object sender, EventArgs e)
        {
            this.CheckResign(((Button)sender).Name.Substring(3, 5) == "White", true);
        }

        private void FormCloseCheck(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !this.CheckResign(game.WhiteTurn, false);

            if (!e.Cancel)
            {
                IPlayerDB db = Main.GetPlayerIO();
                db.updatePlayerRatings(player1, player2);
            }
        }

        #region FEN/PGN
        private void toPGNToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Prompt for where to save file
            if (saveDiag.ShowDialog() == DialogResult.OK)
            {
                // Open stream and serializer
                StreamWriter stream = new StreamWriter(new FileStream(saveDiag.FileName, FileMode.Append));
                GameSerializer s = new GameSerializer();

                try
                {
                    // Format, write and close
                    s.ToStream(new PGNSerializer(), this.game, stream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void toFENToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            /*saveDiag.Filter = Main.FILETYPE_FEN;

            // Prompt for where to save file
            if (saveDiag.ShowDialog() == DialogResult.OK)
            {
                // Open stream and serializer
                StreamWriter stream = new StreamWriter(new FileStream(saveDiag.FileName, FileMode.OpenOrCreate, FileAccess.Write));
                GameSerializer s = new GameSerializer();

                try
                {
                    // Generate string and write to file
                    stream.WriteLine(s.ToFEN(this.game));
                    stream.Flush();
                    s.ToStream(new FENSerializer(), this.game.Layout, stream);
                }
                catch (FENException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex) // May be FENException
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }*/

            FENSpecifier fenRetriever = new FENSpecifier(SerializationModes.Output, this.game.Layout);
            DialogResult result = fenRetriever.ShowDialog();
        }

        private void fromFENFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*string fenString = String.Empty;

            openDiag.Filter = Main.FILETYPE_FEN;

            // Prompt for where to save file
            if (openDiag.ShowDialog() == DialogResult.OK)
            {
                // Open stream and serializer
                StreamReader stream = new StreamReader(new FileStream(openDiag.FileName, FileMode.Open, FileAccess.Read));

                try
                {
                    // Generate string and write to file
                    fenString = stream.ReadLine();
                }
                catch (FENException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            if (String.IsNullOrEmpty(fenString))
                MessageBox.Show("Could not read any data from file");
            else
                SetPositionFromFEN(fenString);*/

            FENSpecifier fenRetriever = new FENSpecifier(SerializationModes.Input, this.game.Layout);
            DialogResult result = fenRetriever.ShowDialog();

            if (result == DialogResult.OK)
                SetPositionFromFEN(fenRetriever.Result);            
        }

        private void fromFENSpecifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FENSpecifier fenRetriever = new FENSpecifier(SerializationModes.Input, this.game.Layout);
            DialogResult result = fenRetriever.ShowDialog();

            if (result == DialogResult.OK)
                SetPositionFromFEN(fenRetriever.Result);
        }

        #endregion
    }
}