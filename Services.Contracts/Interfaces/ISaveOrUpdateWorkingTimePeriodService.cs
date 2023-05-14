using System.Threading.Tasks;
using Services.Contracts.Models;

namespace Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис сохранения или обновления временного промежутка работы над задачей
    /// </summary>
    public interface ISaveOrUpdateWorkingTimePeriodService
    {
        /// <summary>
        /// Соохранить или обновить временной промежуток работы над задачей
        /// </summary>
        Task SaveOrUpdateWorkingTimePeriod(SaveWorkingTimePeriodRequestModel model);
    }
}