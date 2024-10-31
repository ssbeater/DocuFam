# Use the official .NET Core 8 SDK as the build environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5010

# Use the .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./people-ms.csproj", "./"]
RUN dotnet restore "./people-ms.csproj"

# Copy everything else and build the app
COPY . .
WORKDIR "/src/"
RUN dotnet publish "./people-ms.csproj" -c Release -o /app/publish

# Use the runtime image to execute the app
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "people-ms.dll", "--urls", "http://0.0.0.0:5010"]
