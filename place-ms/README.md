# Place Microservice
The purpose of this service is to manage and document locations associated with the Family Welfare Office. Other services can access this one, primarily to retrieve information or link locations to individual case studies.

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

The application should now be running on http://localhost:5021 by default. You can test the email functionality by sending HTTP requests to the endpoint using a tool like Postman or curl.

### Testing the Local Deployment

To verify that the application is running correctly:

Make requests to http://localhost:5021. You can find some examples in the `place-ms.http` file.

## Deployment to Google Artifact Registry

Deploy the docker image to Artifact Registry using Google Cloud’s gcloud CLI with the following commands:

```bash
docker build -t place-ms .
docker tag place-ms us-east4-docker.pkg.dev/<project-id>/<repo-id>/place-ms
docker push us-east4-docker.pkg.dev/<project-id>/<repo-id>/place-ms
```