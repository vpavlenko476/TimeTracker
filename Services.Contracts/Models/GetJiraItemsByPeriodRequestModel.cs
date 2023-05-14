using System;

namespace Services.Contracts.Models
{
    /// <summary>
    /// Модель запроса задач за временной промежуток
    /// </summary>
    public class GetJiraItemsByPeriodRequestModel
    {
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTimeOffset Begin { get; init; }
        
        /// <summary>
        /// Окончание периода
        /// </summary>
        public DateTimeOffset End { get; init; }
    }
}