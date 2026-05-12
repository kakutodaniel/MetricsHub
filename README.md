### Getting Started
- Install Docker (Docker Desktop or Docker Engine) and ensure it is running
- Docker Compose (included with Docker Desktop in most setups)
- .NET SDK (only required if running the solution locally without Docker)

### How to Build and Run the Solution

This project is containerized using Docker Compose.

To run the solution:

```
docker-compose up -d
```

This will:

- Build all services
- Start API and dependencies

To stop the application:

```
docker-compose down
```

## Port Mappings & API Access

Once the services are running, the API can be accessed at:

API Base URL: http://localhost:8080
Swagger: http://localhost:8080/swagger

Example endpoints:
- Inside Presentation layer there is the .http file to make requests


## Design Decisions & Reasoning

The solution is structured using a Layered Architecture:

- API (Presentation layer): Handles HTTP requests and responses
- Application layer: Contains business logic and use cases
- Infrastructure layer: Handles external dependencies (DB, etc.)

Key decisions:

- Manual mapping was used instead of libraries like AutoMapper to improve readability and debugging transparency.
- Centralized exception handling was implemented using middleware for consistent error responses.
- Health checks and rate limiting were added to improve production readiness and observability.
- FluentValidation is used to ensure clear and consistent input validation rules.

## What I Would Improve with More Time / Production Context

If this were a production system, I would:

- As we are dealing with telemetry signals such as pulses and alerts, a large volume of data can arrive, so I would use a scalable, message-driven architecture to handle ingestion and background processing efficiently:

![alt text](<high level diagram.jpg>)

- Add caching layer (e.g. Redis) for performance (depends on the requirements)
- Implement retry + circuit breaker policies for external dependencies
- Add authentication/authorization (JWT or OAuth2)
- Add a rate limiter at the API Gateway level to protect the system from overload and ensure fair usage
- Create a reusable health check library that automatically exposes standardized health endpoints across services
    - Reduces duplication of boilerplate code
    - Ensures consistent behavior across all services
    - Speeds up onboarding of new services


### Known Limitations / Shortcuts
- Used Application DTOs directly in the controller for simplicity, avoiding additional mapping layers, but this approach also couples the API layer to serialization concerns
- No authentication/authorization implemented
- Simplified error handling response model
- Logging is basic and could be improved with structured logging correlation IDs
- No distributed tracing currently implemented
