using ProniaProject.ViewModels;

namespace ProniaProject.Services.Interfaces
{
    public interface IBasketService
    {
        Task<List<BasketItemVM>> GetBasketAsync();
    }
}
