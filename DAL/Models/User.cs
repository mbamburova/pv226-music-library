using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class User
    {
        public User()
        {
            this.AlbumReviews = new List<AlbumReview>();
            this.Playlists = new List<Playlist>();
            this.SongReviews = new List<SongReview>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AlbumReview> AlbumReviews { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<SongReview> SongReviews { get; set; }
    }
}
