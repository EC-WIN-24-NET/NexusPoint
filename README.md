
# 🚀 Project Description

**NexusPoint** is a backend service designed to manage location data for an application named "Khala Event". Built with .NET 9, it functions as a RESTful API that exposes endpoints for retrieving location information. The project is structured following **Clean Architecture** principles, ensuring a clear separation of concerns between the API interface, business logic, and data access layers.

The application connects to an **Azure SQL** database for data persistence and leverages other Azure services for robust cloud-native operation. It integrates with **Azure Key Vault** to securely manage secrets like database connection strings and uses **Azure Blob Storage** for file storage needs. The API is well-documented using **Swagger (OpenAPI)** and **Scalar**, providing an interactive way to explore and test the endpoints.

---

## ⚙️ How It Works

The NexusPoint service is built on a layered architecture that ensures scalability and maintainability. The data flows through distinct layers, each with a specific responsibility.

### 1. API Layer (Presentation)

* **Entry Point**: A request starts at the `LocationController`, which handles incoming HTTP GET requests for a specific location, identified by a GUID.
* **Request Handling**: The controller validates the incoming GUID. It then calls the `ILocationService` to process the request.
* **Response Generation**: Upon receiving a result from the service layer, it uses the `ApiResponseHelper` to format a standardized JSON response, whether it's a success (200 OK) or an error (e.g., 404 Not Found, 500 Internal Server Error).

### 2. Core Layer (Application & Domain Logic)

* **Service (`LocationService`)**: This is the core of the business logic. It receives the request from the controller and orchestrates the data retrieval by calling the `ILocationRepository`.
* **Error & Result Handling**: It uses a `RepositoryResult` pattern to handle different outcomes from the repository—such as a successful data retrieval, a "not found" scenario, or a database connection error—without throwing exceptions for expected conditions.
* **Data Transfer Objects (DTOs)**: The service uses the `ILocationDtoFactory` to convert the internal domain model (`Location`) into a `LocationDisplay` DTO. This ensures that only necessary data is exposed to the client.
* **Domain Models**: The `Domain` folder contains the pure data structures of the application, like `Location` and `Error`, with no dependencies on other layers.

### 3. Infrastructure Layer (Data Access & External Services)

* **Repository (`LocationRepository`)**: This class implements the `ILocationRepository` interface and inherits from a generic `BaseRepository`. It is responsible for all database interactions.
* **Entity Framework Core**: The repository uses `DataContext` to query the database. It translates domain-level queries into SQL queries that Entity Framework Core can execute against the Azure SQL database.
* **Factories (`LocationFactory`)**: The `LocationFactory` handles the mapping between the `Location` domain model and the `LocationEntity` database entity, keeping the core logic completely decoupled from the database schema.
* **Dependency Injection**: The entire system is wired together using .NET's built-in dependency injection. The `Core` and `Infrastructure` layers register their services (`AddCoreServices`, `AddInfrastructure`) in the main `Program.cs` file, allowing components like controllers and services to receive their dependencies automatically.

### 4. CI/CD Pipeline

* A **GitHub Actions** workflow is defined in `main_nexuspoint.yml`.
* When code is pushed to the `main` branch, it automatically triggers a build process on an `ubuntu-latest` runner.
* The workflow builds the .NET application, publishes the artifacts, and then deploys them to an **Azure Web App** named "Nexuspoint" in the "Production" slot.

---

## 🛠️ Tech Stack

### Backend

* **Framework**: ASP.NET Core
* **Language**: C#
* **.NET Version**: 9.0

### Database

* **ORM**: Entity Framework Core
* **Database**: Azure SQL

### API & Documentation

* **API Type**: RESTful Web API
* **Documentation**: Swagger (OpenAPI) & Scalar

### Cloud & DevOps

* **Hosting**: Azure Web Apps
* **Secrets Management**: Azure Key Vault
* **File Storage**: Azure Blob Storage
* **CI/CD**: GitHub Actions

### Architecture

* **Pattern**: Clean Architecture (Onion Architecture)
* **Key Principles**: Dependency Injection, Repository Pattern, Factory Pattern
