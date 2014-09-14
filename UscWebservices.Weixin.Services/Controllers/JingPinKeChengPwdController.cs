using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UscWebservices.Weixin.JingPinKeCheng;

namespace UscWebservices.Weixin.Services.Controllers
{
    public class JingPinKeChengPwdController : ApiController
    {

        /// <summary>
        /// 根据用户名，取登录密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>登录密码</returns>
        public string Get(string username)
        {
            var dal = new DataAccess();
            var pwd = dal.GetPasswordByUsername(username);
            return pwd;
        }

    }
}