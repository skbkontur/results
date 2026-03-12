# Conversion

Methods for transforming generic arguments of `Optional` and `Result`.

All conversion methods except `Upcast` support async extensions for every listed synchronous scenario.
All synchronous methods support upcasts of `TFault` and `TValue`.

Async extensions make use of methods returning `Task<T>`, `ValueTask<T>` and inline async lambdas. For example:
```csharp
async Task<int> Get1(int number) => number + 1;
async ValueTask<int> Get2(int number) => number - 1;

Task<Optional<int>> Get(Task<Optional<int>> optional)
{
  return optional
    .MapValue(i => Get1(i))
    .MapValue(i => Get2(i))
    .MapValue(async i => await Get1(i));
}
```

If there is at least one async method in a chain returning `Task<T>` the result is `Task<T2>`. Otherwise, the result is `ValueTask<T2>`.

## MapValue

`MapValue` changes value or/and type of `TValue`.

```csharp
Optional<string> optional = ...;

Optional<string> extracted = optional.MapValue(str => str + "suffix");
Optional<string> extracted = optional.MapValue(() => "new value");
Optional<string> extracted = optional.MapValue("other string");

Optional<int> extracted = optional.MapValue(str => int.Parse(str));
Optional<int> extracted = optional.MapValue(() => int.Parse("123"));
Optional<int> extracted = optional.MapValue(15);
```

<details>
  <summary>Async examples</summary>

```csharp
Optional<string> optional = ...;

ValueTask<Optional<string>> extracted = optional.MapValue(async str => await Task.FromResult(str));
ValueTask<Optional<string>> extracted = optional.MapValue(str => new ValueTask(str));
Task<Optional<string>> extracted = optional.MapValue(str => Task.FromResult(str));
```

```csharp
ValueTask<Optional<string>> optional = ...;

ValueTask<Optional<string>> extracted = optional.MapValue(str => str + "suffix");
ValueTask<Optional<string>> extracted = optional.MapValue(async str => await Task.FromResult(str));
ValueTask<Optional<string>> extracted = optional.MapValue(str => new ValueTask(str));
Task<Optional<string>> extracted = optional.MapValue(str => Task.FromResult(str));
```

```csharp
Task<Optional<string>> optional = ...;

Task<Optional<string>> extracted = optional.MapValue(str => str + "suffix");
Task<Optional<string>> extracted = optional.MapValue(async str => await Task.FromResult(str));
Task<Optional<string>> extracted = optional.MapValue(str => new ValueTask(str));
Task<Optional<string>> extracted = optional.MapValue(str => Task.FromResult(str));
```
</details>

```csharp
Result<Guid, string> result = ...;

Result<Guid, string> extracted = result.MapValue(str => str + "suffix");
Result<Guid, string> extracted = result.MapValue(() => "new value");
Result<Guid, string> extracted = result.MapValue("other string");

Result<Guid, int> extracted = result.MapValue(str => int.Parse(str));
Result<Guid, int> extracted = result.MapValue(() => int.Parse("123"));
Result<Guid, int> extracted = result.MapValue(15);
```

<details>
  <summary>Async examples</summary>

```csharp
Result<Guid, string> result = ...;

ValueTask<Result<Guid, string>> extracted = result.MapValue(async str => await Task.FromResult(str));
ValueTask<Result<Guid, string>> extracted = result.MapValue(str => new ValueTask(str));
Task<Result<Guid, string>> extracted = result.MapValue(str => Task.FromResult(str));
```

```csharp
ValueTask<Result<Guid, string>> result = ...;

ValueTask<Result<Guid, string>> extracted = result.MapValue(str => str + "suffix");
ValueTask<Result<Guid, string>> extracted = result.MapValue(async str => await Task.FromResult(str));
ValueTask<Result<Guid, string>> extracted = result.MapValue(str => new ValueTask(str));
Task<Result<Guid, string>> extracted = result.MapValue(str => Task.FromResult(str));
```

```csharp
Task<Result<Guid, string>> result = ...;

Task<Result<Guid, string>> extracted = result.MapValue(str => str + "suffix");
Task<Result<Guid, string>> extracted = result.MapValue(async str => await Task.FromResult(str));
Task<Result<Guid, string>> extracted = result.MapValue(str => new ValueTask(str));
Task<Result<Guid, string>> extracted = result.MapValue(str => Task.FromResult(str));
```
</details>

