using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ChessGameLib;

namespace ChessGame
{
    /*
     * The Main form, all other forms are called from this one. It mainly deals with choosing
     * players from a dropdown to play a game with. The dropdown list is populated by the player
     * records from the database, which are pulled on Form instantiation.
     */
    public partial class Main : Form
    {
        /*
         * An indexer class for Player Objects, this has the player objects in duplicate order
         * as they are displayed in the combo boxes.
         */
        PlayerCollection playerList = new PlayerCollection();

        public const string FILETYPE_FEN = "FEN File|*.fen";
        public const string FILETYPE_PGN = "PGN File|*pgn";

        private static PlayerDBTypes storageType = new DBTypeConverter().Parse(Properties.Settings.Default.StorageType);
        
        public Main()
        {
            InitializeComponent();

            IPlayerDB db = GetPlayerIO();
            this.playerList = db.getPlayers();

            foreach (Player p in this.playerList)
            {
                cmbPlayer1.Items.Add(p);
                cmbPlayer2.Items.Add(p);
            }

#if DEBUG
            if (this.playerList.Count > 1)
            {
                cmbPlayer1.SelectedIndex = 0;
                cmbPlayer2.SelectedIndex = 1;
            }
#endif
        }

        public static IPlayerDB GetPlayerIO()
        {
            return PlayerDBFactory.GetInstance().GetDBObj(storageType);
        }

        // Closes program
        private void CloseProgram(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * Triggered when the user wishes to create a player, t displays that form in a DialogBox,
         * and returns the newly-created Player object back. The UserID is then generated as the
         * next one incrementally in line, and it is attempted to be placed in the database.
         * 
         * If the insertion was successful, add the newly created player to the playerList and combo
         * box items, otherwise, display a message to the user.
         */
        private void CreatePlayer(object sender, EventArgs e)
        {
            CreateProfile newPlayer = new CreateProfile();
            Player newCreatedPlayer = newPlayer.createPlayer();
            
            if (newCreatedPlayer != null)
            {
                IPlayerDB db = GetPlayerIO();

                if (this.playerList.Count == 0)
                    newCreatedPlayer.ID = 1;
                else
                    newCreatedPlayer.ID = this.playerList[this.playerList.Count - 1].ID + 1;

                if (db.saveNewPlayer(newCreatedPlayer))
                {
                    this.playerList.Add(newCreatedPlayer);
                    cmbPlayer1.Items.Add(newCreatedPlayer);
                    cmbPlayer2.Items.Add(newCreatedPlayer);
                }
                else
                {
                    MessageBox.Show("The player object could not be saved because the database could not be saved to.\nPlease try and create the player again.");
                }
            }
        }

        /*
         * Triggered when the user wants to learn how to play chess, that form is shown
         * in a Dialog box
         */
        private void howToPlayChessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpPlay helpPlay = new HelpPlay();
            helpPlay.ShowDialog();
        }

        /*
         * Triggered when the user wants to learn about the program, that form is shown
         * in a Dialog box
         */
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpAbout about = new HelpAbout();
            about.ShowDialog();
        }

        /*
         * When the user wants to start a new game, checking occurs that 2 different players have
         * been chosen. If they have been, the game board Form is shown in a Dialog, after which
         * the ratings in both the player list and combo box lists are updated
         */
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbPlayer1.SelectedIndex != -1 && cmbPlayer2.SelectedIndex != -1 && cmbPlayer2.SelectedIndex != cmbPlayer1.SelectedIndex)
            {
                Player player1 = this.playerList[cmbPlayer1.SelectedIndex];
                player1.Colour = Piece.NOTATION_W;

                Player player2 = this.playerList[cmbPlayer2.SelectedIndex];
                player2.Colour = Piece.NOTATION_B;

                Form board = new Board2(player1, player2);
                this.Hide();
                board.ShowDialog();
                this.Show();

                cmbPlayer1.Items[cmbPlayer1.SelectedIndex] = player1;
                cmbPlayer1.Items[cmbPlayer2.SelectedIndex] = player2;
                cmbPlayer2.Items[cmbPlayer1.SelectedIndex] = player1;
                cmbPlayer2.Items[cmbPlayer2.SelectedIndex] = player2;

                playerList[cmbPlayer1.SelectedIndex] = player1;
                playerList[cmbPlayer2.SelectedIndex] = player2;
            }
            else
            {
                MessageBox.Show("Please select 2 different players!");
            }
        }

        private void gameFromFENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check that 2 different players are selected
            if (cmbPlayer1.SelectedIndex == -1 || cmbPlayer2.SelectedIndex == -1 || cmbPlayer1.SelectedIndex == cmbPlayer2.SelectedIndex)
            {
                MessageBox.Show("Please select 2 unique players!");
            }
            else
            {
                FENSpecifier fenRetriever = new FENSpecifier(SerializationModes.Input);

                // Open dialog to enter FEN string
                if (fenRetriever.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(fenRetriever.Result))
                {
                    Game g = new Game((Player)cmbPlayer1.SelectedItem, (Player)cmbPlayer2.SelectedItem);
                    Board2 b = new Board2(g, fenRetriever.Result);
                    b.Show();
                }
            }  
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            DeletePlayer(cmbPlayer1.SelectedItem as Player, cmbPlayer1.SelectedIndex);
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            DeletePlayer(cmbPlayer2.SelectedItem as Player, cmbPlayer2.SelectedIndex);
        }

        private void DeletePlayer(Player p, int index)
        {
            if (p != null && index >= 0 && index < this.playerList.Count)
            {
                IPlayerDB db = GetPlayerIO();

                if (db.deletePlayer(p))
                {
                    this.playerList.RemoveAt(index);
                    cmbPlayer1.Items.RemoveAt(index);
                    cmbPlayer2.Items.RemoveAt(index);
                }
            }
        }
    }
}