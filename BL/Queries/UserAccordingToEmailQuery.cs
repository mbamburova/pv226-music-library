using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using BL.AppInfrastructure;
using BL.DTOs.User;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class UserAccordingToEmailQuery : AppQuery<UserDTO>
    {
        public UserAccordingToEmailQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public string Email { get; set; }

        protected override IQueryable<UserDTO> GetQueryable()
        {
            if (string.IsNullOrEmpty(Email) || !new EmailAddressAttribute().IsValid(Email))
            {
                throw new InvalidOperationException("UserAccordingToUserIdQuery - Email must be valid");
            }

            var user = Context.Users.Include(nameof(User.Account))
                .FirstOrDefault(c => c.Account.Email.Equals(Email));

            if (user == null)
            {
                return new EnumerableQuery<UserDTO>(new List<UserDTO>());
            }

            var userDto = Mapper.Map<UserDTO>(user);
            return new EnumerableQuery<UserDTO>(new List<UserDTO> {userDto});
        }
    }
}