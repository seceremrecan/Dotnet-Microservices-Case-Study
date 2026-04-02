# Dotnet Microservices Case Study

This project was developed as a backend developer case study using **.NET 8** and a **microservice architecture**.  
It includes **Auth**, **Product**, and **Log** services together with **JWT authentication**, **Redis caching**, **RabbitMQ-based event-driven communication**, **YARP API Gateway**, and **SQL Server** integration.

## Project Overview

The purpose of this project is to demonstrate a backend solution designed with:

- Onion Architecture
- CQRS pattern
- JWT-based authentication and authorization
- Refresh token support
- Redis-based caching with cache invalidation
- Event-driven communication with RabbitMQ
- Centralized logging service
- API Gateway routing with YARP
- Docker Compose usage for Redis and RabbitMQ

Each service follows Onion Architecture:

- **Domain** → entities and core business models
- **Application** → CQRS handlers, DTOs, abstractions
- **Infrastructure** → repositories, EF Core, JWT, Redis, RabbitMQ integrations
- **API** → controllers and HTTP endpoints

The Product service uses CQRS by separating command and query operations.

## Technologies Used

- .NET 8
- C#
- Entity Framework Core
- SQL Server Express
- Redis
- RabbitMQ
- YARP Reverse Proxy
- Docker Compose
- Swagger / OpenAPI
- Postman
- Git / GitHub

## Project Structure

```
BackendDeveloperCaseStudy
├── src
│   ├── Gateway
│   │   └── ApiGateway
│   ├── Shared
│   │   └── Shared.Contracts
│   └── Services
│       ├── Auth
│       │   ├── Auth.API
│       │   ├── Auth.Application
│       │   ├── Auth.Domain
│       │   └── Auth.Infrastructure
│       ├── Product
│       │   ├── Product.API
│       │   ├── Product.Application
│       │   ├── Product.Domain
│       │   └── Product.Infrastructure
│       └── Log
│           ├── Log.API
│           ├── Log.Application
│           ├── Log.Domain
│           └── Log.Infrastructure
├── tests
├── docs
├── docker-compose.yml
└── BackendDeveloperCaseStudy.sln
```

## Table of Contents

