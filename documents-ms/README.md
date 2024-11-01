# Documents Microservice
The purpose of this service is to assist with the documentation and tracking of cases within a Family Welfare Office. Its primary function is to record cases, including the victims and offenders involved, and to serve as a documentation hub for cases related to domestic violence, psychological support, and rights restoration processes.

This service interacts with other solution components to index individuals and locations associated with each case.

This service is built with .NET Core 8 as a RESTful CRUD API and is deployed as a Docker image running on Google Cloud Run.

## Running the Application Locally

To run this .NET Core 8 application on your local environment, follow these steps:

### Prerequisites

- Install .NET SDK 8 if not already installed.
- Configure the required settings in appsettings.json to match your local environment (SQL database connection string).

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

The application should now be running on http://localhost:5031 by default. You can test the email functionality by sending HTTP requests to the endpoint using a tool like Postman or curl.

### Testing the Local Deployment

To verify that the application is running correctly:

Make requests to http://localhost:5031. You can find some examples in the `documents-ms.http` file.

## Deployment to Google Artifact Registry

Deploy the docker image to Artifact Registry using Google Cloud’s gcloud CLI with the following commands:

```bash
docker build -t place-ms .
docker tag documents-ms us-east4-docker.pkg.dev/<project-id>/<repo-id>/people-ms
docker push us-east4-docker.pkg.dev/<project-id>/<repo-id>/documents-ms
```
