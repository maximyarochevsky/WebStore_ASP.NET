namespace WebStore.Domain.Entities;

public class Cart
{
	public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

	public int ItemsCount => Items.Sum(item => item.Quantity);
}
