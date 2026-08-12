# Documentation

Full documentation for `Optional<TValue>`, `Result<TFault>`, and `Result<TFault, TValue>`.


## Table of Contents

| Document | Description |
|---|---|
| [Instantiation](instantiation.md) | Creating `Optional` and `Result` instances (explicit, implicit, ternary, conversions) |
| [Extraction](extraction.md) | Getting data out: `TryGetValue`, `Match`, `GetValueOrElse` (with async), `GetValueOrThrow`, status properties, collections |
| [Side Effects](side-effects.md) | Performing actions: `Switch`, `OnSome`/`OnNone`, `OnSuccess`/`OnFailure`, `Ensure*`, chaining (with async) |
| [Conversion](conversion.md) | Transforming containers: `MapValue`, `MapFault` (with async), `Upcast` |
| [Combining](combining.md) | Monadic extensions: `Then`, `OrElse`, `Select`, do-notation (sync + async) |
| [LINQ Integration](linq-integration.md) | Query syntax with `IEnumerable` (opt-in recipes) |
| [Inheritance](inheritance.md) | Creating custom `Result` types by inheriting |


## Brief API Reference

One example per method. See linked pages for full overloads and details.


### Create ([full reference](instantiation.md))

```csharp
Optional<string> optional = "hello";                    // implicit
Optional<string> none = Optional.None();                 // explicit
Result<Exception, string> result = "success";            // implicit
Result<Exception> noFault = Result.Succeed();             // explicit
```


### Extract ([full reference](extraction.md))

```csharp
// TryGetValue — check and extract in one call
if (result.TryGetValue(out string value, out Exception fault))
    Console.WriteLine(value);

// Match — exhaustive handling, returns a value
string msg = optional.Match(onNone: () => "empty", onSome: v => $"got {v}");

// GetValueOrElse — extract with fallback (sync, Task, ValueTask)
string safe = optional.GetValueOrElse("default");
Task<string> asyncSafe = taskOptional.GetValueOrElse("default");

// GetValueOrThrow — extract or throw
string val = result.GetValueOrThrow(); // throws ResultFailedException<Exception> on Failure

// GetValueOrDefault — null/default(T) on missing
string? maybe = optional.GetValueOrDefault();

// GetValueOrNull — for value types only
int? nullable = optionalInt.GetValueOrNull();

// Status properties
bool hasSome = optional.HasSome;
bool isNone = optional.IsNone;
bool success = result.Success;
bool failure = result.Failure;

// Collections
IEnumerable<string> values = optional.GetValues();
IEnumerable<Exception> faults = result.GetFaults();
```


### Side Effects ([full reference](side-effects.md))

```csharp
// Switch — exhaustive side effects
optional.Switch(onNone: () => Log("missing"), onSome: v => Log($"got {v}"));

// OnSome / OnNone / OnSuccess / OnFailure — targeted side effects (chainable)
optional.OnSome(v => Log(v)).OnNone(() => Log("none"));

// Ensure — assert state or throw
optional.EnsureHasValue();           // throws ValueMissingException on None
result.EnsureSuccess();              // throws ResultFailedException<T> on Failure

// Async — all side effect methods support Task/ValueTask
Task<Optional<string>> asyncChained = taskOptional.OnSome(v => Log(v));
```


### Convert ([full reference](conversion.md))

```csharp
// MapValue — transform the value inside
Optional<int> length = optional.MapValue(str => str.Length);

// MapFault — transform the fault inside
Result<string, int> mapped = result.MapFault(ex => ex.Message);

// Upcast — widen generic parameters via covariance
Optional<object> wider = optional.Upcast<object>();

// Async — all conversion methods support Task/ValueTask
Task<Optional<int>> asyncResult = taskOptional.MapValue(str => str.Length);
```


### Combine ([full reference](combining.md))

Requires `Kontur.Results.Monad` package.

```csharp
// Then — continue on success (Bind)
Optional<Guid> user = GetLogin().Then(login => FindUser(login));

// OrElse — fallback on failure
Optional<Guid> user = FindUser(login).OrElse(() => CreateUser(login));

// Chaining
Task<Optional<long>> ticks = GetFormLogin()
  .Then(login => GetUser(login).OrElse(() => CreateUser(login)))
  .Then(userId => GetCreationDate(userId))
  .MapValue(date => date.Ticks);

// Do-notation — LINQ query syntax
Task<Result<Exception, ConvertResult>> result =
  from userId  in GetCurrentUserId()
  where EnsureUserIdIsCorrect(userId)
  from index   in GetCurrentIndex()
  from message in GetMessage(userId, index)
  select Convert(message, userId);
```


### LINQ with IEnumerable ([full reference](linq-integration.md))

Not built-in. The documentation provides opt-in `SelectMany` recipes for combining `Optional`/`Result` with `IEnumerable` in LINQ query syntax.
