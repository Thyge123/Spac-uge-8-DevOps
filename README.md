# Cereal API Documentation

## Overview
The Cereal API is a comprehensive RESTful service that provides detailed information about breakfast cereals, featuring robust user authentication and role-based authorization. This API allows clients to retrieve, filter, and manage cereal product data along with associated product images.

### Data Model

#### Cereal Properties

* **id**: Unique identifier
* **name**: Product name
* **mfr**: Manufacturer code (A, G, K, N, P, Q, R)
* **type**: Type code (C for Cold, H for Hot)
* **calories**: Calories per serving
* **protein**: Protein content (g)
* **fat**: Fat content (g)
* **sodium**: Sodium content (mg)
* **fiber**: Dietary fiber (g)
* **carbo**: Complex carbohydrates (g)
* **sugars**: Sugar content (g)
* **potass**: Potassium content (mg)
* **vitamins**: Vitamins and minerals (% of daily value)
* **shelf**: Display shelf position (1, 2, or 3)
* **weight**: Weight per serving (oz)
* **cups**: Volume per serving (cups)
* **rating**: Consumer rating 

#### Manufacturer Codes

* **A**: American Home Food Products
* **G**: General Mills
* **K**: Kelloggs
* **N**: Nabisco
* **P**: Post
* **Q**: Quaker Oats
* **R**: Ralston Purina

### Error Handling

All endpoints return appropriate HTTP status codes:

* **200 OK**: Request succeeded
* **201 Created**: Resource successfully created
* **400 Bad Request**: Invalid request parameters
* **401 Unauthorized**: Authentication required
* **403 Forbidden**: Insufficient permissions
* **404 Not Found**: Resource not found
* **500 Internal Server Error**: Server-side error

Error responses include meaningful messages to help diagnose issues.

### Authorization Model
•	Anonymous: Can browse cereals and images

•	User: Full access to all API endpoints, including cereal and user management

## Tech Stack

**Framework**: ASP.NET Core 8

**Database**: SQL Server with Entity Framework Core

**Authentication**: JWT (JSON Web Tokens)

**Documentation**: Swagger/OpenAPI

## Design Decisions
1. Layered Architecture
   
The API implements a clean separation of concerns with three distinct layers:

    •	Controllers Layer: Handles HTTP requests/responses, input validation, and routing

    •	Example: CerealController processes HTTP requests and delegates business logic to the manager layer

    •	Benefits: Testable endpoints, simplified request handling, clear responsibility boundaries

    •	Manager Layer: Contains business logic and orchestrates data operations

    •	Example: CerealManager implements filtering, sorting, and data manipulation logic

    •	Benefits: Reusable business logic, separation from data access concerns

    •	Data Access Layer: Interfaces with the database via Entity Framework Core

    •	Example: DBContext implementation with Cereals and Users DbSets

    •	Benefits: Clean data access patterns, simplified CRUD operations

This separation enhances maintainability, testability, and scalability by keeping components focused on specific responsibilities.

2. Entity Framework Core
   
The API leverages Entity Framework Core as an ORM for several key advantages:

    •	Code-First Model: Models like Cereal define the database schema using attributes like [Key]
   
    •	DbContext Pattern: DBContext class provides a unified interface for database operations
   
    •	Migration Support: Enables version control of database schema changes
   
    •	LINQ Integration: Allows expressive, strongly-typed queries against the database
   
    •	Transaction Management: Handles database transactions automatically
   
This approach significantly reduces boilerplate data access code while providing robust database interactions.

3. JWT Authentication
   
JWT-based authentication was chosen for its stateless nature and security benefits:

    •	Stateless Architecture: No server-side session storage needed

    •	Self-Contained Tokens: Tokens carry all necessary user data (ID, username, role)

    •	Digital Signatures: Ensures token integrity through HMAC-SHA256 algorithms

    •	Expiration Control: Tokens include expiry claims (2 hours) limiting authentication windows

    •	Cross-Domain Support: Works seamlessly across different domains and services

Implementation in AuthHelpers generates tokens with proper claims, issuer, and audience validation.

4. Role-Based Authorization
   
The API implements granular access control through role-based authorization:

    •	Role Storage: User model includes a dedicated Role property
   
    •	JWT Claims: Roles embedded in tokens via ClaimTypes.Role
   
    •	Authorization Attributes: Routes protected with [Authorize(Roles = "Admin")]
   
    •	Role Validation: Token validation includes role verification
   
    •	Least Privilege Principle: Public routes require no auth, management routes require admin
   
