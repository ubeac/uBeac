using System;
using System.Collections.Generic;
using System.Linq;

namespace uBeac.Common
{
    public class PaginatedList<T> 
    {

        public int PageSize { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageNumber { get; set; } = 1;
        public long TotalCount { get; set; }
        public bool HasPrevious { get; set; } = false;
        public bool HasNext { get; set; } = false;
        public ICollection<T> Items { get; }

        public PaginatedList(IEnumerable<T> items) : this(items, 1, 20, items.Count())
        {
        }

        public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize, long totalCount)
        {
            Items = items.ToList();
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasPrevious = PageNumber > 1;
            HasNext = PageNumber < TotalPages;
        }

    }
}
