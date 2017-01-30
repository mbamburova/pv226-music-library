namespace BL.DTOs.Filters
{
    public class EventFilter
    {
        public string Name { get; set; }

        public string Place { get; set; }

        public int? InterpretId { get; set; }

        public bool SortAscending { get; set; }
    }
}