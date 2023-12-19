/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

using System.Text.Json.Serialization;

namespace webhooks_api_v2_sample_azure_function.Models;


public class WebhookCreateRequest
    {
    [JsonPropertyName("callbackUrl")]
    [JsonRequired]
    public string CallbackUrl
        {
        get; set;
        }

    [JsonPropertyName("scope")]
    public string Scope
        {
        get; set;
        }

    [JsonPropertyName("secret")]
    public string Secret
        {
        get; set;
        }

    [JsonPropertyName("eventTypes")]
    public List<string> EventTypes
        {
        get; set;
        }
    }

public class WebhookCreateResponse
    {
    [JsonPropertyName("id")]
    [JsonRequired]
    public Guid Id
        {
        get; set;
        }

    [JsonPropertyName("scope")]
    public string Scope
        {
        get; set;
        }

    [JsonPropertyName("scopeId")]
    public string ScopeId
        {
        get; set;
        }

    [JsonPropertyName("active")]
    [JsonRequired]
    public bool Active
        {
        get; set;
        }

    [JsonPropertyName("callbackUrl")]
    [JsonRequired]
    public string CallbackUrl
        {
        get; set;
        }

    [JsonPropertyName("secret")]
    public string Secret
        {
        get; set;
        }

    [JsonPropertyName("eventTypes")]
    public List<string> EventTypes
        {
        get; set;
        }
    }

public class WebhookPatchRequest
    {
    [JsonPropertyName("active")]
    public bool? Active
        {
        get; set;
        }

    [JsonPropertyName("callbackUrl")]
    public string? CallbackUrl
        {
        get; set;
        }

    [JsonPropertyName("secret")]
    public string? Secret
        {
        get; set;
        }

    [JsonPropertyName("eventTypes")]
    public List<string>? EventTypes
        {
        get; set;
        }
    }

public class WebhooksResponse
    {
    [JsonPropertyName("webhooks")]
    [JsonRequired]
    public List<Webhook> Webhooks { get; set; }
    }

public class Webhook
    {
    [JsonPropertyName("id")]
    [JsonRequired]
    public Guid Id
        {
        get; set;
        }

    [JsonPropertyName("scope")]
    public string Scope
        {
        get; set;
        }

    [JsonPropertyName("scopeId")]
    public string ScopeId
        {
        get; set;
        }

    [JsonPropertyName("active")]
    [JsonRequired]
    public bool Active
        {
        get; set;
        }

    [JsonPropertyName("callbackUrl")]
    [JsonRequired]
    public string CallbackUrl
        {
        get; set;
        }

    [JsonPropertyName("eventTypes")]
    public List<string> EventTypes
        {
        get; set;
        }

    [JsonPropertyName("created")]
    [JsonRequired]
    public DateTime Created
        {
        get; set;
        }

    [JsonPropertyName("modified")]
    [JsonRequired]
    public DateTime Modified
        {
        get; set;
        }
    }


