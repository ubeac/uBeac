using System;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class UserProfile : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Picture { get; set; }
    }
}
