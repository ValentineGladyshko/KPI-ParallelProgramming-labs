using Lab9.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Database
{
    public class DB : DbContext
    {
        public DbSet<Tariff> Tariffs { get; set; }
        

        static DB()
        {
            System.Data.Entity.Database.SetInitializer<DB>(new StoreDbInitializer());
        }
        public DB()
            : base("name=DB")
        {
        }
    }

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<DB>
    {
        protected override void Seed(DB db)
        {
            db.SaveChanges();
        }
    }
}