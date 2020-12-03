using System;

namespace uBeac.Common
{
    public class BaseEntity<TKey> : IEntity<TKey>
      where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
    public class BaseEntity : BaseEntity<int>, IEntity
    {
    }
}
