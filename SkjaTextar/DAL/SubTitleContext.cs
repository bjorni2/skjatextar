using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SkjaTextar.Models;


namespace SkjaTextar.DAL
{
    public class SubTitleContext : DbContext
    {
        public SubTitleContext() : base("SubTitleContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}