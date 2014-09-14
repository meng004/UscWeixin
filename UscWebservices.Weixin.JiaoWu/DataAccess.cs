using System.Linq;


namespace UscWebservices.Weixin.JiaoWu
{
    public class DataAccess : UscWebservices.Weixin.IServices.IFindPwd
    {
        /// <summary>
        /// 由登录用户名，取密码
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetPasswordByUsername(string username)
        {
            using (var context = new JiaoWuEntities())
            {
                //只处理学生帐号
                var user = context.V_SYS_UsersInfo.First(t =>
                    t.UserName == username && t.YHLXM == "00"
                    );

                //处理帐号错误
                if (user == null)
                {
                    return string.Empty;
                }
                else
                {
                    return user.Password;
                }
            }
        }
    }
}
