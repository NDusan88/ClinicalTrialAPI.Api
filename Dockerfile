# Use the .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY *.sln ./
COPY ClinicalTrialAPI.Api/*.csproj ./ClinicalTrialAPI.Api/
COPY ClinicalTrialAPI.Application/*.csproj ./ClinicalTrialAPI.Application/
COPY ClinicalTrialAPI.Domain/*.csproj ./ClinicalTrialAPI.Domain/
COPY ClinicalTrialAPI.Infrastructure/*.csproj ./ClinicalTrialAPI.Infrastructure/

RUN dotnet restore "ClinicalTrialAPI.sln"

# Copy all source code and build the project
COPY . .
RUN dotnet build "ClinicalTrialAPI.Api/ClinicalTrialAPI.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ClinicalTrialAPI.Api/ClinicalTrialAPI.Api.csproj" -c Release -o /app/publish

# Use the ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ClinicalTrialAPI.Api.dll"]
