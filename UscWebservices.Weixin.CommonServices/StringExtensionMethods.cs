namespace UscWebservices.Weixin.CommonServices
{

    using System.Text.RegularExpressions;

    public static class StringExtensionMethods
    {
        /// <summary>
        /// 字符串转换为整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ConvertToInt(this string str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// 字符串转换为布尔
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ConvertToBool(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            bool rtnValue = false;

            rtnValue = bool.TryParse(str, out rtnValue) ? rtnValue : Regex.IsMatch(str, "^(?i:true|yes|y|1|ok|是|同意|通过|外借|已归还|征订成功)$");

            return rtnValue;
        }

        /// <summary>
        /// 字符串转换为数值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(this string str)
        {
            decimal result = 0;
            decimal.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// 字符串转GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static System.Guid ConvertToGuid(this string str)
        {
            System.Guid result = System.Guid.Empty;
            System.Guid.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// 字符串转日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static System.DateTime ConvertToDateTime(this string str)
        {
            System.DateTime result = System.DateTime.Now;
            System.DateTime.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="str">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ConvertToSBC(this string str)
        {
            //半角转全角：
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }



        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="str">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ConvertToDBC(this string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }
    }
}
