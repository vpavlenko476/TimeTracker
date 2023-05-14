using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jira.Contracts
{
    /// <summary>
    /// Модель ответа на запрос задач в статусе InProgress
    /// </summary>
    [Serializable]
    public class GetJiraTasksResponseModel
    {
        /// <summary>
        /// Задачи в Jira
        /// </summary>
        [JsonProperty("issues")]
        public IEnumerable<JiraTask> JiraTasks { get; set; }
    }

    /// <summary>
    /// Задача Jira
    /// </summary>
    [Serializable]
    public class JiraTask
    {
        /// <summary>
        /// Ключ
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Поля
        /// </summary>
        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }

    /// <summary>
    /// Поля задачи
    /// </summary>
    [Serializable]
    public class Fields
    {
        /// <summary>
        /// Описание
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }
    }
}