using System;
using Jira.Client.Abstract;
using Microsoft.Extensions.Configuration;

namespace Jira.HostedService.Configurations
{
    public class JiraClientConfiguration: IJiraClientConfiguration
    {
        private readonly IConfiguration configuration;

        public JiraClientConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Uri BaseAddress => new Uri(configuration["REST_JIRA_URL"]);
        
        public string Token => configuration["JIRA_AUTH_TOKEN"];
    }
}