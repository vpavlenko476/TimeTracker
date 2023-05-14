using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Confluent.Kafka;
using Jira.Contracts;

namespace TimeTracker.Api.HostedServices.Helpers
{
    /// <summary>
    /// Десирилизатор массива байт в <inheritdoc cref="JiraItemsInProgressModel"/>
    /// </summary>
    public class GetJiraTasksResponseModelDeserializer : IDeserializer<JiraItemsInProgressModel>
    {
        public JiraItemsInProgressModel Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return null;
            }

            using var stream = new MemoryStream(data.ToArray());
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            var model = (JiraItemsInProgressModel)formatter.Deserialize(stream);
            return model;
        }
    }
}