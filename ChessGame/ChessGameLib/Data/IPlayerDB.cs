namespace ChessGameLib
{
    /*
     * Describes the interface of any class for player-related IO
     * 
     * This is intended to be implemented to allow for a variety of data storage options
     */
    public interface IPlayerDB
    {
        bool updatePlayerRatings(Player p1, Player p2);
        PlayerCollection getPlayers();
        bool saveNewPlayer(Player player);
        bool deletePlayer(Player player);
    }
}
