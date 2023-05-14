using System.Collections.Generic;

namespace Services.Contracts.Models
{
    /// <summary>
    /// Модель ответа на запрос задач за временной промежуток
    /// </summary>
    public class GetJiraItemsByPeriodResponseModel
    {
        /// <summary>
        /// Информация о задачах
        /// </summary>
        public IReadOnlyCollection<JiraItemInfo> JiraItemsTotalWorkingHours { get; init; }
    }
}