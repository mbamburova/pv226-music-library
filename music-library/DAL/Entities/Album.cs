using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Album : IEntity<int>
    {
        public Album()
        {
            Songs = new List<Song>();
            AlbumReviews = new List<AlbumReview>();
        }

        [Required]
        public string Name { get; set; }

        public virtual List<Song> Songs { get; set; }

        public virtual List<AlbumReview> AlbumReviews { get; set; }

        public int Year { get; set; }

        [Required]
        public virtual Interpret Interpret { get; set; }

        [MaxLength(1024)]
        public string AlbumImgUri { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Interpret: {Interpret}";
        }
    }
}