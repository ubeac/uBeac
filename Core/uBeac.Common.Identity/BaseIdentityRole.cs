using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace uBeac.Common.Identity
{
    public abstract class BaseIdentityRole<TKey> : IdentityRole<TKey>, IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey CreateBy { get; set; }
        public TKey UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<IdentityRoleClaim<TKey>> Claims { get; set; }
    }

    public abstract class BaseIdentityRole : BaseIdentityRole<Guid>
    {
    }
}
