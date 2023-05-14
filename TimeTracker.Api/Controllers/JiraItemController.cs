using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Interfaces;
using Services.Contracts.Models;

namespace TimeTracker.Api.Controllers
{
    [ApiController]
    [Route("jira-item")]
    public class JiraItemController : ControllerBase
    {
        private readonly IGetJiraItemByPeriodService getJiraItemByPeriodService;

        public JiraItemController(IGetJiraItemByPeriodService getJiraItemByPeriodService)
        {
            this.getJiraItemByPeriodService = getJiraItemByPeriodService;
        }
        
        /// <summary>
        /// Получить задачи во временном промежутке
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<GetJiraItemsByPeriodResponseModel>> GetJiraItemsByPeriod([FromQuery] DateTimeOffset begin, [FromQuery] DateTimeOffset end)
        {
            return await getJiraItemByPeriodService.GetJiraItemsByPeriod(new GetJiraItemsByPeriodRequestModel
            {
                Begin = begin,
                End = end,
            });
        }
    }
}