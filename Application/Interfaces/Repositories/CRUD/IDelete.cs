namespace PostApp.Core.Application.Interfaces.Repositories.CRUD
{
	public interface IDelete<Entity> where Entity : class
	{
		Task DeleteAsync(Entity entity);
	}
}
