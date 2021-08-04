/*
 * Copyright © Bentley Systems, Incorporated. All rights reserved.
 * See LICENSE.md in the project root for license terms and full copyright notice.
 */

import crypto from "crypto";
import express, { Application } from "express";
import http from "http";
import Api from "./api";
import Configuration from "./config";
import { Config, Event, IModelDeletedEvent, NamedVersionCreatedEvent } from "./models";

export class App {
  private api: Api;
  private app: Application;
  private config: Config;

  // Webhook ID to secret map
  private webhooks: { [key: string]: string };

  constructor() {
    this.config = Configuration.read();

    this.app = express();
    this.app.use(express.text({ type: "application/json" }));

    this.api = new Api();
    this.webhooks = {};

    // Add request handler 'POST [hostname]/events'
    this.app.post("/events", (req, res) => {
      const signatureHeader = req.headers["signature"] as string;
      if (!signatureHeader || !req.body) res.sendStatus(401);

      const event = JSON.parse(req.body) as Event;
      const webhookId = event.subscriptionId;
      const secret = this.webhooks[webhookId];

      if (!this.validateSignature(secret, req.body, signatureHeader)) {
        res.sendStatus(401);
      } else {
        switch (event.contentType) {
          case "iModelDeletedEvent": {
            const content = event.content as IModelDeletedEvent;
            console.log(`iModel (ID: ${content.imodelId}) in project (ID: ${content.projectId}) was deleted`);
            break;
          }
          case "NamedVersionCreatedEvent": {
            const content = event.content as NamedVersionCreatedEvent;
            console.log(
              `New named version (ID: ${content.versionId}, Name: ${content.versionName}) was created for iModel (ID: ${content.imodelId})`
            );
            break;
          }
          default:
            res.status(400).send("Unexpected event type");
        }
        res.sendStatus(204);
      }
    });
  }

  // Starts the server
  public async start(): Promise<void> {
    const server = http.createServer(this.app);

    // Create a webhook before starting the server
    await this.createWebhook();

    server.listen(process.env.PORT, () => {
      return console.log(`Server was started.`);
    });
  }

  // Method for webhook creation
  private async createWebhook(): Promise<void> {
    const imodelId = this.config.IModelId;
    const callbackUrl = `${this.config.AppUrl}/events`;
    const eventTypes = ["iModelDeletedEvent", "NamedVersionCreatedEvent"];

    const webhook = await this.api.createIModelEventWebhook(imodelId, callbackUrl, eventTypes);

    // Store created webhook ID and secret
    this.webhooks[webhook.webhookId] = webhook.secret;
  }

  // Method for event signature validation
  private validateSignature(secret: string, payload: string, signatureHeader: string): boolean {
    // Extract algorithm and signature from signature header value
    // Header value example: sha256=a24a2e58912f4708f21eb043def1b1bcc0684b81a2e3feebe04ca558ff9830ce
    const [algorithm, signature] = signatureHeader.split("=");

    const generated_sig = crypto.createHmac(algorithm, secret).update(payload, "utf-8").digest("hex");

    // Event is valid if the signatures match
    return generated_sig.toLowerCase() === signature.toLowerCase();
  }
}
