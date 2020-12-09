using Microsoft.AspNetCore.Identity;
using System;
using uBeac.Common.Identity;
using uBeac.Repositories.Abstractions;

namespace uBeac.Identity.Repositories.Abstractions
{
    public interface IUserRepository<TKey, TUser, TRole> :
          IEntityRepository<TKey, TUser>,
          IUserClaimStore<TUser>,
          IUserLoginStore<TUser>,
          IUserRoleStore<TUser>,
          IUserPasswordStore<TUser>,
          IUserSecurityStampStore<TUser>,
          IUserEmailStore<TUser>,
          IUserPhoneNumberStore<TUser>,
          IQueryableUserStore<TUser>,
          IUserTwoFactorStore<TUser>,
          IUserLockoutStore<TUser>,
          IUserAuthenticatorKeyStore<TUser>,
          IUserAuthenticationTokenStore<TUser>,
          IUserTwoFactorRecoveryCodeStore<TUser>,
          IProtectedUserStore<TUser>
          where TKey : IEquatable<TKey>
          where TUser : BaseIdentityUser<TKey>
          where TRole : BaseIdentityRole<TKey>
    {
    }

    public interface IUserRepository<TUser, TRole> : IUserRepository<Guid, TUser, TRole>
        where TUser : BaseIdentityUser<Guid>
        where TRole : BaseIdentityRole<Guid>
    {

    }
}
