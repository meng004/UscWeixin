using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UscWebservices.Weixin.JingPinKeCheng
{
    public class User
    {
        public string number { get; set; }
        public string logname { get; set; }
        public string password { get; set; }
        public byte user_type { get; set; }
        public string password_src { get; set; }
    }
}

