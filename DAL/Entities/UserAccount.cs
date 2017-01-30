using System;
using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class UserAccount : RelationalUserAccount, IEntity<Guid>
    {
        [MaxLength(64)]
        public virtual string FirstName { get; set; }

        [MaxLength(64)]
        public virtual string LastName { get; set; }
    }
}