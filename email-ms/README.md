# Email Microservice
The primary purpose of this microservice is to send emails with the necessary attachments to the family commissioner’s document management system. It is mainly utilized by other services, such as the Document Manager and the Historical Records Manager.

This service is developed in .NET Core 8 and deployed on Google Cloud Platform using Google Cloud Run, implemented as a Function-as-a-Service (FaaS).

## Running the Application Locally
To run this .NET Core 8 application on your local environment, follow these steps:

### Prerequisites
- Install .NET SDK 8 if not already installed.
- Configure the required settings in appsettings.json to match your local environment (e.g., SMTP credentials, sender information).

### Running the Application
Clone the repository (if you haven’t already):

```bash
git clone <repository-url>
cd <repository-folder>/<project-folder>
```
Restore dependencies:

```bash
dotnet restore
```
Run the application:

```bash
dotnet run
```
The application should now be running on http://localhost:8080 by default. You can test the email functionality by sending HTTP requests to the endpoint using a tool like Postman or curl.

### Testing the Local Deployment
To verify that the application is running correctly:

Send a POST request to http://localhost:8080 with the required email payload in the body.
Confirm that the application responds and, if configured correctly, sends an email.

***Basic request:***

```json
{
    "ToEmail": "ssuarezs@unal.edu.co",
    "Subject": "Your Subject Here",
    "HtmlContent": "<p>Your HTML content here</p>"
}
```

## Deployment to Google Cloud Run
Deploy the function using Google Cloud’s gcloud CLI with the following command:

```bash
gcloud functions deploy email-fun --gen2 --entry-point=EmailMs.Function --runtime=dotnet8 --region=us-east4 --source=. --trigger-http --allow-unauthenticated
```
### Validate Deployment
To verify that the deployment was successful, use:

```bash
gcloud functions describe csharp-http-function --region=us-east4
```
