using System;
using System.Collections.Generic;

/*
 * A simple indexer class for a list of player objects
 * 
 * This class is used to hold all the created players made, as pulled from the database.
 * Once a player is chosen on the main form, the player object is pulled from here and
 * a copy is stored in the global Game.
 */
namespace ChessGameLib
{
    public class PlayerCollection : System.Collections.ObjectModel.Collection<Player>
    {
        // Constructor, creates a new List of players
        public PlayerCollection()
        {
            
        }
    }
}
