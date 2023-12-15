# Webhooks API V2 sample Azure Function

An Azure Function that shows the basic examples of consuming Webhooks V2 API and reacting to iTwin Platform events.

This sample Azure Function:

* On startup checks if the provided webhook exists and is active.
	- Inactive webhooks will be reactivated
* Creates an Azure Function with an HTTP Trigger.
* Receives events and validates the signature.
* On shutdown the provided webhook will be deactivated.

## Prerequisites

* Create "Service" type application in <https://developer.bentley.com/register/> with the scopes `webhooks:read` and `webhooks:modify`.
* Create a webhook. Store this webhook's secret in the environment variable `WEBHOOK_SECRET`
* [Git](https://git-scm.com/)
* Visual Studio 2019/2022 or [Visual Studio Code](https://code.visualstudio.com/)
* [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0/)
* Provide your configuration values in `host.json` or `local.settings.josn` if you are developing locally.


## Configuration Variables

The following need to be set in the `host.json` or `local.settings.josn` if you are developing locally.

* `WEBHOOK_ID` Id of an existing webhook. If webhook exist, it will be activated on startup.
* `WEBHOOK_SECRET`: 32 character string used for the `secret` property when creating a webhook. This will also be used to validate the post body of the webhook request. For more information of the `secret` property see [here](https://developer.bentley.com/apis/webhooks-v2/overview/#webhooksecret)
* `OAUTH_CLIENT_ID`: Id of the service application
* `OAUTH_CLIENT_SECRET`: Secret of the service application


## Related Materials

* [Webhooks V2 API Overview](https://developer.bentley.com/apis/webhooks-v2/overview/)
* [List of available events](https://developer.bentley.com/apis/webhooks-v2/events/)
