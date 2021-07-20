using System.Collections.Generic;

namespace AuthorizationService.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Account> Accounts { get; set; }

        public Role()
        {
            Accounts = new List<Account>();
        }
    }
}