using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace uBeac.Common
{
    public abstract class FilterCriteria<T>
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public IDictionary<string, bool> Sort { get; set; } = new Dictionary<string, bool>();
        public abstract Expression<Func<T, bool>> Query();
    }
}
