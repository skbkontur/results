# Kontur.Results — Compose operations that can fail

[![NuGet](https://img.shields.io/nuget/v/Kontur.Results.svg)](https://www.nuget.org/packages/Kontur.Results/)

Three `Result` types for C# with first-class async support and LINQ do-notation:

* `Optional<TValue>` — value or nothing
* `Result<TFault>` — success or typed fault
* `Result<TFault, TValue>` — value or typed fault


## Why Kontur.Results?

**No third state, no exceptions, no surprises.** Each Result is always in exactly one of two states. There is no `null`, no `Bottom`, no hidden default. The compiler enforces exhaustive handling via `Match`.

**Do-notation via LINQ query syntax.** Chain multiple fallible operations using familiar `from`/`in`/`select` syntax — sync, `Task`, and `ValueTask` are interchangeable within a single expression. No limit on the number of clauses.

**Explicit, self-documenting API.** `GetValueOrThrow`, `GetValueOrDefault`, `GetValueOrElse` — method names say exactly what happens. No `.Value` properties with undefined behavior.

**Unrestricted type parameters.** `TValue` and `TFault` have zero constraints — any type works, including `null`. Use C# nullable reference types for null safety.

**Minimal surface.** Two NuGet packages, three types, extension methods. No framework, no dependencies, no configuration.


## Quick Start

Install the core package:
```
dotnet add package Kontur.Results
```

For monadic extensions (`Then`, `OrElse`, `Select`, do-notation):
```
dotnet add package Kontur.Results.Monad
```

### Create and extract values

```csharp
using Kontur.Results;

Result<int, string> result = "success!"; // implicit conversion

if (result.TryGetValue(out string value, out var faultCode))
{
   Console.WriteLine(value.ToString()); // OK. Value is not null here. The compiler allows this.
}

Console.WriteLine("Error code: " + faultCode);
Console.WriteLine(value.ToString()); // warning CS8602: Dereference of a possibly null reference.
```

### Convert Result-way to exception-way

```csharp
using Kontur.Results;

class DraftError
{
  public int Code { get; init; }
}

class DraftClient
{
   ...
   public Task<Result<DraftError, Draft>> CreateDraft()
   {
     ...
   }
}

Result<DraftError, Draft> createDraftResult = await new DraftClient().CreateDraft();
try
{
  Draft draft = createDraftResult.GetValueOrThrow();
  Console.WriteLine(draft.Id);
}
catch (ResultFailedException<DraftError> ex)
{
  Console.WriteLine("Error code: " + ex.Fault.Code);
}
```

### Chain operations with Then/OrElse

```csharp
abstract Task<Optional<string>> GetFormLogin();
abstract Optional<Guid> GetUser(LoginModel login);
abstract ValueTask<Optional<Guid>> CreateUser(LoginModel login);

Task<Optional<Guid>> userId =
   GetFormLogin()
  .MapValue(str => new LoginModel(str))
  .Then(login => GetUser(login).OrElse(() => CreateUser(login)))
```

### Do-notation — compose without nesting

Reduce cascading `if`/`await` chains to a flat, readable expression:

```csharp
abstract ValueTask<Result<Exception, Guid>> GetCurrentUserId();
abstract Result<Exception> EnsureUserIdIsCorrect(Guid userId);
abstract Task<int> GetCurrentIndex();
abstract Task<Result<Exception, string>> GetMessage(Guid userId, int index);
abstract Result<Exception, ConvertResult> Convert(string message, Guid userId);

Task<Result<Exception, ConvertResult>> result =
  from userId  in GetCurrentUserId()
  where EnsureUserIdIsCorrect(userId)
  from index   in GetCurrentIndex()
  let nextIndex = index + 1
  from message in GetMessage(userId, nextIndex)
  select Convert(message, userId);
```

Each clause can return sync, `Task<T>`, or `ValueTask<T>` — they compose transparently. The number of `from`/`where`/`let` clauses is unlimited.


### Parse and validate

```csharp
class NaturalNumber
{
  private readonly int value;
  private NaturalNumber(int value) => this.value = value;
  public static NaturalNumber operator +(NaturalNumber a, NaturalNumber b) => new NaturalNumber(a.value + b.value);

  public static bool TryParse(int value, [MaybeNullWhen(returnValue: false)] out NaturalNumber number)
  {
    if (value > 0)
    {
      number = new NaturalNumber(value);
      return true;
    }
    number = null;
    return false;
  }

  public static Optional<NaturalNumber> TryParse(int value) => TryParse(value, out var number) ? number : Optional.None();
  public static NaturalNumber Parse(int value) => TryParse(value).GetValueOrThrow();
}

Result<Exception, int> TryParseString(string input) => int.TryParse(input, out var number)
  ? number
  : new Exception(input + " is not an integer");

Result<Exception, NaturalNumber> result =
  from int1 in TryParseString(Console.ReadLine())
  from natural1 in NaturalNumber.TryParse(int1).OrElse(Result.Fail(new Exception(int1 + " is not positive")))
  from int2 in TryParseString(Console.ReadLine())
  from natural2 in NaturalNumber.TryParse(int2).OrElse(Result.Fail(new Exception(int2 + " is not positive")))
  select natural1 + natural2;
```

### Freeze fault type with inheritance

```csharp
class StringFaultResult<TValue> : Result<string, TValue>
{
  private readonly Result<string, TValue> result;

  public StringFaultResult(string fault) => result = fault;
  public StringFaultResult(TValue value) => result = value;

  public override TResult Match<TResult>(Func<string, TResult> onFailure, Func<TValue, TResult> onSuccess)
    => result.Match(onFailure, onSuccess);
}

public StringFaultResult<int> GenerateInt()
{
  int randomValue = new Random().Next(0, 10);
  if (randomValue > 0)
  {
    return new StringFaultResult<int>(randomValue);
  }

  return new StringFaultResult<int>("Failed to generate a positive number");
}
```


## API at a Glance

| Category | Methods | Docs |
|---|---|---|
| **Create** | `Optional.Some`, `Optional.None`, `Result.Succeed`, `Result.Fail`, implicit conversions | [Instantiation](docs/instantiation.md) |
| **Extract** | `TryGetValue`, `TryGetFault`, `Match`, `GetValueOrElse`, `GetValueOrThrow`, `GetValueOrDefault` | [Extraction](docs/extraction.md) |
| **Side effects** | `Switch`, `OnSuccess`, `OnFailure` (chainable) | [Side Effects](docs/side-effects.md) |
| **Assert** | `EnsureSuccess`, `EnsureFailure` | [Side Effects](docs/side-effects.md) |
| **Status** | `Success`, `Failure` | [Extraction](docs/extraction.md) |
| **Transform** | `MapValue`, `MapFault`, `Upcast` | [Conversion](docs/conversion.md) |
| **Combine** | `Then`, `OrElse`, `Select` (require `Kontur.Results.Monad`) | [Combining](docs/combining.md) |
| **Do-notation** | LINQ `from`/`where`/`let`/`select` (require `Kontur.Results.Monad`) | [Combining](docs/combining.md) |
| **Collections** | `GetValues`, `GetFaults`, `foreach` | [Extraction](docs/extraction.md) |

See [full documentation](docs/readme.md) for all overloads and detailed examples.


## Comparison with Alternatives

| Feature | Kontur.Results | OneOf | LanguageExt | FluentResults | CSharpFunctionalExtensions |
|---|---|---|---|---|---|
| Do-notation (LINQ query syntax) | Full (sync + Task + ValueTask, unlimited clauses) | No | Yes (sync only by default) | No | No |
| Async support | First-class (Task + ValueTask throughout) | No | Partial | No | Partial |
| Third state impossible | Yes (enforced by abstract class polymorphism) | Yes (tagged union) | Depends on type | No (`IsFailed`+`IsSuccess` can both be false) | Yes |
| Type constraints on TValue/TFault | None | None | None | string-based errors only | None |
| Null safety annotations | Yes (C# nullable reference types) | No | Partial | No | No |
| NuGet packages | 2 (core + monad) | 1 | 1 (large) | 1 | 1 |
| Dependencies | Zero | Zero | Zero | Zero | Zero |


## Design Decisions

* The implementation is if-less — it uses abstract classes polymorphism and VMT instead of `if`/ternary checks on a `Success` flag. As a result, there is no null-forgiving operator, no third state, and no accidental mishandling.
* `Result` types are not `struct` and not marked `readonly` (but they are implemented as readonly) — this is the cost of the polymorphism-based design.
* [Inheritance](docs/inheritance.md) allows freezing or limiting generic `TFault` and `TValue` parameters with user custom type arguments.


## Documentation

* [Full Documentation Index](docs/readme.md) — brief API reference with links to detailed pages
* [Instantiation](docs/instantiation.md) — creating Optional and Result instances
* [Extraction](docs/extraction.md) — TryGetValue, Match, GetValueOrElse/OrThrow/OrDefault, status properties
* [Side Effects](docs/side-effects.md) — Switch, OnSome/OnNone, OnSuccess/OnFailure, Ensure*
* [Conversion](docs/conversion.md) — MapValue, MapFault, Upcast (with async support)
* [Combining](docs/combining.md) — Then, OrElse, Select, do-notation with async
* [LINQ Integration](docs/linq-integration.md) — query syntax for IEnumerable
* [Inheritance](docs/inheritance.md) — custom Result types


## Roadmap

* Async extensions for data extraction methods (`TryGetValue`, `Switch`, `GetOrThrow`, etc.)
* Upcast support for synchronous monadic extensions (`Select`, `Then`, `OrElse`, do-notation)
* Do-notations (LINQ query syntax) for collections
* Source generator for async extensions and implicit conversions on custom inherited types
* Do-notation with heterogeneous `TFault` types (limited; currently use `MapFault`)


## Contributing

Contributions are appreciated.


## License

MIT


## Changelog

### 3.2.0 (2026-03-12)
- Added XML documentation to public API
- Improved NuGet package metadata
- Updated installation instructions

### 3.1.0 (2025-06-19)
- Cross-platform build support (Linux/macOS/Windows)

### 3.0.0 (2023-06-22)
- **Breaking:** Removed implicit cast from `bool` to `Result<TFault, bool>` to prevent confusion when returning `Result` with `bool` as `TValue`

### 2.0.0 (2022-05-23)
- Removed built-in `IEnumerable` LINQ query syntax integration as ambiguous (moved to documentation as opt-in recipes)
- Added data parsing example

### 1.0.0 (2022-03-08)
- Published to NuGet
- Exceptions now contain fault or value
- Inheritance support for freezing generic parameters
- Full do-notation with async (`Task` + `ValueTask`) support, unlimited clauses
- `Then`, `OrElse`, `Select` monadic extensions with async
- `MapValue`, `MapFault` with async extensions
- Comprehensive extraction API: `TryGetValue`, `TryGetFault`, `Match`, `Switch`, `GetValueOrElse`, `GetValueOrThrow`, `GetValueOrDefault`, `GetValueOrNull`, `Ensure*`
