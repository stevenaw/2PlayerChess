using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace ChessGameLib
{
    /*
     * A class which handles all player-related data file interaction.
     */
    public class PlayerDAT : IPlayerDB
    {
        #region Variables and Constants
        // The connection string for the database
        private string fullPath = String.Empty;
        #endregion
        #region ctor
        // Constructor, create a new connection to the database
        public PlayerDAT(string dirStr, string fileName)
        {
            this.fullPath = dirStr + fileName;
        }

        public PlayerDAT(string fullPath)
        {
            this.fullPath = fullPath;
        }

        #endregion
        #region Public Methods
        // Delete the given player
        public bool deletePlayer(Player player)
        {
            bool error = false;
            int currIndex = 0;
            Player currPlayer;
            byte[] source;

            // Check for file existence
            if (File.Exists(this.fullPath))
            {
                source = File.ReadAllBytes(this.fullPath);

                while (!error && currIndex < source.Length)
                {
                    currPlayer = Player.FromBytes(source, ref currIndex);

                    if (currPlayer == null)
                        error = true;
                    else if (currPlayer.ID == player.ID)
                    {
                        byte[] newFile = new byte[source.Length - currPlayer.RecordLength];
                        Array.Copy(source, 0, newFile, 0, currIndex - currPlayer.RecordLength);
                        Array.Copy(source, currIndex, newFile, currIndex - currPlayer.RecordLength, source.Length - currIndex);

                        File.WriteAllBytes(this.fullPath, newFile);
                        break;
                    }
                }
            }

            return !error;
        }

        // Update the changeable stats of the player objects passed in
        public bool updatePlayerRatings(Player p1, Player p2)
        {
            bool error = false;
            int recUpdated = 0;
            int currIndex = 0;
            Player currPlayer;
            byte[] source;
            byte[] playerRec;

            // Check for file existence
            if (File.Exists(this.fullPath))
            {
                source = File.ReadAllBytes(this.fullPath);

                while (!error && recUpdated < 2 && currIndex < source.Length)
                {
                    currPlayer = Player.FromBytes(source, ref currIndex);

                    if (currPlayer == null)
                        error = true;
                    else if (currPlayer.ID == p1.ID)
                    {
                        playerRec = p1.ToBytes();
                        Array.Copy(playerRec, 0, source, currIndex - playerRec.Length, playerRec.Length);
                        recUpdated++;
                    }
                    else if (currPlayer.ID == p2.ID)
                    {
                        playerRec = p2.ToBytes();
                        Array.Copy(playerRec, 0, source, currIndex - playerRec.Length, playerRec.Length);
                        recUpdated++;
                    }
                }

                if (!error && recUpdated == 2)
                    File.WriteAllBytes(this.fullPath, source);
            }

            return recUpdated == 2 && !error;
        }

        /*
         * Fetch all players from the database and store them in the playerList object
         * passed in by reference
         */
        public PlayerCollection getPlayers()
        {
            PlayerCollection playerList = new PlayerCollection();

            if (File.Exists(this.fullPath))
            {
                byte[] fileContents = File.ReadAllBytes(this.fullPath);
                Player currPlayer = null;
                int currIndex = 0;

                while ((currPlayer = Player.FromBytes(fileContents, ref currIndex)) != null)
                    playerList.Add(currPlayer);
            }

            return playerList;
        }

        // Add the newly-created player in the database.
        public bool saveNewPlayer(Player player)
        {
            FileStream f;
            byte[] playerAsBytes;

            if (!File.Exists(this.fullPath))
                f = File.Create(this.fullPath);
            else
                f = File.OpenWrite(this.fullPath);

            playerAsBytes = player.ToBytes();
            f.Seek(0, SeekOrigin.End);
            f.Write(playerAsBytes, 0, playerAsBytes.Length);
            
            return true;
        }
        #endregion
    }
}
