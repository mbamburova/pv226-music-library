namespace BL.DTOs.Filters
{
    public class SongFilter
    {
        public string Name { get; set; }

        public int? Genre { get; set; }

        public int? AlbumId { get; set; }

        public int? CreatedBy { get; set; }

        public bool? IsPublic { get; set; }

        public bool? Publish { get; set; }

        public int? OriginalSongId { get; set; }

        //public SongSortCriteria SortCriteria { get; set; }
    }
}