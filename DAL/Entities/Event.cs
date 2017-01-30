using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Event : IEntity<int>
    {
        [Required]
        [MaxLength(1024)]
        public string Name { get; set; }

        [Required]
        public virtual Interpret Interpret { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [MaxLength(255)]
        public string Place { get; set; }

        [MaxLength(1024)]
        public string EventLink { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Interpret: {Interpret}, Time: {Time}, Place: {Place}";
        }
    }
}