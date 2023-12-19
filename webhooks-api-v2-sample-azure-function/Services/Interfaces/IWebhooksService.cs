/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using webhooks_api_v2_sample_azure_function.Models;

namespace webhooks_api_v2_sample_azure_function.Services.Interfaces;

public interface IWebhooksService
{
    Task<WebhooksResponse> GetWebhooksAsync();
    Task<Webhook> GetWebhookAsync(string id);
    Task<WebhookCreateResponse> CreateWebhookAsync(WebhookCreateRequest request);
    Task<Webhook> PatchWebhookAsync(string id, WebhookPatchRequest request);
    Task DeleteWebhooksAsync(string id);
}
