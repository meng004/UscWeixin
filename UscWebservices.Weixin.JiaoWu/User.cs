namespace UscWebservices.Weixin.JiaoWu
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class User
    {
        [Key]
        [Column(Order = 0)]
        public Guid XTYHID { get; set; }

        public Guid? YHID { get; set; }

        public Guid? USERID { get; set; }

        [StringLength(20)]
        public string YHJB { get; set; }

        [StringLength(20)]
        public string YHLX { get; set; }

        [StringLength(20)]
        public string SJBSMC { get; set; }

        public Guid? YXSID { get; set; }

        [StringLength(200)]
        public string YXSMC { get; set; }

        [StringLength(200)]
        public string XM { get; set; }

        [StringLength(4)]
        public string XB { get; set; }

        [StringLength(1)]
        public string SJBS { get; set; }

        [StringLength(2)]
        public string YHJBM { get; set; }

        [StringLength(2)]
        public string YHLXM { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Password { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsLockedOut { get; set; }

        [StringLength(30)]
        public string SFZH { get; set; }
    }
}
