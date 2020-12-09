using Microsoft.AspNetCore.Identity;
using System;
using uBeac.Common.Identity;
using uBeac.Repositories.Abstractions;

namespace uBeac.Identity.Repositories.Abstractions
{
    public interface IRoleRepository<TKey, TRole> :
        IEntityRepository<TKey, TRole>,
        IRoleClaimStore<TRole>,
        IQueryableRoleStore<TRole>
        where TKey : IEquatable<TKey>
        where TRole : BaseIdentityRole<TKey>
    {
    }

    public interface IRoleRepository<TRole> :
        IRoleRepository<Guid, TRole>
        where TRole : BaseIdentityRole<Guid>
    {
    }
}
