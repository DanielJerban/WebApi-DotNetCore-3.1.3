using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public interface IEntity
    {
    }

    public class BaseEntity<TKey> : IEntity
    {
        [Key]
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
