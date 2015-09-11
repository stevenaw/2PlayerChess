using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ChessGameLib
{
    public enum PlayerDBTypes {
        SQL,
        DAT,
        XML
    }

    public class DBTypeConverter
    {
        public PlayerDBTypes Parse(string typeStr)
        {
            string s = typeStr.ToLower();

            switch (s)
            {
                case "dat":
                    return PlayerDBTypes.DAT;
                case "sql":
                    return PlayerDBTypes.SQL;
                case "xml":
                default:
                    return PlayerDBTypes.XML;
            }
        }

    }

    public class PlayerDBFactory
    {
        private static PlayerDBFactory instance = null;

        private PlayerDBFactory()
        {

        }

        public static PlayerDBFactory GetInstance()
        {
            if (instance == null)
                instance = new PlayerDBFactory();

            return instance;
        }

        public string GetConnectionString(PlayerDBTypes connectionType)
        {
            string retVal = String.Empty;

            switch (connectionType)
            {
                case PlayerDBTypes.SQL:
                    retVal = Properties.Settings.Default.SqlConnectionString;
                    break;

                case PlayerDBTypes.XML:
                    retVal = Properties.Settings.Default.StorageDir + Properties.Settings.Default.XmlConnectionString;
                    break;

                case PlayerDBTypes.DAT:
                default:
                    retVal = Properties.Settings.Default.StorageDir + Properties.Settings.Default.BinConnectionString;
                    break;
            }

            return retVal;
        }

        public IPlayerDB GetDBObj(PlayerDBTypes type)
        {
            return GetDBObj(type, GetConnectionString(type));
        }

        public IPlayerDB GetDBObj(PlayerDBTypes type, string connectionStr)
        {
            IPlayerDB returnVal;

            switch (type)
            {
                case PlayerDBTypes.SQL:
                    returnVal = new PlayerSQL(connectionStr);
                    break;
                case PlayerDBTypes.DAT:
                    returnVal = new PlayerDAT(connectionStr);
                    break;
                case PlayerDBTypes.XML:
                    returnVal = new PlayerXML(connectionStr);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return returnVal;
        }
    }
}
