using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace UscWebservices.Weixin.JingPinKeCheng
{
    public class JingPinKeChengEntities : DbContext
    {
        public JingPinKeChengEntities()
            : base("name=JingPinKeChengEntities")
        {            
        }

        public DbSet<User> Users { get; set; }
    }
}
