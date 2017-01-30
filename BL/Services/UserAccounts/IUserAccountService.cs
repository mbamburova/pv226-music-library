using System;
using BL.DTOs.Users;

namespace BL.Services.UserAccounts
{
    public interface IUserAccountService
    {
        Guid RegisterUserAccount(UserRegistrationDTO userRegistration, bool createAdmin = false);

        Guid AuthenticateUser(UserLoginDTO loginDto);
    }
}