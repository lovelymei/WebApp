using AuthorizationService.Dto;
using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public interface IAccounts
    {
        /// <summary>
        /// Аутентификация 
        /// </summary>
        /// <param name="email"> e-mail</param>
        /// <param name="password"> пароль </param>
        /// <returns></returns>
        Task<Account> Authenticate(string email, string password);

        /// <summary>
        /// Удаление аккаунта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAccount(Guid id);

        /// <summary>
        /// Получить аккаунт
        /// </summary>
        /// <param name="id"> идентификатор</param>
        /// <returns></returns>
        Task<Account> GetAccount(Guid id);

        /// <summary>
        /// Получить все аккаунты
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AccountDto>> GetAllAccounts();

        /// <summary>
        /// Получить все удаленные аккаунты
        /// </summary>
        /// <returns></returns>
        Task<List<AccountDto>> GetAllDeletedAccounts();

        /// <summary>
        /// Регистрация нового слушателя
        /// </summary>
        /// <param name="accountCreateDto"></param>
        /// <returns></returns>
        Task<AccountDto> RegisterListenerAccount(AccountCreateDto accountCreateDto);

        /// <summary>
        /// Регистрация нового исполнителя
        /// </summary>
        /// <param name="accountCreateDto"></param>
        /// <returns></returns>
        Task<AccountDto> RegisterPerformerAccount(AccountCreateDto accountCreateDto);

        /// <summary>
        /// Восстановление аккаунта
        /// </summary>
        /// <param name="id"> Индентификатор</param>
        /// <returns></returns>
        Task<bool> RestoreAccount(Guid id);

        /// <summary>
        /// Обновление аккаунта
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <param name="accountCreateDto"></param>
        /// <returns></returns>
        Task<bool> UpdateAccount(Guid id, AccountCreateDto accountCreateDto);
    }
}