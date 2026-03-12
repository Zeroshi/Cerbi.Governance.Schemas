# Cerbi.Governance.Schemas

[![NuGet](https://img.shields.io/nuget/v/Cerbi.Governance.Schemas)](https://www.nuget.org/packages/Cerbi.Governance.Schemas)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**Canonical shared contracts for Cerbi logging governance.** This package defines the core schema types that all Cerbi logger plugins and signature libraries use to represent governed log events, governance profiles, field definitions, and event templates.

## Why This Package?

Cerbi.Governance.Schemas provides the **single source of truth** for the data structures that flow through the entire Cerbi governance pipeline — from your application code, through logger plugins, into the CerbiShield Dashboard. By referencing this package, you ensure your governed events are structurally compatible with all Cerbi tooling.

## Installation

```shell
dotnet add package Cerbi.Governance.Schemas
```

## Core Types

### `GovernedEvent`

The fundamental unit of governed logging. Every governed log event — regardless of which logger produced it — is represented as a `GovernedEvent`.

| Property | Type | Description |
|---|---|---|
| `EventName` | `string` | Identifier for the event (e.g., `"FailedLogin"`, `"EntityCreated"`) |
| `Category` | `string` | Logical grouping (e.g., `"Security"`, `"Audit"`, `"Api"`) |
| `Message` | `string` | Human-readable description |
| `Properties` | `Dictionary<string, object?>` | Structured key-value payload validated against governance profiles |
| `GovernanceProfile` | `string?` | Name of the governance profile to validate against |
| `TemplateVersion` | `string?` | Semantic version of the event template |
| `GovernanceRelaxed` | `bool` | Whether strict governance enforcement is relaxed |
| `GovernanceViolations` | `List<string>?` | Violations detected during validation |

### `GovernedEventBuilder`

Static factory for constructing `GovernedEvent` instances with a clean API.

```csharp
using Cerbi.Governance.Schemas.Events;

// Basic creation
var evt = GovernedEventBuilder.Create(
    eventName: "UserLoggedIn",
    category: "Security",
    message: "User authentication succeeded",
    properties: new Dictionary<string, object?>
    {
        ["userId"] = "usr-123",
        ["tenantId"] = "tenant-abc",
        ["ipAddress"] = "10.0.0.1"
    });

// With governance profile and version
var governed = GovernedEventBuilder.Create(
    eventName: "PaymentProcessed",
    category: "Audit",
    message: "Payment completed",
    properties: new Dictionary<string, object?>
    {
        ["amount"] = 99.99,
        ["currency"] = "USD",
        ["actorId"] = "usr-456"
    },
    governanceProfile: "pci-dss-payment",
    templateVersion: "1.0.0");
```

### `GovernanceProfileDefinition`

Describes a governance profile — the set of rules that log events are validated against.

| Property | Type | Description |
|---|---|---|
| `ProfileName` | `string` | Unique name for the profile |
| `RequiredFields` | `List<string>` | Fields that must be present in every event |
| `DisallowedFields` | `List<string>` | Fields that must never appear (e.g., PII) |
| `FieldSeverities` | `Dictionary<string, string>` | Severity per field (`Info`, `Warn`, `Error`, `Forbidden`) |
| `EncryptionRules` | `Dictionary<string, string>` | Fields requiring encryption |
| `Version` | `string` | Semantic version of the profile |

### `EventFieldDefinition`

Metadata for a single field within a governed event.

| Property | Type | Description |
|---|---|---|
| `Name` | `string` | Field name (camelCase) |
| `Type` | `string` | Expected type (`String`, `Int`, `Decimal`, `Guid`, `DateTime`, `Bool`, `Object`, `Array`) |
| `Sensitive` | `bool` | Whether the field contains sensitive data |
| `Required` | `bool` | Whether the field is mandatory |

### `EventTemplateDefinition`

A reusable template for creating consistent governed events.

| Property | Type | Description |
|---|---|---|
| `EventName` | `string` | Template event name |
| `Category` | `string` | Template category |
| `ProfileName` | `string` | Associated governance profile |
| `DefaultMessage` | `string` | Default message template |
| `Version` | `string` | Semantic version |
| `RequiredFields` | `List<EventFieldDefinition>` | Fields that must be populated |
| `OptionalFields` | `List<EventFieldDefinition>` | Fields that may be populated |

## Supported Loggers

`GovernedEvent` is **logger-agnostic** by design. The `Properties` dictionary is the universal contract that all Cerbi logger governance plugins extract from structured log entries:

| Logger | Cerbi Plugin Package | How It Works |
|---|---|---|
| **CerbiStream** | [`Cerbi-CerbiStream`](https://github.com/Zeroshi/Cerbi-CerbiStream) | Native integration — `GovernedEvent` is the primary logging type |
| **Serilog** | [`Cerbi.Serilog.GovernanceAnalyzer`](https://github.com/Zeroshi/Cerbi.Serilog.GovernanceAnalyzer) | `SerilogEventAdapter.ToDictionary()` converts `LogEvent.Properties` → `Dictionary<string, object?>` |
| **Microsoft.Extensions.Logging** | [`Cerbi.MEL.Governance`](https://github.com/Zeroshi/Cerbi.MEL.Governance) | `CerbiGovernanceLogger.ExtractFields<TState>()` extracts `IEnumerable<KeyValuePair<string, object>>` → dictionary |
| **NLog** | [`Cerbi.NLog.GovernanceAnalyzer`](https://github.com/Zeroshi/Cerbi.NLog.GovernanceAnalyzer) | `GovernanceConfigLoader` maps NLog event properties to governance fields |

> **Java loggers** (Log4j2 via [`Cerbi.Java.Log4j2.Governance`](https://github.com/Zeroshi/Cerbi.Java.Log4j2.Governance), Logback via [`Cerbi.Java.Logback.Governance`](https://github.com/Zeroshi/Cerbi.Java.Logback.Governance)) use the same governance profile JSON format but have their own event types. `GovernedEvent` is .NET-specific.

## CerbiShield Dashboard Integration

Every `GovernedEvent` that passes through a Cerbi logger plugin automatically flows into the **CerbiShield Dashboard** for real-time governance monitoring:

```
┌─────────────┐    ┌────────────────────┐    ┌──────────────┐
│  Your App    │───▶│  Logger Plugin     │───▶│  Service Bus │
│ GovernedEvent│    │  Validate + Score  │    │    Queue     │
└─────────────┘    └────────────────────┘    └──────┬───────┘
                                                     │
                   ┌────────────────────┐    ┌───────▼───────┐
                   │  CerbiShield       │◀───│  Scoring      │
                   │  Dashboard         │    │  Aggregator   │
                   │                    │    │               │
                   │ • Governance Score │    │ • Trend Data  │
                   │ • Violation Charts │    │ • Breakdowns  │
                   │ • Compliance Views │    │ • Top Rules   │
                   │ • Audit History    │    │ • App Metrics │
                   └────────────────────┘    └───────────────┘
```

### What Appears on the Dashboard

| Dashboard Feature | Schema Source | Description |
|---|---|---|
| **Governance Score** | `GovernedEvent.Properties` validated against profile `RequiredFields`/`DisallowedFields` | 0–100 score based on field compliance |
| **Violation Breakdown** | `GovernedEvent.GovernanceViolations` | Violations by severity (Critical, Error, Warning) |
| **Compliance Trend** | Aggregated `GovernedEvent` history | Score trends over 7/14/30 day windows |
| **Top Violated Rules** | `GovernanceProfileDefinition.FieldSeverities` matches | Which rules are most frequently broken |
| **App Health** | Per-app `GovernedEvent` volume and score | Real-time governance health per application |

### Key Design Properties

- **Case-insensitive matching**: `GovernanceValidator.TryGetPropertyCI()` matches `Properties` keys regardless of casing
- **Logger-agnostic**: All plugins extract to `Dictionary<string, object?>` — same type as `GovernedEvent.Properties`
- **Zero coupling**: Schemas have no dependency on any logger framework
- **Transitive dependency**: Signature packages (`Cerbi.Signatures.*`) reference this package, which in turn becomes available to governance plugins

## Related Packages

| Package | Purpose |
|---|---|
| [`Cerbi.Signatures.Security`](https://github.com/Zeroshi/Cerbi.Signatures.Security) | Typed factories for security events (login failures, permission denied, token errors) |
| [`Cerbi.Signatures.Audit`](https://github.com/Zeroshi/Cerbi.Signatures.Audit) | Typed factories for audit events (entity CRUD operations) |
| [`Cerbi.Signatures.Api`](https://github.com/Zeroshi/Cerbi.Signatures.Api) | Typed factories for API lifecycle events (request start/complete/fail) |
| [`Cerbi.Governance.Core`](https://github.com/Zeroshi/Cerbi.Governance.Core) | Validation engine that scores `GovernedEvent` against profiles |

## License

MIT
