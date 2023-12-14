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
        var defaultWebhookId = Environment.GetEnvironmentVariable("WEBHOOK_ID");
        if (!string.IsNullOrWhiteSpace(defaultWebhookId))
            {
            Webhook? webhook = _webhooksService.GetWebhookAsync(defaultWebhookId).Result;
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

