using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities;

internal class Brand : NamedEntity, IOrderedEntity
{
    public int Order { get; set; }
}
