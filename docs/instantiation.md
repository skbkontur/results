# Instantiation

Creating `Optional<TValue>`, `Result<TFault>`, and `Result<TFault, TValue>` instances.

## Explicit examples

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

## Implicit examples

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

## Ternary expressions

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

## Conversions

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
