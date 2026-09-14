# PortfolioPulse Development Status

## Project Overview

PortfolioPulse is an investment portfolio tracking application built with ASP.NET Core and Entity Framework Core.

The backend provides APIs for managing investment accounts and holdings, with SQL Server used for persistence. The project follows a layered architecture that separates HTTP concerns, application/business operations, and data access.

## Current Checkpoint

The backend service layer and automated testing foundation are complete for the current Account and Holding functionality.

API/integration testing is now underway. The Accounts API integration tests are complete, covering the important HTTP behaviors for the current account endpoints.

The application currently has:

- Account CRUD API
- Holding CRUD API
- Account holdings endpoint
- DTOs and validation
- Reusable entity-to-DTO mappings
- EF Core projection expressions
- Account service layer
- Holding service layer
- Thin `AccountsController` using `IAccountService`
- Thin `HoldingsController` using `IHoldingService`
- Explicit service result handling for account deletion
- Dependency injection registrations for both application services
- Automated tests for `AccountService`
- Automated tests for `HoldingService`
- API/integration test infrastructure using `WebApplicationFactory`
- SQLite-based database for API/integration tests
- 12 passing `AccountsController` integration tests
- 32 passing tests in total

The next focus is API/integration testing for the Holdings API.

## Completed Milestones

- Project and solution setup
- ASP.NET Core Web API setup
- Entity Framework Core and SQL Server configuration
- Portfolio database context
- Account entity and account CRUD endpoints
- Holding entity and holding CRUD endpoints
- Account holdings endpoint
- Request and response DTOs
- Model validation
- Reusable entity-to-DTO mapping extensions
- EF Core projection expressions for query endpoints
- AccountService implementation
- AccountsController refactored to use AccountService
- Account deletion business result handling
- HoldingService implementation
- HoldingsController refactored to use HoldingService
- Dependency injection registration for AccountService and HoldingService
- Holding API behavior retested after service-layer refactoring
- Backend test project created
- EF Core InMemory test database configured
- AccountService automated tests
- HoldingService automated tests
- 20 service tests passing
- API/integration test infrastructure created
- SQLite configured for API/integration tests
- Custom `WebApplicationFactory` created for API/integration testing
- Per-test database isolation established for API integration tests
- AccountsController integration tests
- 32 tests passing

## Current Architecture

The application uses a layered backend structure:

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

Controllers remain focused on HTTP responsibilities:

- Receiving HTTP requests
- Using ASP.NET Core model validation
- Calling application services
- Translating service results into HTTP responses

Services are responsible for:

- Application and business operations
- Database access through `PortfolioPulseDbContext`
- Entity-to-DTO mapping
- Enforcing application-level business rules
- Returning explicit results for expected business outcomes where appropriate

The Account service is implemented through `IAccountService` and `AccountService`.

The Holding service is implemented through `IHoldingService` and `HoldingService`.

Account deletion uses an explicit `DeleteAccountResult` value to represent expected outcomes:

- `Deleted`
- `NotFound`
- `HasHoldings`

This allows the controller to return appropriate HTTP responses without embedding the account deletion business rule directly in the controller.

## Current API Capabilities

### Accounts

- Create an account
- Retrieve all accounts
- Retrieve an account by ID
- Update an account
- Delete an account when it has no holdings
- Return an appropriate result when an account does not exist
- Prevent deletion of an account that still has holdings

### Holdings

- Create a holding
- Retrieve all holdings
- Retrieve a holding by ID
- Update a holding
- Delete a holding
- Retrieve holdings associated with a specific account
- Validate that the referenced account exists when creating a holding

## Automated Testing

The project currently has automated tests at two levels:

1. Service-layer tests
2. API/integration tests

### Service Tests

Service tests are located under:

```text
tests/
└── PortfolioPulse.Api.Tests/
    └── Services/
        ├── AccountServiceTests.cs
        └── HoldingServiceTests.cs
```

The service-test suite contains:

- 11 `AccountService` tests
- 9 `HoldingService` tests
- 20 tests total
- 20 tests passing
- 0 failures

The tests use Entity Framework Core's InMemory provider with a unique database for each test to keep tests isolated.

The tests currently cover:

### AccountService

- Retrieving all accounts
- Creating accounts
- Retrieving existing accounts
- Handling missing accounts
- Updating existing accounts
- Handling updates for missing accounts
- Deleting accounts without holdings
- Handling deletion of missing accounts
- Preventing deletion of accounts with holdings
- Retrieving holdings for existing accounts
- Handling account-holdings requests for missing accounts

### HoldingService

- Retrieving all holdings
- Retrieving existing holdings
- Handling missing holdings
- Creating holdings for existing accounts
- Rejecting creation when the referenced account does not exist
- Updating existing holdings
- Handling updates for missing holdings
- Deleting existing holdings
- Handling deletion of missing holdings

The tests focus on observable service behavior rather than implementation details.

### API/Integration Tests

API/integration tests are located under:

