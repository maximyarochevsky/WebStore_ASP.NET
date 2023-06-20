
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    public abstract string Name { get; set; }
}
