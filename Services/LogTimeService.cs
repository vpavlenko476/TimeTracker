using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jira.Client.Abstract;
using Services.Contracts.Interfaces;
using Services.Contracts.Models;

namespace Services
{
    public class LogTimeService : ILogTimeService
    {
        private readonly IJiraTaskClient jiraTaskClient;
        private const string UtcOffsetStringRepresentation = "+0000";

        public LogTimeService(IJiraTaskClient jiraTaskClient)
        {
            this.jiraTaskClient = jiraTaskClient;
        }

        public async Task LogTime(IEnumerable<LogTimeRequestModel> workingTimePeriods, CancellationToken token)
        {
            foreach (var workingTimePeriod in workingTimePeriods)
            {
                var totalSpentSeconds = (int)workingTimePeriod.WorkingDuration.TotalSeconds;
                if(totalSpentSeconds<=0) continue;
                await jiraTaskClient.LogTime(workingTimePeriod.JiraItem, new Jira.Contracts.LogTimeRequestModel
                {
                    TimeSpentSeconds = totalSpentSeconds,
                    Started = workingTimePeriod.StartDate.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff") + UtcOffsetStringRepresentation,
                }, token);
            }
        }
    }
}