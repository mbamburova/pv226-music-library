using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            this.SongLists = new List<SongList>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<SongList> SongLists { get; set; }
    }
}
