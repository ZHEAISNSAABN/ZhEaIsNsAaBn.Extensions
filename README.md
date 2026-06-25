# ZhEaIsNsAaBn.Extensions

A multi-library .NET workspace that groups reusable building blocks (extensions/utilities/infrastructure helpers) under one solution.

## Solution Structure

This repository is organized as a solution that can host multiple libraries.
Current top-level areas include:

- `src/UtilitiesPackages/` - actively maintained utility packages
- `OldCode/` - legacy/previous package implementations

### Current Utility Packages

- `src/UtilitiesPackages/ZhEaIsNsAaBn.Utilities.LayerResponse`  
  Shared layer-response primitives (`LayerResponse`, `LayerResponse<TResult>`, `Error`, `Severity`).

## Package Documentation

Each package can provide its own `README.md` next to its `.csproj`.

- `src/UtilitiesPackages/ZhEaIsNsAaBn.Utilities.LayerResponse/README.md`

## Getting Started

### Prerequisites

- .NET SDK (the utility package currently targets `net10.0`)

### Build a Specific Library

```bash
dotnet build "D:\Workspaces\ZhEaIsNsAaBn.New\ZhEaIsNsAaBn.Extensions\src\UtilitiesPackages\ZhEaIsNsAaBn.Utilities.LayerResponse\ZhEaIsNsAaBn.Utilities.LayerResponse.csproj"
```

### Build the Full Solution

If needed, build from the solution file at the repository root:

```bash
dotnet build "D:\Workspaces\ZhEaIsNsAaBn.New\ZhEaIsNsAaBn.Extensions\ZhEaIsNsAaBn.Extensions.slnx"
```

## Development Notes

- Libraries are designed for reuse across applications/services.
- XML documentation is enabled for package APIs.
- NuGet packaging metadata is configured in each project file.

## Contributing

1. Add or update code in the relevant package folder.
2. Keep XML docs and package README docs aligned with API changes.
3. Build the modified package (and solution when required) before publishing.
