# PortfolioPulse Development Status

## Current Checkpoint

**Status:** Backend foundation complete; Service layer is next.

**Last completed milestone:**

- Reusable DTO projection expressions
- AccountController refactored to use projection expressions
- HoldingsController refactored to use projection expressions
- API endpoints tested successfully
- Changes committed and pushed

**Last Git checkpoint:**

```text
Add reusable DTO projection expressions
```

## Completed Milestones

### Project Setup

- ASP.NET Core Web API created
- Git repository initialized
- `.gitignore` configured
- SQL Server installed and configured
- PortfolioPulse database created

### Entity Framework Core

- EF Core configured
- SQL Server connection configured
- Initial migrations created
- Account entity created
- Holding entity created
- Holding decimal precision configured

### Account API

- GET all accounts
- GET account by ID
- Create account
- Update account
- Delete account
- Get holdings belonging to an account
- Prevent deletion of accounts containing holdings

### Holding API

- GET all holdings
- GET holding by ID
- Create holding
- Update holding
- Delete holding
- Validate that the referenced account exists

### DTO Layer

- AccountDto
- CreateAccountRequest
- UpdateAccountRequest
- HoldingDto
- CreateHoldingRequest
- UpdateHoldingRequest

DTOs use C# record types.

### Validation

Account validation:

- Required Name
- Required Brokerage
- Required AccountType
- Maximum string lengths
- Three-character currency validation
- Create requests default Currency to CAD
- Update requests require Currency

Holding validation:

- Symbol length validation
- Positive Quantity
- Non-negative AverageCost
- Three-character currency validation
- Create requests default Currency to CAD
- AccountId must be at least 1
- Referenced Account must exist

### Mapping

Separate mapping classes are used:

- `AccountMappings`
- `HoldingMappings`

Each provides:

- `ToDto()` for in-memory entities
- `ToDtoExpression` for EF Core query projection

EF Core GET queries use the reusable projection expressions.

### Testing

- API endpoints tested manually with Postman
- Validation scenarios tested
- Build verified with `dotnet build`

## Next Milestone

### Service Layer

Introduce application services to move application/business logic out of controllers.

Planned structure:

```text
Controller
    ↓
Service
    ↓
DbContext
    ↓
SQL Server
```

First service:

```text
AccountService
```

Then:

```text
HoldingService
```

## Planned Backend Work

1. Create AccountService
2. Move Account operations into AccountService
3. Refactor AccountsController
4. Test AccountService-backed endpoints
5. Create HoldingService
6. Move Holding operations into HoldingService
7. Refactor HoldingsController
8. Add automated backend tests
9. Improve error handling
10. Add authentication/security foundation

## Planned Frontend

After the backend foundation and service layer are sufficiently stable:

- React Native
- Expo
- Mobile navigation
- Account screens
- Holdings screens
- Portfolio dashboard
- Charts and analytics

Backend and frontend development will eventually proceed in parallel.

## Long-Term Features

- Questrade integration
- Market data
- Portfolio valuation
- Performance calculations
- Charts
- Portfolio analytics
- Authentication
- Potential React web client

## Development Principles

- Work incrementally
- Prefer simple architecture
- Avoid unnecessary abstractions
- Keep controllers thin
- Keep DTOs separate from entities
- Keep mapping separate from DTO definitions
- Reuse EF Core projection expressions
- Test each meaningful change
- Commit at meaningful checkpoints
- Document important architectural decisions
