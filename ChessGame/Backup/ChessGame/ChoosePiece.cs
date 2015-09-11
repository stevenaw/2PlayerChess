using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ChessGameLib;

namespace ChessGame
{
    /*
     * This form handles the choosing of a piece when pawn promotion occurs.
     * 
     * Pawn promotion occurs when a pawn reaches the other side of the board, upon which
     * the player gets to "swap" it for a piece of their choosing.  Each player is only
     * allowed one King, and keeping it a pawn would be pointless, so the options they
     * can set it to are Queen, Rook, Bishop, and Knight. The chosen piece is then
     * returned when this class is referenced with the getPromotion() method.
     */
    public partial class ChoosePiece : Form
    {
        // The colour of the piece which is getting promoted
        char col;
        // The piece to be promoted to
        Piece newPiece;

        /*
         * Upon instantiation, store the colour in a class variable. This is later refernced
         * both when creating the piece and when loading the form, to generate the correctly
         * coloured images to choose from.
         */
        public ChoosePiece(char colour)
        {
            col = colour;
            InitializeComponent();
        }

        /*
         * By default, the images loaded are of white pieces, but if a black pawn is to be
         * promoted, the images are changed accordingly.
         */
        private void ChoosePiece_Load(object sender, EventArgs e)
        {
            if (col == Piece.NOTATION_B)
            {
                picQueen.Image = Properties.Resources.queenB;
                picRook.Image = Properties.Resources.rookB;
                picBishop.Image = Properties.Resources.bishopB;
                picKnight.Image = Properties.Resources.knightB;
            }
        }

        /*
         * Displays the Form, recieves input, creates the piece, and returns it to the
         * calling program.
         */
        public Piece getPromotion()
        {
            this.ShowDialog();
            return this.newPiece;
        }

        /*
         * Fires when the "Promote" button is pressed, t instantiates a new piece object
         * based on the colour being promoted and the piece type chosen by the player.
         */
        private void btnPromote_Click(object sender, EventArgs e)
        {
            if (radChooseQ.Checked)
                this.newPiece = new Queen(col);
            else if (radChooseR.Checked)
                this.newPiece = new Rook(col);
            else if (radChooseB.Checked)
                this.newPiece = new Bishop(col);
            else if (radChooseN.Checked)
                this.newPiece = new Knight(col);

            this.Close();
        }

        /*
         * Triggered when one of the pictures is clicked on, this method will auto-select
         * the appropriate radio button.
         */
        private void picture_Click(object sender, MouseEventArgs e)
        {
            char symbol = (((PictureBox)sender).Name.ToCharArray(3, 1))[0];
            if (symbol == (char)Piece.PieceNotation.Queen)
                radChooseQ.Checked = true;
            else if (symbol == (char)Piece.PieceNotation.Rook)
                radChooseR.Checked = true;
            else if (symbol == (char)Piece.PieceNotation.Bishop)
                radChooseB.Checked = true;
            else if (symbol == (char)Piece.PieceNotation.King)
                radChooseN.Checked = true;
        }

        private void SetFocus(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        private void EnsureSelected(object sender, FormClosingEventArgs e)
        {
            if (this.newPiece == null)
                e.Cancel = true;
        }
    }
}