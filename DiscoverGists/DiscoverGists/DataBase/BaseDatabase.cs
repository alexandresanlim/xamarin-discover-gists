using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscoverGists.DataBase
{
    public abstract class BaseDatabase
    {
        private static LiteDatabase _dataBase;

        public static LiteDatabase GetDatabase
        {
            get
            {
                if (_dataBase == null)
                {
                    _dataBase = new LiteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "discovergists.db"));
                }

                return _dataBase;
            }
        }
    }
}
