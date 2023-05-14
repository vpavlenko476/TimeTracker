using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Services.Contracts.Models;
using TimeTracker.Client.Abstract;

namespace TimeTracker.Client
{
    public class TimeTrackerClient : BaseClient, ITimeTrackerClient
    {
        public TimeTrackerClient(Uri baseUrl) : base(baseUrl)
        {
        }

        public async Task<GetJiraItemsByPeriodResponseModel> GetJiraItemsByTimePeriod(GetJiraItemsByPeriodRequestModel requestModel, CancellationToken token)
        {
            var requestUrl = $"?begin={requestModel.Begin:s}Z&end={requestModel.End:s}Z";
            return await Get<GetJiraItemsByPeriodResponseModel>("jira-item" + requestUrl, token);
        }

        public async Task LogTime(IEnumerable<LogTimeRequestModel> requestModel, CancellationToken token)
        {
            await PostWitJsonBody($"log-time", requestModel, token);
        }
    }
}