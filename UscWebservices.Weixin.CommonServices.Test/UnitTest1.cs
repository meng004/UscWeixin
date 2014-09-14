using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace UscWebservices.Weixin.CommonServices.Test
{
    [TestClass]
    public class UnitTest1
    {

        #region xml报文

        const string xmlTextForJiaoWuSuccess = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xml>
    <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
    <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
    <CreateTime>1357986928</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[教务,nhu_xjzx]]></Content>
    <MsgId>5832509444155992350</MsgId>
</xml>
";

        const string xmlTextForJiaoWuFailure = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xml>
    <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
    <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
    <CreateTime>1357986928</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[]]></Content>
    <MsgId>5832509444155992350</MsgId>
</xml>
";

        const string xmlTextForJingPinKeChengSuccess = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xml>
    <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
    <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
    <CreateTime>1357986928</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[数字化教学中心,330011]]></Content>
    <MsgId>5832509444155992350</MsgId>
</xml>
";

        const string xmlTextForJingPinKeChengFailure = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xml>
    <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
    <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
    <CreateTime>1357986928</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[]]></Content>
    <MsgId>5832509444155992350</MsgId>
</xml>
";
        #endregion

        [TestMethod]
        public async Task WeixinMessageHandlerTest()
        {
            //取传入参数
            string signature = "a6a62a99a70b4ef26341b848395aca425b40f516";
            string timestamp = "1357986928";
            string nonce = "1357986928";
            string echostr = "1357986928";

            //构造访问Url，包含QueryString
            string query = string.Format("http://localhost:2432/Weixin.aspx?" +
                "signature={0}&" +
                "timestamp={1}&" +
                "nonce={2}&" +
                "echostr={3}",
                signature,
                timestamp,
                nonce,
                echostr
                );

            //访问客户端
            HttpClient client = new HttpClient();
            //创建HttpContent类型的实例
            var content = new StringContent(xmlTextForJiaoWuSuccess);
            //使用Post方法，发起异步请求
            HttpResponseMessage response = await client.PostAsync(query, content);
            //检查响应信息，操作是否成功
            response.EnsureSuccessStatusCode();
            Assert.IsTrue(response.IsSuccessStatusCode);

            //取出响应报文
            var contents = await response.Content.ReadAsStreamAsync();
            XmlTextReader reader = new XmlTextReader(contents);

            var result = string.Empty;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.LocalName == "Content")
                    {
                        result = reader.ReadString();
                        break;
                    }
                }
            }
            //关闭阅读器
            reader.Close();

            StringAssert.Contains(result, "密码");
        }

        [TestMethod]
        public void GetSignatureTest()
        {
            //取传入参数
            string timestamp = "1357986928";
            string nonce = "1357986928";
            string token = "UscJpkc";
            var signature = CheckSignature.GetSignature(timestamp, nonce, token);
            //a6a62a99a70b4ef26341b848395aca425b40f516
            Assert.IsNotNull(signature);
        }

        [TestMethod]
        public void OnTextRequestForJiaoWuSuccessTest()
        {
            var messageHandlers = new WeixinMessageHandler(XDocument.Parse(xmlTextForJiaoWuSuccess));
            Assert.IsNotNull(messageHandlers.RequestDocument);
            messageHandlers.Execute();
            Assert.IsNotNull(messageHandlers.ResponseMessage);
            Assert.IsNotNull(messageHandlers.ResponseDocument);
            Console.Write(messageHandlers.ResponseDocument.ToString());

            Assert.AreEqual("gh_a96a4a619366", messageHandlers.ResponseMessage.FromUserName);

            var responseMessage = messageHandlers.ResponseMessage as ResponseMessageText;
            Assert.IsNotNull(responseMessage);
            Assert.IsTrue(responseMessage.Content.Contains("登录密码"));
        }



        [TestMethod]
        public void OnTextRequestForJiaoWuFailureTest()
        {
            var messageHandlers = new WeixinMessageHandler(XDocument.Parse(xmlTextForJiaoWuFailure));
            Assert.IsNotNull(messageHandlers.RequestDocument);
            messageHandlers.Execute();
            Assert.IsNotNull(messageHandlers.ResponseMessage);
            Assert.IsNotNull(messageHandlers.ResponseDocument);
            Console.Write(messageHandlers.ResponseDocument.ToString());

            Assert.AreEqual("gh_a96a4a619366", messageHandlers.ResponseMessage.FromUserName);

            var responseMessage = messageHandlers.ResponseMessage as ResponseMessageText;
            Assert.IsNotNull(responseMessage);
            Assert.IsTrue(responseMessage.Content.Contains("欢迎"));
        }

        [TestMethod]
        public void OnTextRequestForJingPinKeChengSuccessTest()
        {
            var messageHandlers = new WeixinMessageHandler(XDocument.Parse(xmlTextForJingPinKeChengSuccess));
            Assert.IsNotNull(messageHandlers.RequestDocument);
            messageHandlers.Execute();
            Assert.IsNotNull(messageHandlers.ResponseMessage);
            Assert.IsNotNull(messageHandlers.ResponseDocument);
            Console.Write(messageHandlers.ResponseDocument.ToString());

            Assert.AreEqual("gh_a96a4a619366", messageHandlers.ResponseMessage.FromUserName);

            var responseMessage = messageHandlers.ResponseMessage as ResponseMessageText;
            Assert.IsNotNull(responseMessage);
            Assert.IsTrue(responseMessage.Content.Contains("登录密码"));
        }



        [TestMethod]
        public void OnTextRequestForJingPinKeChengFailureTest()
        {
            var messageHandlers = new WeixinMessageHandler(XDocument.Parse(xmlTextForJingPinKeChengFailure));
            Assert.IsNotNull(messageHandlers.RequestDocument);
            messageHandlers.Execute();
            Assert.IsNotNull(messageHandlers.ResponseMessage);
            Assert.IsNotNull(messageHandlers.ResponseDocument);
            Console.Write(messageHandlers.ResponseDocument.ToString());

            Assert.AreEqual("gh_a96a4a619366", messageHandlers.ResponseMessage.FromUserName);

            var responseMessage = messageHandlers.ResponseMessage as ResponseMessageText;
            Assert.IsNotNull(responseMessage);
            Assert.IsTrue(responseMessage.Content.Contains("欢迎"));
        }
    }
}
