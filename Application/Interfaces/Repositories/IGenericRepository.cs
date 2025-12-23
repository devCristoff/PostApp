using PostApp.Core.Application.Interfaces.Repositories.CRUD;

namespace PostApp.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> : 
        ICreate<Entity>,
        IRead<Entity>,
        IUpdate<Entity>,
        IDelete<Entity>
        where Entity : class
    {
    }
}
