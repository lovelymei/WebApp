using AuthorizationService.Models;
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
            Role = account.Role.ToString();
        }

        public string NickName { get; set; }
        public string Role { get; set; }

    }
}