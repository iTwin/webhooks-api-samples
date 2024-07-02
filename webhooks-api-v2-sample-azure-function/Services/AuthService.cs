/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using System.Text.Json;
using webhooks_api_v2_sample_azure_function.Models;
using webhooks_api_v2_sample_azure_function.Services.Interfaces;

namespace webhooks_api_v2_sample_azure_function.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _authHttpClient;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _authHttpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> GetAccessToken()
    {
        var clientId = Environment.GetEnvironmentVariable("OAUTH_CLIENT_ID");
        var secret = Environment.GetEnvironmentVariable("OAUTH_CLIENT_SECRET");

        var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("OAUTH_CLIENT_ID", clientId),
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("scope", "itwin-platform"),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

        var response = await _authHttpClient.PostAsync("https://ims.bentley.com/connect/token", formData);

        var stringResponse = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(stringResponse);

        return tokenResponse.access_token;
    }
}

