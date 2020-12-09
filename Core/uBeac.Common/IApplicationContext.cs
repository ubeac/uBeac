using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace uBeac.Common
{
    public interface IApplicationContext<TUserKey> where TUserKey : IEquatable<TUserKey>
    {
        TUserKey UserId { get; }
        ClaimsPrincipal User { get; }
        IPAddress UserIP { get; }
        string Language { get; set; }
        public string TraceId { get; }
        public string SessionId { get; }
        public Dictionary<object, object> ContextData { get; }
    }
    public interface IApplicationContext : IApplicationContext<Guid>
    {
    }
}
