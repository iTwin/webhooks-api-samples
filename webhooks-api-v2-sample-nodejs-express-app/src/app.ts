/*
 * Copyright © Bentley Systems, Incorporated. All rights reserved.
 * See LICENSE.md in the project root for license terms and full copyright notice.
 */

import crypto from "crypto";
import express, { Application } from "express";
import http from "http";
import Api from "./api";
import Configuration from "./config";
import { Config, Event, IModelDeletedEvent, MemberAddedEvent } from "./models";

export class App {
  private api: Api;
  private app: Application;
  private config: Config;

  constructor() {
    this.config = Configuration.read();

    this.app = express();
    this.app.use(express.text({ type: "application/json" }));

    this.api = new Api();

    // Add request handler 'POST [hostname]/events'
    this.app.post("/events", (req, res) => {
      const signatureHeader = req.headers["signature"] as string;
      if (!signatureHeader || !req.body) res.sendStatus(401);

      const event = JSON.parse(req.body) as Event;
      const secret = this.config.WebhookSecret

      if (!this.validateSignature(secret, req.body, signatureHeader)) res.sendStatus(401);

      // do cool stuff here
      // but we are not awaiting.
      // there is a five second timeout
      // any request taking more than that
      // will be marked as fail and resent.
      this.DoCoolStuffAsync(event);

      res.sendStatus(200);
    });
  }

  // Starts the server
  public async start(): Promise<void> {
    const server = http.createServer(this.app);

    // activate the webhook before starting the server
    await this.activateWebhook(this.config.WebhookId);

    server.listen(process.env.PORT, () => {
      return console.log(`Server was started.`);
    });
  }

  // Method for webhook activation
  private async activateWebhook(id: string): Promise<void> {
    await this.api.activateWebhook(id);
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

  private async DoCoolStuffAsync(event: Event): Promise<void> {
    switch (event.eventType) {
      case "iModels.iModelDeleted.v1": {
        const content = event.content as IModelDeletedEvent;
        console.log(`iModel (ID: ${content.imodelId}) in itwin (ID: ${event.iTwinId}) was deleted`);
        break;
      }
      case "accessControl.memberAdded.v1": {
        const content = event.content as MemberAddedEvent;
        console.log(`Member (Id:${content.memberId}) was added to iTwin (${event.iTwinId})!  Member was granted the ${content.roleName} role (Id: ${content.roleId}).`);
        break;
      }
      default:
        console.log("Unexpected event type");
    }
  }
}
