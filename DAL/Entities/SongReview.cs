using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class SongReview : Review
    {
        [Required]
        public virtual Song Song { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, Song: {Song}";
        }
    }
}