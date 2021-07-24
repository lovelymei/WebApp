using System;
using System.Collections.Generic;

namespace AuthorizationService.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Account> Accounts { get; set; }

        public Role()
        {
            Accounts = new List<Account>();
        }
    }
}