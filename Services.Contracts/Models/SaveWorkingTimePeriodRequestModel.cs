using System;
using System.Collections.Generic;

namespace Services.Contracts.Models
{
    /// <summary>
    /// Модель запроса на сохранение или обновления временного промежутка задачи
    /// </summary>
    public class SaveWorkingTimePeriodRequestModel
    {
        /// <summary>
        /// Список задач
        /// </summary>
        public IReadOnlyCollection<JiraItem> JiraItems { get; init; }
        
        /// <summary>
        /// Текущее время
        /// </summary>
        public DateTimeOffset DateTime { get; init; }
    }
}