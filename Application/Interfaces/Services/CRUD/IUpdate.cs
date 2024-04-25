namespace PostApp.Core.Application.Interfaces.Services.CRUD
{
	public interface IUpdate<SaveViewModel> 
		where SaveViewModel : class
	{
		Task Update(SaveViewModel viewModel, int id);
	}
}
