using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace UscWebservices.Weixin.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //教务系统密码找回映射配置
            config.Routes.MapHttpRoute(
                name: "JiaoWuPwdApiRoute",
                routeTemplate: "api/JiaoWu/Pwd/{username}",
                defaults: new { controller = "JiaoWuPwd" }
            );

            //精品课程密码找回映射配置
            config.Routes.MapHttpRoute(
                name: "JingPinKeChengPwdApiRoute",
                routeTemplate: "api/JingPinKeCheng/Pwd/{username}",
                defaults: new { controller = "JingPinKeChengPwd" }
            );

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


    }
}
