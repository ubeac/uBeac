//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;

//namespace uBeac.Common.Identity
//{
//    public abstract class BaseIdentityUser<TKey> : IdentityUser<TKey>, IBaseEntity<TKey> where TKey : IEquatable<TKey>
//    {
//        public TKey CreateBy { get; set; }
//        public TKey UpdateBy { get; set; }
//        public DateTime CreateDate { get; set; }
//        public DateTime UpdateDate { get; set; }
//        public List<TKey> Roles { get; set; } = new List<TKey>();
//        public List<IdentityUserClaim<TKey>> Claims { get; set; } = new List<IdentityUserClaim<TKey>>();
//        public List<IdentityUserLogin<TKey>> Logins { get; set; } = new List<IdentityUserLogin<TKey>>();
//        public List<IdentityUserToken<TKey>> Tokens { get; set; } = new List<IdentityUserToken<TKey>>();
//        public List<TwoFactorRecoveryCode> RecoveryCodes { get; set; } = new List<TwoFactorRecoveryCode>();
//        public string AuthenticatorKey { get; set; }
//    }

//    public abstract class BaseIdentityUser : BaseIdentityUser<Guid>, IBaseEntity
//    {
//    }
//}
