# Order Matching Engine

## Project Overview
This project implements a simple **Order Matching Engine** in .NET 8, which processes buy and sell orders, matches them based on price, and updates their statuses accordingly.

## Project Structure
```
order-matching-engine/
│── OrderMatchingEngine.sln                  # Solution file
│
├── OrderMatchingEngine.ConsoleApp/         # Console application
│   ├── ExceptionHandler.cs                  # Global exception handler
│   ├── OrderMatchingApp.cs                  # Order Matching application service
│   ├── Program.cs                           # Main entry point
│
├── OrderMatchingEngine.Domain/              # Domain layer handling business logic and interactions with data access
│   ├── Enums/                               # Enums for Order Matching Service
│   │   ├── OrderType.cs                     # Enum for order types (Buy/Sell)
│   │   ├── OrderStatus.cs                   # Enum for order statuses (New, Filled, PartiallyFilled, Canceled)
│   ├── Order.cs                             # Entity representing order information
│   ├── Trade.cs                             # Entity representing trade information
│
├── OrderMatchingEngine.ApplicationLogic/    # Business logic layer managing order operations
│   ├── IOrderMatchingService.cs             # Interface for Order Matching Service
│   ├── OrderMatchingService.cs              # Service handling order placement, cancellation, active orders, and trade history
│
├── OrderMatchingEngine.DataAccess/          # Data access layer managing in-memory order and trade storage
│   ├── IOrderRepository.cs                  # Interface for Order repository
│   ├── OrderRepository.cs                   # Repository implementation
│
├── OrderMatchingEngine.Tests/               # Test project
│   ├── OrderMatchingServiceTests.cs         # Unit tests using xUnit & FluentAssertions
│   ├── OrderTests.cs                        # Tests for order behavior
│
└── README.md                                # Documentation
```

## How to Run the Solution
This project is a **console application**. To execute it, follow these steps:

### 1. Clone the Repository
```sh
git clone https://github.com/LokeswariMelapattu/order-matching-engine.git
cd order-matching-engine
```

### 2. Build the Project
Ensure you have **.NET 8 SDK** installed. Then run:
```sh
dotnet build
```

### 3. Run the Application
Execute the console app (if applicable):
```sh
dotnet run --project OrderMatchingEngine.ConsoleApp
```
- The application takes user input for placing buy/sell orders and processes matches accordingly.

## Running Tests
Unit tests are written using **xUnit** and **FluentAssertions**.

### Run Tests
```sh
dotnet test
```

### Expected Output
- If all tests pass, you’ll see a success message.
- If a test fails, detailed error messages will indicate what went wrong.

## Test Cases Covered
1. Placing a single order (no match).
2. Matching two opposing orders (fully or partially).
3. Status transitions (`New → PartiallyFilled → Filled`).
4. Canceling an order before it is fully matched.
5. Complex order sequences and proper matching behavior.

---
 

