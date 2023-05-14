using System;
using System.Collections.Generic;

namespace Jira.Contracts
{
    /// <summary>
    /// Задачи в статусе InProgress
    /// </summary>
    [Serializable]
    public class JiraItemsInProgressModel
    {
        /// <summary>
        /// Список задач
        /// </summary>
        public IReadOnlyCollection<JiraTask> JiraTasks { get; set; }

        /// <summary>
        /// Текущее время
        /// </summary>
        public DateTimeOffset DateTime { get; set; }
    }
}