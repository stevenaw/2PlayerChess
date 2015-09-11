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
     * A fairly simple class which handles the creation of a new player. This form then
     * returns that player to the calling method if createPlayer() is called. If player
     * creation fails for any reason, an null object is returned.
     */
    public partial class CreateProfile : Form
    {
        // The player object to be returned
        Player player = null;

        // The starting rating for a player
        const ushort startRating = 1000;

        public CreateProfile()
        {
            InitializeComponent();
        }

        /*
         * Show this form in a dialog box, then return the player object to the calling
         * method.
         */
        public Player createPlayer()
        {
            this.ShowDialog();
            return player;
        }

        /*
         * Triggers when the okay button is clicked, determines if the user entered a name
         * into the box. If they did, instantiate the player object with the default rating
         * and stats, followed by closing the window. Otherwise, alert the user to the mistake.
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtPlayerName.Text != "")
            {
                player = new Player(txtPlayerName.Text, startRating, (ushort)0, (ushort)0, (ushort)0, (ushort)0);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a name", "User name error");
            }
        }

        // Closes the window
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}