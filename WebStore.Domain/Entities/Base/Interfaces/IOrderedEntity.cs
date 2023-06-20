namespace WebStore.Domain.Entities.Base.Interfaces;
public interface IOrderedEnity : IEntity
{
	int Order { get; set; }
}