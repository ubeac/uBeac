using System;

namespace uBeac.IoT.Models
{
    public class UserAccess
    {
        public Guid UserId { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
