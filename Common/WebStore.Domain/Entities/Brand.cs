using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities;

[Table("Brands")]
[Index(nameof(Name), IsUnique = true)]
public class Brand : NamedEntity, IOrderedEntity
{
    [Column("Order")]
    public int Order { get; set; }

    public ICollection<Product> Products { get; set; }
}
