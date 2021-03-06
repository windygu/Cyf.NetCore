namespace Cyf.EF.Temperary.Model.Mssql
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CyfMSSQLContext : DbContext
    {
        public CyfMSSQLContext()
            : base("name=CyfMSSQLConnect")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsFixedLength();
        }
    }
}
