using Newtonsoft.Json;

namespace Jira.Contracts
{
    public class LogTimeRequestModel
    {
        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }
        
        [JsonProperty("started")]
        public string Started { get; set; }
    }
}