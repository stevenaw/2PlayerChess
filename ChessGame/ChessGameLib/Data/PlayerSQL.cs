using System;
using System.Data.SqlClient;
using System.Data;

namespace ChessGameLib
{
    /*
     * A class which handles all player-related SQL database interaction
     * 
     * The connection string, required upon instantiation, is pulled from app.config using
     * the fllowing command:
     * 
     * ConfigurationSettings.AppSettings["SQLServer"].ToString()
     */
    public class PlayerSQL : IPlayerDB
    {
        #region Variables and Constants
        // The connection the the database
        private SqlConnection conn;

        // The command to send to the database
        private SqlCommand cmd;
        #endregion
        #region ctor
        // Constructor, create a new connection to the database
        public PlayerSQL(string connStr)
        {
            conn = new SqlConnection(connStr);
        }
        #endregion
        #region Public Methods

        // Delete the given player
        public bool deletePlayer(Player player)
        {
            int returnVal = 0;
            const string statement = "DELETE FROM player WHERE id = @id";

            cmd = new SqlCommand(statement, conn);
            cmd.Parameters.AddWithValue("@id", player.ID);

            try
            {
                conn.Open();
                returnVal = cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
            }
            finally
            {
                conn.Close();
            }

            return returnVal == 1;
        }

        // Update the changeable stats of the player object passed in
        public bool updatePlayerRatings(Player p1, Player p2)
        {
            int count = 0;
            const string updateStatement = "UPDATE player SET rating = @rating, wins = @wins, losses = @losses, draws = @draws WHERE id = @id";

            cmd = new SqlCommand(updateStatement, conn);
            cmd.Parameters.AddWithValue("@rating", p1.Rating);
            cmd.Parameters.AddWithValue("@wins", p1.Wins);
            cmd.Parameters.AddWithValue("@losses", p1.Losses);
            cmd.Parameters.AddWithValue("@draws", p1.Draws);
            cmd.Parameters.AddWithValue("@id", p1.ID);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                count = 0;
            }

            if (count == 1)
            {
                cmd.Parameters.AddWithValue("@rating", p2.Rating);
                cmd.Parameters.AddWithValue("@wins", p2.Wins);
                cmd.Parameters.AddWithValue("@losses", p2.Losses);
                cmd.Parameters.AddWithValue("@draws", p2.Draws);
                cmd.Parameters.AddWithValue("@id", p2.ID);

                try
                {
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    count = 0;
                }
                finally
                {
                    conn.Close();
                }
            }
            else
                conn.Close();

            return count == 1;
        }

        /*
         * Fetch all players from the database and rturn the resulant playerList object
         */
        public PlayerCollection getPlayers()
        {
            PlayerCollection playerList = new PlayerCollection();
            const string selectStatement = "SELECT * FROM player";

            cmd = new SqlCommand(selectStatement, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                    playerList.Add(new Player(reader["playerName"].ToString(), (int)reader["rating"], (int)reader["wins"], (int)reader["losses"], (int)reader["draws"], (int)reader["id"]));

                reader.Close();
            }
            catch (SqlException)
            {
            }
            finally
            {
                conn.Close();
            }

            return playerList;
        }

        // Store the newly-created player in the database.
        public bool saveNewPlayer(Player player)
        {
            bool success = true;

            const string insertStatement = "INSERT player (id, playerName, rating, wins, losses, draws) "
                +"VALUES (@id, @playerName, @rating, @wins, @losses, @draws)";
            
            cmd = new SqlCommand(insertStatement, conn);
            cmd.Parameters.AddWithValue("@id", player.ID);
            cmd.Parameters.AddWithValue("@playerName", player.Name);
            cmd.Parameters.AddWithValue("@rating", player.Rating);
            cmd.Parameters.AddWithValue("@wins", player.Wins);
            cmd.Parameters.AddWithValue("@losses", player.Losses);
            cmd.Parameters.AddWithValue("@draws", player.Draws);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                success = false;
            }
            finally
            {
                conn.Close();
            }

            return success;
        }
        #endregion
    }
}
