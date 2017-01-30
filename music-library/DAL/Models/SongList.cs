using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class SongList
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
        public virtual Playlist Playlist { get; set; }
        public virtual Song Song { get; set; }
    }
}
