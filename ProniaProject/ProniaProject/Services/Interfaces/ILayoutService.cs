using ProniaProject.ViewModels;

namespace ProniaProject.Services.Interfaces
{
    public interface ILayoutService
    {
        Task<Dictionary<string, string>> GetSettingAsync();

        Task<List<BasketItemVM>> GetBasketAsync();
    }
}