This ensures sensitive operations (create, update, delete) are only accessible to authorized administrators.

5. Password Hashing
   
BCrypt hashing provides strong security for stored passwords:

    •	One-Way Hashing: Passwords can't be reverse-engineered from stored hashes
   
    •	Salt Integration: Each password hash includes a unique salt to prevent rainbow table attacks
   
    •	Computationally Intensive: Deliberate workload makes brute force attacks impractical
   
    • Future-Proof: Work factor can be adjusted as hardware advances
   
    •	Implementation: UsersManager.HashPassword() and VerifyPassword() methods encapsulate hashing logic
   
This prevents password exposure even in case of database compromise.

6. Comprehensive Error Handling
   
The API implements a multi-layered error handling strategy:

    •	Controller-Level Try-Catch: Each endpoint wraps operations in try-catch blocks
   
    •	Manager-Level Error Handling: Business logic includes appropriate exception handling
   
    •	Standardized HTTP Responses: Consistent error status codes (400, 401, 404, 500)
   
    •	Detailed Error Messages: Informative feedback for clients while avoiding security disclosures
   
    •	Exception Propagation: Critical errors bubble up with context maintained
   
    •	Logging: Console logging of errors with relevant context
   
This approach improves reliability, debuggability, and provides meaningful feedback to API consumers.

7. Flexible Querying
   
The API supports dynamic data retrieval through specialized models:

    •	Filter Models: CerealFilterModel allows filtering on any cereal property
   
    •	Sort Models: CerealSortModel enables dynamic sorting by any field in any direction
   
    •	Dynamic LINQ: Query composition using conditional LINQ expressions
   
    •	Stateless Design: No server-side query state is maintained between requests
   
    •	Performance Optimized: Queries execute at the database level rather than in memory
   
This allows clients to retrieve precisely the data they need in the desired format without requiring specialized endpoints for each query scenario.


## API Reference

#### Obtain JWT Token

```http
POST /login

```

| username | password     
| :-------- | :------- |
| `admin` | `password123` 

Response: 

| Token | 
| :-------- | 
| `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`

#### Cereal Endpoints
##### Get All Cereals

Retrieves a list of cereals with optional filtering and sorting.
```http
GET /api/products/cereals
```

Query Parameters

•	Filtering (all optional):

•	calories: Filter by calorie content

•	manufacturer: Filter by manufacturer code or name

•	type: Filter by cereal type (C for cold, H for hot)

•	protein, fat, sodium, etc.: Filter by nutritional values

•	rating: Filter by consumer rating

•	Sorting (optional):

•	sortBy: Property to sort by (name, calories, rating, etc.)

•	sortOrder: Sort direction (asc, desc)

#### Example
```http
GET /api/products/cereals?manufacturer=Kellogg&sortBy=rating&sortOrder=desc
```

Get Cereal by ID
```http
GET /api/products/cereal/42
```
Get Cereal by Name
```http
GET /api/products/cereal/Cheerios
```
Get Available Cereal Images
```http
GET /api/products/cereal/images
```
Get Specific Cereal Image
```http
GET /api/products/cereals/image/Cheerios
```
Add New Cereal (Admin only)
```http
POST /api/products/cereal/add
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

| name | mfr | type | calories | protein | fat | sodium | fiber | carbo | sugars | potass | vitamins | shelf | weight | cups | rating |
| :-------- | :------- | :------- | :-------- | :------- | :------- | :------- | :------- | :------- | :------- | :------- | :------- | :------- | :------- | :------- | :------- |
| `Healthy Crunch` | `K` | `C` | `120` | `3` | `2` | `140` | `5.0` | `12.0` | `7` | `130` | `25` | `2` | `1.0` | `0.75` | `45.32` |

Update Cereal (Admin only)
```http
PUT /api/products/cereal/update
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

{
  "id": 42,
  "name": "Healthy Crunch",
  "calories": 115,
  // Other properties...
}

```

Delete Cereal (Admin only)
```http
DELETE /api/products/cereal/delete/42
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...


```

#### User Endpoints
Get All Users
```http
GET /api/users
```
Create User

```http
POST /api/users

{
  "username": "newuser",
  "password": "securePassword123",
  "role": "User"
}
```



