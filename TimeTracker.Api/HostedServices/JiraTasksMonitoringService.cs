using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Jira.Client.Abstract;
using Jira.Contracts;
using Jira.HostedService.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TimeTracker.Api.HostedServices
{
    /// <summary>
    /// Сервис для мониторинга задач в jira и отправки в kafka
    /// </summary>
    public class JiraTasksMonitoringService : BackgroundService
    {
        private readonly ILogger<JiraTasksMonitoringService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly TimeSpan taskRequestDelay;
        private readonly IProducer<Null, JiraItemsInProgressModel> producer;

        public JiraTasksMonitoringService(IConfiguration configuration, ILogger<JiraTasksMonitoringService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
            taskRequestDelay = configuration.GetValue("TASK_REQUEST_DELAY", TimeSpan.FromMinutes(10));
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["KAFKA_CONNECTION_STRING"],
                Acks = Acks.None,
            };

            producer = new ProducerBuilder<Null, JiraItemsInProgressModel>(config).SetValueSerializer(new GetJiraTasksResponseModelSerializer()).Build();
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Запущен процесс мониторинга задач в Jira");
            using var scope = serviceScopeFactory.CreateScope();
            var jiraTaskClient = scope.ServiceProvider.GetRequiredService<IJiraTaskClient>();
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
                await Task.Delay(taskRequestDelay, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}