using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Album
    {
        public Album()
        {
            this.AlbumReviews = new List<AlbumReview>();
            this.Songs = new List<Song>();
        }

        public int Id { get; set; }
        public System.DateTime Year { get; set; }
        public Nullable<System.TimeSpan> Duration { get; set; }
        public string AlbumImgUri { get; set; }
        public int InterpretId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AlbumReview> AlbumReviews { get; set; }
        public virtual Interpret Interpret { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
