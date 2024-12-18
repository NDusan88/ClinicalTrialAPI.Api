# Use .NET SDK for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY *.sln ./  
COPY ClinicalTrialAPI.Api/*.csproj ./ClinicalTrialAPI.Api/
COPY ClinicalTrialAPI.Application/*.csproj ./ClinicalTrialAPI.Application/
COPY ClinicalTrialAPI.Domain/*.csproj ./ClinicalTrialAPI.Domain/
COPY ClinicalTrialAPI.Infrastructure/*.csproj ./ClinicalTrialAPI.Infrastructure/
COPY ClinicalTrialAPI.Tests/*.csproj ./ClinicalTrialAPI.Tests/

# Restore dependencies
RUN dotnet restore "ClinicalTrialAPI.sln"

# Copy source code and build
COPY . .
RUN dotnet publish "ClinicalTrialAPI.Api/ClinicalTrialAPI.Api.csproj" -c Release -o /app

# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .

# Expose port and set entry point
EXPOSE 80
ENTRYPOINT ["dotnet", "ClinicalTrialAPI.Api.dll"]
