using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.User;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class UserListQuery : AppQuery<UserDTO>
    {
        public UserListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        protected override IQueryable<UserDTO> GetQueryable()
        {
            return Context.Users
                .Include(nameof(User.PlayLists))
                .ProjectTo<UserDTO>();
        }
    }
}