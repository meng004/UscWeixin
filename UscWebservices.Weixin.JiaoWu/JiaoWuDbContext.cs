namespace UscWebservices.Weixin.JiaoWu
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class JiaoWuDbContext : DbContext
    {
        public JiaoWuDbContext()
            : base("name=JiaoWu")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.YHJB)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.YHLX)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.SJBSMC)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.XB)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.SJBS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.YHJBM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.YHLXM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.SFZH)
                .IsUnicode(false);
        }
    }
}
