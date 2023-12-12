/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using System.Text;
using System.Text.Json;
using webhooks_api_v2_sample_azure_function.Models;
using webhooks_api_v2_sample_azure_function.Services.Interfaces;

namespace webhooks_api_v2_sample_azure_function.Services;

public class WebhooksService : IWebhooksService
    {
    private readonly HttpClient _webhooksHttpClient;

    public WebhooksService(IHttpClientFactory httpClientFactory,
                           IAuthService authService)
        {
        _webhooksHttpClient = httpClientFactory.CreateClient();
        _webhooksHttpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.bentley.itwin-platform.v2+json");

        var token = authService.GetAccessToken().Result;
        _webhooksHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        _webhooksHttpClient.BaseAddress = new Uri("https://api.bentley.com/webhooks/");
        }

    public async Task<WebhookCreateResponse> CreateWebhookAsync(WebhookCreateRequest request)
        {
        var webhookResponse = await _webhooksHttpClient.PostAsync("", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
                   
        return JsonSerializer.Deserialize<WebhookCreateResponse>(await webhookResponse.Content.ReadAsStringAsync());
        }

    public async Task DeleteWebhooksAsync(string id)
        {
        await _webhooksHttpClient.DeleteAsync(id);
        }

    public async Task<Webhook> GetWebhookAsync(string id)
        {
        var webhookResponse = await _webhooksHttpClient.GetAsync(id);
        
        if (!webhookResponse.IsSuccessStatusCode)
            {
            return null;
            }

        return JsonSerializer.Deserialize<Webhook>(await webhookResponse.Content.ReadAsStringAsync());
        }

    public async Task<WebhooksResponse> GetWebhooksAsync()
        {
        var webhookResponse = await _webhooksHttpClient.GetAsync("");

        return JsonSerializer.Deserialize<WebhooksResponse>(await webhookResponse.Content.ReadAsStringAsync());
        }

    public async Task<Webhook> PatchWebhookAsync(string id, WebhookPatchRequest request)
        {
        var webhookResponse = await _webhooksHttpClient.PatchAsync(id, new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        return JsonSerializer.Deserialize<Webhook>(await webhookResponse.Content.ReadAsStringAsync());
        }
    }

