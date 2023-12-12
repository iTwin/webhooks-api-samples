/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webhooks_api_v2_sample_azure_function.Models;
using webhooks_api_v2_sample_azure_function.Services.Interfaces;

namespace webhooks_api_v2_sample_azure_function;

public class WebhooksHostedService : IHostedService
    {
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger _logger;
    private readonly IWebhooksService _webhooksService;

    private string? _webhookId;

    public WebhooksHostedService(IHostApplicationLifetime hostApplicationLifetime,
                                 ILoggerFactory loggerFactory,
                                 IWebhooksService webhooksService)
        {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = loggerFactory.CreateLogger<WebhooksHostedService>();
        _webhooksService = webhooksService;

        _webhookId = Environment.GetEnvironmentVariable("WEBHOOK_ID");
        }

    public Task StartAsync(CancellationToken cancellationToken)
        {
        _hostApplicationLifetime.ApplicationStarted.Register(CheckForActiveWebhook);
        _hostApplicationLifetime.ApplicationStopped.Register(DeactiveWebhook);

        return Task.CompletedTask;
        }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void CheckForActiveWebhook()
        {
        Webhook? webhook = null;

        var defaultWebhookId = Environment.GetEnvironmentVariable("WEBHOOK_ID");
        if(!string.IsNullOrWhiteSpace(defaultWebhookId))
            {
            webhook = _webhooksService.GetWebhookAsync(defaultWebhookId).Result;
            }

        // Create a new webhook if provided one does not exist
        if (webhook == null)
            {
            var createWebhookRequest = new WebhookCreateRequest()
                {
                CallbackUrl = Environment.GetEnvironmentVariable("WEBHOOK_CALLBACK_URL"),
                Secret = Environment.GetEnvironmentVariable("WEBHOOK_SECRET"),
                EventTypes = Environment.GetEnvironmentVariable("WEBHOOK_EVENT_TYPES").Split(",").Select(et => et.Trim()).ToList(),
                Scope = "Account"
                };

            var createdWebhook = _webhooksService.CreateWebhookAsync(createWebhookRequest).Result;
            _logger.LogInformation("Created webhook id: " + createdWebhook.Id.ToString());

            webhook = new Webhook
                {
                Id = createdWebhook.Id,
                Active = createdWebhook.Active,
                CallbackUrl = createdWebhook.CallbackUrl,
                EventTypes = createdWebhook.EventTypes,
                Scope = createdWebhook.Scope,
                ScopeId = createdWebhook.ScopeId
                };

            _webhookId = createdWebhook.Id.ToString();
            }

        // activate webhook if not active.
        if (!webhook.Active)
            {
            var patchWebhookRequest = new WebhookPatchRequest()
                {
                Active = true,
                };

            _webhooksService.PatchWebhookAsync(webhook.Id.ToString(), patchWebhookRequest).ConfigureAwait(false);
            }
        }

    private void DeactiveWebhook()
        {
        var patchWebhookRequest = new WebhookPatchRequest()
            {
            Active = false,
            };

        _webhooksService.PatchWebhookAsync(_webhookId, patchWebhookRequest).ConfigureAwait(false);
        }
    }

