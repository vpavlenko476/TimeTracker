using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Jira.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Contracts.Interfaces;
using Services.Contracts.Models;
using TimeTracker.Api.HostedServices.Helpers;

namespace TimeTracker.Api.HostedServices
{
    public class JiraTasksConsumerService : BackgroundService
    {
        private readonly ILogger<JiraTasksConsumerService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConsumer<Null, JiraItemsInProgressModel> consumer;

        public JiraTasksConsumerService(ILogger<JiraTasksConsumerService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["KAFKA_CONNECTION_STRING"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                GroupId = "foo",
            };
            consumer = new ConsumerBuilder<Null, JiraItemsInProgressModel>(config).SetValueDeserializer(new GetJiraTasksResponseModelDeserializer()).Build();
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var saveOrUpdateWorkingTimePeriodService = scope.ServiceProvider.GetRequiredService<ISaveOrUpdateWorkingTimePeriodService>();
            consumer.Subscribe("jira-tasks");
            while(true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    consumer.Dispose();
                    break;
                }
                var msg = await Task.Run(() => consumer.Consume(stoppingToken).Message.Value, stoppingToken);
                if (!msg.JiraTasks.Any())
                {
                    continue;
                }

                var logMsg = string.Join(", ", msg.JiraTasks.Select(t => t.Key));
                logger.LogInformation("Получены задачи в статусе 'In Progress': {Msg}", logMsg);

                await saveOrUpdateWorkingTimePeriodService.SaveOrUpdateWorkingTimePeriod(new SaveWorkingTimePeriodRequestModel
                {
                    JiraItems = msg.JiraTasks.Select(x => new JiraItem
                    {
                        Key = x.Key,
                        Summary = x.Fields.Summary,
                    }).ToList().AsReadOnly(),
                    DateTime = msg.DateTime,
                });
            }
        }
    }
}