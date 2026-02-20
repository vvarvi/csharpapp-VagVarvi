# CSharpApp

Production-ready .NET 10 Web API implementing Clean Architecture, CQRS, functional error handling, resilient external API integration, JWT lifecycle management, observability, testing, and Docker support.

## Overview

CSharpApp is a versioned REST API built with .NET 10 that integrates with an external REST service. The project demonstrates production-grade backend engineering practices including:
•	Clean Architecture
•	CQRS with MediatR
•	Functional error handling (Result)
•	Typed HttpClient
•	JWT authentication (access + refresh flow)
•	Polly retry policies
•	Structured logging (Serilog)
•	Health checks (liveness & readiness)
•	Docker multi-stage build
•	Unit & Integration testing

## Architecture

The solution follows Clean Architecture principles with strict separation of concerns.
Project Structure
CSharpApp.Api            → Presentation (Minimal API)
CSharpApp.Application    → Use cases (CQRS, handlers, interfaces)
CSharpApp.Infrastructure → External integrations, HttpClient, Auth
CSharpApp.Core           → Domain entities & shared abstractions
CSharpApp.Tests          → Unit & Integration tests

Flow

HTTP Request
   ↓
Minimal API Endpoint
   ↓
MediatR Query
   ↓
Handler
   ↓
Service (Typed HttpClient)
   ↓
External API

## Authentication Flow

The external API requires JWT authentication.
Implemented lifecycle:

1.	Login → retrieve access_token & refresh_token
2.	Parse JWT exp claim
3.	Apply expiration buffer
4.	Automatic refresh when expired
5.	Token injection via DelegatingHandler

Features:

•	Thread-safe token refresh
•	No token exposure in logs
•	Automatic handling per request

## Resilience Strategy

Polly is used for resilience:
•	Configurable retry count
•	Configurable backoff
•	Applied to outbound HTTP calls
HTTP pipeline order:
LoggingHandler
   ↓
AuthDelegatingHandler
   ↓
Polly Retry Policy
   ↓
External API

 
## Error Handling

The project avoids exception-driven flow for business logic.
Instead, it uses a functional Result pattern:

•	Success → returns value
•	Failure → returns Error (Code, Message, StatusCode)

Endpoints map Result to proper HTTP responses using ProblemDetails.

This ensures:

•	Predictable behavior
•	Clear control flow
•	Easier testing

## Observability

Structured logging with Serilog:

•	Console logging
•	File logging
•	Slow request threshold (configurable)
•	Outbound HTTP logging

Performance settings are strongly typed via configuration.

## Health Checks

Endpoints:

GET /health/live
GET /health/ready

Readiness check validates external API connectivity.
Docker HEALTHCHECK is configured for container orchestration readiness.

## Testing

Unit Tests
•	Handlers
•	TokenProvider
•	Middleware

Integration Tests
•	CustomWebApplicationFactory
•	Fake service overrides
•	Full pipeline verification

Docker Support
Multi-stage Docker build is implemented.
Build
docker build -t csharpapp .
Run
docker run -p 8080:8080 csharpapp
API will be available at:
http://localhost:8080

## Configuration

Strongly-typed configuration objects:
•	RestApiSettings
•	HttpClientSettings
•	JwtOptions
•	PerformanceSettings

Configured via appsettings.json and injected using IOptions.

## API Endpoints

Products

/api/v1/products

GET getAll
GET getOne
POST create

Categories 

/api/v1/categories

GET getAll
GET getOne
POST create

Health
GET /health/live
GET /health/ready

## Production-Ready Features

•	API Versioning
•	Clean Architecture
•	CQRS
•	Functional error handling
•	JWT auto-refresh
•	Resilient HttpClient
•	Structured logging
•	Health checks
•	Docker support
•	Unit & Integration testing

## Future Improvements Suggestions

•	Circuit breaker policy
•	OpenTelemetry distributed tracing
•	Caching layer
•	Rate limiting
•	Metrics endpoint (Prometheus)
•	Kubernetes deployment manifests

## Author

Backend engineering assessment project demonstrating senior-level architecture, resilience, and clean design principles.
