using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Services.Contracts.Models;

namespace TimeTracker.Client.Abstract
{
    /// <summary>
    /// Клиент к TimeTracker.Api
    /// </summary>
    public interface ITimeTrackerClient
    {
        /// <summary>
        /// Получить задачи за промежуток времени
        /// </summary>
        Task<GetJiraItemsByPeriodResponseModel> GetJiraItemsByTimePeriod(GetJiraItemsByPeriodRequestModel requestModel, CancellationToken token);
        
        /// <summary>
        /// Затрекать время на задачи
        /// </summary>
        Task LogTime(IEnumerable<LogTimeRequestModel> requestModel, CancellationToken token);
    }
}