- [Project Overview](#project-overview)
- [Services](#services)
- [Authentication and Authorization](#authentication-and-authorization)
- [Redis Caching](#redis-caching)
- [Event-Driven Communication](#event-driven-communication)
- [Logging](#logging)
- [Databases](#databases)
- [Prerequisites](#prerequisites)
- [Running Redis and RabbitMQ with Docker Compose](#running-redis-and-rabbitmq-with-docker-compose)
- [Running Services Locally](#running-services-locally)
- [Database Setup](#database-setup)
- [Testing via API Gateway](#testing-via-api-gateway)
- [Suggested Postman Demo Flow](#suggested-postman-demo-flow)
- [Branch Strategy](#branch-strategy)
- [Repository](#repository)
- [Notes](#notes)
- [Future Improvements](#future-improvements)

## Services

### Auth Service

Responsible for:

- User registration
- User login
- JWT access token generation
- Refresh token generation

Endpoints:

- `POST /api/Auth/register`
- `POST /api/Auth/login`
- `POST /api/Auth/refresh-token`

### Product Service

Responsible for:

- Creating products
- Updating products
- Listing products
- JWT-protected update operations
- Redis caching for product listing
- Cache invalidation after create/update
- Publishing product events to RabbitMQ

Endpoints:

- `POST /api/Products`
- `PUT /api/Products/{id}`
- `GET /api/Products`

### Log Service

Responsible for:

- Creating centralized logs
- Listing logs
- Consuming RabbitMQ product events
- Supporting log levels: Info, Warning, Error, Critical

Endpoints:

- `POST /api/Logs`
- `GET /api/Logs`

### API Gateway

The API Gateway is implemented with YARP Reverse Proxy.

Gateway routes:

- `/auth/{**catch-all}` → Auth Service
- `/products/{**catch-all}` → Product Service
- `/logs/{**catch-all}` → Log Service

## Authentication and Authorization

JWT-based authentication is implemented in the Auth Service.

The Product update endpoint is protected with JWT:

- Requests without token return 401 Unauthorized
- Requests with a valid bearer token succeed

## Redis Caching

Redis is used in the Product Service for product listing.

Flow:

1. `GET /api/Products` first checks Redis
2. If data exists in cache, it returns cached data
3. If not, data is fetched from SQL Server and stored in Redis
4. After `POST` or `PUT`, product cache is invalidated

Redis key used: `products:list`

## Event-Driven Communication

RabbitMQ is used to support asynchronous communication between services.

The Product Service publishes events after product creation and update operations.

Published events:

- `ProductCreatedEvent`
- `ProductUpdatedEvent`

The Log Service consumes these events and automatically creates centralized log records.

This improves:

- Loose coupling between services
- Asynchronous processing
- Scalability of the system

## Logging

A dedicated Log Service is implemented for centralized logging.

Each log entry includes:

- Service name
- Message
- Exception
- Level
- Created timestamp

Example event-driven log flow:

1. Product is created
2. Product Service publishes `ProductCreatedEvent`
3. Log Service consumes the event
4. A new log entry is stored in the Log database

## Databases

This project uses local SQL Server Express.

Databases:

- `BackendDeveloperCaseStudy_AuthDb`
- `BackendDeveloperCaseStudy_ProductDb`
- `BackendDeveloperCaseStudy_LogDb`

## Prerequisites

Before running the project, make sure the following are installed:

- .NET 8 SDK
- SQL Server Express
- Docker Desktop
- Visual Studio Code or Visual Studio
- Git
- Postman

## Running Redis and RabbitMQ with Docker Compose

Redis and RabbitMQ are containerized using Docker Compose.

`docker-compose.yml`:

```yaml
services:
    redis:
        image: redis:latest
        container_name: redis-cache
        ports:
            - "6379:6379"

    rabbitmq:
        image: rabbitmq:3-management
        container_name: rabbitmq-broker
        ports:
            - "5672:5672"
            - "15672:15672"
```

Run services:

```bash
docker compose up -d
docker compose ps
```

RabbitMQ Management Panel: [http://localhost:15672](http://localhost:15672)  
Username: `guest`  
Password: `guest`

## Running Services Locally

Run each service in a separate terminal.

- **Auth API**: `dotnet run --project .\src\Services\Auth\Auth.API\Auth.API.csproj`
- **Product API**: `dotnet run --project .\src\Services\Product\Product.API\Product.API.csproj`
- **Log API**: `dotnet run --project .\src\Services\Log\Log.API\Log.API.csproj`
- **API Gateway**: `dotnet run --project .\src\Gateway\ApiGateway\ApiGateway.csproj`

Default Local Ports:

- Auth API → [http://localhost:5250](http://localhost:5250)
- Product API → [http://localhost:5189](http://localhost:5189)
- Log API → [http://localhost:5141](http://localhost:5141)
- API Gateway → [http://localhost:5118](http://localhost:5118)
- Redis → localhost:6379
- RabbitMQ → localhost:5672
- RabbitMQ Management → [http://localhost:15672](http://localhost:15672)

## Database Setup

This project uses Entity Framework Core migrations for each service.

If needed, database migrations can be applied with the following commands.

- **Product Service**: `dotnet ef database update --project .\src\Services\Product\Product.Infrastructure\Product.Infrastructure.csproj --startup-project .\src\Services\Product\Product.API\Product.API.csproj`
- **Auth Service**: `dotnet ef database update --project .\src\Services\Auth\Auth.Infrastructure\Auth.Infrastructure.csproj --startup-project .\src\Services\Auth\Auth.API\Auth.API.csproj`
- **Log Service**: `dotnet ef database update --project .\src\Services\Log\Log.Infrastructure\Log.Infrastructure.csproj --startup-project .\src\Services\Log\Log.API\Log.API.csproj`

## Testing via API Gateway

- **Product list**: `GET http://localhost:5118/products/api/Products`
- **Logs list**: `GET http://localhost:5118/logs/api/Logs`
- **Auth login**: `POST http://localhost:5118/auth/api/Auth/login`  
   Content-Type: `application/json`

  Request body:

  ```json
  {
    "email": "emre@example.com",
    "password": "123456"
  }
  ```

## Suggested Postman Demo Flow

1. Login from Auth Service
2. Copy the `accessToken`
3. Send update request to Product Service without token → expect 401
4. Send the same request with bearer token → expect 200
5. Get product list again
6. Get logs list
7. Create or update a product
8. Verify RabbitMQ event-driven log generation in Log Service

## Branch Strategy

The project uses the following branches:

- `test/v1.0.0`
- `prod/v1.0.0`

## Repository

GitHub Repository: [https://github.com/seceremrecan/Dotnet-Microservices-Case-Study](https://github.com/seceremrecan/Dotnet-Microservices-Case-Study)

## Notes

- Redis and RabbitMQ are run with Docker Compose.
- SQL Server Express is used locally.
- APIs are run locally for stable integration with SQL Server Express.
- This solution demonstrates the required microservice structure and core backend patterns.
- Full containerization of all APIs was not preferred because local SQL Server Express integration is more stable in this setup.

## Future Improvements

- Full Docker Compose setup for all services
- SQL authentication support for fully containerized APIs
- YARP rate limiting
- Role-based and policy-based authorization
- Structured logging with Serilog + Seq / ELK
- CI/CD pipeline
