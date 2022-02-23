using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Metanury.Apps.MVC
{
    public class BaseController : Controller
    {
        protected IActionContextAccessor Accessor { get; set; }

        protected IConfiguration Config { get; set; }

        protected MailService MailServiceInfo { get; set; }

        public BaseController(IActionContextAccessor accessor) : base()
        {
            this.Accessor = accessor;
        }

        protected virtual T SessionGet<T>(string key)
        {
            return HttpContext.Session.Get<T>(key);
        }

        protected virtual object SessionGet(string key)
        {
            try
            {
                using (var stream = new MemoryStream(HttpContext.Session.Get(key)))
                {
                    var binaryFormatter = new BinaryFormatter();
                    stream.Position = 0;
                    return binaryFormatter.Deserialize(stream);
                }
            }
            catch
            {
                
            }

            return null;
        }

        protected virtual void SessionSet<T>(string key, T t)
        {
            HttpContext.Session.Set<T>(key, t);
        }

        protected virtual void SessionSet(string key, object value)
        {
            HttpContext.Session.Set(key, value);
        }

        protected virtual string CookieGet(string key)
        {
            return this.Accessor.ActionContext.HttpContext.Request.Cookies[key];
        }

        protected virtual void CookieSet(string key, string value, DateTime? expireTime = null)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
            {
                option.Expires = expireTime.Value;
            }

            this.Accessor.ActionContext.HttpContext.Response.Cookies.Append(key, value, option);
        }

        protected virtual void CookieErase(string key)
        {
            this.Accessor.ActionContext.HttpContext.Response.Cookies.Delete(key);
        }

        protected virtual IConfigurationSection GetConfigSection(string sectionName)
        {
            return this.Config.GetSection(sectionName);
        }

        protected virtual string GetConfigValue(string sectionName)
        {
            return this.Config.GetSection(sectionName).Value;
        }

        public virtual HtmlString WriteContent(string content, bool IsEnter = false)
        {
            if (IsEnter)
            {
                content = content.Replace("\r\n", "<br/>");
                content = content.Replace("\r", "<br/>");
                content = content.Replace("\n", "<br/>");
                content = content.Replace(Environment.NewLine, "<br/>");
            }

            return new HtmlString(content);
        }

        /*
            var mail = new MailData(this.MailServiceInfo);
            mail.ReceiverName = "홍길동";
            mail.ReceiverMail = "test@test.com";
            mail.Subject = "메일 발송 테스트";
            mail.SenderMail = "webmaster@metanury.com";
            mail.Body = "<p>Hello, World~</p>";
            mail.SenderName = "관리자";
            this.SendMail(mail);
        */
        protected bool SendMail(MailData mail)
        {
            bool result = false;

            try
            {
                var message = new MimeMessage();
                var from = new MailboxAddress(mail.SenderName, mail.SenderMail);
                var to = new MailboxAddress(mail.ReceiverName, mail.ReceiverMail);
                message.From.Add(from);
                message.To.Add(to);
                message.Subject = mail.Subject;
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = mail.Body;
                message.Body = bodyBuilder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.Connect(mail.SmtpAddress, mail.SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
                    client.Authenticate(mail.AccountID, mail.AccountPW);
                    client.Send(message);
                    client.Disconnect(true);
                    result = true;
                }
            }
            catch 
            {
                result = false;
            }

            return result;
        }
    }
}
