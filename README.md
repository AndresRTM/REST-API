# Person & Interest REST API

This repository contains a RESTful API built with **ASP.NET Core (.NET 10)** and Entity Framework Core. This project serves as a demonstration of modern backend development practices, utilizing a **Code First** approach and LINQ for data manipulation.

## Project Description

The application is a contact management system that allows users to store profiles, manage interests, and associate specific web links with those interests for each person. A key feature of the system is that URL links are specific to the connection between a person and an interest, rather than being generic to the interest itself.

### Key Features
The API implements the following core functionalities:
* **Person Management:** Store and retrieve individuals with name and phone number.
* **Interest Management:** Create and list various interests/hobbies.
* **Complex Linking:** Connect a person to an interest.
* **Link Storage:** Store multiple specific URLs for each person's interest.

## Technology Stack

| Component | Technology |
| :--- | :--- |
| **Framework** | ASP.NET Core (.NET 10) |
| **Architecture** | Minimal API |
| **Language** | C# (utilizing LINQ) |
| **Database Method** | Entity Framework Core (Code First) |
| **Database** | SQL Server (LocalDB) |
| **Documentation** | Scalar |
| **IDE** | Visual Studio Community 2026 |

### Key NuGet Packages
The project relies on the following dependencies to enable database functionality and documentation:
* `Microsoft.AspNetCore.OpenApi`
* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.Design`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Scalar.AspNetCore`

## Architecture & Code Organization

The project follows modern API design principles with a focus on maintainability, security, and separation of concerns.

### Modular Endpoint Structure
API endpoints are organized into dedicated endpoint classes using **Dependency Injection (DI)**, rather than being centralized in `Program.cs`. This approach provides:
* **Improved maintainability:** Each feature has its own dedicated class
* **Better testability:** Dependencies can be easily mocked
* **Loose coupling:** Components are independent and interchangeable

Endpoint classes are registered in the DI container in `Program.cs` and injected where needed, ensuring proper lifecycle management and resource handling.

### Data Transfer Objects (DTOs)
The API uses **DTOs** for input/output operations to:
* **Prevent over-posting vulnerabilities:** Clients cannot manipulate internal model properties
* **Improve security:** Sensitive fields are not exposed through the API
* **Enable validation:** Input data is validated before reaching the business logic
* **Provide abstraction:** API contracts are decoupled from database models

### Data Annotations
Model classes utilize **Data Annotations** for validation, ensuring:
* **Input sanitization:** Invalid data is rejected at the entry point
* **Consistent validation rules:** Validation logic is centralized in model classes
* **Automatic error responses:** Framework handles validation errors automatically

Examples of annotations used:
* `[Required]` - Ensures mandatory fields are provided
* `[StringLength]` - Limits text field length
* `[Phone]` - Validates phone number format
* `[Url]` - Validates URL format

## Database Schema

The database schema was designed using **Entity-Relationship (ER) modeling** utilizing **Crow's Foot notation** to visualize dependencies and cardinalities.

The core structure is a **Many-to-Many relationship** between **Person** and **Interest**. A junction table, `PersonInterestLink`, allows for this connection while also carrying unique payload data (the specific URL), ensuring that web links are distinct per user-interest pair.

<img width="1109" height="368" alt="image" src="https://github.com/user-attachments/assets/38146011-adb9-4a97-b29e-4e238802a688" />

## API Endpoints

Below is a list of the available endpoints. The documentation specifies whether data is required via **Route Parameters** (URL) or **JSON Body**.

### Persons
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/persons` | Retrieves a list of all persons in the database using LINQ queries. |
| `GET` | `/persons/{id}` | **Route Param:** `id` (int)<br>Retrieves a specific person by their ID, including their interests and links. |
| `POST` | `/persons` | **Body:** JSON `{ "firstName": "...", "lastName": "...", "phoneNumber": "..." }`<br>Creates a new person in the database. |

### Interests
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/interests` | Retrieves a list of all unique interests stored in the system. |
| `POST` | `/interests` | **Body:** JSON `{ "title": "...", "description": "..." }`<br>Creates a new interest type. |

### Connections & Links
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/persons/{id}/links` | **Route Param:** `id` (int)<br>Retrieves all web links associated with a specific person. |
| `POST` | `/person/{id}/interest` | **Route Param:** `id` (int) <br>**Body:** JSON `{ "title": "...", "description": "...", "url": "..." }`<br><br>Connects a person to an interest and saves a link. <br>**Logic:** Checks if the interest title exists (case-insensitive check via LINQ). Links to existing interest or creates a new one before linking. |

## Data Seeding

To facilitate testing, the project includes a data seeding mechanism located in `RestApiDBContext.cs`. Upon database creation, the system automatically populates tables with sample data, including:
* **Persons:** Pre-configured user profiles.
* **Interests:** Common hobbies.
* **Links:** Sample URLs linking specific users to interests.

This ensures the API returns valid data immediately after the first run without requiring manual input.
