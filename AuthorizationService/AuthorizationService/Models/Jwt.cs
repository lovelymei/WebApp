using System;

namespace AuthorizationService.Models
{
    public class Jwt
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public Guid RefreshTokenId { get; set; }
    }
}
