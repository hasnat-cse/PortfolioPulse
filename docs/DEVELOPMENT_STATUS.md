# PortfolioPulse Development Status

## Project Overview

PortfolioPulse is an investment portfolio tracking application built with ASP.NET Core and Entity Framework Core.

The backend provides APIs for managing investment accounts and holdings, with SQL Server used for persistence. The project follows a layered architecture that separates HTTP concerns, application/business operations, and data access.

## Current Checkpoint

The backend service layer and automated service tests are complete for the current Account and Holding functionality.

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
- Automated unit tests for `AccountService`
- Automated unit tests for `HoldingService`
- 20 passing service tests

The next milestone is API/integration testing for important HTTP behaviors.

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

The service layer has automated unit-test coverage for the current Account and Holding functionality.

Tests are located under:

```text
tests/
└── PortfolioPulse.Api.Tests/
    └── Services/
        ├── AccountServiceTests.cs
        └── HoldingServiceTests.cs
```

The current test suite contains:

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

## Next Milestone

Add API/integration tests for important HTTP behaviors.

Planned sequence:

1. Choose the appropriate integration-test approach
2. Add tests for important Accounts API behaviors
3. Add tests for important Holdings API behaviors
4. Verify HTTP status codes and response bodies
5. Verify model-validation responses
6. Verify important business-rule responses such as account deletion conflicts
7. Run the complete test suite
8. Commit the API/integration testing milestone

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
- 20 passing service tests

The next Git checkpoint should capture the completed automated service-testing milestone and documentation updates.

## Current Development Position

The backend has progressed from direct controller-to-`DbContext` access to a service-based architecture with automated tests around the service layer.

The Account and Holding services now have automated coverage for their current observable behavior.

The next focus is API/integration testing to verify the application's HTTP layer, including status codes, validation behavior, response contracts, and important business-rule responses.
