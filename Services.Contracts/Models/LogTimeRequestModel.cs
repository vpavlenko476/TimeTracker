using System;

namespace Services.Contracts.Models
{
    /// <summary>
    /// Модель запроса для логирования времени
    /// </summary>
    public class LogTimeRequestModel
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