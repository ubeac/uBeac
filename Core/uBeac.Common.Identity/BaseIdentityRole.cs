//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;

//namespace uBeac.Common.Identity
//{
//    public abstract class BaseIdentityRole<TKey> : IdentityRole<TKey>, IEntity<TKey> where TKey : IEquatable<TKey>
//    {
//        public List<IdentityRoleClaim<TKey>> Claims { get; set; } = new List<IdentityRoleClaim<TKey>>();
//    }

//    public abstract class BaseIdentityRole : BaseIdentityRole<Guid>
//    {
//    }
//}
