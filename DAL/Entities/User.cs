using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class User : IEntity<int>
    {
        public User()
        {
            PlayLists = new List<Playlist>();
        }

        [Required]
        public virtual UserAccount Account { get; set; }

        public virtual List<Playlist> PlayLists { get; set; }
        public int ID { get; set; }
    }
}