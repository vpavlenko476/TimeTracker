using System.Threading;
using System.Threading.Tasks;
using Jira.Client.Abstract;
using Jira.Contracts;

namespace Jira.Client
{
    public class JiraTaskClient : BaseClient, IJiraTaskClient
    {
        private const string ServiceAddress = "rest/api/2";
        public JiraTaskClient(IJiraClientConfiguration jiraConfiguration) : base(jiraConfiguration.BaseAddress, jiraConfiguration.Token)
        {
        }

        public async Task<GetJiraTasksResponseModel> GetTasksInProgress(CancellationToken cancellationToken)
        {
            return await Get<GetJiraTasksResponseModel>($"{ServiceAddress}/search/?jql=assignee=currentuser()%20AND%20status%20%3D%20\"In%20Progress\"&fields=key,summary", cancellationToken);
        }

        public async Task<LogTimeResponseModel> LogTime(string jiraItem, LogTimeRequestModel requestModel, CancellationToken cancellationToken)
        {
            return await PostWitJsonBody<LogTimeResponseModel, LogTimeRequestModel>($"{ServiceAddress}/issue/{jiraItem}/worklog", requestModel, cancellationToken);
        }
    }
}