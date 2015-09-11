using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace ChessGameLib
{
    public interface IMapper
    {
        ICollection GetList();
        void Update(Object o);
        Object GetItem(Object o);
        void UpdateItem(Object o);
        void CreateItem(Object o);
        void DeleteItem(int id);
    }

    public abstract class MapperFactory
    {
        private static volatile MapperFactory instance = null;

        protected MapperFactory() { }

        public abstract IMapper GetMapper();

        public MapperFactory GetInstance()
        {
            if (instance != null)
            {
                lock (instance)
                {
                    if (instance != null)
                    {
                        // check settings, instance proper of below factories
                    }
                }
            }

            return instance;
        }
    }

    public sealed class DATMapperFactory : MapperFactory
    {
        private DATMapperFactory() { }

        public override IMapper GetMapper() { return null; }
    }

    public sealed class SqlMapperFactory : MapperFactory
    {
        private SqlMapperFactory() { }

        public override IMapper GetMapper() { return null; }
    }

    public sealed class XmlMapperFactory : MapperFactory
    {
        private XmlMapperFactory() { }

        public override IMapper GetMapper() { return null; }
    }
}
