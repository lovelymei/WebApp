using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Dto
{
    public class AccountDtoForAuthorization
    {
        public AccountDtoForAuthorization(Account account)
        {
            Id = account.AccountId;
            NickName = account.NickName;
            RoleName = account.Role.Name; ///??? как достать роль?????
        }

        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string RoleName { get; set; }
    }
}
