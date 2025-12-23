namespace PostApp.Core.Application.Interfaces.Repositories.CRUD
{
	public interface IRead<Entity> where Entity : class
	{
		Task<Entity> GetByIdAsync(int id);
		Task<List<Entity>> GetAllAsync();
	}
}
