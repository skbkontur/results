# Side Effects and Assertions

Methods for performing actions based on state without extracting the contained value.

## Content

* [Switch](#switch)
* [OnSome](#onsome)
* [OnNone](#onnone)
* [OnSuccess](#onsuccess)
* [OnFailure](#onfailure)
* [Method chaining](#method-chaining)
* [Return value upcasting](#return-value-upcasting)
* [EnsureHasValue](#ensurehasvalue)
* [EnsureNone](#ensurenone)
* [EnsureSuccess](#ensuresuccess)
* [EnsureFailure](#ensurefailure)


## Switch

`Switch`, `OnSome`, `OnNone`, `OnSuccess`, `OnFailure`, `EnsureHasValue`, `EnsureNone`, `EnsureSuccess` and `EnsureFailure` support async extensions for every listed synchronous scenario. Async extensions accept `Task`- or `ValueTask`-wrapped source and async callbacks. If there is at least one `Task` in a chain, the result is `Task`-wrapped. Otherwise, the result is `ValueTask`-wrapped.

```csharp
Optional<int> optional = ...;

optional.Switch(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: value => Console.WriteLine($"Value is {value}")
);

optional.Switch(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: () => Console.WriteLine("Value is present")
);
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Optional<int>> optional = ...;

Task<Optional<int>> chained = optional.Switch(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: value => Console.WriteLine($"Value is {value}")
);

Task<Optional<int>> chained = optional.Switch(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: () => Console.WriteLine("Value is present")
);
```

```csharp
ValueTask<Optional<int>> optional = ...;

ValueTask<Optional<int>> chained = optional.Switch(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: value => Console.WriteLine($"Value is {value}")
);

ValueTask<Optional<int>> chained = optional.Switch(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: () => Console.WriteLine("Value is present")
);
```
</details>

```csharp
Result<Exception, int> result = ...;

result.Switch(
  onFailure: fault => Console.WriteLine($"Fault is {fault.Message}"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

result.Switch(
  onFailure: fault => Console.WriteLine($"There is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"Value is present")
);

result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("Value is present")
);
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception, int>> result = ...;

Task<Result<Exception, int>> chained = result.Switch(
  onFailure: fault => Console.WriteLine($"Fault is {fault.Message}"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

Task<Result<Exception, int>> chained = result.Switch(
  onFailure: fault => Console.WriteLine($"There is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"Value is present")
);

Task<Result<Exception, int>> chained = result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

Task<Result<Exception, int>> chained = result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("Value is present")
);
```

```csharp
ValueTask<Result<Exception, int>> result = ...;

ValueTask<Result<Exception, int>> chained = result.Switch(
  onFailure: fault => Console.WriteLine($"Fault is {fault.Message}"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

ValueTask<Result<Exception, int>> chained = result.Switch(
  onFailure: fault => Console.WriteLine($"There is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"Value is present")
);

ValueTask<Result<Exception, int>> chained = result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

ValueTask<Result<Exception, int>> chained = result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("Value is present")
);
```
</details>

```csharp
Result<Exception> result = ...;

result.Switch(
  onFailure: fault => Console.WriteLine($"Fault is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"There is no fault")
);

result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("There is no fault")
);
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception>> result = ...;

Task<Result<Exception>> chained = result.Switch(
  onFailure: fault => Console.WriteLine($"Fault is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"There is no fault")
);

Task<Result<Exception>> chained = result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("There is no fault")
);
```

```csharp
ValueTask<Result<Exception>> result = ...;

ValueTask<Result<Exception>> chained = result.Switch(
  onFailure: fault => Console.WriteLine($"Fault is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"There is no fault")
);

ValueTask<Result<Exception>> chained = result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("There is no fault")
);
```
</details>

## OnSome
```csharp
Optional<int> optional = ...;

optional.OnSome(value => Console.WriteLine($"Value is {value}"));
optional.OnSome(() => Console.WriteLine("Value is present"));
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Optional<int>> optional = ...;

Task<Optional<int>> chained = optional.OnSome(value => Console.WriteLine($"Value is {value}"));
Task<Optional<int>> chained = optional.OnSome(() => Console.WriteLine("Value is present"));
```

```csharp
ValueTask<Optional<int>> optional = ...;

ValueTask<Optional<int>> chained = optional.OnSome(value => Console.WriteLine($"Value is {value}"));
ValueTask<Optional<int>> chained = optional.OnSome(() => Console.WriteLine("Value is present"));
```
</details>

## OnNone
```csharp
Optional<int> optional = ...;

optional.OnNone(() => Console.WriteLine("There is no value"));
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Optional<int>> optional = ...;

Task<Optional<int>> chained = optional.OnNone(() => Console.WriteLine("There is no value"));
```

```csharp
ValueTask<Optional<int>> optional = ...;

ValueTask<Optional<int>> chained = optional.OnNone(() => Console.WriteLine("There is no value"));
```
</details>

## OnSuccess
```csharp
Result<Exception, int> result = ...;

result.OnSuccess(value => Console.WriteLine($"Value is {value}"));
result.OnSuccess(() => Console.WriteLine("Success"));
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception, int>> result = ...;

Task<Result<Exception, int>> chained = result.OnSuccess(value => Console.WriteLine($"Value is {value}"));
Task<Result<Exception, int>> chained = result.OnSuccess(() => Console.WriteLine("Success"));
```

```csharp
ValueTask<Result<Exception, int>> result = ...;

ValueTask<Result<Exception, int>> chained = result.OnSuccess(value => Console.WriteLine($"Value is {value}"));
ValueTask<Result<Exception, int>> chained = result.OnSuccess(() => Console.WriteLine("Success"));
```
</details>

```csharp
Result<Exception> result = ...;

result.OnSuccess(() => Console.WriteLine("Success"));
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception>> result = ...;

Task<Result<Exception>> chained = result.OnSuccess(() => Console.WriteLine("Success"));
```

```csharp
ValueTask<Result<Exception>> result = ...;

ValueTask<Result<Exception>> chained = result.OnSuccess(() => Console.WriteLine("Success"));
```
</details>

## OnFailure
```csharp
Result<Exception, string> result = ...;

result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
result.OnFailure(() => Console.WriteLine("Failure"));
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception, string>> result = ...;

Task<Result<Exception, string>> chained = result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
Task<Result<Exception, string>> chained = result.OnFailure(() => Console.WriteLine("Failure"));
```

```csharp
ValueTask<Result<Exception, string>> result = ...;

ValueTask<Result<Exception, string>> chained = result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
ValueTask<Result<Exception, string>> chained = result.OnFailure(() => Console.WriteLine("Failure"));
```
</details>

```csharp
Result<Exception> result = ...;

result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
result.OnFailure(() => Console.WriteLine("Failure"));
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception>> result = ...;

Task<Result<Exception>> chained = result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
Task<Result<Exception>> chained = result.OnFailure(() => Console.WriteLine("Failure"));
```

```csharp
ValueTask<Result<Exception>> result = ...;

ValueTask<Result<Exception>> chained = result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
ValueTask<Result<Exception>> chained = result.OnFailure(() => Console.WriteLine("Failure"));
```
</details>

## Method chaining
```csharp
Optional<int> optional = ...;

string result = optional
  .Switch(
    onNone: () => Console.WriteLine("There is no value"),
    onSome: value => Console.WriteLine($"Value is {value}"))
  .OnSome(value => Console.WriteLine($"Value is {value}"))
  .OnNone(() => Console.WriteLine("There is no value"))
  .Match(onNoneValue: "valueOnNone", onSome: value => value.ToString());
```

`Result<TFault, TValue>` and `Result<TFault>` have similar syntax.

## Return value upcasting
```csharp
Optional<string> optional = ...;

Optional<object> upcasted = optional.Switch<object>(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: value => Console.WriteLine($"Value is {value}")
);
Optional<object> upcasted = optional.Switch<object>(
  onNone: () => Console.WriteLine("There is no value"),
  onSome: () => Console.WriteLine("Value is present")
);

Optional<object> upcasted = optional.OnSome<object>(value => Console.WriteLine($"Value is {value}"));
Optional<object> upcasted = optional.OnSome<object>(() => Console.WriteLine("Value is present"));

Optional<object> upcasted = optional.OnNone<object>(() => Console.WriteLine("There is no value"));
```

`Result<TFault, TValue>` and `Result<TFault>` have similar syntax.

## EnsureHasValue
```csharp
Optional<string> optional = ...;

// Nothing if `Some`
optional.EnsureHasValue(); // Throws `ValueMissingException` on None
optional.EnsureHasValue(new Exception("There is no value"));
optional.EnsureHasValue(() => new Exception("There is no value"))
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Optional<string>> optional = ...;

// Nothing if `Some`
Task ensured = optional.EnsureHasValue(); // Throws `ValueMissingException` on None
Task ensured = optional.EnsureHasValue(new Exception("There is no value"));
Task ensured = optional.EnsureHasValue(() => new Exception("There is no value"));
```

```csharp
ValueTask<Optional<string>> optional = ...;

// Nothing if `Some`
ValueTask ensured = optional.EnsureHasValue(); // Throws `ValueMissingException` on None
ValueTask ensured = optional.EnsureHasValue(new Exception("There is no value"));
ValueTask ensured = optional.EnsureHasValue(() => new Exception("There is no value"));
```
</details>

To override exception thrown by default exception for a specific `TValue` you can implement an extension method with the specific `TValue` in your namespace. You can also use [inheritance](inheritance.md) to override exception thrown by default for `TValue` in any namespace.
<details>
  <summary>Example</summary>

```csharp
namespace Custom
{
  class CustomValue
  {
  }
  static class EnsureHasValueExtensions
  {
    static void EnsureHasValue(this Optional<CustomValue> optional)
    {
      optional.EnsureHasValue(new Exception("Overridden!"));
    }
  }
}
```
</details>

## EnsureNone
```csharp
Optional<string> optional = ...;

// Nothing if `None`
optional.EnsureNone(); // Throws `ValueExistsException<string>` if `Some`
optional.EnsureNone(new Exception("There is value"));
optional.EnsureNone(() => new Exception("There is value"))
optional.EnsureNone(value => new Exception($"There is value: {value}"))
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Optional<string>> optional = ...;

// Nothing if `None`
Task ensured = optional.EnsureNone(); // Throws `ValueExistsException<string>` if `Some`
Task ensured = optional.EnsureNone(new Exception("There is value"));
Task ensured = optional.EnsureNone(() => new Exception("There is value"));
Task ensured = optional.EnsureNone(value => new Exception($"There is value: {value}"));
```

```csharp
ValueTask<Optional<string>> optional = ...;

// Nothing if `None`
ValueTask ensured = optional.EnsureNone(); // Throws `ValueExistsException<string>` if `Some`
ValueTask ensured = optional.EnsureNone(new Exception("There is value"));
ValueTask ensured = optional.EnsureNone(() => new Exception("There is value"));
ValueTask ensured = optional.EnsureNone(value => new Exception($"There is value: {value}"));
```
</details>

To override exception thrown by default exception for a specific `TValue` you can implement an extension method with the specific `TValue` in your namespace. You can also use [inheritance](inheritance.md) to override exception thrown by default for `TValue` in any namespace.
<details>
  <summary>Example</summary>

```csharp
namespace Custom
{
  class CustomValue
  {
  }
  static class EnsureNoneExtensions
  {
    static void EnsureNone(this Optional<CustomValue> optional)
    {
      optional.EnsureNone(new Exception("Overridden!"));
    }
  }
}
```
</details>

## EnsureSuccess
```csharp
Result<Exception, string> result = ...;

// Nothing if `Success`
result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
result.EnsureSuccess(new Exception("There is no value"));
result.EnsureSuccess(() => new Exception("There is no value"))
result.EnsureSuccess(fault => new Exception(fault.Message))
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception, string>> result = ...;

// Nothing if `Success`
Task ensured = result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
Task ensured = result.EnsureSuccess(new Exception("There is no value"));
Task ensured = result.EnsureSuccess(() => new Exception("There is no value"));
Task ensured = result.EnsureSuccess(fault => new Exception(fault.Message));
```

```csharp
ValueTask<Result<Exception, string>> result = ...;

// Nothing if `Success`
ValueTask ensured = result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
ValueTask ensured = result.EnsureSuccess(new Exception("There is no value"));
ValueTask ensured = result.EnsureSuccess(() => new Exception("There is no value"));
ValueTask ensured = result.EnsureSuccess(fault => new Exception(fault.Message));
```
</details>

```csharp
Result<Exception> result = ...;

// Nothing if `Success`
result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
result.EnsureSuccess(new Exception("It's failure"));
result.EnsureSuccess(() => new Exception("It's failure"))
result.EnsureSuccess(fault => new Exception(fault.Message))
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception>> result = ...;

// Nothing if `Success`
Task ensured = result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
Task ensured = result.EnsureSuccess(new Exception("It's failure"));
Task ensured = result.EnsureSuccess(() => new Exception("It's failure"));
Task ensured = result.EnsureSuccess(fault => new Exception(fault.Message));
```

```csharp
ValueTask<Result<Exception>> result = ...;

// Nothing if `Success`
ValueTask ensured = result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
ValueTask ensured = result.EnsureSuccess(new Exception("It's failure"));
ValueTask ensured = result.EnsureSuccess(() => new Exception("It's failure"));
ValueTask ensured = result.EnsureSuccess(fault => new Exception(fault.Message));
```
</details>

To override exception thrown by default exception for a specific `TValue` or `TFault` you can implement an extension method with the specific `TValue` or `TFault` in your namespace. You can also use [inheritance](inheritance.md) to override exception thrown by default for `TFault` or `TValue` in any namespace.
<details>
  <summary>Example</summary>

```csharp
namespace CustomTypes
{
  class CustomFault
  {
  }
  class CustomValue
  {
  }
  static class EnsureSuccessExtensions
  {
    static void EnsureSuccess<TFault>(this Result<TFault, CustomValue> result)
    {
      result.EnsureSuccess(new Exception("Overridden!"));
    }
    static void EnsureSuccess(this Result<CustomFault> result)
    {
      result.EnsureSuccess(new Exception("Overridden!"));
    }
  }
}
```
</details>

## EnsureFailure
```csharp
Result<Exception, string> result = ...;

// Nothing if `Failure`
result.EnsureFailure(); // Throws `ResultSucceedException<string>` on Success
result.EnsureFailure(new Exception("There is no fault"));
result.EnsureFailure(() => new Exception("There is no fault"))
result.EnsureFailure(value => new Exception(value))
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception, string>> result = ...;

// Nothing if `Failure`
Task ensured = result.EnsureFailure(); // Throws `ResultSucceedException<string>` on Success
Task ensured = result.EnsureFailure(new Exception("There is no fault"));
Task ensured = result.EnsureFailure(() => new Exception("There is no fault"));
Task ensured = result.EnsureFailure(value => new Exception(value));
```

```csharp
ValueTask<Result<Exception, string>> result = ...;

// Nothing if `Failure`
ValueTask ensured = result.EnsureFailure(); // Throws `ResultSucceedException<string>` on Success
ValueTask ensured = result.EnsureFailure(new Exception("There is no fault"));
ValueTask ensured = result.EnsureFailure(() => new Exception("There is no fault"));
ValueTask ensured = result.EnsureFailure(value => new Exception(value));
```
</details>

```csharp
Result<Exception> result = ...;

// Nothing if `Failure`
result.EnsureFailure(); // Throws `ResultSucceedException` on Success
result.EnsureFailure(new Exception("It's success"));
result.EnsureFailure(() => new Exception("It's success"))
```

<details>
  <summary>Async examples</summary>

```csharp
Task<Result<Exception>> result = ...;

// Nothing if `Failure`
Task ensured = result.EnsureFailure(); // Throws `ResultSucceedException` on Success
Task ensured = result.EnsureFailure(new Exception("It's success"));
Task ensured = result.EnsureFailure(() => new Exception("It's success"));
```

```csharp
ValueTask<Result<Exception>> result = ...;

// Nothing if `Failure`
ValueTask ensured = result.EnsureFailure(); // Throws `ResultSucceedException` on Success
ValueTask ensured = result.EnsureFailure(new Exception("It's success"));
ValueTask ensured = result.EnsureFailure(() => new Exception("It's success"));
```
</details>

To override exception thrown by default exception for a specific `TValue` or `TFault` you can implement an extension method with the specific `TValue` or `TFault` in your namespace. You can also use [inheritance](inheritance.md) to override exception thrown by default for `TFault`or `TValue` in any namespace.
<details>
  <summary>Example</summary>

```csharp
namespace CustomTypes
{
  class CustomFault
  {
  }
  class CustomValue
  {
  }
  static class EnsureFailureExtensions
  {
    static void EnsureFailure<TFault>(this Result<TFault, CustomValue> result)
    {
      result.EnsureFailure(new Exception("Overridden!"));
    }
    static void EnsureFailure(this Result<CustomFault> result)
    {
      result.EnsureFailure(new Exception("Overridden!"));
    }
  }
}
```
</details>
