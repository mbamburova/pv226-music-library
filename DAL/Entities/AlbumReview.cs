using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class AlbumReview : Review
    {
        [Required]
        public virtual Album Album { get; set; }

        public override string ToString()
        {
            return $"Album: {Album}";
        }
    }
}