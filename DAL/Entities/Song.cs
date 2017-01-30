using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Enums;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Song : IEntity<int>
    {
        public Song()
        {
            SongReviews = new List<SongReview>();
        }

        [Required]
        public virtual Album Album { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public DateTime Added { get; set; }

        [MaxLength(1024)]
        public string YTLink { get; set; }

        public virtual List<SongReview> SongReviews { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public bool Publish { get; set; }

        public int OriginalSongId { get; set; }

        public string SongPath { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Album: {Album}, Name: {Name}, Genre: {Genre}";
        }
    }
}