# Clinical Trial API

## 🛠️ Setup
1. Clone the repo
2. Run `dotnet restore`
3. Run `dotnet ef database update`
4. Run the API using `dotnet run`

## 🛠️ Usage
- **POST /api/clinicaltrials/upload**: Upload JSON file
- **GET /api/clinicaltrials/{id}**: Get trial by ID
- **GET /api/clinicaltrials?status={status}**: Filter trials by status
