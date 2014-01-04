using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using UscWebservices.Weixin.CommonServices;

namespace UscWebservices.Weixin.CommonServices
{
    public class WeixinMessageHandler : MessageHandler<MessageContext>
    {

        #region 字段

        const string returnMessage = @"欢迎使用南华大学web自助服务
目前提供
【教务系统】与【数字化教学中心】
学生帐号的密码找回服务
请发送消息：
教务，登录名
或
数字化教学中心，登录名
找回密码";

        const string subscribeMessage = @"欢迎关注南华大学web自助服务
目前提供
【教务系统】与【数字化教学中心】
学生帐号的密码找回服务
请发送消息：
教务，登录名
或
数字化教学中心，登录名
找回密码";
        #endregion

        #region 构造函数

        public WeixinMessageHandler(Stream inputStream)
            : base(inputStream)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
        }

        public WeixinMessageHandler(System.Xml.Linq.XDocument requestDocument)
            : base(requestDocument)
        {
            WeixinContext.ExpireMinutes = 3;
        }
        #endregion

        #region 实现抽象类

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //TODO:这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs

            //方法一（v0.1），此方法调用太过繁琐，已过时（但仍是所有方法的核心基础），建议使用方法二到四
            //var responseMessage =
            //    ResponseMessageBase.CreateFromRequestMessage(RequestMessage, ResponseMsgType.Text) as
            //    ResponseMessageText;

            //方法二（v0.4）
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(RequestMessage);

            //方法三（v0.4），扩展方法，需要using Senparc.Weixin.MP.Helpers;
            //var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();

            //方法四（v0.6），仅适合在HandlerMessage内部使用，本质上是对方法三的封装
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (requestMessage.Content == "约束")
            {
                responseMessage.Content = "<a href=\"http://weixin.senparc.com/FilterTest/\">点击这里</a>进行客户端约束测试（地址：http://weixin.senparc.com/FilterTest/）。";
            }
            else
            {
                var result = new StringBuilder();

                //取发送的消息
                //符号统一，全角转半角
                var content = requestMessage.Content.ConvertToDBC();

                //文本格式为：系统编码 + ， + 用户登录名
                var msg = content.Split(',');
                //检查消息是否正确
                if (msg.Length != 2)
                {
                    //result.AppendFormat("您发送了格式不正确的信息：{0}\r\n\r\n", requestMessage.Content);
                    //result.AppendLine(Environment.NewLine);
                    //result.AppendFormat("正确格式为：系统编码+','+登录名，如：教务,20129060107");
                    result.AppendFormat(returnMessage);
                }
                else
                {
                    var pwd = string.Empty;
                    switch (msg[0])
                    {
                        case "教务":
                            pwd = JiaoWuGetPassword(msg[1]);
                            break;
                        default:
                        case "数字化教学中心":
                            pwd = JingPinKeChengGetPassword(msg[1]);
                            break;
                    }
                    //创建返回消息
                    CreateMessage(msg, pwd, ref result);
                }
                responseMessage.Content = result.ToString();
            }

            return responseMessage;
        }

        /// <summary>
        /// 创建返回消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="pwd"></param>
        /// <param name="result"></param>
        private void CreateMessage(string[] msg, string pwd, ref StringBuilder result)
        {
            if (string.IsNullOrWhiteSpace(pwd))
            {
                result.AppendFormat("只提供学生帐号密码找回{0}请检查帐号{1}欢迎下次使用", Environment.NewLine, Environment.NewLine);
            }
            else
            {
                result.AppendFormat("登录名【{0}】{1}", msg[1], Environment.NewLine);
                result.AppendFormat("在【{0}】的登录密码为：{1}{2}{3}", msg[0], Environment.NewLine, pwd, Environment.NewLine);
                result.AppendLine("请妥善保管!");
            }
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(RequestMessage);
            responseMessage.Content = "这里是正文内容，一共将发2条Article。";
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = requestMessage.PicUrl,
                Url = "http://weixin.senparc.com"
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "第二条",
                Description = "第二条带连接的内容",
                PicUrl = requestMessage.PicUrl,
                Url = "http://weixin.senparc.com"
            });
            return responseMessage;
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageMusic>(requestMessage);
            responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
Title：{0}
Description:{1}
Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(RequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            throw new Exception("暂不可用");
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);

            responseMessage.Content = subscribeMessage;

            //获取Senparc.Weixin.MP.dll版本信息
            //            var fileVersionInfo = FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
            //            var version = string.Format("{0}.{1}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart);
            //            responseMessage.Content = string.Format(@"欢迎关注【Senparc.Weixin.MP 微信公众平台SDK】，当前运行版本：v{0}。
            //                                              您可以发送【文字】【位置】【图片】【语音】等不同类型的信息，查看不同格式的回复。
            //                                              SDK官方地址：http://weixin.senparc.com
            //                                              源代码及Demo下载地址：https://github.com/JeffreySu/WeiXinMPSDK",
            //                                                                                                    version);
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            throw new Exception("Demo中还没有加入CLICK的测试！");
        }

        #endregion

        #region 私有方法

        private string JingPinKeChengGetPassword(string username)
        {
            var dal = new JingPinKeCheng.DataAccess();
            return dal.GetPasswordByUsername(username);
        }

        private string JiaoWuGetPassword(string username)
        {
            var dal = new JiaoWu.DataAccess();
            return dal.GetPasswordByUsername(username);
        }
        #endregion




    }
}