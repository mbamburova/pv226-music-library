using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class SongList : IEntity<int>
    {
        [Required]
        public virtual Song Song { get; set; }

        [Required]
        public virtual Playlist Playlist { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Song: {Song}, Playlist: {Playlist}";
        }
    }
}