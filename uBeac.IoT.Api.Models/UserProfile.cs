using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class UserProfile : IEntity
    {
        public UserProfile()
        {
            TeamUsers = new HashSet<TeamUser>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<TeamUser> TeamUsers { get; set; }
    }
}
