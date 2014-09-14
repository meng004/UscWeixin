namespace UscWebservices.Weixin.JiaoWu
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class JiaoWuEntities : DbContext
    {
        public JiaoWuEntities()
            : base("name=JiaoWu")
        {
        }

        public virtual DbSet<V_SYS_UsersInfo> Users { get; set; }
    }
}
