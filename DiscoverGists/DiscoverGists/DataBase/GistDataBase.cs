﻿using DiscoverGists.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DiscoverGists.DataBase
{
    public abstract class GistDataBase : BaseDatabase
    {
        public static ILiteCollection<Gist> ItemCollection => GetDatabase.GetCollection<Gist>();

        public static List<Gist> GetAll(int skip)
        {
            try
            {
                return ItemCollection
                    .FindAll()
                    .Where(x => !string.IsNullOrEmpty(x?.Owner?.Login))
                    .OrderBy(x => x.Owner.Login)
                    .Skip(skip)
                    .Take(5)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<Gist>();
            }
        }

        public static List<Gist> Find(Expression<Func<Gist, bool>> predicate)
        {
            try
            {
                return ItemCollection.Find(predicate).ToList();
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

        public static Gist FindById(Gist item)
        {
            try
            {
                return ItemCollection.FindById(item.Id);
            }
            catch (Exception)
            {
                return new Gist();
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
