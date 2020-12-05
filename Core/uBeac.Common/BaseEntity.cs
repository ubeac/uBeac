using System;

namespace uBeac.Common
{
    public interface IBaseEntity<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey CreateBy { get; set; }
        TKey UpdateBy { get; set; }
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
    }

    public interface IBaseEntity : IBaseEntity<Guid>
    {
    }

    public class BaseEntity<TKey> : IBaseEntity<TKey>
      where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public TKey CreateBy { get; set; }
        public TKey UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class BaseEntity : BaseEntity<Guid>, IEntity
    {
    }
}
