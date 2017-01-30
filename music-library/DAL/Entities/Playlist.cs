using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Playlist : IEntity<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public virtual User User { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}