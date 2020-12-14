namespace uBeac.Identity.MongoDB
{
    public class MongoDBIdentityOptions
    {
        public string ConnectionStringName { get; set; } = "AuthConnectionString";
        public string UsersCollection { get; set; } = "Users";
        public string RolesCollection { get; set; } = "Roles";
        public string RoleClaimsCollection { get; set; } = "RoleClaims";
        public string UserClaimsCollection { get; set; } = "UserClaims";
        public string UserLoginsCollection { get; set; } = "UserLogins";
        public string UserRolesCollection { get; set; } = "UserRoles";
        public string UserTokensCollection { get; set; } = "UserTokens";
    }
}
