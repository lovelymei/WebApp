
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Dto
{
    public class AccountDto
    {
        public AccountDto(Account account)
        {
            NickName = account.NickName;
            RoleId = account.RoleId;
        }

        public string NickName { get; set; }
        public int RoleId { get; set; }

    }
}