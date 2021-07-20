using AuthorizationService.Models;
using System;

namespace AuthorizationService.Dto
{
    public class TokenDto
    {
        public Account Account { get; set; } // Создавать отдельное Dto????? 
        public string Jwt { get; set; }
        public DateTime Expires { get; set; }
        public Guid RefreshTokenId { get; set; }
    }
}
