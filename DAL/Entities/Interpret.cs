using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Enums;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Interpret : IEntity<int>
    {
        public Interpret()
        {
            Albums = new List<Album>();
            Events = new List<Event>();
        }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Event> Events { get; set; }

        [MaxLength(1024)]
        public string InterpretImgUri { get; set; }

        [Required]
        public Language Language { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Language: {Language}";
        }
    }
}