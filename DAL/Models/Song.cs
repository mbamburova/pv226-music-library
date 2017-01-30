using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Song
    {
        public Song()
        {
            this.SongLists = new List<SongList>();
            this.SongReviews = new List<SongReview>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Genre { get; set; }
        public Nullable<System.TimeSpan> Duration { get; set; }
        public Nullable<System.DateTime> Added { get; set; }
        public string YTLink { get; set; }
        public double Size { get; set; }
        public string Lyrics { get; set; }
        public Nullable<int> Album_Id { get; set; }
        public virtual Album Album { get; set; }
        public virtual ICollection<SongList> SongLists { get; set; }
        public virtual ICollection<SongReview> SongReviews { get; set; }
    }
}
