/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webhooks_api_v2_sample_azure_function;
using webhooks_api_v2_sample_azure_function.Services;
using webhooks_api_v2_sample_azure_function.Services.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
                
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IWebhooksService, WebhooksService>();
        services.AddHttpClient();
        services.AddHostedService<WebhooksHostedService>();
    })
    .Build();

host.Run();
