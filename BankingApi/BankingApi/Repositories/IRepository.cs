using BankingApi.Models;
using System.Threading.Tasks;

namespace BankingApi.Repositories
{
    public interface IRepository
    {
        void SaveDataAsync(InstitutionModel model);
        Task<InstitutionModel> ReadDataAsync();
    }
}
