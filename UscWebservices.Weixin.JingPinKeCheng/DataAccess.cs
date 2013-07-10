using System;
using System.Collections.Generic;
using System.Linq;

namespace UscWebservices.Weixin.JingPinKeCheng
{
    public class DataAccess
    {
        /// <summary>
        /// 由登录用户名，取密码
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetPasswordByUsername(string username)
        {
            using (var context = new JingPinKeChengEntities())
            {
                //只接受学生帐号
                var user = context.user.First(t =>
                    t.logname == username
                    );
                //处理帐号错误
                if (user == null)
                {
                    return string.Empty;
                }
                else if (user.user_type == 2)
                {
                    return user.password_src;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
