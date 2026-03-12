# Extraction

Methods for getting data out of `Optional` and `Result` instances.

## TryGetValue
```csharp
Optional<string> optional = ...;

if (optional.TryGetValue(out string value))
{
  // value is not null here
}

// value may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
```

```csharp
Result<Exception, string> result = ...;

if (result.TryGetValue(out string value))
{
  // value is not null here
}

// value may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
```

```csharp
Result<Exception, string> result = ...;

if (result.TryGetValue(out string value, out Exception fault))
{
  // value is not null here
  // fault may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
}
else
{
  // fault is not null here
  // value may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
}

// Both value and fault may be null here. If you used any of them, you would get "warning CS8602: Dereference of a possibly null reference"

```

## TryGetFault
```csharp
Result<Exception> result = ...;

if (result.TryGetFault(out Exception fault))
{
  // fault is not null here
}

// fault may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
```

```csharp
Result<Exception, string> result = ...;

if (result.TryGetFault(out Exception fault))
{
  // fault is not null here
}

// fault may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
```

```csharp
Result<Exception, string> result = ...;

if (result.TryGetFault(out Exception fault, out string value))
{
  // fault is not null here
  // value may be null here. If you used it, you would get "warning CS8602: Dereference of a possibly null reference"
}
else
{
  // value is not null here
  // fault may be null here. If you use it, you get "warning CS8602: Dereference of a possibly null reference"
}

// Both value and fault may be null here. If you used any of them, you would get "warning CS8602: Dereference of a possibly null reference"
```

## Match
```csharp
Optional<int> optional = ...;

string extracted = optional.Match(onNone: () => "valueOnNone", onSome: i => $"Number {i}");
string extracted = optional.Match(onNone: () => "valueOnNone", onSome: () => "Number is present");
string extracted = optional.Match(onNone: () => "valueOnNone", onSomeValue: "Number is present");
string extracted = optional.Match(onNoneValue: "valueOnNone", onSome: i => $"Number {i}");
string extracted = optional.Match(onNoneValue: "valueOnNone", onSome: () => "Number is present");
string extracted = optional.Match(onNoneValue: "valueOnNone", onSomeValue: "Number is present");
```

<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = optional.Match(onNone: () => new object(), onSome: i => $"Number {i}");
object upcasted = optional.Match(onNone: () => new object(), onSome: () => "Number is present");
object upcasted = optional.Match(onNone: () => new object(), onSomeValue: "Number is present");
object upcasted = optional.Match(onNoneValue: new object(), onSome: i => $"Number {i}");
object upcasted = optional.Match(onNoneValue: new object(), onSome: () => "Number is present");
object upcasted = optional.Match(onNoneValue: new object(), onSomeValue: "Number is present");

object upcasted = optional.Match(onNone: () => "valueOnNone", onSome: _ => new object());
object upcasted = optional.Match(onNone: () => "valueOnNone", onSome: () => new object());
object upcasted = optional.Match(onNone: () => "valueOnNone", onSomeValue: new object());
object upcasted = optional.Match(onNoneValue: "valueOnNone", onSome: _ => new object());
object upcasted = optional.Match(onNoneValue: "valueOnNone", onSome: () => new object());
object upcasted = optional.Match(onNoneValue: "valueOnNone", onSomeValue: new object());

object upcasted = optional.Match<object>(onNone: () => new Exception("There is no value"), onSome: i => $"Number {i}");
object upcasted = optional.Match<object>(onNone: () => new Exception("There is no value"), onSome: () => "Number is present");
object upcasted = optional.Match<object>(onNone: () => new Exception("There is no value"), onSomeValue: "Number is present");
object upcasted = optional.Match<object>(onNoneValue: new Exception("There is no value"), onSome: i => $"Number {i}");
object upcasted = optional.Match<object>(onNoneValue: new Exception("There is no value"), onSome: () => "Number is present");
object upcasted = optional.Match<object>(onNoneValue: new Exception("There is no value"), onSomeValue: "Number is present");
```
</details>

```csharp
Result<Exception, int> result = ...;

