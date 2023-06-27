using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Orders;

public class OrderItem : Entity
{
    [Required]
    public Product Product { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public Order Order { get; set; } = null!;

    [NotMapped]  //EF не добавляет это свойство в БД
    public decimal TotalItemPrice => Price * Quantity;
}
