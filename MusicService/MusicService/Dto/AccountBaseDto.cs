using EntitiesLibrary;
using System;

namespace MusicService.Dto
{
    public class AccountBaseDto 
    {
        public AccountBaseDto(AccountBase accountBase)
        {
            AccountId = accountBase.AccountId;
            IsDeleted = accountBase.IsDeleted;
        }

        public Guid AccountId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