string extracted = result.Match(onFailure: ex => ex.Message, onSuccess: i => $"Number {i}");
string extracted = result.Match(onFailure: ex => ex.Message, onSuccess: () => "Number is present");
string extracted = result.Match(onFailure: ex => ex.Message, onSuccessValue: "Number is present");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccess: i => $"Number {i}");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccess: () => "Number is present");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccessValue: "Number is present");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccess: i => $"Number {i}");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccess: () => "Number is present");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccessValue: "Number is present");
```

<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = result.Match(onFailure: ex => new object(), onSuccess: i => $"Number {i}");
object upcasted = result.Match(onFailure: ex => new object(), onSuccess: () => "Number is present");
object upcasted = result.Match(onFailure: ex => new object(), onSuccessValue: "Number is present");
object upcasted = result.Match(onFailure: () => new object(), onSuccess: i => $"Number {i}");
object upcasted = result.Match(onFailure: () => new object(), onSuccess: () => "Number is present");
object upcasted = result.Match(onFailure: () => new object(), onSuccessValue: "Number is present");
object upcasted = result.Match(onFailureValue: new object(), onSuccess: i => $"Number {i}");
object upcasted = result.Match(onFailureValue: new object(), onSuccess: () => "Number is present");
object upcasted = result.Match(onFailureValue: new object(), onSuccessValue: "Number is present");

object upcasted = result.Match(onFailure: ex => ex.Message, onSuccess: _ => new object());
object upcasted = result.Match(onFailure: ex => ex.Message, onSuccess: () => new object());
object upcasted = result.Match(onFailure: ex => ex.Message, onSuccessValue: new object());
object upcasted = result.Match(onFailure: () => "valueOnNone", onSuccess: _ => new object());
object upcasted = result.Match(onFailure: () => "valueOnNone", onSuccess: () => new object());
object upcasted = result.Match(onFailure: () => "valueOnNone", onSuccessValue: new object());
object upcasted = result.Match(onFailureValue: "valueOnNone", onSuccess: _ => new object());
object upcasted = result.Match(onFailureValue: "valueOnNone", onSuccess: () => new object());
object upcasted = result.Match(onFailureValue: "valueOnNone", onSuccessValue: new object());

object upcasted = result.Match<object>(onFailure: ex => ex.Message, onSuccess: i => $"Number {i}");
object upcasted = result.Match<object>(onFailure: ex => ex.Message, onSuccess: () => "Number is present");
object upcasted = result.Match<object>(onFailure: ex => ex.Message, onSuccessValue: "Number is present");
object upcasted = result.Match<object>(onFailure: () => new Exception("There is no value"), onSuccess: i => $"Number {i}");
object upcasted = result.Match<object>(onFailure: () => new Exception("There is no value"), onSuccess: () => "Number is present");
object upcasted = result.Match<object>(onFailure: () => new Exception("There is no value"), onSuccessValue: "Number is present");
object upcasted = result.Match<object>(onFailureValue: new Exception("There is no value"), onSuccess: i => $"Number {i}");
object upcasted = result.Match<object>(onFailureValue: new Exception("There is no value"), onSuccess: () => "Number is present");
object upcasted = result.Match<object>(onFailureValue: new Exception("There is no value"), onSuccessValue: "Number is present");
```
</details>

```csharp
Result<Exception> result = ...;

string extracted = result.Match(onFailure: ex => ex.Message, onSuccess: () => "Fault is not present");
string extracted = result.Match(onFailure: ex => ex.Message, onSuccessValue: "Fault is not present");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccess: () => "Fault is not present");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccessValue: "Fault is not present");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccess: () => "Fault is not present");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccessValue: "Fault is not present");
```

<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = result.Match(onFailure: ex => new object(), onSuccess: () => "Fault is not present");
object upcasted = result.Match(onFailure: ex => new object(), onSuccessValue: "Fault is not present");
object upcasted = result.Match(onFailure: () => new object(), onSuccess: () => "Fault is not present");
object upcasted = result.Match(onFailure: () => new object(), onSuccessValue: "Fault is not present");
object upcasted = result.Match(onFailureValue: new object(), onSuccess: () => "Fault is not present");
object upcasted = result.Match(onFailureValue: new object(), onSuccessValue: "Fault is not present");

object upcasted = result.Match(onFailure: ex => ex.Message, onSuccess: () => new object());
object upcasted = result.Match(onFailure: ex => ex.Message, onSuccessValue: new object());
object upcasted = result.Match(onFailure: () => "valueOnNone", onSuccess: () => new object());
object upcasted = result.Match(onFailure: () => "valueOnNone", onSuccessValue: new object());
object upcasted = result.Match(onFailureValue: "valueOnNone", onSuccess: () => new object());
object upcasted = result.Match(onFailureValue: "valueOnNone", onSuccessValue: new object());

