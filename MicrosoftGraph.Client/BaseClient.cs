using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;

namespace MicrosoftGraph.Client
{
    public class BaseClient
    {
        private GraphServiceClient GraphClient { get; set; }

        private string[] scopes = {"User.Read"};


        public async Task Get()
        {
            var interactiveBrowserCredentialOptions = new InteractiveBrowserCredentialOptions
            {
                ClientId = "e510ed83-f847-48f9-b0f9-d624c9441516",
                RedirectUri = new Uri("http://localhost/"),
            };
            var interactiveBrowserCredential = new InteractiveBrowserCredential(interactiveBrowserCredentialOptions);

            var graphClient =
                new GraphServiceClient(interactiveBrowserCredential,
                    scopes); // you can pass the TokenCredential directly to the GraphServiceClient

            var me = await graphClient.Me.Calendar.Events.Request()
                .GetAsync();
        }

        public async Task Get2()
        {
            string[] scopes = {"https://graph.microsoft.com/.default"};
            //to-7Q~YgN-0r5NCnCUj~Nz9XFo3thuppgsUl.
            ClientSecretCredential clientSecretCredential = new ClientSecretCredential("organizations",
                "e510ed83-f847-48f9-b0f9-d624c9441516", "to-7Q~YgN-0r5NCnCUj~Nz9XFo3thuppgsUl.");

            GraphServiceClient graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);

            var mes = await graphServiceClient.Me.Calendar.Events.Request()
                .GetAsync();
            User me = await graphServiceClient.Users["user-id"].Request()
                .GetAsync();
        }

        public async Task Get3()
        {
            string[] scopes = {"https://graph.microsoft.com/common"};

            UsernamePasswordCredential usernamePasswordCredential =
                new UsernamePasswordCredential("vladislav.pavlenko@idp.zyfra.com", "Wag42293", "organizations",
                    "e510ed83-f847-48f9-b0f9-d624c9441516");

            GraphServiceClient
                graphClient =
                    new GraphServiceClient(usernamePasswordCredential,
                        scopes); // you can pass the TokenCredential directly to the GraphServiceClient

            User me = await graphClient.Me.Request()
                .GetAsync();
        }

        public async Task Get4()
        {
            Guid.TryParse("4ed37ffc-21eb-4c08-89da-54bd27795201", out var tenantId);
            var clientApp = ConfidentialClientApplicationBuilder.Create("e510ed83-f847-48f9-b0f9-d624c9441516")
                .WithClientSecret("Lm17Q~P_JLZzG96spOgKdxgq8LIisG8IWslyR")
                .WithTenantId("common")
                .WithRedirectUri("http://localhost")
               .Build(); //clientId
            string[] scopes = {"https://graph.microsoft.com/.default"};
            string token = null;
            var app = PublicClientApplicationBuilder.Create("e510ed83-f847-48f9-b0f9-d624c9441516")
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .Build();
            AuthenticationResult result = null;
            var accounts = await clientApp.GetAccountsAsync();
            var securePassword = new SecureString();
            foreach (char c in "Wag42293") // you should fetch the password
                securePassword.AppendChar(c); // keystroke by keystroke
            result = await clientApp.AcquireTokenForClient(scopes)
                .ExecuteAsync();
            token = result.AccessToken;
            GraphServiceClient graphClient = new GraphServiceClient(
                "https://graph.microsoft.com/v1.0",
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                    }));
            //var me = await graphClient.Users.Request().GetAsync();

            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(new Uri("https://graph.microsoft.com/v1.0/users"));
            var content = response.Content.ReadAsStringAsync();
        }
    }
}