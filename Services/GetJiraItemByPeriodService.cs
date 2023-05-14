using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Interfaces;
using Services.Contracts.Models;
using TimeTracker.Data;
using TimeTracker.Data.Entities;

namespace Services
{
    public class GetJiraItemByPeriodService : IGetJiraItemByPeriodService
    {
        private readonly TimeTrackerContext context;

        public GetJiraItemByPeriodService(TimeTrackerContext context)
        {
            this.context = context;
        }

        public async Task<GetJiraItemsByPeriodResponseModel> GetJiraItemsByPeriod(GetJiraItemsByPeriodRequestModel requestModel)
        {
            var workingTimePeriods = (await context.WorkingTimePeriods.Include(p => p.JiraItem).AsNoTracking()
                .Where(i => i.End > requestModel.Begin && i.Begin < requestModel.End).ToListAsync())
                .GroupBy(p => p.JiraItem.Key, p => (Summary: p.JiraItem.Summary, WorkingPeriod: GetPeriodWorkingTime(p, requestModel.Begin, requestModel.End)));
            var response = workingTimePeriods.Select(i => new JiraItemInfo
                {
                    Key = i.Key,
                    Summary = i.First(s => !string.IsNullOrEmpty(s.Summary)).Summary,
                    TotalWorkingPeriod = TimeSpan.FromMinutes(i.Sum(p => p.WorkingPeriod.TotalMinutes)),
                }).ToList().AsReadOnly();
            return new GetJiraItemsByPeriodResponseModel
            {
                JiraItemsTotalWorkingHours = response,
            };
        }

        private TimeSpan GetPeriodWorkingTime(WorkingTimePeriod period, DateTimeOffset begin, DateTimeOffset end)
        {
            if (period.End > begin && period.Begin < begin)
            {
                return end > period.End ? period.End - begin : end - begin;
            }

            return end > period.End ? period.End - period.Begin : end - period.Begin;
        }
    }
}