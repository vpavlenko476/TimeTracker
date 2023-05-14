using System.Threading;
using System.Threading.Tasks;
using Jira.Contracts;

namespace Jira.Client.Abstract
{
    /// <summary>
    /// Клиент к Jira
    /// </summary>
    public interface IJiraTaskClient
    {
        /// <summary>
        /// Получить все задачи текущего пользователя в статусе InProgress
        /// </summary>
        Task<GetJiraTasksResponseModel> GetTasksInProgress(CancellationToken cancellationToken);
        
        /// <summary>
        /// Затрекать время на задачу
        /// </summary>
        Task<LogTimeResponseModel> LogTime(string jiraItem, LogTimeRequestModel model, CancellationToken cancellationToken);
    }
}