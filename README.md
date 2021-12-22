# Yet another `Result` types implementation

This library consists of three `Result` types with some tempting [features](#features) :
* `Optional<TValue>`
* `Result<TFault>`
* `Result<TFault, TValue>`

## Content

* [License](#license)
* [Installation](#installation)
    * [Examples](#examples)
* [Features](#features)
* [Drawbacks](#drawbacks)
* [Instantiation of types](#instantiation-of-types)
* [Extraction of data from instances](#extraction-of-data-from-instances)
    * [TryGetValue](#trygetvalue)
    * [TryGetFault](#trygetfault)
    * [Match](#match)
    * [Switch](#switch)
    * [OnSome](#onsome)
    * [OnNone](#onnone)
    * [OnSuccess](#onsuccess)
    * [OnFailure](#onfailure)
    * [Switch/OnSome/OnNone/OnSuccess/OnFailure method chaining](#switchonsomeonnoneonsuccessonfailure-method-chaining)
    * [Switch/OnSome/OnNone/OnSuccess/OnFailure return value upcasting](#switchonsomeonnoneonsuccessonfailure-return-value-upcasting)
    * [GetValueOrElse](#getvalueorelse)
    * [GetFaultOrElse](#getfaultorelse)
    * [GetValueOrThrow](#getvalueorthrow)
    * [GetFaultOrThrow](#getfaultorthrow)
    * [GetValueOrDefault](#getvalueordefault)
    * [GetFaultOrDefault](#getfaultordefault)
    * [GetValueOrNull](#getvalueornull)
    * [GetFaultOrNull](#getfaultornull)
    * [EnsureHasValue](#ensurehasvalue)
    * [EnsureNone](#ensurenone)
    * [EnsureSuccess](#ensuresuccess)
    * [EnsureFailure](#ensurefailure)
    * [HasSome](#hassome)
    * [IsNone](#isnone)
    * [Success](#success)
    * [Failure](#failure)
    * [Implicit conversion to bool](#implicit-conversion-to-bool)
    * [LINQ method syntax (GetValues, GetFaults)](#linq-method-syntax-getvalues-getfaults)
    * [LINQ query syntax](#linq-query-syntax)
    * [foreach](#foreach)
    * [ToString](#tostring)
    * [Upcasts](#upcasts)
* [Conversion of generic arguments](#conversion-of-generic-arguments)
    * [MapValue](#mapvalue)
    * [MapFault](#mapfault)
    * [Upcast](#upcast)
* [Result combining](#result-combining) or monadic extensions
    * [Then](#then)
    * [OrElse](#orelse)
    * [Then/OrElse method chaining](#thenorelse-method-chaining)
    * [Select](#select)
    * [Do notation](#do-notation)
    * [Do notation with async extensions](#do-notation-with-async-extensions)
* [Inheritance](#inheritance)
* [Other](#other)
* [Yet to be implemented](#yet-to-be-implemented)
* [Contributing](#contributing)


## License

MIT


## Installation

Use [cement](https://github.com/skbkontur/cement#get-started) to add a reference to `Kontur.Results` assembly.

Execute that command in your cement module:

`cm ref add results your-csproj.csproj`

Execute the following command in your cement module instead if you are willing for [monadic extensions](#result-combining) (implemented separately). It consists of [Then](#then), [OrElse](#orelse), [Select](#select) and [do notation](#do-notation-with-async-extensions):

`cm ref add results/monad your-csproj.csproj`

### Examples
Basic example:
```csharp
using Kontur.Results;

Result<Exception, string> result = "success!"; // implicit conversion

if (result.TryGetValue(out value)) {
   return value.ToString() // OK. Value is not null here. The compiler allows this.
}

return value.ToString() // warning CS8602: Dereference of a possibly null reference.
```

Example with exceptions:
```csharp
using Kontur.Results;

class DraftClient
{
   ...
   Task<Result<DraftError, Draft>> CreateDraft()
   {
     ...
   }
}

Result<DraftError, Draft> createDraftResult = await new DraftClient().CreateDraft();
try
{
  Draft draft = createDraftResult.GetValueOrThrow();
  return draft.Id;
}
catch (ResultFailedException<DraftError> ex)
{
  log.Warn("Error code: " + ex.Fault.Code);
}
```

Example with inheritance to freeze fault type:
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

You can work with data without extracting values from instances:
```csharp
abstract Task<Optional<string>> GetFormLogin();
abstract Optional<Guid> GetUser(string login);
abstract ValueTask<Optional<Guid>> CreateUser(string login);

Task<Optional<Guid>> userId =
   GetFormLogin()
  .Then(login => GetUser(login).OrElse(() => CreateUser(login)))
```

Do notation reduces the count of checks and await operators significantly:
```csharp
abstract Result<Exception, Guid> GetCurrentUserId();
abstract Result<Exception> EnsureUserIdIsCorrect(Guid userId);
abstract Task<int> GetCurrentIndex();
abstract ValueTask<Result<Exception, string>> GetMessage(Guid userId, int index);
abstract Result<Exception, ConvertResult> Convert(string message, Guid userId);

Task<Result<Exception, ConvertResult>> result =
  from userId  in GetCurrentUserId()
  where EnsureUserIdIsCorrect(userId)
  from index   in GetCurrentIndex()
  let nextIndex = index + 1
  from message in GetMessage(userId, nextIndex)
  select Convert(message, userId);
```


## Features

* [Do notation](#do-notation-with-async-extensions) with support of async (`Task` and `ValueTask`) execution and with no limit on the expression count.
* [Then](#then) (`And`, `ContinueWith`, `ContinueOnSome`, `Bind`) and [OrElse](#orelse) (`Or`, `Else`, `Catch`, `ContinueOnNone`) async extensions that allow chaining.

* Great interface that allows checking and extraction of data with a single method. See [TryGetValue](#trygetvalue) and [Match](#match).
* Explicit behavior of methods. See [GetValueOrThrow](#getvalueorthrow) and [GetValueOrDefault](#getvalueordefault).

* `TValue` and `TFault` generic parameters are not restricted in any way.
* There is no specific handling of null values. So you can store `nulls` as `TValue` or `TFault`. Use C# 8 nullable reference types to handle nulls.

* Assemblies contain only three `Result` type implementations and extension methods for them. There is no other stuff.

* `Result<TFault, TValue>` has no third state. It is restricted by a schema and the compiler that only two states are possible in memory of an application. One of them is `HasValue`. Other one is `HasFault`. So there are not `bottom` or `empty` state.
* `Result` type implementations are if-less and make use of abstract classes polymorphism and VMT to maintain simplicity and error safety. As a result, there is no null-forgiving operator and no third state. Also, there are no ternary operators that check `Success` flag or similar stuff.
* [Inheritance](#inheritance) allows to freeze or limit generic `TFault` and `TValue` parameters with user custom type arguments.

## Drawbacks

* To enable some features the implementation is based on abstract classes polymorphism. So `Result` types are not marked `readonly` (but they are implemented as readonly) and are not `struct`.


## Instantiation of types

Explicit examples:
```csharp
var optional = Optional.Some("hello"); // Optional<string>
var optional = Optional<string>.Some("hello"); // Optional<string>

var result = Result<Exception>.Succeed("hello"); // Result<Exception, string>
var result = Result<Exception, string>.Succeed("hello"); // Result<Exception, string>

var result = Result<Exception>.Succeed(); // Result<Exception>
var result = Result.Succeed<Exception>(); // Result<Exception>
```

```csharp
var optional = Optional.None<string>(); // Optional<string>
var optional = Optional<string>.None(); // Optional<string>

var result = ResultFailure<string>.Create(new Exception()); // Result<Exception, string>
var result = Result<Exception, string>.Fail(new Exception()); // Result<Exception, string>

var result = Result.Fail(new Exception()); // Result<Exception>
var result = Result<Exception>.Fail(new Exception()); // Result<Exception>
```

Implicit examples:
```csharp
Optional<string> optional = "hello";
Optional<string> optional = Optional.None();

Result<Exception, string> result = "hello";
Result<Exception, string> result = new Exception();
Result<int, int> result = Result.Succeed(55);
Result<int, int> result = Result.Fail(0);

Result<Exception> result = Result.Succeed();
Result<Exception> result = new Exception();
```

```csharp
Optional<string> optional = flag
  ? "Hello"
  : Optional.None();

var optional = flag
  ? Optional.Some("Hello")
  : Optional.None();

var optional = flag
  ? "Hello"
  : Optional<string>.None();

```

```csharp
Result<Exception, string> result = flag
  ? "hello"
  : new Exception();

var result = flag
  ? "hello"
  : ResultFailure<string>.Create(new Exception());

var result = flag
  ? Result<Exception>.Succeed("hello")
  : new Exception();

Result<int, int> result = flag
  ? Result.Succeed(0)
  : Result.Fail(-1);

var result = flag
  ? Result<int>.Succeed(0)
  : Result.Fail(-1);

var result = flag
  ? Result.Succeed(0)
  : ResultFailure<int>.Create(-1);
```

```csharp
Result<Exception> = flag
  ? new Exception()
  : Result.Succeed();

var result = flag
  ? Result<Exception>.Fail(new Exception())
  : Result.Succeed();

var result = flag
  ? new Exception()
  : Result<Exception>.Succeed();

```

Some conversions:
```csharp
Result<Exception, int> source = ...

Result<Exception> target = source;
```

```csharp
Optional<int> GetResult(Random random)
{
  int randomValue = random.Next(0, 10);
  if (randomValue > 10)
  {
    return randomValue;
  }

  return Optional.None();
}
```


## Extraction of data from instances

### TryGetValue
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

// Both value and fault may be null here. If you used them, you would get "warning CS8602: Dereference of a possibly null reference"

```

### TryGetFault
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

// Both value and fault may be null here. If you used them, you would get "warning CS8602: Dereference of a possibly null reference"
```

### Match
```csharp
Optional<int> optional = ...;

string extracted = optional.Match(onNone: () => "valueOnNone", onSome: i => $"Number {i}");
string extracted = optional.Match(onNone: () => "valueOnNone", onSome: () => "Number is present");
string extracted = optional.Match(onNone: () => "valueOnNone", onSomeValue: "Number is present");
string extracted = optional.Match(onNoneValue: "valueOnNone", onSome: i => $"Number {i}");
string extracted = optional.Match(onNoneValue: "valueOnNone", onSome: () => "Number is present");
string extracted = optional.Match(onNoneValue: "valueOnNone", onSomeValue: "Number is present");

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
object upcasted = result.Match<object>(onFailure: ex => ex.Message), onSuccessValue: "Number is present");
object upcasted = result.Match<object>(onFailure: () => new Exception("There is no value"), onSuccess: i => $"Number {i}");
object upcasted = result.Match<object>(onFailure: () => new Exception("There is no value"), onSuccess: () => "Number is present");
object upcasted = result.Match<object>(onFailure: () => new Exception("There is no value"), onSuccessValue: "Number is present");
object upcasted = result.Match<object>(onFailureValue: new Exception("There is no value"), onSuccess: i => $"Number {i}");
object upcasted = result.Match<object>(onFailureValue: new Exception("There is no value"), onSuccess: () => "Number is present");
object upcasted = result.Match<object>(onFailureValue: new Exception("There is no value"), onSuccessValue: "Number is present");
```

```csharp
Result<Exception> result = ...;

string extracted = result.Match(onFailure: ex => ex.Message, onSuccess: () => "Fault is not present");
string extracted = result.Match(onFailure: ex => ex.Message, onSuccessValue: "Fault is not present");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccess: () => "Fault is not present");
string extracted = result.Match(onFailure: () => "valueOnNone", onSuccessValue: "Fault is not present");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccess: () => "Fault is not present");
string extracted = result.Match(onFailureValue: "valueOnNone", onSuccessValue: "Fault is not present");

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
object upcasted = result.Match<object>(onFailure: ex => ex.Message), onSuccessValue: "Fault is not present");
object upcasted = result.Match<object>(onFailure: () => new Exception(), onSuccess: () => "Fault is not present");
object upcasted = result.Match<object>(onFailure: () => new Exception(), onSuccessValue: "Fault is not present");
object upcasted = result.Match<object>(onFailureValue: new Exception(), onSuccess: () => "Fault is not present");
object upcasted = result.Match<object>(onFailureValue: new Exception(), onSuccessValue: "Fault is not present");
```

### Switch
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
); =

result.Switch(
  onFailure: () => Console.WriteLine("Fault is present"),
  onSuccess: () => Console.WriteLine("There is no fault")
);
```

### OnSome
```csharp
Optional<int> optional = ...;

optional.OnSome(value => Console.WriteLine($"Value is {value}"));
optional.OnSome(() => Console.WriteLine("Value is present"));
```

### OnNone
```csharp
Optional<int> optional = ...;

optional.OnNone(() => Console.WriteLine("There is no value"));
```

### OnSuccess
```csharp
Result<Exception, int> result = ...;

result.OnSuccess(value => Console.WriteLine($"Value is {value}"));
result.OnSuccess(() => Console.WriteLine("Success"));
```

```csharp
Result<Exception> result = ...;

result.OnSuccess(() => Console.WriteLine("Success"));
```

### OnFailure
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

### Switch/OnSome/OnNone/OnSuccess/OnFailure method chaining
```csharp
Optional<int> optional = ...;

string result = optional
  .Switch(
    onNone: () => Console.WriteLine("There is no value"),
    onSome: value => Console.WriteLine($"Value is {value}"))
  .OnSome(value => log.Info($"Value is {value}"))
  .OnNone(() => log.Info("There is no value"))
  .Match(onNoneValue: "valueOnNone", onSome: value => value.ToString());
```

`Result<TFault, TValue>` and `Result<TFault>` have similar syntax.

### Switch/OnSome/OnNone/OnSuccess/OnFailure return value upcasting

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

### GetValueOrElse
```csharp
Optional<string> optional = ...;

string extracted = optional.GetValueOrElse(() => "defaultValue");
string extracted = optional.GetValueOrElse("defaultValue");

object upcasted = optional.GetValueOrElse(() => new object());
object upcasted = optional.GetValueOrElse(new object());

object upcasted = optional.GetValueOrElse<object>(() => new Exception("There is no value"));
object upcasted = optional.GetValueOrElse<object>(new Exception("There is no value"));

Optional<object> objectOptional = ...;
object upcasted = objectOptional.GetValueOrElse(() => "defaultValue");
object upcasted = objectOptional.GetValueOrElse("defaultValue");
```

```csharp
Result<Exception, string> result = ...;

string extracted = result.GetValueOrElse(fault => $"Converted to success fault: {fault.Message}");
string extracted = result.GetValueOrElse(() => "defaultValue");
string extracted = result.GetValueOrElse("defaultValue");

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

### GetFaultOrElse
```csharp
Result<Exception, string> result = ...;

Exception extracted = result.GetFaultOrElse(value => new Exception(value));
Exception extracted = result.GetFaultOrElse(() => new Exception());
Exception extracted = result.GetFaultOrElse(new Exception());

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

```csharp
Result<Exception> result = ...;

Exception extracted = result.GetFaultOrElse(() => new Exception());
Exception extracted = result.GetFaultOrElse(new Exception());

object upcasted = result.GetFaultOrElse(() => new object());
object upcasted = result.GetFaultOrElse(new object());

object upcasted = result.GetFaultOrElse<object>(() => "There is no fault");
object upcasted = result.GetFaultOrElse<object>("There is no fault");

Result<object> objectResult = ...;
object upcasted = objectResult.GetFaultOrElse(() => "defaultFault");
object upcasted = objectResult.GetFaultOrElse("defaultFault");
```

### GetValueOrThrow
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

object upcasted = result.GetValueOrThrow<object>();
object upcasted = result.GetValueOrThrow<object>(new Exception("There is no value"));
object upcasted = result.GetValueOrThrow<object>(() => new Exception("There is no value"));
object upcasted = result.GetValueOrThrow<object>(fault => new Exception(fault.Message));
```

To override exception thrown by default for a specific `TValue` you can implement an extension method with the specific `TValue` in your namespace. You can also use [inheritance](#inheritance) to override exception thrown by default for `TValue` in any namespace.
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

### GetFaultOrThrow
```csharp
Result<Exception, string> result = ...;

Exception extracted = result.GetFaultOrThrow(); // Throws `ResultSucceedException<string>` on Success
Exception extracted = result.GetFaultOrThrow(new Exception("There is no fault"));
Exception extracted = result.GetFaultOrThrow(() => new Exception("There is no fault"));
Exception extracted = result.GetFaultOrThrow(value => new Exception(value));

object upcasted = result.GetFaultOrThrow<object>();
object upcasted = result.GetFaultOrThrow<object>(new Exception("There is no fault"));
object upcasted = result.GetFaultOrThrow<object>(() => new Exception("There is no fault"));
object upcasted = result.GetFaultOrThrow<object>(value => new Exception(value));
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

To override exception thrown by default exception for a specific `TFault` you can implement an extension method with the specific `TFault` in your namespace. You can also use [inheritance](#inheritance) to override exception thrown by default for `TFault` in any namespace.
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

### GetValueOrDefault
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

### GetFaultOrDefault
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

### GetValueOrNull
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

### GetFaultOrNull
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

### EnsureHasValue
```csharp
Optional<string> optional = ...;

// Nothing if `Some`
optional.EnsureHasValue(); // Throws `ValueMissingException` on None
optional.EnsureHasValue(new Exception("There is no value"));
optional.EnsureHasValue(() => new Exception("There is no value"))
```

To override exception thrown by default exception for a specific `TValue` you can implement an extension method with the specific `TValue` in your namespace. You can also use [inheritance](#inheritance) to override exception thrown by default for `TValue` in any namespace.
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
      return optional.EnsureHasValue(new Exception("Overridden!"));
    }
  }
}
```

### EnsureNone
```csharp
Optional<string> optional = ...;

// Nothing if `None`
optional.EnsureNone(); // Throws `ValueExistsException<string>` if `Some`
optional.EnsureNone(new Exception("There is value"));
optional.EnsureNone(() => new Exception("There is value"))
optional.EnsureNone(value => new Exception($"There is value: {value}"))
```

To override exception thrown by default exception for a specific `TValue` you can implement an extension method with the specific `TValue` in your namespace. You can also use [inheritance](#inheritance) to override exception thrown by default for `TValue` in any namespace.
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
      return optional.EnsureNone(new Exception("Overridden!"));
    }
  }
}
```

### EnsureSuccess
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

To override exception thrown by default exception for a specific `TValue` or `TFault` you can implement an extension method with the specific `TValue` or `TFault` in your namespace. You can also use [inheritance](#inheritance) to override exception thrown by default for `TFault` or `TValue` in any namespace.
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
      return result.EnsureSuccess(new Exception("Overridden!"));
    }
    static void EnsureSuccess(this Result<CustomFault> result)
    {
      return result.EnsureSuccess(new Exception("Overridden!"));
    }
  }
}
```

### EnsureFailure
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

To override exception thrown by default exception for a specific `TValue` or `TFault` you can implement an extension method with the specific `TValue` or `TFault` in your namespace. You can also use [inheritance](#inheritance) to override exception thrown by default for `TFault`or `TValue` in any namespace.
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
      return result.EnsureFailure(new Exception("Overridden!"));
    }
    static void EnsureFailure(this Result<CustomFault> result)
    {
      return result.EnsureFailure(new Exception("Overridden!"));
    }
  }
}
```

### HasSome
```csharp
Optional<string> optional = "has value";

bool extracted = optional.HasSome; // true
```

```csharp
Optional<string> optional = Optional.None();

bool extracted = optional.HasSome; // false
```

### IsNone
```csharp
Optional<string> optional = "has value";

bool extracted = optional.IsNone; // false
```

```csharp
Optional<string> optional = Optional.None();

bool extracted = optional.IsNone; // true
```

### Success
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

### Failure
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

### Implicit conversion to bool
```csharp
Optional<string> optional = ...;

if (optional)
{
  // On some
}
else
{
  // On none
}

bool extracted = optional;
```

```csharp
Result<Exception, string> result = ...;

if (result)
{
  // On success
}
else
{
  // On failure
}

bool extracted = result;
```

```csharp
Result<Exception> result = ...;

if (result)
{
  // On success
}
else
{
  // On failure
}

bool extracted = result;
```

### LINQ method syntax (GetValues, GetFaults)
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

### LINQ query syntax
```csharp
Optional<int> optional = Optional.Some(10);

IEnumerable<int> extracted =
  from value in optional
  from i1 in new [] { 1, 2 }
  from i2 in new [] { 100, 200 }
  select value + i1 + i2;

IEnumerable<int> extracted =
  from i1 in new [] { 1, 2 }
  from value in optional
  from i2 in new [] { 100, 200 }
  select i1 + value + i2;

// extracted is [111, 112, 211, 212].
```

```csharp
Optional<int> optional = Optional.None();

IEnumerable<int> extracted =
  from value in optional
  from i in new [] { 1, 2 }
  select value + i;

IEnumerable<int> extracted =
  from i in new [] { 1, 2 }
  from value in optional
  select value + i;

// extracted is empty.

```

```csharp
Result<Exception, int> result = Result.Succeed(10);

IEnumerable<int> extracted =
  from value in result
  from i1 in new [] { 1, 2 }
  from i2 in new [] { 100, 200 }
  select value + i1 + i2;

IEnumerable<int> extracted =
  from i1 in new [] { 1, 2 }
  from value in result
  from i2 in new [] { 100, 200 }
  select i1 + value + i2;

// extracted is [111, 112, 211, 212].
```

```csharp
Result<Exception, int> result = Result.Fail(new Exception());

IEnumerable<int> extracted =
  from value in result
  from i in new [] { 1, 2 }
  select value + i;

IEnumerable<int> extracted =
  from i in new [] { 1, 2 }
  from value in result
  select value + i;

// extracted is empty.

```

### foreach
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

### ToString
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

### Upcasts

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


## Conversion of generic arguments

All conversion methods except `Upcast` method support async extensions for every listed synchronous scenario.
All synchronous methods support upcasts of `TFault` and `TValue`.

Async extensions make use of methods returning `Task<T>`, `ValueTask<T>` and inline async lambdas. For example:
```csharp
async Task<int> Get1(int number) => number + 1;
async ValueTask<int> Get2(number) => number - 1;

Task<Optional<int>> Get(Task<Optional<int>> optional)
{
  return optional
    .MapValue(i => Get1(i))
    .MapValue(i => Get2(i))
    .MapValue(async i => await Get1(i));
}
```

If there is at least one async method in a chain returning `Task<T>` the result is `Task<T2>`. Otherwise, the result is `ValueTask<T2>`.

### MapValue

`MapValue` changes value or/and type of `TValue`.

```csharp
Optional<string> optional = ...;

Optional<string> extracted = optional.MapValue(str => str + "suffix");
Optional<string> extracted = optional.MapValue(() => 1.ToString());
Optional<string> extracted = optional.MapValue("other string");

Optional<int> extracted = optional.MapValue(str => int.Parse(str));
Optional<int> extracted = optional.MapValue(() => int.Parse("123"));
Optional<int> extracted = optional.MapValue(15);
```

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

```csharp
Result<Guid, string> result = ...;

Result<Guid, string> extracted = result.MapValue(str => str + "suffix");
Result<Guid, string> extracted = result.MapValue(() => 1.ToString());
Result<Guid, string> extracted = result.MapValue("other string");

Result<Guid, int> extracted = result.MapValue(str => int.Parse(str));
Result<Guid, int> extracted = result.MapValue(() => int.Parse("123"));
Result<Guid, int> extracted = result.MapValue(15);
```

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

### MapFault

`MapFault` changes value or/and type of `TFault`.

```csharp
Result<string, Guid> result = ...;

Result<string, Guid> extracted = result.MapFault(str => str + "suffix");
Result<string, Guid> extracted = result.MapFault(() => 1.ToString());
Result<string, Guid> extracted = result.MapFault("other string");

Result<int, Guid> extracted = result.MapFault(str => int.Parse(str));
Result<int, Guid> extracted = result.MapFault(() => int.Parse("123"));
Result<int, Guid> extracted = result.MapFault(15);
```

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

```csharp
Result<string> result = ...;

Result<string> extracted = result.MapFault(str => str + "suffix");
Result<string> extracted = result.MapFault(() => 1.ToString());
Result<string> extracted = result.MapFault("other string");

Result<int> extracted = result.MapFault(str => int.Parse(str));
Result<int> extracted = result.MapFault(() => int.Parse("123"));
Result<int> extracted = result.MapFault(15);
```

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

### Upcast

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

Async extensions are not supported `Upcast` methods.


## Result combining

There are some monadic extensions that can help you to reduce lines and errors in your code.
All `Optional`/`Result` combining methods support async extensions for every listed synchronous scenario.

Currently synchronous `Optional`/`Result` combining methods do not support upcasts of `TFault` and `TValue`.


### Then

`Then` is `Then` (`And`, `ContinueWith`, `ContinueOnSome`, `Bind`) `Result` combining method.

If a first optional/result is `None`/`Failure` then the `None`/`Failure` is returned.
If the first optional/result is `Some`/`Success` then a second optional/result is returned.
The second `Optional`/`Result` factory method is only executed if the first `Optional`/`Result` is `Some`/`Success`.

`TFault` types should be identical. Upcasts are not supported yet.
`TValue` types can be different.

```csharp
Optional<int> optional1 = ...;
Optional<string> optional2 = ...;

// If optional1 is None then None is returned.
// Otherwise optional2 is returned.
Optional<string> result = optional1.Then(optional2);
Optional<string> result = optional1.Then(() => optional2);
Optional<string> result = optional1.Then(i => i > 10 ? Optional<string>.Some(i.ToString()) : Optional<string>.None());
```

Async examples:
```csharp
Optional<int> optional1 = ...;
Optional<string> optional2 = ...;

ValueTask<Optional<string>> result = optional1.Then(async value => await Task.FromResult(optional2));
ValueTask<Optional<string>> result = optional1.Then(value => new ValueTask(optional2));
Task<Optional<string>> result = optional1.Then(value => Task.FromResult(optional2));

ValueTask<Optional<string>> result = new ValueTask(optional1).Then(value => optional2);
ValueTask<Optional<string>> result = new ValueTask(optional1).Then(async value => await Task.FromResult(optional2));
ValueTask<Optional<string>> result = new ValueTask(optional1).Then(value => new ValueTask(optional2));
Task<Optional<string>> result = new ValueTask(optional1).Then(value => Task.FromResult(optional2));

Task<Optional<string>> result = Task.FromResult(optional1).Then(value => optional2);
Task<Optional<string>> result = Task.FromResult(optional1).Then(async value => await Task.FromResult(optional2));
Task<Optional<string>> result = Task.FromResult(optional1).Then(value => new ValueTask(optional2));
Task<Optional<string>> result = Task.FromResult(optional1).Then(value => Task.FromResult(optional2));
```

```csharp
Result<Exception, int> result1 = ...;
Result<Exception, string> result2 = ...;

// If option1 is None then None is returned.
// Otherwise option2 is returned.
Result<Exception, string> result = result1.Then(result2);
Result<Exception, string> result = result1.Then(() => result2);
Result<Exception, string> result = result1.Then(i => i > 10 ? Result<Exception, string>.Success(i.ToString()) : Result<Exception, string>.Failure());
```

Async examples:
```csharp
Result<Exception, int> result1 = ...;
Result<Exception, string> result2 = ...;

ValueTask<Result<Exception, string>> result = result1.Then(async value => await Task.FromResult(result2));
ValueTask<Result<Exception, string>> result = result1.Then(value => new ValueTask(result2));
Task<Result<Exception, string>> result = result1.Then(value => Task.FromResult(result2));

ValueTask<Result<Exception, string>> result = new ValueTask(result1).Then(value => result2);
ValueTask<Result<Exception, string>> result = new ValueTask(result1).Then(async value => await Task.FromResult(result2));
ValueTask<Result<Exception, string>> result = new ValueTask(result1).Then(value => new ValueTask(result2));
Task<Result<Exception, string>> result = new ValueTask(result1).Then(value => Task.FromResult(result2));

Task<Result<Exception, string>> result = Task.FromResult(result1).Then(value => result2);
Task<Result<Exception, string>> result = Task.FromResult(result1).Then(async value => await Task.FromResult(result2));
Task<Result<Exception, string>> result = Task.FromResult(result1).Then(value => new ValueTask(result2));
Task<Result<Exception, string>> result = Task.FromResult(result1).Then(value => Task.FromResult(result2));
```

All meaningful combinations of `Optional<TValue>`, `Result<TFault>` and `Result<TFault, TValue>` are also supported.

### OrElse

`OrElse` is `Or` (`OrElse`, `Else`, `Catch`, `ContinueOnNone`) `Result` combining method.

If a first optional/result is `Some`/`Success` then it is returned.
If the first optional/result is `None`/`Failure` then a second optional/result is returned.
The second `Optional`/`Result` factory method is only executed if the first `Optional`/`Result` is `None`/`Failure`.

`TValue` types should be identical. Upcasts are not supported yet.
`TFault` types can be different.


```csharp
Optional<string> optional1 = ...;
Optional<string> optional2 = ...;

// If optional1 is Some then option1 is returned.
// Otherwise optional2 is returned.
Optional<string> result = optional1.OrElse(optional2);
Optional<string> result = optional1.OrElse(() => optional2);
```

Async examples:
```csharp
Optional<string> optional1 = ...;
Optional<string> optional2 = ...;

ValueTask<Optional<string>> result = optional1.OrElse(async () => await Task.FromResult(optional2));
ValueTask<Optional<string>> result = optional1.OrElse(() => new ValueTask(optional2));
Task<Optional<string>> result = optional1.OrElse(() => Task.FromResult(optional2));

ValueTask<Optional<string>> result = new ValueTask(optional1).OrElse(() => optional2);
ValueTask<Optional<string>> result = new ValueTask(optional1).OrElse(async () => await Task.FromResult(optional2));
ValueTask<Optional<string>> result = new ValueTask(optional1).OrElse(() => new ValueTask(optional2));
Task<Optional<string>> result = new ValueTask(optional1).OrElse(() => Task.FromResult(optional2));

Task<Optional<string>> result = Task.FromResult(optional1).OrElse(() => optional2);
Task<Optional<string>> result = Task.FromResult(optional1).OrElse(async () => await Task.FromResult(optional2));
Task<Optional<string>> result = Task.FromResult(optional1).OrElse(() => new ValueTask(optional2));
Task<Optional<string>> result = Task.FromResult(optional1).OrElse(() => Task.FromResult(optional2));
```

```csharp
Result<int, string> result1 = ...;
Result<Exception, string> result2 = ...;

// If option1 is Some then option1 is returned.
// Otherwise option2 is returned.
Result<Exception, string> result = result1.OrElse(result2);
Result<Exception, string> result = result1.OrElse(() => result2);
Result<Exception, string> result = result1.OrElse(fault => result2);
```

Async examples:
```csharp
Result<int, string> result1 = ...;
Result<Exception, string> result2 = ...;

ValueTask<Result<Exception, string>> result = result1.OrElse(async fault => await Task.FromResult(result2));
ValueTask<Result<Exception, string>> result = result1.OrElse(fault => new ValueTask(result2));
Task<Result<Exception, string>> result = result1.OrElse(fault => Task.FromResult(result2));

ValueTask<Result<sException, tring>> result = new ValueTask(result1).OrElse(fault => result2);
ValueTask<Result<sException, tring>> result = new ValueTask(result1).OrElse(async fault => await Task.FromResult(result2));
ValueTask<Result<Exception, string>> result = new ValueTask(result1).OrElse(fault => new ValueTask(result2));
Task<Result<Exception, string>> result = new ValueTask(result1).OrElse(fault => Task.FromResult(result2));

Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(fault => result2);
Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(async fault => await Task.FromResult(result2));
Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(fault => new ValueTask(result2));
Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(fault => Task.FromResult(result2));
```

All meaningful combinations of `Optional<TValue>`, `Result<TFault>` and `Result<TFault, TValue>` are also supported.

### Then/OrElse method chaining

```csharp
abstract Optional<string> GetFormLogin();
abstract Optional<Guid> GetUser(string login);
abstract Task<Optional<Guid>> CreateUser(string login);
abstract Optional<DateTime> GetCreationDate(Guid userId);

Task<Optional<long>> userCreationDateTicks =
   GetFormLogin()
  .Then(login => GetUser(login).OrElse(() => CreateUser(login)))
  .Then(userId => GetCreationDate(userId))
  .MapValue(date => date.Ticks);
```

```csharp
abstract Result<TicksError, string> GetFormLogin();
abstract Result<UserException, Guid> GetUser(string login);
abstract Task<Result<TicksError, Guid>> CreateUser(string login);
abstract Result<TicksError, DateTime> GetCreationDate(Guid userId);
abstract Result<TicksError> EnsureTicksIsValid(long ticks);

Task<Result<GetTicksError>> userCreationDateTicksValid =
   GetFormLogin()
  .Then(login => GetUser(login).OrElse(() => CreateUser(login)))
  .Then(userId => GetCreationDate(userId))
  .MapValue(date => date.Ticks);
  .Then(ticks => EnsureTicksIsValid(ticks))
```

### Select

`Select` is a mix of `MapValue` and `Then`.
Like both of them, a factory method of a second `Optional`/`Result` or value is only executed if the first `Optional`/`Result` is `Some`/`Success`.
Like `MapValue` it allows changing type and value by using value factory.
Unlike `MapValue` and like `Then` it allows creating a second `Optional`/`Result` which is a result of the whole operation.
It also allows you to change `Some`/`Success` to `None`/`Failure` if you want.

`TFault` types should be identical. Upcasts are not supported yet.
`TValue` types can be different.

```csharp
Optional<string> optional = ...;

Optional<string> extracted = optional.Select(str => str.Length > 5 ? Optional.Some(str) : Optional.None());
Optional<string> extracted = optional.Select(str => str + "suffix");
Optional<int> extracted = optional.Select(str => int.Parse(str));

ValueTask<Optional<int>> extracted = optional.Select(async str => await Task.FromResult(int.Parse(str)));
ValueTask<Optional<int>> extracted = optional.Select(str => new ValueTask(int.Parse(str)));
Task<Optional<int>> extracted = optional.Select(str => Task.FromResult(int.Parse(str)));

ValueTask<Optional<int>> extracted = new ValueTask(optional).Select(async str => await Task.FromResult(int.Parse(str)));
ValueTask<Optional<int>> extracted = new ValueTask(optional).Select(str => new ValueTask(int.Parse(str)));
Task<Optional<int>> extracted = new ValueTask(optional).Select(str => Task.FromResult(int.Parse(str)));

Task<Optional<int>> extracted = Task.FromResult(optional).Select(async str => await Task.FromResult(int.Parse(str)));
Task<Optional<int>> extracted = Task.FromResult(optional).Select(str => new ValueTask(int.Parse(str)));
Task<Optional<int>> extracted = Task.FromResult(optional).Select(str => Task.FromResult(int.Parse(str)));
```

```csharp
Result<Exception, string> result = ...;

Result<Exception, string> extracted = result.Select(str => str.Length > 5 ? Result<Exception, string>.Succeed(str) : Result<Exception, string>.Fail(new Exception(str)));
Result<Exception, string> extracted = result.Select(str => str + "suffix");
Result<Exception, int> extracted = result.Select(str => int.Parse(str));

ValueTask<Optional<int>> extracted = result.Select(async str => await Task.FromResult(int.Parse(str)));
ValueTask<Optional<int>> extracted = result.Select(str => new ValueTask(int.Parse(str)));
Task<Optional<int>> extracted = result.Select(str => Task.FromResult(int.Parse(str)));

ValueTask<Optional<int>> extracted = new ValueTask(result).Select(str => int.Parse(str));
ValueTask<Optional<int>> extracted = new ValueTask(result).Select(async str => await Task.FromResult(int.Parse(str)));
ValueTask<Optional<int>> extracted = new ValueTask(result).Select(str => new ValueTask(int.Parse(str)));
Task<Optional<int>> extracted = new ValueTask(result).Select(str => Task.FromResult(int.Parse(str)));

Task<Optional<int>> extracted = Task.FromResult(result).Select(str => int.Parse(str));
Task<Optional<int>> extracted = Task.FromResult(result).Select(async str => await Task.FromResult(int.Parse(str)));
Task<Optional<int>> extracted = Task.FromResult(result).Select(str => new ValueTask(int.Parse(str)));
Task<Optional<int>> extracted = Task.FromResult(result).Select(str => Task.FromResult(int.Parse(str)));
```

### Do notation
```csharp
abstract Optional<Guid> GetCurrentUserId();
abstract Optional<int> GetCurrentIndex();
abstract Optional<Product> GetCurrentProduct();
abstract Optional<string> GetMessage(Guid userId, int index, Product product);
abstract Optional<ConvertResult> Convert(string message, Product product);

Optional<ConvertResult> result =
  from userId  in GetCurrentUserId()
  where userId != Guid.Empty
  from index   in GetCurrentIndex()
  from product in GetCurrentProduct()
  let nextIndex = index + 1
  from message in GetMessage(userId, nextIndex, product)
  select Convert(message, product);

```
The last `select` expression can return either `Optional<TResult>` or just `TResult`.

```csharp
abstract Result<Exception, Guid> GetCurrentUserId();
abstract Result<Exception> EnsureUserIdIsCorrect(Guid userId);
abstract Result<Exception, int> GetCurrentIndex();
abstract Result<Exception, Product> GetCurrentProduct();
abstract Result<int, string> GetMessage(Guid userId, int index, Product product);
abstract Result<Exception, ConvertResult> Convert(string message, Product product);

Result<Exception, ConvertResult> result =
  from userId  in GetCurrentUserId()
  where EnsureUserIdIsCorrect(userId)
  from index   in GetCurrentIndex()
  from product in GetCurrentProduct()
  let nextIndex = index + 1
  from message in GetMessage(userId, nextIndex, product).MapFault(i => new Exception(i.ToString()))
  select Convert(message, product);

```
The last `select` expression can return `Result<Exception, TResult>`, `Result<Exception>` or just `TResult`.
`TFault` should be identical in all clauses. Use `MapFault` to convert different `TFault` to the same one.

### Do notation with async extensions
```csharp
abstract Optional<Guid> GetCurrentUserId();
abstract Task<int> GetCurrentIndex();
abstract ValueTask<Optional<Product>> GetCurrentProduct();
abstract Task<Optional<string>> GetMessage(Guid userId, int index, Product product);
abstract Task<Format> GetFormat(int index, string message);
abstract Optional<ConvertResult> Convert(string message, Format format);
abstract Task<bool> IsValid(int index);

Task<Optional<ConvertResult>> result =
  from userId  in GetCurrentUserId() // A
  where userId != Guid.Empty       // B
  from index   in GetCurrentIndex() // C
  from product in GetCurrentProduct() // D
  let nextIndex = index + 1
  where IsValid(nextIndex) // E
  from message in GetMessage(userId, nextIndex, product) // F
  from format  in GetFormat(nextIndex, message) // G
  select Convert(message, format); // H

```
Where:
* `A` (first `from`/`in` clause) must return `Optional<T>`, `ValueTask<Optional<T>>` or `Task<Optional<T>>`.
* `B` and `E` may return `bool`, `ValueTask<bool>` or `Task<bool>`.
* `C`, `D`, `F` and `G` may return one of `Optional<T>`, `ValueTask<T>`, `ValueTask<Optional<T>>`, `Task<T>` or `Task<Optional<T>>`. Subsequent expressions may depend on previous expressions (`F` and `G` for example) or may not depend on previous expressions (`C` and `D` for example). The total count of `B`, `C`, `D`, `E`, `F` and `G`-like statements is efficiently unlimited.
* `H` should return  `TResult`, `Optional<TResult>`, `ValueTask<TResult>`, `ValueTask<Optional<TResult>>`, `Task<TResult>` or `Task<Optional<TResult>>`.

```csharp
abstract Result<Exception, Guid> GetCurrentUserId();
abstract Result<Exception> EnsureUserIdIsCorrect(Guid userId);
abstract Task<int> GetCurrentIndex();
abstract ValueTask<Result<Exception, Product>> GetCurrentProduct();
abstract Task<Result<int, string>> GetMessage(Guid userId, int index, Product product);
abstract Task<Format> GetFormat(int index, string message);
abstract Result<Exception, ConvertResult> Convert(string message, Format format);
abstract Task<Result<Exception>> IsValid(int index);

Task<Result<Exception, ConvertResult>> result =
  from userId  in GetCurrentUserId() // A
  where EnsureUserIdIsCorrect(userId) // B
  from index   in GetCurrentIndex() // C
  from product in GetCurrentProduct() // D
  let nextIndex = index + 1
  where IsValid(nextIndex) // E
  from message in GetMessage(userId, nextIndex, product).MapFault(i => new Exception(i.ToString())) // F
  from format  in GetFormat(nextIndex, message) // G
  select Convert(message, format); // H
```

Where:
* `A` (first `from`/`in` clause) must return `Result<TFault, TValue>`, `ValueTask<Result<TFault, TValue>>` or `Task<Result<TFault, TValue>>`.
* `B` and `E` may return `Result<TFault>`, `ValueTask<Result<TFault>>` or `Task<Result<TFault>>`.
* `C`, `D`, `F` and `G` may return one of `Result<TFault, TValue>`, `ValueTask<TValue>`, `ValueTask<Result<TFault, TValue>>`, `Task<TValue>` or `Task<Result<TFault, TValue>>`. Subsequent expressions may depend on previous expressions (`F` and `G` for example) or may not depend on previous expressions (`C` and `D` for example). The total count of `B`, `C`, `D`, `E`, `F` and `G`-like statements is efficiently unlimited.
* `H` should return  `TResult`, `Result<TFault, TResult>`, `ValueTask<TResult>`, `ValueTask<Result<TFault, TResult>>`, `Task<TResult>` or `Task<Result<TFault, TResult>>`.

`TFault` should be identical in all clauses. Use `MapFault` to convert different `TFault` to the same one.


## Inheritance

You can create your own `Result` or `Option` type by inheriting provided types.

```csharp
class StringFaultResult<TValue> : Result<string, TValue>
{
  private readonly Result<string, TValue> result;

  public StringFaultResult(string fault) => result = Fail(fault);

  public StringFaultResult(TValue value) => result = Succeed(value);

  public override TResult Match<TResult>(Func<string, TResult> onFailure, Func<TValue, TResult> onSuccess)
    => result.Match(onFailure, onSuccess);
}
```

All synchronous methods are inherited.

If you inherit, to bring back some lost features you can reimplement the following:
* implicit conversion operators
* some async extensions that do not inherit by default due to invariance of `Task<T>` and `ValueTask<T>`

Because this implementation relies heavily on C# 9 source generators it is not a hard task to implement a new feature that reimplements lost features automatically.
Send me requests if you need such a feature.

You can also override extensions methods. For example:
* `MapValue` to override return type with inherited type
* `GetValueOrThrow` to override Exception type thrown by default


## Other

* `Equals` and `GetHashCode` make use of type arguments, faults and values (if present) for calculations.
* There is no `fold` method.


## Yet to be implemented

* (easy) Currently only `Result` combining methods (`Map`, `Select`, `Then`, `OrElse` and do notation), `MapValue` and `MapFault` support async extensions with `Task` or `ValueTask` types. Data extraction methods (like `TryGetValue`, `Switch` or `GetOrThrow`) only support synchronous execution.
* (easy) Currently monadic extensions (`Select`, `Then`, `OrElse` and do notation) do not support upcasts for synchronous methods.
* (medium) Currently implemented source generators can not generate async extensions and implicit conversion operators for custom inherited classes in your assemblies.
* (hard) Do notation for `Result<TFault, TValue>` with different `TFault` type arguments is possible in a limited way but unimplemented. If it is implemented, it would disallow a few struct type arguments for `TFault` and would not enable all scenarios of selecting `Result<TFaultOther>` with different `TFault` argument in subsequent `from/in` clauses. The current workaround is to use `MapFault` method before passing `Result` to a do notation clause.

Send me your requests if you need such features.


## Contributing

Contributions are appreciated.
