/*
 * Copyright © Bentley Systems, Incorporated. All rights reserved.
 * See LICENSE.md in the project root for license terms and full copyright notice.
 */

import axios from "axios";
import AuthService from "./auth";
import Configuration from "./config";
import { Config, WebhookMap } from "./models";

// Class for consuming iTwin Platform APIs
export default class Api {
  private authService: AuthService;
  private config: Config;

  constructor() {
    this.authService = new AuthService();
    this.config = Configuration.read();
  }

  public async createWebhook(
    callbackUrl: string,
    eventTypes: string[]
  ): Promise<WebhookMap> {

    const requestBody = {
      callbackUrl,
      eventTypes,
      scope: "Account"
    };

    try {
      var response = await axios({
        method: "POST",
        headers: {
          Authorization: "Bearer " + (await this.authService.getAccessToken()),
          Accept: "application/vnd.bentley.itwin-platform.v2+json"
        },
        data: requestBody,
        url: `${this.config.ApiUrl}/webhooks`
      });

      const webhookId = response.data["id"]
      const secret = response.data["secret"] as string;
      console.log(secret);

      console.log(`Created new webhook (ID: ${webhookId})`);
      return { secret, webhookId };
    } catch (err) {
      console.log("Could not create a webhook.");
      console.log(err.response.data);
    }
  }

  public async activateWebhook(
    id: string
  ): Promise<void> {

    const requestBody = {
      active: true
    };

    try {
      await axios({
        method: "PATCH",
        headers: {
          Authorization: "Bearer " + (await this.authService.getAccessToken()),
          Accept: "application/vnd.bentley.itwin-platform.v2+json"
        },
        data: requestBody,
        url: `${this.config.ApiUrl}/webhooks/${id}`
      });

      console.log(`Activated webhook (ID: ${id})`);
    } catch (err) {
      console.log("Could not activate a webhook.");
      console.log(err.response.data);
    }
  }
}
