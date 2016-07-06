using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Managers
{
    public static class DbManager
    {
        public static T GetByKey<T, DB>(int id, string include = null)
            where T : EntityBase
            where DB : DbContext, new()
        {
            using (var db = new DB())
            {
                if (string.IsNullOrEmpty(include))
                {
                    return db.Set<T>().Where(t => t.Id == id).FirstOrDefault();
                }
                else
                {
                    return db.Set<T>().Include(include).Where(t => t.Id == id).FirstOrDefault();
                }
            }
        }

        public static List<T> SelectAll<T, DB>(string include = null)
            where T : EntityBase
            where DB : DbContext, new()
        {
            using (var db = new DB())
            {
                if (string.IsNullOrEmpty(include))
                {
                    return db.Set<T>().ToList();
                }
                else
                {
                    return db.Set<T>().Include(include).ToList();
                }
            }
        }
    }
}