object upcasted = result.Match<object>(onFailure: ex => ex.Message, onSuccess: () => "Fault is not present");
object upcasted = result.Match<object>(onFailure: ex => ex.Message, onSuccessValue: "Fault is not present");
object upcasted = result.Match<object>(onFailure: () => new Exception(), onSuccess: () => "Fault is not present");
object upcasted = result.Match<object>(onFailure: () => new Exception(), onSuccessValue: "Fault is not present");
object upcasted = result.Match<object>(onFailureValue: new Exception(), onSuccess: () => "Fault is not present");
object upcasted = result.Match<object>(onFailureValue: new Exception(), onSuccessValue: "Fault is not present");
```
</details>


## GetValueOrElse
```csharp
Optional<string> optional = ...;

string extracted = optional.GetValueOrElse(() => "defaultValue");
string extracted = optional.GetValueOrElse("defaultValue");
```

<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = optional.GetValueOrElse(() => new object());
object upcasted = optional.GetValueOrElse(new object());

object upcasted = optional.GetValueOrElse<object>(() => new Exception("There is no value"));
object upcasted = optional.GetValueOrElse<object>(new Exception("There is no value"));

Optional<object> objectOptional = ...;
object upcasted = objectOptional.GetValueOrElse(() => "defaultValue");
object upcasted = objectOptional.GetValueOrElse("defaultValue");
```
</details>


```csharp
Result<Exception, string> result = ...;

string extracted = result.GetValueOrElse(fault => $"Converted to success fault: {fault.Message}");
string extracted = result.GetValueOrElse(() => "defaultValue");
string extracted = result.GetValueOrElse("defaultValue");
```

<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = result.GetValueOrElse(_ => new object());
object upcasted = result.GetValueOrElse(() => new object());
object upcasted = result.GetValueOrElse(new object());

object upcasted = result.GetValueOrElse<object>(fault => new Exception(fault.Message));
object upcasted = result.GetValueOrElse<object>(() => new Exception("There is no value"));
object upcasted = result.GetValueOrElse<object>(new Exception("There is no value"));

Result<Exception, object> objectResult = ...;
object upcasted = objectResult.GetValueOrElse(fault => fault.Message);
object upcasted = objectResult.GetValueOrElse(() => "defaultValue");
object upcasted = objectResult.GetValueOrElse("defaultValue");
```
</details>


## GetFaultOrElse
```csharp
Result<Exception, string> result = ...;

Exception extracted = result.GetFaultOrElse(value => new Exception(value));
Exception extracted = result.GetFaultOrElse(() => new Exception());
Exception extracted = result.GetFaultOrElse(new Exception());
```
<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = result.GetFaultOrElse(_ => new object());
object upcasted = result.GetFaultOrElse(() => new object());
object upcasted = result.GetFaultOrElse(new object());

object upcasted = result.GetFaultOrElse<object>(value => value);
object upcasted = result.GetFaultOrElse<object>(() => "There is no fault");
object upcasted = result.GetFaultOrElse<object>("There is no fault");

Result<object, string> objectResult = ...;
object upcasted = objectResult.GetFaultOrElse(value => value);
object upcasted = objectResult.GetFaultOrElse(() => "defaultFault");
object upcasted = objectResult.GetFaultOrElse("defaultFault");
```
</details>

```csharp
Result<Exception> result = ...;

Exception extracted = result.GetFaultOrElse(() => new Exception());
Exception extracted = result.GetFaultOrElse(new Exception());
```
<details>
  <summary>Examples with upcast</summary>

```csharp
object upcasted = result.GetFaultOrElse(() => new object());
object upcasted = result.GetFaultOrElse(new object());

object upcasted = result.GetFaultOrElse<object>(() => "There is no fault");
object upcasted = result.GetFaultOrElse<object>("There is no fault");

Result<object> objectResult = ...;
object upcasted = objectResult.GetFaultOrElse(() => "defaultFault");
object upcasted = objectResult.GetFaultOrElse("defaultFault");
```
</details>

## GetValueOrThrow
```csharp
Optional<string> optional = ...;

string extracted = optional.GetValueOrThrow(); // Throws `ValueMissingException` on None
string extracted = optional.GetValueOrThrow(new Exception("There is no value"));
string extracted = optional.GetValueOrThrow(() => new Exception("There is no value"));

object upcasted = optional.GetValueOrThrow<object>();
object upcasted = optional.GetValueOrThrow<object>(new Exception("There is no value"));
object upcasted = optional.GetValueOrThrow<object>(() => new Exception("There is no value"));
```

