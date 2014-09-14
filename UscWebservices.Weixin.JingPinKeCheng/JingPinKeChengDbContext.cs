namespace UscWebservices.Weixin.JingPinKeCheng
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class JingPinKeChengDbContext : DbContext
    {
        public JingPinKeChengDbContext()
            : base("name=JingPinKeCheng")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.number)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.logname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password_src)
                .IsUnicode(false);
        }
    }
}
