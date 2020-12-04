using System;

namespace uBeac.Common
{
    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
    public interface IEntity : IEntity<Guid>
    {
    }
}
