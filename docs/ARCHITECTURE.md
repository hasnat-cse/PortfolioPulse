# PortfolioPulse Architecture

## Overview

PortfolioPulse is an investment portfolio tracking application built with ASP.NET Core, Entity Framework Core, and SQL Server.

The backend uses a layered architecture to keep HTTP handling, application/business logic, and database access clearly separated.

## High-Level Architecture

```text
Client
  |
  v
Controller
  |
  v
Application Service Interface
  |
  v
Application Service
  |
  v
PortfolioPulseDbContext
  |
  v
SQL Server
```

For account operations, this flow is implemented as:

```text
Client
  |
  v
AccountsController
  |
  v
IAccountService
  |
  v
AccountService
  |
  v
PortfolioPulseDbContext
  |
  v
SQL Server
```

## Layers and Responsibilities

### Controllers

Controllers are responsible for HTTP concerns only.

Their responsibilities include:

- Receiving HTTP requests
- Using ASP.NET Core model validation
- Calling application services
- Translating service results into HTTP responses
- Returning appropriate status codes and response DTOs

Controllers should not contain database queries, entity mapping logic, or application-level business rules.

### Service Layer

Application and business operations are handled by service classes rather than directly by controllers.

Services are responsible for:

- Application and business operations
- Database access through `PortfolioPulseDbContext`
- Entity-to-DTO mapping
- Enforcing application-level business rules
- Returning explicit results for expected business outcomes where appropriate

The Account service is implemented through `IAccountService` and `AccountService`.

The Holding service layer is the next planned service-layer milestone.

### Data Access

`PortfolioPulseDbContext` is the application’s Entity Framework Core database context.

It is responsible for:

- Mapping entities to database tables
- Managing database queries and persistence operations
- Providing access to account and holding data
- Applying configured entity relationships and constraints

SQL Server is used as the persistent data store.

## Domain Model

The application currently manages two primary entities:

### Account

An account represents an investment account or portfolio container.

Accounts support standard create, read, update, and delete operations.

### Holding

A holding represents an investment position associated with an account.

Holdings support standard create, read, update, and delete operations. The API also supports retrieving holdings for a specific account.

## DTOs and Validation

The API uses request and response DTOs rather than exposing entity classes directly.

DTOs provide:

- Clear API contracts
- Input validation
- Separation between persistence models and API models
- Controlled response shapes

ASP.NET Core model validation is used to reject invalid request data before application services perform operations.

## Query Projection and Mapping

Entity-to-DTO mappings are reusable and are used to keep response construction consistent.

Entity Framework Core projection expressions are used where appropriate so query endpoints can shape results efficiently before data is materialized.

This approach helps reduce duplication and keeps mapping logic out of controllers.

## Account Service

`IAccountService` defines the application operations available for account management.

`AccountService` contains the implementation of those operations, including:

- Creating accounts
- Retrieving accounts
- Retrieving an account by ID
- Updating accounts
- Deleting accounts
- Retrieving holdings associated with an account
- Applying account-related business rules

`AccountsController` delegates account operations to this service.

## Explicit Business Results

Expected business outcomes are represented by explicit service results where appropriate.

For example, account deletion returns `DeleteAccountResult` values
