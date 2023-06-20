using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities;

internal class Section: NamedEntity, IOrderedEntity
{
    public int Order { get; set; }

    public int? ParentId { get; set; }
}
