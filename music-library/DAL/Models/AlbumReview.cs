using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class AlbumReview
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public double Rating { get; set; }
        public int AlbumId { get; set; }
        public int UserId { get; set; }
        public virtual Album Album { get; set; }
        public virtual User User { get; set; }
    }
}
