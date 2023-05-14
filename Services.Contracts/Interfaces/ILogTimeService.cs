using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Services.Contracts.Models;

namespace Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис для логирования времени
    /// </summary>
    public interface ILogTimeService
    {
        /// <summary>
        /// Залогировать время
        /// </summary>
        Task LogTime(IEnumerable<LogTimeRequestModel> workingTimePeriods, CancellationToken token);
    }
}