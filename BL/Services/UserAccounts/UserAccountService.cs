using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using BL.DTOs.Users;
using BL.Utils.AccountPolicy;
using DAL.Entities;

namespace BL.Services.UserAccounts
{
    public class UserAccountService : MusicLibraryService, IUserAccountService
    {
        private readonly BrockAllen.MembershipReboot.UserAccountService<UserAccount> coreService;

        public UserAccountService(BrockAllen.MembershipReboot.UserAccountService<UserAccount> service)
        {
            coreService = service;
        }

        public Guid RegisterUserAccount(UserRegistrationDTO userRegistration, bool createAdmin = false)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userClaims = new List<Claim>();

                userClaims.Add(createAdmin
                    ? new Claim(ClaimTypes.Role, Claims.Admin)
                    : new Claim(ClaimTypes.Role, Claims.User));

                var account = coreService.CreateAccount(null, userRegistration.Password, userRegistration.Email,
                    null as Guid?,
                    null);

                Mapper.Map(userRegistration, account);

                foreach (var claim in userClaims)
                {
                    coreService.AddClaim(account.ID, claim.Type, claim.Value);
                }
                coreService.Update(account);

                return account.ID;
            }
        }

        public Guid AuthenticateUser(UserLoginDTO loginDto)
        {
            UserAccount account;
            var result = coreService.Authenticate(loginDto.Username, loginDto.Password, out account);
            if (!result)
            {
                Debug.WriteLine($"Failed to authenticate user: {loginDto.Username}");
                return Guid.Empty;
            }
            return account.ID;
        }
    }
}