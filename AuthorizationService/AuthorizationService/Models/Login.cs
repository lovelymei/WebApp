using System;

namespace AuthorizationService.Models
{
    public class Login
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public Account Account { get; set; }

    }
}
