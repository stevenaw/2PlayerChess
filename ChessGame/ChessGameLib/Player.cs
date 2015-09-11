using System;
using System.Text; // Encoding class

namespace ChessGameLib
{
    /*
     * A simple class which handles all player-related functions.
     * 
     * Aside from storage of class variables, it also calculates new ratings and packs and
     * unpacks key variables of itself into an array of bytes for streaming purposes. In this
     * array. variable-length fields (such as name) are prepended by a value indicating its
     * length. All integers lengths are platform-independant, and as a result, data files can
     * not be transfered between 32 and 64 bit systems.
     * 
     * In-depth details of packing are as follows below:
     * 
     *      Variable        Class-Level     Length
     *      ¯¯¯¯¯¯¯¯        ¯¯¯¯¯¯¯¯¯¯¯     ¯¯¯¯¯¯
     *      NameLength           N          Integer (32 or 64 bits)
     *      Name                 Y          Variable length ASCII string
     *      ID                   Y          Integer (32 or 64 bits)
     *      Wins                 Y          Integer (32 or 64 bits)
     *      Losses               Y          Integer (32 or 64 bits)
     *      Draws                Y          Integer (32 or 64 bits)
     *      Rating               Y          Integer (32 or 64 bits)
     */
    public class Player
    {
        #region Variables and Constants
        // The number of characters allowable in a name (ASCII-encoded, translates to bytes)
        public const int MAX_NAME_LEN = 1000;

        // A null id, representative of a player record without an id
        private const int NULL_ID = -1;

        // The player's name
        private string name;
        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        // The number of wins the player has achieved
        private int wins;
        public int Wins
        {
            get { return wins; }
            internal set { wins = value; }
        }

        // The number of losses the player has suffered
        private int losses;
        public int Losses
        {
            get { return losses; }
            internal set { losses = value; }
        }

        // The number of games the player has drawn
        private int draws;
        public int Draws
        {
            get { return draws; }
            internal set { draws = value; }
        }

        // The numeric rating of the player (ELO, FIDE, etc)
        private int rating;
        public int Rating
        {
            get { return rating; }
            internal set { rating = value; }
        }

        // ID is determined externally, upon record creation
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        // The colour the player is playing as ('w' or 'b')
        private char colour;
        public char Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        // The length of the byte array generated for this class (representative of all persistent data)
        public int RecordLength
        {
            get
            {
                return 6 * sizeof(int) + name.Length;
            }
        }
        #endregion
        #region ctor
        public Player() { }
        public Player(string name, int rating, int wins, int losses, int draws, int id)
        {
            this.name = name;
            this.rating = rating;
            this.wins = wins;
            this.losses = losses;
            this.draws = draws;
            this.id = id;
        }
        #endregion
        #region Public Methods
        public override string ToString()
        {
            return name + " (" + rating + ")";
        }

        /*
         * Pack the object into a byte array.
         * 
         * The sister function to this is the static function FromBytes
         */
        public byte[] ToBytes()
        {
            int intSize = sizeof(int), offset = 0;
            byte[] nameAsBytes = Encoding.ASCII.GetBytes(this.Name);
            byte[] resultant = new byte[intSize * 6 + nameAsBytes.Length];

            Array.Copy(BitConverter.GetBytes(this.id), 0, resultant, offset, intSize);
            offset += intSize;

            Array.Copy(BitConverter.GetBytes(nameAsBytes.Length), 0, resultant, offset, intSize);
            offset += intSize;

            Array.Copy(nameAsBytes, 0, resultant, offset, nameAsBytes.Length);
            offset += nameAsBytes.Length;

            Array.Copy(BitConverter.GetBytes(this.wins), 0, resultant, offset, intSize);
            offset += intSize;

            Array.Copy(BitConverter.GetBytes(this.losses), 0, resultant, offset, intSize);
            offset += intSize;

            Array.Copy(BitConverter.GetBytes(this.draws), 0, resultant, offset, intSize);
            offset += intSize;

            Array.Copy(BitConverter.GetBytes(this.rating), 0, resultant, offset, intSize);
            offset += intSize;

            return resultant;
        }

        /*
         * Unpack an object from a byte array.
         * 
         * A Player object is returned on success, though on failure this is null. Supplied
         * parameters must be the source from which to draw, as well as the index from which
         * to reference as the start of the object.
         * 
         * The sister function to this is the non-static function ToBytes.
         */
        public static Player FromBytes(byte[] source, ref int offset)
        {
            Player p = null;
            int intSize = sizeof(int);

            if (source.Length - offset > intSize)
            {
                byte[] intField = new byte[intSize];
                Array.Copy(source, offset + intSize, intField, 0, intSize);
                int nameLen = BitConverter.ToInt32(intField, 0);

                // Can hold 5 class-level ints, variable-length name, and length of name as an integer
                if (source.Length - offset >= nameLen + intSize * 6)
                {
                    byte[] nameField = new byte[nameLen];

                    p = new Player("", 0, 0, 0, 0, 0);

                    Array.Copy(source, offset, intField, 0, intSize);
                    p.id = BitConverter.ToInt32(intField, 0);
                    offset += intSize * 2;

                    Array.Copy(source, offset, nameField, 0, nameLen);
                    p.name = Encoding.ASCII.GetString(nameField);
                    offset += nameLen;

                    Array.Copy(source, offset, intField, 0, intSize);
                    p.wins = BitConverter.ToInt32(intField, 0);
                    offset += intSize;

                    Array.Copy(source, offset, intField, 0, intSize);
                    p.losses = BitConverter.ToInt32(intField, 0);
                    offset += intSize;

                    Array.Copy(source, offset, intField, 0, intSize);
                    p.draws = BitConverter.ToInt32(intField, 0);
                    offset += intSize;

                    Array.Copy(source, offset, intField, 0, intSize);
                    p.rating = BitConverter.ToInt32(intField, 0);
                    offset += intSize;
                }
            }

            return p;
        }

        /*
         * Extract specifically the player's ID from a stream of bytes. This ID is then returned,
         * a negative value of which indicates failure. The byte steam must be long enough to contain
         * an entire Player object for successful execution of the function.
         */
        public static int IDFromBytes(byte[] source, int offset)
        {
            int intSize = sizeof(int), currFieldInt = -1;

            if (source.Length > intSize)
            {
                byte[] intField = new byte[intSize];
                Array.Copy(source, offset, intField, 0, intSize);
                currFieldInt = BitConverter.ToInt32(intField, 0);
            }

            return currFieldInt;
        }

        /*
         * Record the result of a game
         * 
         * Based on the opponent's ratin and the decision of the game, a new rating is
         * calculated for the player. This rating is based on the Elo Rating scale, and
         * has the appropriate formula for it.  The scoring system for wins and losses
         * is as follows: A win is worth 1, a loss is worth 0, and a draw is worth 0.5.
         */
        public void CalcNewELO(int opponentRating, double decision)
        {
            rating += (int)Math.Round(((rating >= 2200 ? 16 : 32) * (decision - (1 / (1 + Math.Pow(10, ((opponentRating - rating) / 400)))))), MidpointRounding.AwayFromZero);

            if (decision == 1)
                wins++;
            else if (decision == 0)
                losses++;
            else
                draws++;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            else
                return this.ID == ((Player)obj).ID;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }
        #endregion
    }
}