## MapFault

`MapFault` changes value or/and type of `TFault`.

```csharp
Result<string, Guid> result = ...;

Result<string, Guid> extracted = result.MapFault(str => str + "suffix");
Result<string, Guid> extracted = result.MapFault(() => "new fault");
Result<string, Guid> extracted = result.MapFault("other string");

Result<int, Guid> extracted = result.MapFault(str => int.Parse(str));
Result<int, Guid> extracted = result.MapFault(() => int.Parse("123"));
Result<int, Guid> extracted = result.MapFault(15);
```

<details>
  <summary>Async examples</summary>

```csharp
Result<string, Guid> result = ...;

ValueTask<Result<string, Guid>> extracted = result.MapFault(async str => await Task.FromResult(str));
ValueTask<Result<string, Guid>> extracted = result.MapFault(str => new ValueTask(str));
Task<Result<string, Guid>> extracted = result.MapFault(str => Task.FromResult(str));
```

```csharp
ValueTask<Result<string, Guid>> result = ...;

ValueTask<Result<string, Guid>> extracted = result.MapFault(str => str + "suffix");
ValueTask<Result<string, Guid>> extracted = result.MapFault(async str => await Task.FromResult(str));
ValueTask<Result<string, Guid>> extracted = result.MapFault(str => new ValueTask(str));
Task<Result<string, Guid>> extracted = result.MapFault(str => Task.FromResult(str));
```

```csharp
Task<Result<string, Guid>> result = ...;

Task<Result<string, Guid>> extracted = result.MapFault(str => str + "suffix");
Task<Result<string, Guid>> extracted = result.MapFault(async str => await Task.FromResult(str));
Task<Result<string, Guid>> extracted = result.MapFault(str => new ValueTask(str));
Task<Result<string, Guid>> extracted = result.MapFault(str => Task.FromResult(str));
```
</details>

```csharp
Result<string> result = ...;

Result<string> extracted = result.MapFault(str => str + "suffix");
Result<string> extracted = result.MapFault(() => "new fault");
Result<string> extracted = result.MapFault("other string");

Result<int> extracted = result.MapFault(str => int.Parse(str));
Result<int> extracted = result.MapFault(() => int.Parse("123"));
Result<int> extracted = result.MapFault(15);
```

<details>
  <summary>Async examples</summary>

```csharp
Result<string> result = ...;

ValueTask<Result<string>> extracted = result.MapFault(async str => await Task.FromResult(str));
ValueTask<Result<string>> extracted = result.MapFault(str => new ValueTask(str));
Task<Result<string>> extracted = result.MapFault(str => Task.FromResult(str));
```

```csharp
ValueTask<Result<string>> result = ...;

ValueTask<Result<string>> extracted = result.MapFault(str => str + "suffix");
ValueTask<Result<string>> extracted = result.MapFault(async str => await Task.FromResult(str));
ValueTask<Result<string>> extracted = result.MapFault(str => new ValueTask(str));
Task<Result<string>> extracted = result.MapFault(str => Task.FromResult(str));
```

```csharp
Task<Result<string>> result = ...;

Task<Result<string>> extracted = result.MapFault(str => str + "suffix");
Task<Result<string>> extracted = result.MapFault(async str => await Task.FromResult(str));
Task<Result<string>> extracted = result.MapFault(str => new ValueTask(str));
Task<Result<string>> extracted = result.MapFault(str => Task.FromResult(str));
```
</details>

## Upcast

```csharp
Optional<string> optional = ...;

// Compiles
Optional<object> objectOptional = optional.Upcast<object>();

// Does not compile
var doesNotCompile = objectOptional.Upcast<string>();
```

```csharp
Result<string> result = ...;

// Compiles
Result<object> objectResult = result.Upcast<object>();

// Does not compile
var doesNotCompile = objectResult.Upcast<string>();
```

```csharp
Result<string, string> result = ...;

// Compiles
Result<object, string> objectResult1 = result.Upcast<object, string>();
Result<string, object> objectResult2 = result.Upcast<string, string>();
Result<object, object> objectResult3 = result.Upcast<object, object>();

// Does not compile
var doesNotCompile = objectResult1.Upcast<string, string>();
var doesNotCompile = objectResult2.Upcast<string, string>();
var doesNotCompile = objectResult3.Upcast<string, string>();
```

Async extensions are not supported for `Upcast` methods.
