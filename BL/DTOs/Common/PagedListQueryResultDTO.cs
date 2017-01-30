using System.Collections.Generic;

namespace BL.DTOs.Common
{
    public abstract class PagedListQueryResultDTO<T>
    {
        public int TotalResultCount { get; set; }

        public int RequestedPage { get; set; }

        public IEnumerable<T> ResultsPage { get; set; }
    }
}