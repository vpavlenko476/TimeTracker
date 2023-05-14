using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Jira.Client.Abstract;
using Jira.Contracts;
using Jira.HostedService.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jira.HostedService.BackgroundServices
{
    /// <summary>
    /// Сервис для мониторинга задач в jira и отправки в kafka
    /// </summary>
    public class JiraTasksMonitoringService : BackgroundService
    {
        private readonly ILogger<JiraTasksMonitoringService> logger;
        private readonly IJiraTaskClient jiraTaskClient;
        private readonly TimeSpan taskRequestDelay;
        private readonly IProducer<Null, JiraItemsInProgressModel> producer;

        public JiraTasksMonitoringService(IConfiguration configuration, ILogger<JiraTasksMonitoringService> logger, IJiraTaskClient jiraTaskClient)
        {
            this.logger = logger;
            this.jiraTaskClient = jiraTaskClient;
            taskRequestDelay = configuration.GetValue("TaskRequestDelay", TimeSpan.FromMinutes(10));
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["KAFKA_CONNECTION_STRING"],
                Acks = Acks.All,
            };

            producer = new ProducerBuilder<Null, JiraItemsInProgressModel>(config).SetValueSerializer(new GetJiraTasksResponseModelSerializer()).Build();
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Запущен процесс мониторинга задач в Jira");
            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    producer?.Dispose();
                    break;
                }
                var jiraTasks = (await jiraTaskClient.GetTasksInProgress(stoppingToken)).JiraTasks;
                if (!jiraTasks.Any())
                {
                    await Task.Delay(taskRequestDelay, stoppingToken);
                    continue;
                }
                await producer.ProduceAsync("jira-tasks", new Message<Null, JiraItemsInProgressModel>
                {
                    Value = new JiraItemsInProgressModel
                    {
                        JiraTasks = jiraTasks.ToList().AsReadOnly(),
                        DateTime = DateTimeOffset.UtcNow,
                    },
                }, stoppingToken);
                producer.Flush(TimeSpan.FromSeconds(10));
                logger.LogInformation($"Отправлены задачи в статусе 'In Progress': {string.Join(", ", jiraTasks.Select(t => t.Key))}");
                await Task.Delay(taskRequestDelay, stoppingToken);
            }
        }
    }
}