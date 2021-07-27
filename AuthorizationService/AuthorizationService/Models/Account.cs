using EntitiesLibrary;
using System;
using System.Collections.Generic;

namespace AuthorizationService.Models
{
    public class Account : AccountBase
    {
        public string NickName { get; set; }
        public Login Login { get; set; }
        public Roles Role{ get; set; }
        public IEnumerable<RefreshToken> RefreshTokens { get; set; }
        public Account() : base() { RefreshTokens = new List<RefreshToken>(); }

    }
}
