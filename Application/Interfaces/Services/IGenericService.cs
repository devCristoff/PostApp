using PostApp.Core.Application.Interfaces.Services.CRUD;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity> :
        ICreate<SaveViewModel>, 
        IRead<SaveViewModel, ViewModel>, 
        IUpdate<SaveViewModel>, 
        IDelete
		where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {        

    }
}
