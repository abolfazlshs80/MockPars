# MockPars

MockPars is a modular ASP.NET Core project designed to demonstrate a clean architecture approach. It consists of multiple layers, including Application, Domain, Infrastructure, and WebApi, to ensure separation of concerns and maintainability.

## Project Structure

The project is organized into the following layers:

1. **MockPars.Application**: Contains the application logic, including use cases and service interfaces.
2. **MockPars.Domain**: Contains the core domain entities and business rules.
3. **MockPars.Infrastructure**: Implements the infrastructure concerns, such as data access and external integrations.
4. **MockPars.WebApi**: Exposes the application functionality through RESTful APIs.

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or any other IDE that supports .NET development

## Getting Started

1. Clone the repository:
   ```powershell
   git clone https://github.com/abolfazlshs80/MockPars.git
   cd MockPars
   ```

2. Open the solution file `MockPars.sln` in Visual Studio or your preferred IDE.

3. Restore the NuGet packages:
   ```powershell
   dotnet restore
   ```

4. Build the solution:
   ```powershell
   dotnet build
   ```

5. Run the Web API project:
   ```powershell
   dotnet run --project MockPars.WebApi/MockPars.WebApi.csproj
   ```

6. Access the API in your browser or API client at `https://localhost:<port>`.

## Configuration

- The `appsettings.json` and `appsettings.Development.json` files in the `MockPars.WebApi` project contain configuration settings for the application.
- Update the connection strings and other settings as needed.

## Features

- Clean architecture with separation of concerns
- Modular project structure
- RESTful API implementation

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.