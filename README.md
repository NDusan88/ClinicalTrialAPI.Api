
# Clinical Trial API

The **Clinical Trial API** allows users to **upload, process, and retrieve clinical trial data**. It follows **Clean Architecture** principles and includes **file upload constraints**, **data validation**, **data storage**, and **API endpoints** for record retrieval.

## 🚀 Features
- **File Upload**: Upload JSON files with clinical trial data.
- **Data Validation**: Validates JSON files against a JSON schema.
- **Data Storage**: Stores data in a **SQL Server** database.
- **API Endpoints**:
  - **POST /api/clinicaltrials/upload** - Upload JSON file with trial data.
  - **GET /api/clinicaltrials/{id}** - Retrieve a trial by **ID**.
  - **GET /api/clinicaltrials?status=Ongoing** - Filter trials by **status**.
- **Error Handling**: Handles invalid files, schema mismatches, and more.
- **Containerized**: Run with **Docker** and **Docker Compose**.

## 🛠️ Prerequisites
Before you begin, ensure you have the following installed:

- **.NET 8 SDK**: [Install .NET SDK](https://dotnet.microsoft.com/download/dotnet)
- **SQL Server**: Use **local SQL Server** or run the SQL container provided in **docker-compose.yml**.
- **Docker**: [Install Docker](https://www.docker.com/products/docker-desktop)

## ⚙️ Setup Instructions

### 1️⃣ Local Setup (Without Docker)
1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-repo-name/ClinicalTrialAPI.git
   cd ClinicalTrialAPI
   ```

2. **Set Up SQL Server Database**
   - Ensure you have SQL Server running locally.
   - Update **appsettings.json** with your local SQL Server configuration:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ClinicalTrialDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
     ```

3. **Run Database Migrations**
   ```bash
   dotnet ef database update --project ClinicalTrialAPI.Infrastructure --startup-project ClinicalTrialAPI.API
   ```

4. **Run the Application**
   ```bash
   dotnet run --project ClinicalTrialAPI.Api
   ```

5. **Access the API**
   - Open your browser and navigate to:  
     **http://localhost:5000/swagger/index.html**

## 🐳 Running with Docker
To run the API and SQL Server using Docker, follow these steps.

### 1️⃣ Docker Requirements
Ensure **Docker Desktop** is installed and running.

### 2️⃣ Build and Run Containers
```bash
docker-compose up --build
```

### 3️⃣ Services
The following services will be running:
| **Service**     | **URL**             | **Port** |
|-----------------|---------------------|---------|
| **API**         | http://localhost:8080 | **8080** |
| **SQL Server**  | localhost:1433       | **1433** |

## 📦 API Usage

### 1️⃣ Upload a Clinical Trial (POST)
- **URL**: `/api/clinicaltrials/upload`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "trial": {
      "trialId": "9d7f814e-0eec-4e9f-a6d9-4e3ec11e2f77",
      "title": "COVID-19 Vaccine Trial",
      "startDate": "2024-06-01",
      "endDate": "2024-12-01",
      "participants": 100,
      "status": "Ongoing"
    }
  }
  ```

### 2️⃣ Get Clinical Trial by ID (GET)
- **URL**: `/api/clinicaltrials/{id}`
- **Method**: `GET`
- **Response**:
  ```json
  {
    "trialId": "9d7f814e-0eec-4e9f-a6d9-4e3ec11e2f77",
    "title": "COVID-19 Vaccine Trial",
    "startDate": "2024-06-01",
    "endDate": "2024-12-01",
    "participants": 100,
    "status": "Ongoing",
    "duration": 180
  }
  ```

### 3️⃣ Get Clinical Trials by Status (GET)
- **URL**: `/api/clinicaltrials?status=Ongoing`
- **Method**: `GET`
- **Response**:
  ```json
  [
    {
      "trialId": "9d7f814e-0eec-4e9f-a6d9-4e3ec11e2f77",
      "title": "COVID-19 Vaccine Trial",
      "status": "Ongoing"
    }
  ]
  ```

## 🧪 Running Tests
Run the tests for controllers, services, and use cases.

### Run All Tests
```bash
dotnet test
```

### Run a Specific Test
```bash
dotnet test --filter FullyQualifiedName~ClinicalTrialAPI.Tests.Controllers.ClinicalTrialsControllerTests
```

### Test Coverage
- **Unit Tests**: Tests for **Controller**, **Service**, and **Use Cases**.
- **Integration Tests**: Tests for **full API flow**.

## 📦 Docker Commands

### Build Image
```bash
docker build -t clinical-trial-api .
```

### Run Container
```bash
docker run -p 8080:80 clinical-trial-api
```

### Stop Containers
```bash
docker-compose down
```

## 📂 Project Structure
```
ClinicalTrialAPI
├── ClinicalTrialAPI.Api
├── ClinicalTrialAPI.Application
├── ClinicalTrialAPI.Domain
├── ClinicalTrialAPI.Infrastructure
├── ClinicalTrialAPI.Tests
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## 🐛 Troubleshooting
- **Error: SQL Server is not available**: Ensure **SQL Server is running locally** or in Docker.
- **Error: Cannot connect to localhost from container**: Change **localhost** to **host.docker.internal**.