```csharp
Result<Exception, string> result = ...;

string extracted = result.GetValueOrThrow(); // Throws `ResultFailedException<Exception>` on Failure
string extracted = result.GetValueOrThrow(new Exception("There is no value"));
string extracted = result.GetValueOrThrow(() => new Exception("There is no value"));
string extracted = result.GetValueOrThrow(fault => new Exception(fault.Message));

object upcasted = result.GetValueOrThrow<object, object>();
object upcasted = result.GetValueOrThrow<object, object>(new Exception("There is no value"));
object upcasted = result.GetValueOrThrow<object, object>(() => new Exception("There is no value"));
object upcasted = result.GetValueOrThrow<object, object>(fault => new Exception(fault.Message));
```

To override exception thrown by default for a specific `TValue` you can implement an extension method with the specific `TValue` in your namespace. You can also use [inheritance](inheritance.md) to override exception thrown by default for `TValue` in any namespace.
<details>
  <summary>Example</summary>

```csharp
namespace CustomValues
{
  class CustomValue
  {
  }
  static class GetValueOrThrowExtensions
  {
    static CustomValue GetValueOrThrow(this Optional<CustomValue> optional)
    {
      return optional.GetValueOrThrow(new Exception("Overiden!"));
    }

    static CustomValue GetValueOrThrow<TFault>(this Result<TFault, CustomValue> result)
    {
      return result.GetValueOrThrow(new Exception("Overiden!"));
    }
  }
}
```
</details>

## GetFaultOrThrow
```csharp
Result<Exception, string> result = ...;

Exception extracted = result.GetFaultOrThrow(); // Throws `ResultSucceedException<string>` on Success
Exception extracted = result.GetFaultOrThrow(new Exception("There is no fault"));
Exception extracted = result.GetFaultOrThrow(() => new Exception("There is no fault"));
Exception extracted = result.GetFaultOrThrow(value => new Exception(value));

object upcasted = result.GetFaultOrThrow<object, object>();
object upcasted = result.GetFaultOrThrow<object, object>(new Exception("There is no fault"));
object upcasted = result.GetFaultOrThrow<object, object>(() => new Exception("There is no fault"));
object upcasted = result.GetFaultOrThrow<object, object>(value => new Exception(value?.ToString()));
```

```csharp
Result<Exception> result = ...;

Exception extracted = result.GetFaultOrThrow(); // Throws `ResultSucceedException` on Success
Exception extracted = result.GetFaultOrThrow(new Exception("There is no fault"));
Exception extracted = result.GetFaultOrThrow(() => new Exception("There is no fault"));

object upcasted = result.GetFaultOrThrow<object>();
object upcasted = result.GetFaultOrThrow<object>(new Exception("There is no fault"));
object upcasted = result.GetFaultOrThrow<object>(() => new Exception("There is no fault"));
```

To override exception thrown by default exception for a specific `TFault` you can implement an extension method with the specific `TFault` in your namespace. You can also use [inheritance](inheritance.md) to override exception thrown by default for `TFault` in any namespace.
<details>
  <summary>Example</summary>

```csharp
namespace CustomFaults
{
  class CustomFault
  {
  }
  static class GetValueOrThrowExtensions
  {
    static CustomFault GetFaultOrThrow(this Result<CustomFault> result)
    {
      return result.GetFaultOrThrow(new Exception("Overridden!"));
    }

    static CustomFault GetFaultOrThrow<TValue>(this Result<CustomFault, TValue> result)
    {
      return result.GetFaultOrThrow(new Exception("Overridden!"));
    }
  }
}
```
</details>

## GetValueOrDefault
```csharp
Optional<string> optional = ...;

string? extracted = optional.GetValueOrDefault(); // null or actual value for reference types

object? upcasted = optional.GetValueOrDefault<object>();
```

```csharp
Optional<int> optional = ...;

int extracted = optional.GetValueOrDefault();  // 0 or actual value for the value type
```

```csharp
Result<Exception, string> result = ...;

string? extracted = result.GetValueOrDefault(); // null or actual value for reference types

object? upcasted = result.GetValueOrDefault<object>();
```

```csharp
Result<Exception, int> result = ...;

int extracted = result.GetValueOrDefault();  // 0 or actual value for the value type
```

## GetFaultOrDefault
```csharp
Result<Exception, string> result = ...;

Exception? extracted = result.GetFaultOrDefault(); // null or actual fault for reference types

object? upcasted = result.GetFaultOrDefault<object>();
```

```csharp
Result<int, string> result = ...;

int extracted = result.GetFaultOrDefault();  // 0 or actual fault for the value type
```

```csharp
Result<Exception> result = ...;

Exception? extracted = result.GetFaultOrDefault(); // null or actual fault for reference types

object? upcasted = result.GetFaultOrDefault<object>();
```

```csharp
Result<int> result = ...;

int extracted = result.GetFaultOrDefault();  // 0 or actual fault for the value type
```

