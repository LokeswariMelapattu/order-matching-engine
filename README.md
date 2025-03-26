# Order Matching Engine

## Project Overview
This project implements a simple **Order Matching Engine** in .NET 8, which processes buy and sell orders, matches them based on price, and updates their statuses accordingly.

## Project Structure
```
OrderMatchingEngine/
│── OrderMatchingEngine.sln      # Solution file
│
├── OrderMatchingEngine/         # Core application
│   ├── Models/                  # Domain models
│   │   ├── Order.cs             # Represents an order (Buy/Sell)
│   │   ├── Enums.cs             # Defines order types & statuses
│   ├── Services/                # Business logic
│   │   ├── MatchingEngine.cs    # Order matching logic
│   ├── Program.cs               # Main entry point (if console app)
│
├── OrderMatchingEngine.Tests/   # Test project
│   ├── MatchingEngineTests.cs   # Unit tests using xUnit & FluentAssertions
│   ├── OrderTests.cs            # Tests for order behavior
│
└── README.md                    # Documentation
```

## How to Run the Solution
This project is a **console application**. To execute it, follow these steps:

### 1. Clone the Repository
```sh
git clone https://github.com/your-repo/order-matching-engine.git
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
dotnet run --project OrderMatchingEngine
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
For any issues or contributions, feel free to submit a pull request!

