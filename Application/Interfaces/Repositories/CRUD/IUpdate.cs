namespace PostApp.Core.Application.Interfaces.Repositories.CRUD
{
	public interface IUpdate<Entity> 
		where Entity : class
	{
		Task UpdateAsync(Entity entity, int id);
	}
}
