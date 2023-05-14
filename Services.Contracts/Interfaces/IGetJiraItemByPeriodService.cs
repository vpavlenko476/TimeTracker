using System.Threading.Tasks;
using Services.Contracts.Models;

namespace Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис для получения задач во временном промежутке
    /// </summary>
    public interface IGetJiraItemByPeriodService
    {
        /// <summary>
        /// Получить задачи во временном промежутке
        /// </summary>
        Task<GetJiraItemsByPeriodResponseModel> GetJiraItemsByPeriod(GetJiraItemsByPeriodRequestModel requestModel);
    }
}