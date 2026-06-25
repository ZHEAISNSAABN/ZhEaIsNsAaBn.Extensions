# ZhEaIsNsAaBn.Utilities.LayerResponse

`ZhEaIsNsAaBn.Utilities.LayerResponse` provides a small response model for application/service layers.
It standardizes:

- success/failure state
- a typed result payload
- rich error metadata
- a lightweight severity value object

## Package Target

- Target framework: `net10.0`

## Core Types

### `LayerResponse`
Non-generic operation result:

- `IsSuccess` / `IsFailure`
- `Errors`
- `Success()` and `Failure(...)` factory methods
- implicit conversion from `Error` and `List<Error>`

### `LayerResponse<TResult>`
Generic operation result with payload:

- `Result`
- `IsFailureOrEmpty`
- `Success(result)` and `Failure(...)` factory methods
- implicit conversion from `TResult`, `Error`, `List<Error>`, and `Exception`

### `Error`
Error contract with:

- `Code`
- `Type`
- `MessageAR`
- `Message`
- `Severity`
- optional `Exception`
- optional `AdditionalData`

Utility values:

- `Error.None` sentinel
- `Error.Unknown(exception, additionalData)` helper

### `Severity`
Severity value object with predefined values:

- `Severity.Low`
- `Severity.Medium`
- `Severity.High`
- `Severity.Unknown`

JSON behavior:

- serialized as a string (`Severity.Value`)
- deserialized from a string into `Severity`

## Basic Usage

```csharp
using ZhEaIsNsAaBn.Utilities;

LayerResponse ok = LayerResponse.Success();
LayerResponse fail = CommonErrors.NotFound;

LayerResponse<int> number = 42;
LayerResponse<int> failedNumber = Error.Unknown(new Exception("Boom"));
```

## JSON Example

```csharp
using System.Text.Json;
using ZhEaIsNsAaBn.Utilities;

var error = new Error("E-001", "خطأ", "Validation failed", Severity.High);
var json = JsonSerializer.Serialize(error);
```

`Severity` is emitted as a JSON string value, not an object.

## Notes

- XML documentation is enabled in the project file and public APIs are documented in code comments.
- This package is intended to be consumed by other libraries/services in the same solution.

