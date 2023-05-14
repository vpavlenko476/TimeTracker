using System;

namespace TimeTracker.Api.Contracts
{
    /// <summary>
    /// Модель для логирования времени
    /// </summary>
    public class CreateLogTimeApiModel
    {
        /// <summary>
        /// Номомер задачи
        /// </summary>
        public string JiraItem { get; init; }
    
        /// <summary>
        /// Врменной промежуток работы
        /// </summary>
        public TimeSpan WorkingDuration { get; init; }
    
        /// <summary>
        /// Дата начала работы
        /// </summary>
        public DateTimeOffset StartDate { get; init; }
    }
}


