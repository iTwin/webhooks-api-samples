# Webhooks API V2 sample NodeJs Express application

Node.js (Express) application that shows the basic examples of consuming Webhooks V2 API and reacting to iTwin Platform events.

This sample application:

1. Activates the webhook with two event types (specify the webhook id with the `WEBHOOK_ID`).
2. Starts Express.js server.
3. Receives events and validates the signature.
4. Reacts accordingly to event type.

## Prerequisites

1. Create "Service" type application in <https://developer.bentley.com/register/>.
2. Create or update a webhooks with the event types `iModels.iModelDeleted.v1` and `accessControl.memberAdded.v1`. Store this webhook's secret in the environment variable `WEBHOOK_SECRET`
3. Prepare an existing iModel that your client has access to.
4. Deploy this application (e.g. Heroku/Netlify or any other preference).
5. Provide your configuration values in `.env` file.

### Local

You can try and run the application locally to see if it works, but it won't capture any events unless it is publicly available.

To run the application locally execute:

```ps
npm install
npm start
```

### Deployment

Deploy the application using Heroku:

1. Create new Heroku application and empty Heroku Git repository <https://devcenter.heroku.com/articles/git#for-a-new-heroku-app>.
2. Set `APP_URL` in `.env` with newly created public address.
3. Replace other configuration values in `.env`.
4. Deploy by pushing the code <https://devcenter.heroku.com/articles/git#deploying-code>.
5. Use `heroku logs --tail` for monitoring the behavior of the application.

#### Related material

- [Deploying an iTwin app to Heroku/Netlify](https://medium.com/itwinjs/deploying-the-itwin-viewer-to-a-web-host-d45c5cfdf0cf)
