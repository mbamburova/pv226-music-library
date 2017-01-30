using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public abstract class Review : IEntity<int>
    {
        [MaxLength(65536)]
        public string Name { get; set; }

        [MaxLength(65536)]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        public int Rating { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"Note: {Note}, Rating: {Rating}";
        }
    }
}