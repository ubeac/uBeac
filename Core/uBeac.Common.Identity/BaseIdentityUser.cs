using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace uBeac.Common.Identity
{
    public abstract class BaseIdentityUser<TKey> : IdentityUser<TKey>, IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey CreateBy { get; set; }
        public TKey UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<TKey> Roles { get; set; }
        public List<IdentityUserClaim<TKey>> Claims { get; set; }
        public List<IdentityUserLogin<TKey>> Logins { get; set; }
        public List<IdentityUserToken<TKey>> Tokens { get; set; }
        public List<TwoFactorRecoveryCode> RecoveryCodes { get; set; }
        public string AuthenticatorKey { get; set; }
    }

    public abstract class BaseIdentityUser : BaseIdentityUser<Guid>, IBaseEntity
    {
    }
}
