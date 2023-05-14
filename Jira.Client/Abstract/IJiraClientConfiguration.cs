using System;

namespace Jira.Client.Abstract
{
    /// <summary>
    /// Конфигурация клиента к Jira
    /// </summary>
    public interface IJiraClientConfiguration
    {
        /// <summary>
        /// Базовый адрес Jira.Api
        /// </summary>
        Uri BaseAddress { get; }
        
        /// <summary>
        /// Токен
        /// </summary>
        string Token { get; }
    }
}