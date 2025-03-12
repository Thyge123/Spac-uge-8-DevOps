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



