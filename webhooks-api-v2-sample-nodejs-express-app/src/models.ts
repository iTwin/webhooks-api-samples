/*
 * Copyright © Bentley Systems, Incorporated. All rights reserved.
 * See LICENSE.md in the project root for license terms and full copyright notice.
 */

export interface Config {
  Authority: string;
  ApiUrl: string;
  AppUrl: string;
  ClientId: string;
  ClientSecret: string;
}

export type Event = {
  content: IModelDeletedEvent | MemberAddedEvent;
  eventType: string;
  enqueuedDateTime: string;
  iTwinId: string;
  messageId: string;
  webhookId: string;
};

export type IModelDeletedEvent = {
  imodelId: string;
  userId: string;
};

export type MemberAddedEvent = {
  memberId: string;
  eventCreatedBy: string;
  memberType: string;
  roleId: string;
  roleName: string;
}

export type WebhookMap = {
  secret: string;
  webhookId: string;
};
