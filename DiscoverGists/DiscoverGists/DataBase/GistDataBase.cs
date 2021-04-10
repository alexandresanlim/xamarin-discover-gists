using DiscoverGists.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscoverGists.DataBase
{
    public abstract class GistDataBase : BaseDatabase
    {
        public static ILiteCollection<Gist> ItemCollection => GetDatabase.GetCollection<Gist>();

        public static List<Gist> GetAll()
        {
            try
            {
                return ItemCollection.FindAll().ToList();
            }
            catch (Exception)
            {
                return new List<Gist>();
            }
        }

        public static bool UpInsert(Gist item)
        {
            try
            {
                DateTime now = DateTime.Now;
                item.AddedFavorite = now;

                return ItemCollection.Upsert(item);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Remove(Gist item)
        {
            try
            {
                return ItemCollection.Delete(item.Id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveAll()
        {
            try
            {
                return ItemCollection.DeleteAll() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
