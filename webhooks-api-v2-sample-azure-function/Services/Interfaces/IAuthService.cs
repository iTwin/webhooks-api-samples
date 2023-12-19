/*---------------------------------------------------------------------------------------------
* Copyright (c) Bentley Systems, Incorporated. All rights reserved.
* See LICENSE.md in the project root for license terms and full copyright notice.
*--------------------------------------------------------------------------------------------*/

namespace webhooks_api_v2_sample_azure_function.Services.Interfaces;

public interface IAuthService
{
    Task<string> GetAccessToken();
}
