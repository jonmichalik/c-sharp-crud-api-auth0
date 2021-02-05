# Task Board API: C# and Azure Sample

## Quick Setup

### 1. Configure Auth0

You need an Auth0 account. If you don't have one yet, <a href="https://auth0.com/signup">sign up for a free Auth0 account</a>.

Open the [APIs section of the Auth0 Dashboard](https://manage.auth0.com/#/apis), click the "Create API" button.

Fill out the form that comes up:

- **Name:**

```
Task Board API Demo
```

- **Identifier:**

```
https://c-sharp-crud-api.azurewebsites.net
```

Leave the signing algorithm as `RS256`. It's the best option from a security standpoint.

Once you've added those values, click the "Create" button.

Once you create an Auth0 API, a new page loads up, presenting you with your Auth0 API information.

Keep page open to complete the next step.

### 2. Update `appsettings.json` values

In the project's `appsettings.json` file, you'll find the following:

```json
"Auth0": {
    "Domain": "<AUTH0_DOMAIN>",
    "Identifier": "<AUTH0_API_IDENTIFIER>"
  }
```

You will replace `<AUTH0_DOMAIN>` with the domain associated to your Auth0 account created in step 1.  You will also replace `<AUTH0_API_IDENTIFIER>` with the URL identifier you configured for your Auth0 API in step 1.

If you're having trouble finding your domain, you can follow the steps below:

![Get the Auth0 Domain to configure an API](https://cdn.auth0.com/blog/complete-guide-to-user-authentication/get-the-auth0-domain.png)

1. Click on the **"Test"** tab.
2. Locate the section called **"Asking Auth0 for tokens from my application"**.
3. Click on the **cURL** tab to show a mock `POST` request.
4. Copy your Auth0 domain, which is _part_ of the `--url` parameter value: `tenant-name.region.auth0.com`.
5. Paste the Auth0 domain value as the value of `AUTH0_DOMAIN` in `.env`.

The domain value included in the `appsettings.json` should include the `https://`.  For example:

```
https://example.us.auth0.com
```

### 3. Configure Azure with Visual Studio 2019 for Windows

You need an Azure account to be able to deploy applications to Azure.  You can start the sign up process on the [Microsoft Azure website](https://azure.microsoft.com/en-us/free/dotnet/).

You will also need Visual Studio 2019 to follow these steps.  You can download Visual Studio 2019 Community for free on the [Microsoft Visual Studio website](https://visualstudio.microsoft.com/downloads/)

With the project open in Visual Studio, under the **"Build"** menu, choose **"Publish c-sharp-crud-api"** and follow the steps below:

1. Choose **"Azure"** as your Target and click **"Next"**.
2. Choose **"Azure App Service (Windows)"** as your Specific Target and click **"Next"**.
3. Click the **"+"** button at the top-right of the App Service instances box.
4. Type a unique name for your App Service (this will be a part of your API URL).
5. Click **"new..."** to the right of the Resource Group dropdown.
6. Name the resource group, your app service name with a suffix of "ResourceGroup" works well (e.g. c-sharp-crud-apiResourceGroup).
7. Click **"new..."** to the right of the Hosting Plan dropdown.
8. Name the resource group, your app service name with a suffix of "Plan" works well (e.g. c-sharp-crud-apiPlan).
9. Choose the Location nearest you, this is where you API will be hosted.
10. Choose **"Free"** for Size and click **"OK"**.
11. Click the **"Create"** button.
12. Click the App Service instance with your App Service name and click **"Next"**.
13. Check the **"Skip this step"** checkbox for API Management and click **"Finish"**.
14. Click the **"Publish"** button on your newly made public profile and the API with be deployed to Azure!

If at any point you want to check the status of your deployed API, you can visit your Azure account dashboard and list your App Services there for more details.

### 4. Test a public endpoint

Open a terminal window and test if the server is working by making the following request to get all the task items:

```bash
curl https://<your-app-name>.azurewebsites.net/api/tasks
```

You should get the following response from the server:

```json
[
    {
        "id": "1",
        "name": "C# CRUD API Walkthrough",
        "description": "Finish a C# CRUD API tutorial and deploy with Visual Studio to Azure",
        "status": "Defined"
    }
]
```

### Test a protected endpoint

You need an access token to call any of the protected API endpoints.

Open a terminal window and try to make the following request (the `-i` flag will include more information in the response, including the HTTP status code):

```bash
curl --request DELETE -i --url https://<your-app-name>.azurewebsites.net/api/tasks/1
```

You'll get the following response status:

```bash
HTTP/1.1 401 Unauthorized
```

To get an access token, head back to your API configuration page in the Auth0 Dashboard.

Click on the "Test" tab and locate the "Sending the token to the API".

Click on the "cURL" tab.

You should see something like this:

```bash
curl --request GET \
  --url https://path_to_your_api/ \
  --header 'authorization: Bearer really-long-string'
```

Copy and paste that value in a text editor.

In the value of the `--header` parameter, the value of `authorization` is your access token.

Replace the value of the `--url` parameter with your `DELETE api/tasks/` endpoint URL:

```bash
curl --request DELETE \
  --url https://<your-app-name>.azurewebsites.net/api/tasks/1 \
  --header 'authorization: Bearer really-long-string'
```

Copy and paste the updated cURL command into a terminal window and execute it. You should now get a successful response.

Try calling any of the API endpoints outlined in the next section.


## API Endpoints

### 🔓 List Tasks

Lists all tasks on the task board.

```bash
GET /api/tasks
```

#### Response

```bash
Status: 200 OK
```

```json
[
    {
        "id": "1",
        "name": "C# CRUD API Walkthrough",
        "description": "Finish a C# CRUD API tutorial and deploy with Visual Studio to Azure",
        "status": "Defined"
    }
]
```

### 🔓 Get an item

Gets information for a single task from the task board.

```bash
GET /api/tasks/:id
```

#### Response

##### If item is not found

```bash
Status: 404 Not Found
```

##### If item is found

```bash
Status: 200 OK
```

```json
{
    "id": "1",
    "name": "C# CRUD API Walkthrough",
    "description": "Finish a C# CRUD API tutorial and deploy with Visual Studio to Azure",
    "status": "Defined"
}
```

> 🔐 Protected Endpoints: These endpoints require the request to include an access token issued by Auth0 in the authorization header.

### 🔐 Create a task for the authenticated user

Creates a task on the task board for the authenticated user.

```bash
POST /api/tasks
```

#### Input

| Name          | Type       | Description                         |
|---------------|:-----------|:------------------------------------|
| `name`        | `string`   | The name of the task.               |
| `description` | `string`   | The description of the task.        |
| `status`      | `string`   | The status of the task.             |

##### Example

```json
{
    "name": "Finish a task today",
    "description": "Yes, that can include this one :)",
    "status": "Defined"
}
```

#### Response

```bash
Status: 201 Created
```

```json
{
    "id": "2",
    "name": "Finish a task today",
    "description": "Yes, that can include this one :)",
    "status": "Defined"
}
```

### 🔐 Update a task

Update a task from the task board.

```bash
PUT /api/tasks/:id
```

#### Input

| Name          | Type       | Description                         |
|---------------|:-----------|:------------------------------------|
| `name`        | `string`   | The name of the task.               |
| `description` | `string`   | The description of the task.        |
| `status`      | `string`   | The status of the task.             |

If you only need to update some of the item properties, leave the other values as they are.
                             
##### Example

Take the following item as an example: 

```json
{
    "name": "Finish a task today",
    "description": "Yes, that can include this one :)",
    "status": "Defined"
}
```

If you want to update the description only, you'll send a request body like the following:

```json
{
    "name": "Finish a task today",
    "description": "Actually, better make it a new one...",
    "status": "Defined"
}
```

#### Response

##### If item is not found

```bash
Status: 404 Not Found
```

##### If item is found

```bash
Status: 200 OK
```

```bash
{
    "id": "2",
    "name": "Finish a task today",
    "description": "Actually, better make it a new one...",
    "status": "Defined"
}
```

### 🔐 Remove an item

Remove a task from the task board.

```
DELETE /api/tasks/:id
```

#### Response

##### If item is not found

```bash
Status: 404 Not Found
```

##### If item is found

```bash
Status: 204 No Content
```
