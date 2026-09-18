# PortfolioPulse Development Status

## Project Overview

PortfolioPulse is an investment portfolio tracking application built with ASP.NET Core, Entity Framework Core, React, and TypeScript.

The backend provides APIs for managing investment accounts and holdings, with SQL Server used for persistence. The backend follows a layered architecture that separates HTTP concerns, application/business operations, and data access.

The frontend is a React web application that consumes the ASP.NET Core API.

## Current Checkpoint

The backend service layer and automated testing foundation are complete for the current Account and Holding functionality.

API/integration testing is complete for the current Accounts and Holdings API functionality. The integration tests exercise the application through the ASP.NET Core HTTP pipeline and verify important HTTP behaviors, validation behavior, response contracts, and business-rule responses.

API error handling has been standardized using ASP.NET Core ProblemDetails. Expected business and resource errors return structured ProblemDetails responses, including 400-level validation/business errors, 404 missing-resource errors, and 409 account-deletion conflicts. Integration tests verify the HTTP status, ProblemDetails content type, title, detail, and validation error structure where applicable.

The React web frontend has also been established and is now consuming real data from the ASP.NET Core API.

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
- 10 passing `HoldingsController` integration tests
- 43 passing tests in total
- React + TypeScript + Vite frontend
- React dashboard page
- Reusable dashboard components
- React navigation component
- Portfolio summary component
- Holdings table
- Frontend API service for holdings
- React state management for holdings
- API data loading with `useEffect`
- Frontend loading and error states
- CORS configuration for local React development
- Mapping from API holding data to frontend UI data

The next focus is continuing development of the React web application while keeping the existing API foundation stable and tested.

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
- HoldingsController integration tests
- Standardized API error responses using ProblemDetails
- Added integration-test coverage for ProblemDetails responses
- React + TypeScript + Vite frontend created
- Initial React application configured
- Dashboard page created
- Reusable dashboard components created
- Navigation component created
- Portfolio summary component created
- Holdings table created
- Responsive dashboard layout established
- Frontend API type for holdings created
- Frontend holdings service created
- React holdings state created
- React API data loading implemented with `useEffect`
- Frontend loading state implemented
- Frontend error state implemented
- Local development CORS configuration added
- API holding data mapped to frontend UI data

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

The frontend currently follows a simple component-based React structure:

```text
React Application
      |
      v
    App
      |
      v
DashboardPage
      |
      +----------------+
      |                |
      v                v
  AppHeader      PortfolioSummary
      |
      v
 Navigation
      |
      v
HoldingsSection
      |
      v
holdingService
      |
      v
ASP.NET Core API
```

The frontend uses TypeScript types to represent API data and maps API data into UI-specific data structures where appropriate.

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
    │   ├── AccountsControllerTests.cs
    │   └── HoldingsControllerTests.cs
    └── Infrastructure/
        └── CustomWebApplicationFactory.cs
```

The API/integration tests use ASP.NET Core's `WebApplicationFactory` to exercise the application through its HTTP pipeline.

SQLite is used as the database provider for the Testing environment. Each test creates its own `CustomWebApplicationFactory`, which provides an isolated in-memory SQLite database.

The normal application continues to use SQL Server.

The current API/integration test suite contains:

- 12 `AccountsController` tests
- 10 `HoldingsController` tests
- 22 API/integration tests total
- 43 tests across the entire project
- 43 tests passing
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

The Holdings API tests currently cover:

- `GET /api/holdings` returning holdings
- `GET /api/holdings/{id}` for an existing holding
- `GET /api/holdings/{id}` for a missing holding
- `POST /api/holdings` with valid data
- `POST /api/holdings` when the referenced account does not exist
- `POST /api/holdings` with invalid data
- `PUT /api/holdings/{id}` for an existing holding
- `PUT /api/holdings/{id}` for a missing holding
- `DELETE /api/holdings/{id}` for an existing holding
- `DELETE /api/holdings/{id}` for a missing holding

The integration tests verify HTTP status codes, response bodies, validation behavior, resource-location behavior for creation, and important account and holding business-rule responses.

## Integration Test Database Strategy

The application uses different database providers depending on the environment:

```text
Development / Production
        |
        v
    SQL Server

Service Tests
        |
        v
EF Core InMemory

API / Integration Tests
        |
        v
      SQLite
