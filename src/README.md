# Alex75.AIAgents.Core

A .NET 10 F# library for AI Agents built with the Microsoft Agent Framework.

## Overview

This project provides core functionality for building AI agents using F# and the Microsoft Agent Framework.

## Getting Started

### Prerequisites

- .NET 10 SDK or later
- Visual Studio 2026 (or any IDE supporting F# and .NET)

### Building the Project

```bash
cd src
dotnet build Core/Core.fsproj
```

### Usage

```fsharp
open Alex75.AIAgents.Core

let greeting = Library.hello "World"
printfn "%s" greeting
```

## Package Management

This solution uses central package management via `Directory.Packages.props`. All package versions are managed centrally.

## License

See the LICENSE file in the root directory.
