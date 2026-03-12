# Cerbi.Governance.Schemas

Canonical governance schema models for the Cerbi logging governance ecosystem.

## Purpose

This library defines the shared data models used across all Cerbi signature packs and runtime validation components. It is **runtime-neutral** and **logger-agnostic**.

## Models

| Model | Namespace | Description |
|-------|-----------|-------------|
| `GovernedEvent` | `Events` | A structured log event before it is written to a logger |
| `GovernedEventBuilder` | `Events` | Utility to construct `GovernedEvent` instances |
| `GovernanceProfileDefinition` | `Profiles` | High-level governance rules (required/disallowed fields, severities) |
| `EventTemplateDefinition` | `Templates` | Defines the structure of an event type |
| `EventFieldDefinition` | `Fields` | Defines a single field within an event template |

## Usage

```csharp
using Cerbi.Governance.Schemas.Events;

var evt = GovernedEventBuilder.Create(
    eventName: "Security.FailedLogin",
    category: "Security",
    message: "Failed login attempt detected",
    properties: new Dictionary<string, object?>
    {
        ["userId"] = "user-123",
        ["ipAddress"] = "10.0.0.1"
    }
);
```

## Installation

```bash
dotnet add package Cerbi.Governance.Schemas
```

## Build

```bash
dotnet build
dotnet test
```

## Design Principles

- **No runtime validation** — schemas only, validation lives in `Cerbi.Governance.Runtime`
- **No logger dependencies** — events are plain objects, not log calls
- **Runtime-neutral** — targets `net8.0`, no platform-specific code
- **Logger-agnostic** — compatible with future Serilog, NLog, MEL adapters

## License

MIT
