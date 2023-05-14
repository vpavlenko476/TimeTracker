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
    public class SaveOrUpdateWorkingTimePeriodService : ISaveOrUpdateWorkingTimePeriodService
    {
        private readonly TimeTrackerContext context;

        public SaveOrUpdateWorkingTimePeriodService(TimeTrackerContext context)
        {
            this.context = context;
        }
        public async Task SaveOrUpdateWorkingTimePeriod(SaveWorkingTimePeriodRequestModel model)
        {
            var jiraItemsKeys = model.JiraItems.Select(i=> i.Key);
            var timePeriods = context.WorkingTimePeriods.Include(p => p.JiraItem).ToLookup(i => i.JiraItem.Key);
            
            var itemsToClose = timePeriods.Where(i => !jiraItemsKeys.Contains(i.Key) && i.Any(x => !x.IsClosed)).Select(i => i.Key).ToList();
            foreach (var item in itemsToClose)
            {
                var periodToClose = timePeriods[item].SingleOrDefault(i => !i.IsClosed);
                if (periodToClose == null)
                {
                    throw new Exception($"У задачи '{item}' больше одного незакрытого периода");
                }

                periodToClose.End = model.DateTime;
                periodToClose.IsClosed = true;
            }
            
            foreach (var item in model.JiraItems)
            {
                if (!timePeriods.Any(i => i.Key == item.Key))
                {
                    var itemId = Guid.NewGuid();
                    context.JiraItems.Add(new TimeTracker.Data.Entities.JiraItem
                    {
                        Id = itemId,
                        Key = item.Key,
                        Summary = item.Summary
                    });
                    context.WorkingTimePeriods.Add(new WorkingTimePeriod
                    {
                        Id = Guid.NewGuid(),
                        JiraItemId = itemId,
                        Begin = model.DateTime,
                        End = model.DateTime,
                        IsClosed = false,
                    });
                    continue;
                };

                var workingPeriod = timePeriods[item.Key].SingleOrDefault(p => !p.IsClosed);
                if (workingPeriod != null)
                {
                    workingPeriod.End = model.DateTime;
                    continue;
                }
                context.WorkingTimePeriods.Add(new WorkingTimePeriod
                {
                    Id = Guid.NewGuid(),
                    JiraItemId = timePeriods[item.Key].First().JiraItemId,
                    Begin = model.DateTime,
                    End = model.DateTime,
                    IsClosed = false,
                });
            }

            await context.SaveChangesAsync();
        }
    }
}