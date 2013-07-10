using System;
using System.Collections.Generic;
using System.Linq;
using Senparc.Weixin.MP;
using System.IO;
using UscWebservices.Weixin.CommonServices;

namespace UscWebservices.Weixin.WebForms
{
    public partial class Weixin : System.Web.UI.Page
    {

        //与微信公众账号后台的Token设置保持一致，区分大小写。
        private const string Token = "UscJpkc";

        protected void Page_Load(object sender, EventArgs e)
        {
            //取传入参数
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];

            //记录传入参数
            SaveParameters(signature, timestamp, nonce, echostr);

            //标准回应
            if (Request.HttpMethod == "GET")
            {
                //get method - 仅在微信后台填写URL验证时触发
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    WriteContent(String.Format("failed:{0},{1}", signature, CheckSignature.GetSignature(timestamp, nonce, Token)));
                }
                Response.End();
            }
            else
            {
                //post method - 当有用户想公众账号发送消息时触发
                if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent("参数错误！");
                }
                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new WeixinMessageHandler(Request.InputStream);

                //var inputStream = Request.InputStream;
                //var pos = inputStream.Position;
                //var reader = new StreamReader(inputStream);
                //inputStream.Position = 0;
                //var text = reader.ReadToEnd();
                //inputStream.Position = pos;

                //var xml = XDocument.Parse(text);

                //var messageHandler = new WeixinMessageHandler(xml);
                try
                {
                    //测试时可开启此记录，帮助跟踪数据
                    SaveRequest(messageHandler);
                    //执行微信处理过程
                    messageHandler.Execute();
                    //测试时可开启，帮助跟踪数据
                    SaveResponse(messageHandler);
                    WriteContent(messageHandler.ResponseDocument.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    //记录错误
                    SaveError(messageHandler, ex);
                    WriteContent("");
                }
                finally
                {
                    Response.End();
                }

                ////执行微信处理过程
                //messageHandler.Execute();
                ////输出结果
                //WriteContent(messageHandler.ResponseDocument.ToString());
            }
            //Response.End();
        }

        #region 私有方法
        
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <param name="ex"></param>
        private void SaveError(WeixinMessageHandler messageHandler, Exception ex)
        {
            using (TextWriter tw = new StreamWriter(Server.MapPath(String.Format("~/App_Data/Error_{0}.txt", DateTime.Now.Ticks))))
            {
                tw.WriteLine(ex.Message);
                tw.WriteLine(ex.InnerException.Message);
                if (messageHandler.ResponseDocument != null)
                {
                    tw.WriteLine(messageHandler.ResponseDocument.ToString());
                }
                tw.Flush();
                tw.Close();
            }
        }

        /// <summary>
        /// 记录响应消息
        /// </summary>
        /// <param name="messageHandler"></param>
        private void SaveResponse(WeixinMessageHandler messageHandler)
        {
            messageHandler.ResponseDocument.Save(
                Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" +
                               messageHandler.ResponseMessage.ToUserName + ".txt"));
        }

        /// <summary>
        /// 记录请求消息
        /// </summary>
        /// <param name="messageHandler"></param>
        private void SaveRequest(WeixinMessageHandler messageHandler)
        {
            messageHandler.RequestDocument.Save(
                Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" +
                               messageHandler.RequestMessage.FromUserName + ".txt"));
        }

        /// <summary>
        /// 记录传入的QueryString
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        private void SaveParameters(string signature, string timestamp, string nonce, string echostr)
        {
            //记录参数
            using (TextWriter tw = new StreamWriter(Server.MapPath(String.Format("~/App_Data/QueryStrings_{0}.txt", DateTime.Now.Ticks))))
            {
                tw.WriteLine("Signature = " + signature);
                tw.WriteLine("Timestamp = " + timestamp);
                tw.WriteLine("Nonce = " + nonce);
                tw.WriteLine("Echostr = " + echostr);
                tw.Flush();
                tw.Close();
            }
        }

        /// <summary>
        /// 写入响应文本
        /// </summary>
        /// <param name="str"></param>
        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }
        #endregion
                
    }

}