namespace MyServer.Services.Users
{
    using System.Collections.Generic;
    using System.Net.Http;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;

    public static class CreateAuthorizationProviderClass
    {
        private static GraphServiceClient _graphServiceClient;
        private static HttpClient _httpClient;

        public static IAuthenticationProvider CreateAuthorizationProvider(IConfiguration config)
        {
            var clientId = config["AzureGraph:applicationId"];
            var clientSecret = config["AzureGraph:applicationSecret"];
            var redirectUri = config["AzureGraph:redirectUri"];
            var authority = $"https://login.microsoftonline.com/{config["AzureGraph:tenantId"]}/v2.0";

            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");

            var cca = new ConfidentialClientApplication(clientId, authority, redirectUri, new ClientCredential(clientSecret), null, null);
            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }

        public static GraphServiceClient GetAuthenticatedGraphClient(IConfiguration config)
        {
            var authenticationProvider = CreateAuthorizationProvider(config);
            _graphServiceClient = new GraphServiceClient(authenticationProvider);
            return _graphServiceClient;
        }

        public static HttpClient GetAuthenticatedHTTPClient(IConfiguration config)
        {
            var authenticationProvider = CreateAuthorizationProvider(config);
            _httpClient = new HttpClient(new AuthHandler(authenticationProvider, new HttpClientHandler()));
            return _httpClient;
        }
    }
}
