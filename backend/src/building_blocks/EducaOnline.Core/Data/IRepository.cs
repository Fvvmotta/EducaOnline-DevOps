using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Core.Data
{
    public interface IRepository<T> : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
