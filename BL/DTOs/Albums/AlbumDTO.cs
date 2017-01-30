using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Albums
{
    public class AlbumDTO
    {
        public int ID { get; set; }

        [Display(Name = "Album's title")]
        [Required]
        public string Name { get; set; }

        public int Year { get; set; }

        public int InterpretId { get; set; }

        [Display(Name = "URI of image")]
        [MaxLength(1024)]
        public string AlbumImgUri { get; set; }
    }
}