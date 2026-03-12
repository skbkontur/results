# Side Effects and Assertions

Methods for performing actions based on state without extracting the contained value.

## Switch
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

```csharp
Result<Exception, int> result = ...;

result.Switch(
  onFailure: fault => Console.WriteLine("Fault is {fault.Message}"),
  onSuccess: value => Console.WriteLine($"Value is {value}")
);

result.Switch(
  onFailure: fault => Console.WriteLine("There is {fault.Message}"),
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

```csharp
Result<Exception> result = ...;

result.Switch(
  onFailure: fault => Console.WriteLine("Fault is {fault.Message}"),
  onSuccess: () => Console.WriteLine($"There is no fault")
);

result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("There is no fault")
);
```

## OnSome
```csharp
Optional<int> optional = ...;

optional.OnSome(value => Console.WriteLine($"Value is {value}"));
optional.OnSome(() => Console.WriteLine("Value is present"));
```

## OnNone
```csharp
Optional<int> optional = ...;

optional.OnNone(() => Console.WriteLine("There is no value"));
```

## OnSuccess
```csharp
Result<Exception, int> result = ...;

result.OnSuccess(value => Console.WriteLine($"Value is {value}"));
result.OnSuccess(() => Console.WriteLine("Success"));
```

```csharp
Result<Exception> result = ...;

result.OnSuccess(() => Console.WriteLine("Success"));
```

## OnFailure
```csharp
Result<Exception, string> result = ...;

result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
result.OnFailure(() => Console.WriteLine("Failure"));
```

```csharp
Result<Exception> result = ...;

result.OnFailure(fault => Console.WriteLine($"Fault is {fault.Message}"));
result.OnFailure(() => Console.WriteLine("Failure"));
```

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

```csharp
Result<Exception> result = ...;

// Nothing if `Success`
result.EnsureSuccess(); // Throws `ResultFailedException<Exception>` on Failure
result.EnsureSuccess(new Exception("It's failure"));
result.EnsureSuccess(() => new Exception("It's failure"))
result.EnsureSuccess(fault => new Exception(fault.Message))
```

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

```csharp
Result<Exception> result = ...;

// Nothing if `Failure`
result.EnsureFailure(); // Throws `ResultSucceedException` on Success
result.EnsureFailure(new Exception("It's success"));
result.EnsureFailure(() => new Exception("It's success"))
```

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
