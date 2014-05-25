using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UscWebservices.Weixin.IServices
{
    /// <summary>
    /// 密码找回接口
    /// </summary>
    public interface IFindPwd
    {
        /// <summary>
        /// 根据用户名取密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>密码</returns>
        string GetPasswordByUsername(string username);
    }
}
