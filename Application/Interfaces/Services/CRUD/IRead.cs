namespace PostApp.Core.Application.Interfaces.Services.CRUD
{
	public interface IRead<SaveViewModel, ViewModel> 
		where SaveViewModel : class
		where ViewModel : class
	{
		Task<SaveViewModel> GetByIdSaveViewModel(int id);
		Task<List<ViewModel>> GetAllViewModel();
	}
}
