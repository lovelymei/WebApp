using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService
{
    public class CreateSuperAdminMiddleware
    {
        private readonly RequestDelegate _delegate;
        private readonly AuthorizationDbContext _db;

        public CreateSuperAdminMiddleware(RequestDelegate requestDelegate, AuthorizationDbContext db)
        {
            _delegate = requestDelegate;
            _db = db;      
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_db.Accounts.Any())
            {
                string password = "123";
                var salt = AccountsInSQlRepository.GenerateSalt();

                var superLogin = new Login()
                {
                    Email = "superAdminAndrewst@bk.ru",
                    Salt = Convert.ToBase64String(salt),
                    PasswordHash = Convert.ToBase64String(password.ToPasswordHash(salt)),
                };

                await _db.Accounts.AddAsync(new Account { NickName = "superAdminAndrewst", Role = Roles.superadministrator, Login = superLogin });
                await  _db.SaveChangesAsync();
                await _db.DisposeAsync();

                await _delegate.Invoke(context);
            }

            await _delegate.Invoke(context);
        }
    }
}
