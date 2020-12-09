using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using uBeac.Common;

namespace uBeac.Web
{
    public class ApplicationContext<TUserKey> : IApplicationContext<TUserKey> where TUserKey : IEquatable<TUserKey>
    {
        public TUserKey UserId { get; protected set; }
        public IPAddress UserIP { get; protected set; }
        public string Language { get; set; }
        public ClaimsPrincipal User { get; }
        public string TraceId { get; protected set; }
        public string SessionId { get; protected set; }
        public Dictionary<object, object> ContextData { get; }

        protected readonly IServiceProvider ServiceProvider;
        protected readonly IHttpContextAccessor HttpContextAccessor;

        public ApplicationContext(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            ContextData = new Dictionary<object, object>();
            ServiceProvider = serviceProvider;
            HttpContextAccessor = httpContextAccessor;
            User = HttpContextAccessor.HttpContext.User;
            UserIP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            TraceId = HttpContextAccessor.HttpContext.TraceIdentifier;
            // todo: support multi language with Accept-Language header
            //Language = HttpContextAccessor.HttpContext.Request.Cookies.TryGetValue("Language", out string language) ? language : "EN";
        }
    }

    public class ApplicationContext : ApplicationContext<Guid>, IApplicationContext
    {
        public ApplicationContext(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, httpContextAccessor)
        {
            var userIdClaim = HttpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).SingleOrDefault();
            if (userIdClaim is null)
                UserId = Guid.Empty;
        }
    }
}