## GetValueOrNull
This method can be applied to non-nullable value types (structs) only.

```csharp
Optional<int> optional = ...;

int? extracted = optional.GetValueOrNull(); // null or actual value
```

```csharp
Result<Exception, int> result = ...;

int? extracted = result.GetValueOrNull(); // null or actual value
```

Upcasts are not supported.

## GetFaultOrNull
This method can be applied to non-nullable value types (structs) only.

```csharp
Result<int, string> result = ...;

int? extracted = result.GetFaultOrNull(); // null or actual fault
```

```csharp
Result<int> result = ...;

int? extracted = result.GetFaultOrNull(); // null or actual fault
```

Upcasts are not supported.

## HasSome
```csharp
Optional<string> optional = "has value";

bool extracted = optional.HasSome; // true
```

```csharp
Optional<string> optional = Optional.None();

bool extracted = optional.HasSome; // false
```

## IsNone
```csharp
Optional<string> optional = "has value";

bool extracted = optional.IsNone; // false
```

```csharp
Optional<string> optional = Optional.None();

bool extracted = optional.IsNone; // true
```

## Success
```csharp
Result<Exception, string> result = "has value";

bool extracted = result.Success; // true
```

```csharp
Result<Exception, string> result = new Exception();

bool extracted = result.Success; // false
```

```csharp
Result<Exception> result = Result.Succeed();

bool extracted = result.Success; // true
```

```csharp
Result<Exception> result = new Exception();

bool extracted = result.Success; // false
```

## Failure
```csharp
Result<Exception, string> result = "has value";

bool extracted = result.Failure; // false
```

```csharp
Result<Exception, string> result = new Exception();

bool extracted = result.Failure; // true
```

```csharp
Result<Exception> result = Result.Succeed();

bool extracted = result.Failure; // false
```

```csharp
Result<Exception> result = new Exception();

bool extracted = result.Failure; // true
```

## GetValues / GetFaults
```csharp
Optional<string> optional = ...;

// `[]` if `None`
// `[value]` if `Some`
IEnumerable<string> values = optional
  .GetValues()
  .ToArray(); // optional

IEnumerable<object> upcasted = optional.GetValues<object>();
```

```csharp
Result<Exception, string> result = ...;

// `[]` if `Failure`
// `[value]` if `Success`
IEnumerable<string> values = result.GetValues().ToArray();

IEnumerable<object> upcasted = result.GetValues<object>();
```

```csharp
Result<Exception, string> result = ...;

// `[exception]` if `Failure`
// `[]` if `Success`
IEnumerable<Exception> faults = result.GetFaults().ToArray();

IEnumerable<object> upcasted = result.GetFaults<object>();
```

```csharp
Result<Exception> result = ...;

// `[exception]` if `Failure`
// `[]` if `Success`
IEnumerable<Exception> faults = result.GetFaults().ToArray();

IEnumerable<object> upcasted = result.GetFaults<object>();
```

## foreach
```csharp
string FindValue(Optional<int> optional)
{
  foreach(var value in optional)
  {
    return value + " is found!";
  }

  return "no value found";
}

```

```csharp
string FindValue(Result<Exception, int> result)
{
  foreach(var value in result)
  {
    return value + " is found!";
  }

  return "no value found";
}

```

## ToString
```csharp
Optional<string> optional = ...;

// `None<string>` if `None`
// `Some<string> value={Value}` if `Some`
string str = optional.ToString();
```

```csharp
Result<int, string> result = ...;

// `ResultFailure<Int32, string> fault={Fault}` if `Failure`
// `ResultSuccess<Int32, string> value={Value}` if `Success`
string str = result.ToString();
```

```csharp
Result<int> result = ...;

// `ResultFailure<Int32> fault={Fault}` if `Failure`
// `ResultSuccess<Int32>` if `Success`
string str = result.ToString();
```

## Upcasts

You can upcast a return value of many data extraction methods by providing a type argument or by combining two arguments of different types in a single method call.

Examples:
```csharp
Optional<string> optional = ...

Optional<object> upcasted1 = optional.OnNone<object>(str => Console.WriteLine(str));
Optional<object> upcasted2 = optional.Match(onNone: () => new object(), onSome: str => str);
```

Only safe upcasts are allowed.
For example, `Optional<string>` can be converted to `Optional<object>` but not vice versa.

Safety is enforced by covariance rules. So:
* Upcasts are only supported for reference types because covariance can not be used with value types.
* Upcasts are only supported for synchronous methods because `Task<T>` and `ValueTask<T>` types do not support covariance.
