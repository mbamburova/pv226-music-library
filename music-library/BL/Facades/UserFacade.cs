using System;
using System.Collections.Generic;
using BL.DTOs.User;
using BL.DTOs.Users;
using BL.Services.UserAccounts;
using BL.Services.Users;

namespace BL.Facades
{
    public class UserFacade
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IUserService _userService;

        public UserFacade(IUserService userService, IUserAccountService userAccountService)
        {
            _userService = userService;
            _userAccountService = userAccountService;
        }

        public UserDTO GetUserAccordingToEmail(string email)
        {
            return _userService.GetUserAccordingToEmail(email);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return _userService.ListUsers();
        }

        public Guid RegisterUser(UserRegistrationDTO registrationDto, out bool success)
        {
            if (_userService.GetUserAccordingToEmail(registrationDto.Email) != null)
            {
                success = false;
                return new Guid();
            }
            var accountId = _userAccountService.RegisterUserAccount(registrationDto);
            _userService.CreateUser(accountId);
            success = true;

            return accountId;
        }

        public Guid AuthenticateUser(UserLoginDTO loginDto)
        {
            return _userAccountService.AuthenticateUser(loginDto);
        }
    }
}