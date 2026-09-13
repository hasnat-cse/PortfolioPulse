# PortfolioPulse Architecture

## Overview

PortfolioPulse is a personal investment portfolio analytics application.

The long-term goal is to provide a cross-platform application that can connect to brokerage data, display investment holdings, and provide portfolio analytics, charts, and insights.

The backend is being developed first as an ASP.NET Core Web API.

## Technology Stack

### Backend

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- REST API
- OpenAPI

### Planned Frontend

- React Native
- Expo
- React
- Cross-platform mobile application

A React web client may be added later.

## Current Architecture

The application is being developed using a layered approach:

```text
Client
  |
  v
Controller
  |
  v
Service
  |
  v
DbContext
  |
  v
SQL Server
```

The Service layer is the next architectural milestone.

Controllers should primarily handle HTTP concerns, while application and business logic should gradually move into Services.

## Project Structure

```text
PortfolioPulse/
├── PortfolioPulse.sln
├── src/
│   └── PortfolioPulse.Api/
│       ├── Controllers/
│       ├── Data/
│       ├── DTOs/
│       ├── Models/
│       └── ...
├── tests/
└── docs/
```

## Database

The application currently uses SQL Server with the database:

```text
PortfolioPulse
```

Entity Framework Core is used for database access and migrations.

## Current Entities

### Account

An investment account belongs to a brokerage and contains zero or more holdings.

Current properties:

- Id
- Name
- Brokerage
- AccountType
- Currency
- Holdings

### Holding

A holding represents an investment position within an account.

Current properties:

- Id
- AccountId
- Symbol
- Quantity
- AverageCost
- Currency

`Holding.AccountId` is the foreign key to `Account`.

Current portfolio market values and prices are intentionally not stored in the database because they will eventually depend on market data.

## DTO Architecture

API requests and responses use DTOs rather than exposing EF Core entities directly.

DTOs use C# record types.

Current DTOs include:

- AccountDto
- CreateAccountRequest
- UpdateAccountRequest
- HoldingDto
- CreateHoldingRequest
- UpdateHoldingRequest

This keeps the API contract separate from the database entity model.

## Entity-to-DTO Mapping

Mapping logic is kept separate from DTO definitions.

Each mapping class contains:

1. `ToDto()` for entities that are already loaded into memory.
2. `ToDtoExpression` for EF Core database queries.

Example:

```csharp
.Select(AccountMappings.ToDtoExpression)
```

The expression allows EF Core to translate the projection into SQL and retrieve only the fields required by the DTO.

The same pattern is used for Holdings.

## Validation

Request DTOs use DataAnnotations for basic API validation.

Examples:

- Required strings
- String length restrictions
- Numeric ranges
- Three-character currency codes
- Positive Account IDs

Create requests may provide sensible defaults where appropriate.

For example:

```text
Currency = "CAD"
```

Update requests require the currency explicitly.

## Current API Endpoints

### Accounts

```text
GET    /api/accounts
GET    /api/accounts/{id}
POST   /api/accounts
PUT    /api/accounts/{id}
DELETE /api/accounts/{id}

GET    /api/accounts/{id}/holdings
```

### Holdings

```text
GET    /api/holdings
GET    /api/holdings/{id}
POST   /api/holdings
PUT    /api/holdings/{id}
DELETE /api/holdings/{id}
```

## Account Deletion Rule

An account cannot be deleted while it contains holdings.

The API returns:

```text
409 Conflict
```

when deletion is attempted for an account that still has holdings.

This prevents accidental deletion of associated investment data.

## Controller Responsibilities

Controllers currently handle:

- HTTP routing
- HTTP status codes
- Request/response handling
- Basic orchestration

GET operations use DTO projection expressions.

Write operations load or create EF Core entities as necessary.

The next architectural improvement is to move application logic out of controllers and into Services.

## Testing

API endpoints are currently tested manually using Postman.

Validation scenarios have been tested for Accounts and Holdings.

Build verification is performed with:

```bash
dotnet build
```

Automated backend tests are planned as a future milestone.

## Architectural Direction

The intended direction is:

```text
Controllers
    ↓
Services
    ↓
EF Core / DbContext
    ↓
SQL Server
```

DTOs remain the API contract.

Mapping remains separate from DTO definitions.

EF Core projection expressions are reused for database-to-DTO queries.

The architecture should remain simple and avoid introducing unnecessary abstractions until they provide a clear benefit.
