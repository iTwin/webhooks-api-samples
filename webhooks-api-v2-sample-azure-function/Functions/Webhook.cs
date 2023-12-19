/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace webhooks_api_v2_sample_azure_function.Functions;

public class Webhook
{
    private readonly ILogger _logger;

    public Webhook(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Webhook>();        
    }

    [Function("Webhook")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        if (!req.Headers.TryGetValues("Signature", out var match))
        {
            _logger.LogInformation($"Signature header was not proviced");
            return req.CreateResponse(HttpStatusCode.Unauthorized);
        }

        var signature = match.FirstOrDefault().ToString().Split('=')[1];

        var webhookSecret = Environment.GetEnvironmentVariable("WEBHOOK_SECRET");
        var body = await new StreamReader(req.Body).ReadToEndAsync();

        var generatedSignature = GenerateSignature(webhookSecret, body);
        if (!signature.Equals(generatedSignature, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogInformation($"Signatures did not match");
            return req.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // do cool stuff here
        // but we are not awaiting.
        // there is a five second timeout
        // any request taking more than that
        // will be marked as failed and resent.
        DoCoolStuffAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private static string GenerateSignature(string key, string message)
    {
        var encoding = new UTF8Encoding();

        var keyEncoded = encoding.GetBytes(key);
        var messageEncoded = encoding.GetBytes(message);

        using var hmac = new HMACSHA256(keyEncoded);
        var hash = hmac.ComputeHash(messageEncoded);

        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    private static async Task DoCoolStuffAsync()
        {
        // cool stuff
        }
}