```

SQLite is used for API/integration tests because it provides relational database behavior while remaining lightweight and self-contained.

The service tests continue to use EF Core InMemory because those tests focus on service behavior and benefit from the simplicity and speed of the InMemory provider.

## Frontend

The frontend is located under:

```text
frontend/
├── public/
├── src/
│   ├── assets/
│   ├── components/
│   ├── pages/
│   │   └── DashboardPage.tsx
│   ├── services/
│   │   └── holdingService.ts
│   ├── types/
│   │   └── Holding.ts
│   ├── App.css
│   ├── App.tsx
│   ├── index.css
│   └── main.tsx
├── package.json
├── package-lock.json
└── vite.config.ts
```

The frontend currently uses:

- React
- TypeScript
- Vite
- CSS
- Browser `fetch` for API communication

The dashboard currently contains:

- Application header
- Navigation
- Page header
- Portfolio summary cards
- Holdings table

The holdings table currently retrieves real holding data from the ASP.NET Core API.

The frontend currently displays:

- Symbol
- Quantity
- Average Cost

Current market value is not yet displayed because market-price integration has not been implemented.

The frontend uses a dedicated service for API communication rather than placing HTTP requests directly inside presentation components.

The holdings service currently communicates with:

```text
GET /api/Holdings
```

The React component uses `useEffect` to load holdings when the component is initially rendered.

Loading and error states are currently represented using React state.

For local development, the ASP.NET Core API allows requests from the Vite development server origin.

## Next Milestone

Continue building the React web dashboard using the existing API foundation.

Potential areas to evaluate:

1. Improve the holdings presentation
2. Add account selection or account-aware holdings
3. Add portfolio-level calculations using available data
4. Improve loading and error UI
5. Add additional frontend API services as needed
6. Add frontend validation and interaction
7. Continue testing the frontend/API integration

## Upcoming Work

After the React dashboard foundation is established, likely next areas include:

- Additional business validation for holdings
- API documentation improvements
- Portfolio dashboard and analytics
- Portfolio-level calculations and summaries
- Current market-price integration
- Authentication and security foundation
- React Native + Expo mobile application
- Questrade / market-data integration
- Additional portfolio and reporting features
- Frontend integration improvements

## Frontend Direction

The frontend will begin with a React web application before moving to React Native.

The initial web frontend direction is:

- React
- TypeScript
- API integration with the ASP.NET Core backend
- Account screens
- Holding screens
- Portfolio dashboard
- Portfolio charts and analytics
- Responsive design

The web application will consume the API through DTO-based contracts rather than accessing database entities directly.

After the web application is established, React Native and Expo will be used to build a mobile client against the same ASP.NET Core API.

The intended frontend progression is:

```text
ASP.NET Core API
       |
       +---- React Web
       |
       +---- React Native / Expo
```

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
- React Native / Expo mobile client
- Additional portfolio and reporting features

## Development Principles

The project is being developed incrementally with an emphasis on maintainability and modern .NET and frontend practices.

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
- Keep API and frontend responsibilities separated
- Keep HTTP communication in frontend service modules
- Keep UI components focused on presentation and user interaction
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
- Backend test project
- `AccountServiceTests`
- `HoldingServiceTests`
- API/integration test infrastructure
- SQLite testing configuration
- `CustomWebApplicationFactory`
- `AccountsControllerTests`
- `HoldingsControllerTests`
- Standardized ProblemDetails error responses
- 43 passing tests
- React + TypeScript + Vite frontend
- React dashboard page
- Reusable dashboard components
- Holdings API service
- React API data loading
- Loading and error states
- Local development CORS configuration
- API-to-UI holding data mapping

The next Git checkpoint will capture the completed React holdings API integration milestone.

## Current Development Position

The backend has progressed from direct controller-to-`DbContext` access to a service-based architecture with automated tests around the service layer and HTTP integration tests.

The Account and Holding services have automated coverage for their current observable behavior.

Both the Accounts and Holdings APIs have integration-test coverage for their current HTTP behavior, including status codes, validation behavior, response contracts, resource-location behavior, and important business-rule responses.

The backend now has a solid testing foundation before moving into more complex portfolio functionality.

The React web application has been established and successfully connected to the existing Holdings API.

The current React dashboard can retrieve real holding data from the ASP.NET Core backend and display it with loading and error handling.

The next focus is to continue building the React web dashboard and gradually introduce portfolio calculations, richer UI behavior, and additional API integration.
