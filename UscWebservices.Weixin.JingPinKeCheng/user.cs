namespace UscWebservices.Weixin.JingPinKeCheng
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("scr2008.user")]
    public class User
    {
        [Key]
        [StringLength(20)]
        public string number { get; set; }

        [Required]
        [StringLength(50)]
        public string logname { get; set; }

        [Required]
        [StringLength(32)]
        public string password { get; set; }

        public byte user_type { get; set; }

        [StringLength(50)]
        public string password_src { get; set; }
    }
}
