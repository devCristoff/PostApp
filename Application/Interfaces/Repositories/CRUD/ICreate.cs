namespace PostApp.Core.Application.Interfaces.Repositories.CRUD
{
	public interface ICreate<Entity> 
		where Entity : class
	{
		Task AddAsync(Entity entity);
	}
}
