using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class SongReview
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public double Rating { get; set; }
        public int SongId { get; set; }
        public int UserId { get; set; }
        public virtual Song Song { get; set; }
        public virtual User User { get; set; }
    }
}
