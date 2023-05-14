using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Confluent.Kafka;
using Jira.Contracts;

namespace Jira.HostedService.Helpers
{
    /// <summary>
    /// Сериализатор <inheritdoc cref="JiraItemsInProgressModel"/> в массив байт
    /// </summary>
    public class GetJiraTasksResponseModelSerializer : ISerializer<JiraItemsInProgressModel>
    {
        public byte[] Serialize(JiraItemsInProgressModel data, SerializationContext context)
        {
            if (data == null)
            {
                return null;
            }
            var binaryFormatter = new BinaryFormatter();
            using var ms = new MemoryStream();
            binaryFormatter.Serialize(ms, data);
            return ms.ToArray();
        }
    }
}