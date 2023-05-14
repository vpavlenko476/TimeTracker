using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Jira.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Contracts.Interfaces;
using Services.Contracts.Models;
using TimeTracker.HostedService.Helpers;

namespace TimeTracker.HostedService.BackgroundServices
{
    public class JiraTasksConsumerService : BackgroundService
    {
        private readonly ILogger<JiraTasksConsumerService> logger;
        private readonly ISaveOrUpdateWorkingTimePeriodService saveOrUpdateWorkingTimePeriodService;
        private readonly IConsumer<Null, JiraItemsInProgressModel> consumer;

        public JiraTasksConsumerService(ILogger<JiraTasksConsumerService> logger, ISaveOrUpdateWorkingTimePeriodService saveOrUpdateWorkingTimePeriodService, IConfiguration configuration)
        {
            this.logger = logger;
            this.saveOrUpdateWorkingTimePeriodService = saveOrUpdateWorkingTimePeriodService;
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["KAFKA_CONNECTION_STRING"],
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            consumer = new ConsumerBuilder<Null, JiraItemsInProgressModel>(config).SetValueDeserializer(new GetJiraTasksResponseModelDeserializer()).Build();
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.Subscribe("jira-tasks");
            while(true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    consumer.Dispose();
                    break;
                }
                var msg = consumer.Consume(stoppingToken).Message.Value;
                if (!msg.JiraTasks.Any())
                {
                    continue;
                }
                logger.LogInformation("Получены задачи в статусе 'In Progress': {string.Join(", ",msg.JiraTasks.Select(t => t.Key))}");

                saveOrUpdateWorkingTimePeriodService.SaveOrUpdateWorkingTimePeriod(new SaveWorkingTimePeriodRequestModel
                {
                    JiraItems = msg.JiraTasks.Select(x => new JiraItem
                    {
                        Key = x.Key,
                        Summary = x.Fields.Summary,
                    }).ToList().AsReadOnly(),
                    DateTime = msg.DateTime,
                });
                consumer.Commit();
            }        
            return Task.CompletedTask;
        }
    }
}