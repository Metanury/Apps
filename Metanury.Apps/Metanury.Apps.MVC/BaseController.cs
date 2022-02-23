using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;

namespace Metanury.Apps.MVC
{
    public class BaseController : Controller
    {
        protected IActionContextAccessor Accessor { get; set; }

        protected IConfiguration Config { get; set; }

        public BaseController(IActionContextAccessor accessor) : base()
        {
            this.Accessor = accessor;
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
    }
}
