using System;

namespace Services.Contracts.Models
{
    /// <summary>
    /// Информация о задаче
    /// </summary>
    public class JiraItemInfo
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string Key { get; init; }
        
        /// <summary>
        /// Описание
        /// </summary>
        public string Summary { get; init; }
        
        /// <summary>
        /// Продолжительность работы над задачей
        /// </summary>
        public TimeSpan TotalWorkingPeriod { get; init; }
    }
}