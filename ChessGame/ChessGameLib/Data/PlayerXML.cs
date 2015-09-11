using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ChessGameLib
{
    /*
     * A class which handles all player-related xml data file interaction.
     */
    public class PlayerXML : IPlayerDB
    {
        #region Variables and Constants
        // The connection string for the database
        private string dirStr = String.Empty;
        private string fileName = String.Empty;
        private string fullPath = String.Empty;

        private const string XML_NODE_ENTITY_LIST = "players";
        private const string XML_NODE_ENTITY = "player";
        private const string XML_PLAYER_ID = "id";
        private const string XML_PLAYER_NAME = "name";
        private const string XML_PLAYER_RATING = "rating";
        private const string XML_PLAYER_WINS = "wins";
        private const string XML_PLAYER_LOSSES = "losses";
        private const string XML_PLAYER_DRAWS = "draws";
        #endregion
        #region ctor
        // Constructor, create a new connection to the database
        public PlayerXML(string dirStr, string fileName)
        {
            this.dirStr = dirStr;
            this.fileName = fileName;
            this.fullPath = dirStr + fileName;
        }

        public PlayerXML(string file)
        {
            this.dirStr = Path.GetDirectoryName(file);
            this.fileName = Path.GetFileName(file);
            this.fullPath = file;
        }
        #endregion
        #region Public Methods
        // Delete the given player
        public bool deletePlayer(Player player)
        {
            XmlDocument doc = GetFile(fullPath);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                Player thisOne = FromElement(node as XmlElement);

                if (player.Equals(thisOne))
                {
                    doc.DocumentElement.RemoveChild(node);
                    doc.Save(fullPath);
                    return true;
                }
            }

            return false;
        }

        // Update the changeable stats of the player objects passed in
        public bool updatePlayerRatings(Player p1, Player p2)
        {
            XmlDocument doc = GetFile(fullPath);

            foreach (XmlElement elem in doc.DocumentElement.ChildNodes)
            {
                int id = Convert.ToInt32(elem.GetAttribute(XML_PLAYER_ID));
                if (id == p1.ID)
                {
                    elem.SetAttribute(XML_PLAYER_RATING, p1.Rating.ToString());
                    elem.SetAttribute(XML_PLAYER_WINS, p1.Wins.ToString());
                    elem.SetAttribute(XML_PLAYER_LOSSES, p1.Losses.ToString());
                    elem.SetAttribute(XML_PLAYER_DRAWS, p1.Draws.ToString());
                }
                else if (id == p2.ID)
                {
                    elem.SetAttribute(XML_PLAYER_RATING, p2.Rating.ToString());
                    elem.SetAttribute(XML_PLAYER_WINS, p2.Wins.ToString());
                    elem.SetAttribute(XML_PLAYER_LOSSES, p2.Losses.ToString());
                    elem.SetAttribute(XML_PLAYER_DRAWS, p2.Draws.ToString());
                }
            }

            doc.Save(fullPath);

            return true;
        }

        /*
         * Fetch all players from the database and store them in the playerList object
         * passed in by reference
         */
        public PlayerCollection getPlayers()
        {
            XmlDocument doc = GetFile(fullPath);
            PlayerCollection list = new PlayerCollection();
            foreach (XmlElement elem in doc.DocumentElement.ChildNodes)
            {
                list.Add(FromElement(elem));
            }
            return list;
        }

        // Add the newly-created player in the database.
        public bool saveNewPlayer(Player player)
        {
            XmlDocument doc = GetFile(fullPath);
            doc.DocumentElement.AppendChild(ToElement(player, doc));
            doc.Save(fullPath);

            return true;
        }

        private XmlDocument GetFile(string fullPath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fullPath);
            }
            catch (FileNotFoundException)
            {
                // No file, create from scratch!
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlElement root = doc.CreateElement(XML_NODE_ENTITY_LIST);
                doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
                doc.AppendChild(root);
            }

            return doc;
        }

        private XmlElement ToElement(Player p, XmlDocument root)
        {
            XmlElement node = root.CreateElement(XML_NODE_ENTITY);
            node.SetAttribute(XML_PLAYER_ID, p.ID.ToString());
            node.SetAttribute(XML_PLAYER_NAME, p.Name);
            node.SetAttribute(XML_PLAYER_RATING, p.Rating.ToString());
            node.SetAttribute(XML_PLAYER_WINS, p.Wins.ToString());
            node.SetAttribute(XML_PLAYER_LOSSES, p.Losses.ToString());
            node.SetAttribute(XML_PLAYER_DRAWS, p.Draws.ToString());

            return node;
        }

        private Player FromElement(XmlElement elem)
        {
            Player p = new Player();
            p.ID = Convert.ToInt32(elem.GetAttribute(XML_PLAYER_ID));
            p.Name = elem.GetAttribute(XML_PLAYER_NAME);
            p.Rating = Convert.ToInt32(elem.GetAttribute(XML_PLAYER_RATING));
            p.Wins = Convert.ToInt32(elem.GetAttribute(XML_PLAYER_WINS));
            p.Losses = Convert.ToInt32(elem.GetAttribute(XML_PLAYER_LOSSES));
            p.Draws = Convert.ToInt32(elem.GetAttribute(XML_PLAYER_DRAWS));

            return p;
        }
        #endregion
    }
}