```text
tests/
└── PortfolioPulse.Api.Tests/
    ├── Controllers/
    │   └── AccountsControllerTests.cs
    └── Infrastructure/
        └── CustomWebApplicationFactory.cs
```

The API/integration tests use ASP.NET Core's `WebApplicationFactory` to exercise the application through its HTTP pipeline.

SQLite is used as the database provider for the Testing environment. Each test creates its own `CustomWebApplicationFactory`, which provides an isolated in-memory SQLite database.

The normal application continues to use SQL Server.

The current API/integration test suite contains:

- 12 `AccountsController` tests
- 12 API/integration tests total
- 32 tests across the entire project
- 32 tests passing
- 0 failures

The Accounts API tests currently cover:

- `GET /api/accounts` returning accounts
- `GET /api/accounts/{id}` for an existing account
- `GET /api/accounts/{id}` for a missing account
- `POST /api/accounts` with valid data
- `POST /api/accounts` with invalid data
- `PUT /api/accounts/{id}` for an existing account
- `PUT /api/accounts/{id}` for a missing account
- `DELETE /api/accounts/{id}` for an account without holdings
- `DELETE /api/accounts/{id}` for a missing account
- `DELETE /api/accounts/{id}` when the account has holdings
- `GET /api/accounts/{id}/holdings` for an existing account
- `GET /api/accounts/{id}/holdings` for a missing account

The integration tests verify HTTP status codes, response bodies, validation behavior, resource-location behavior for creation, and important account business-rule responses.

## Integration Test Database Strategy

The application uses different database providers depending on the environment:

```text
Development / Production
        |
        v
    SQL Server

Testing
        |
        v
      SQLite
```

SQLite is used for API/integration tests because it provides relational database behavior while remaining lightweight and self-contained.

The service tests continue to use EF Core InMemory because those tests focus on service behavior and benefit from the simplicity and speed of the InMemory provider.

## Next Milestone

Continue API/integration testing for the Holdings API.

Planned sequence:

1. Add tests for important `HoldingsController` behaviors
2. Verify HTTP status codes and response bodies
3. Verify model-validation responses
4. Verify behavior when creating a holding for a nonexistent account
5. Run the complete test suite
6. Review the integration-test coverage
7. Commit the completed API/integration testing milestone

## Upcoming Work

After API/integration testing is established, likely next areas include:

- Consistent error-handling conventions across endpoints
- ProblemDetails-based API error responses
- Additional business validation for holdings
- Portfolio-level calculations and summaries
- API documentation improvements
- Authentication and security foundation
- Frontend integration

## Frontend Direction

The frontend is planned after the backend architecture and testing foundation are sufficiently stable.

The initial frontend direction is:

- React Native
- Expo
- API integration with the ASP.NET Core backend
- Account screens
- Holding screens
- Portfolio dashboard
- Portfolio charts and analytics

The frontend should consume the API through DTO-based contracts rather than accessing database entities directly.

## Future Direction

Longer-term functionality may include:

- Questrade account integration
- Market data integration
- Current portfolio valuation
- Performance calculations
- Portfolio analytics
- Investment allocation analysis
- Authentication and authorization
- React web client
- Additional portfolio and reporting features

## Development Principles

The project is being developed incrementally with an emphasis on maintainability and modern .NET practices.

Key principles include:

- Keep controllers thin
- Separate API DTOs from EF Core entities
- Keep application/business operations in services
- Use dependency injection
- Use asynchronous database operations
- Use reusable entity-to-DTO mappings
- Use EF Core projection expressions for query endpoints
- Validate API input
- Avoid unnecessary entity loading
- Represent expected business outcomes explicitly where appropriate
- Add automated tests before the application grows significantly
- Use appropriate test-database strategies for different test levels
- Keep architecture and development documentation updated when meaningful design changes occur
- Prefer simple architecture over premature abstraction
- Make meaningful Git commits at architectural milestones

## Latest Git Checkpoint

The latest completed development milestone includes:

- `IAccountService` and `AccountService`
- `DeleteAccountResult`
- `IHoldingService` and `HoldingService`
- `AccountsController` refactoring
- `HoldingsController` refactoring
- Dependency injection registrations for both services
- Account deletion business result handling
- Architecture and development-status documentation updates
- Backend test project
- `AccountServiceTests`
- `HoldingServiceTests`
- API/integration test infrastructure
- SQLite testing configuration
- `CustomWebApplicationFactory`
- `AccountsControllerTests`
- 32 passing tests

The next Git checkpoint should capture the completed Accounts API integration-testing milestone and the corresponding development-status documentation update.

## Current Development Position

The backend has progressed from direct controller-to-`DbContext` access to a service-based architecture with automated tests around the service layer and HTTP integration tests.

The Account and Holding services have automated coverage for their current observable behavior.

The Accounts API now also has integration-test coverage for its current HTTP behavior, including status codes, validation behavior, response contracts, resource-location behavior, and account deletion business rules.

The next focus is API/integration testing for the Holdings API.
