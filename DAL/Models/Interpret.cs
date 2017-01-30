using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Interpret
    {
        public Interpret()
        {
            this.Albums = new List<Album>();
            this.Events = new List<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string InterpretImgUri { get; set; }
        public int Language { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
