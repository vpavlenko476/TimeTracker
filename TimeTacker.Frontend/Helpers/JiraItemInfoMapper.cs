using System;
using Services.Contracts.Models;
using TimeTacker.Frontend.Models;

namespace TimeTacker.Frontend.Helpers
{
    /// <summary>
    /// Маппер 
    /// </summary>
    public static class JiraItemInfoMapper
    {
        public static JiraItemInfoViewModel MapToViewModel(this JiraItemInfo item)
        {
            return new JiraItemInfoViewModel
            {
                Key = item.Key,
                Summary = item.Summary,
                TotalWorkingPeriod = string.Format("{0}:{1:00}", (int) item.TotalWorkingPeriod.TotalHours,
                    item.TotalWorkingPeriod.Minutes),
            };
        }

        public static JiraItemInfo MapFromViewModel(this JiraItemInfoViewModel item)
        {
            return new JiraItemInfo
            {
                Key = item.Key,
                Summary = item.Summary,
                TotalWorkingPeriod = TimeSpan.TryParse(item.TotalWorkingPeriod, out var workingPeriod)
                    ? workingPeriod
                    : throw new Exception($"Невозможно преобразовать '{item.TotalWorkingPeriod}' во временной промежуток"),
            };
        }
    }
}