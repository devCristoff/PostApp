namespace PostApp.Core.Application.Interfaces.Services.CRUD
{
	public interface ICreate<SaveViewModel> 
		where SaveViewModel : class
	{
		Task Add(SaveViewModel viewModel);
	}
}
