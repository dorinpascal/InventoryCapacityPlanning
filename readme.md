# Inventory Capacity Planning

This project is a **Inventory Capacity Planning API** built with **.NET 8**. The API is designed to manage and monitor inventory across multiple distribution centers, track stock transport orders, and handle sales orders.

## Project Overview

The Inventory Capacity Planning API provides endpoints to manage inventory and orders across regional and local distribution centers. Key functionalities include:

- **Creating and managing sales orders**
- **Tracking stock transport orders (STO)** between distribution centers
- **Updating stock levels** and generating goods receipts
- **Picking and handling orders** within regional distribution centers

## Technologies Used

- **.NET 8** - The main framework for building the API.
- **AutoMapper** - For mapping between data transfer objects (DTOs) and domain models.
- **Serilog** - For structured logging, providing readable and structured output for easier debugging and monitoring.
- **xUnit** - For unit testing the application’s components.
- **Moq** - For creating mock objects to simulate dependencies during testing.
- **AutoFixture** - For auto-generating test data to simplify unit tests.

## Prerequisites

- **.NET 8 SDK** - Ensure you have the latest .NET 8 SDK installed
- **Visual Studio** or **VS Code** - Recommended for local development

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/dorinpascal/InventoryCapacityPlanning
```

### 2. Start the project
```bash
cd lego.inventory.capacity.planning
dotnet build
dotnet run
```