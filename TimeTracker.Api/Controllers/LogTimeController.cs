using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Interfaces;
using Services.Contracts.Models;
using TimeTracker.Api.Contracts;

namespace TimeTracker.Api.Controllers
{
    [ApiController]
    [Route("log-time")]
    public class LogTimeController : ControllerBase
    {
        private readonly ILogTimeService logTimeService;
        private readonly IMapper mapper;

        public LogTimeController(ILogTimeService logTimeService, IMapper mapper)
        {
            this.logTimeService = logTimeService;
            this.mapper = mapper;
        }
        
        /// <summary>
        /// Залогировать время
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> LogTime(IEnumerable<CreateLogTimeApiModel> workingTimePeriods)
        {
            await logTimeService.LogTime(mapper.Map<IEnumerable<LogTimeRequestModel>>(workingTimePeriods), HttpContext.RequestAborted);
            return NoContent();
        }
    }
}