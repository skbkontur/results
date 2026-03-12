# Result Combining (Monadic Extensions)

Monadic extensions for chaining `Optional` and `Result` operations: `Then`, `OrElse`, `Select`, and do notation.

All `Optional`/`Result` combining methods support async extensions for every listed synchronous scenario.

Currently synchronous `Optional`/`Result` combining methods do not support upcasts of `TFault` and `TValue`.

## Content

* [Then](#then)
* [OrElse](#orelse)
* [Then/OrElse method chaining](#thenorelse-method-chaining)
* [Select](#select)
* [Do notation](#do-notation)
* [Do notation with async extensions](#do-notation-with-async-extensions)


## Then

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

<details>
  <summary>Async examples</summary>

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
</details>

```csharp
Result<Exception, int> result1 = ...;
Result<Exception, string> result2 = ...;

// If result1 is Failure then Failure is returned.
// Otherwise result2 is returned.
Result<Exception, string> result = result1.Then(result2);
Result<Exception, string> result = result1.Then(() => result2);
Result<Exception, string> result = result1.Then(i => i > 10 ? Result<Exception, string>.Succeed(i.ToString()) : Result<Exception, string>.Fail(new Exception()));
```

<details>
  <summary>Async examples</summary>

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
</details>

All meaningful combinations of `Optional<TValue>`, `Result<TFault>` and `Result<TFault, TValue>` are also supported.

## OrElse

`OrElse` is `Or` (`OrElse`, `Else`, `Catch`, `ContinueOnNone`) `Result` combining method.

If a first optional/result is `Some`/`Success` then it is returned.
If the first optional/result is `None`/`Failure` then a second optional/result is returned.
The second `Optional`/`Result` factory method is only executed if the first `Optional`/`Result` is `None`/`Failure`.

`TValue` types should be identical. Upcasts are not supported yet.
`TFault` types can be different.


```csharp
Optional<string> optional1 = ...;
Optional<string> optional2 = ...;

// If optional1 is Some then optional1 is returned.
// Otherwise optional2 is returned.
Optional<string> result = optional1.OrElse(optional2);
Optional<string> result = optional1.OrElse(() => optional2);
```

<details>
  <summary>Async examples</summary>

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
</details>

```csharp
Result<int, string> result1 = ...;
Result<Exception, string> result2 = ...;

// If result1 is Success then result1 is returned.
// Otherwise result2 is returned.
Result<Exception, string> result = result1.OrElse(result2);
Result<Exception, string> result = result1.OrElse(() => result2);
Result<Exception, string> result = result1.OrElse(fault => result2);
```

<details>
  <summary>Async examples</summary>

```csharp
Result<int, string> result1 = ...;
Result<Exception, string> result2 = ...;

ValueTask<Result<Exception, string>> result = result1.OrElse(async fault => await Task.FromResult(result2));
ValueTask<Result<Exception, string>> result = result1.OrElse(fault => new ValueTask(result2));
Task<Result<Exception, string>> result = result1.OrElse(fault => Task.FromResult(result2));

ValueTask<Result<Exception, string>> result = new ValueTask(result1).OrElse(fault => result2);
ValueTask<Result<Exception, string>> result = new ValueTask(result1).OrElse(async fault => await Task.FromResult(result2));
ValueTask<Result<Exception, string>> result = new ValueTask(result1).OrElse(fault => new ValueTask(result2));
Task<Result<Exception, string>> result = new ValueTask(result1).OrElse(fault => Task.FromResult(result2));

Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(fault => result2);
Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(async fault => await Task.FromResult(result2));
Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(fault => new ValueTask(result2));
Task<Result<Exception, string>> result = Task.FromResult(result1).OrElse(fault => Task.FromResult(result2));
```
</details>

All meaningful combinations of `Optional<TValue>`, `Result<TFault>` and `Result<TFault, TValue>` are also supported.

## Then/OrElse method chaining

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

Task<Result<TicksError>> userCreationDateTicksValid =
   GetFormLogin()
  .Then(login => GetUser(login).OrElse(() => CreateUser(login)))
  .Then(userId => GetCreationDate(userId))
  .MapValue(date => date.Ticks)
  .Then(ticks => EnsureTicksIsValid(ticks))
```

## Select

`Select` is a mix of `MapValue` and `Then`.
Like both of them, a factory method of a second `Optional`/`Result` or value is only executed if the first `Optional`/`Result` is `Some`/`Success`.
Like `MapValue` it allows changing type and value by using value factory.
Unlike `MapValue` and like `Then` it allows creating a second `Optional`/`Result` which is a result of the whole operation.
It also allows you to change `Some`/`Success` to `None`/`Failure` if you want.

`TFault` types should be identical. Upcasts are not supported yet.
`TValue` types can be different.

<details>
  <summary>Optional examples</summary>

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
</details>

<details>
  <summary>Result examples</summary>

```csharp
Result<Exception, string> result = ...;

Result<Exception, string> extracted = result.Select(str => str.Length > 5 ? Result<Exception, string>.Succeed(str) : Result<Exception, string>.Fail(new Exception(str)));
Result<Exception, string> extracted = result.Select(str => str + "suffix");
Result<Exception, int> extracted = result.Select(str => int.Parse(str));

ValueTask<Result<Exception, int>> extracted = result.Select(async str => await Task.FromResult(int.Parse(str)));
ValueTask<Result<Exception, int>> extracted = result.Select(str => new ValueTask(int.Parse(str)));
Task<Result<Exception, int>> extracted = result.Select(str => Task.FromResult(int.Parse(str)));

ValueTask<Result<Exception, int>> extracted = new ValueTask(result).Select(str => int.Parse(str));
ValueTask<Result<Exception, int>> extracted = new ValueTask(result).Select(async str => await Task.FromResult(int.Parse(str)));
ValueTask<Result<Exception, int>> extracted = new ValueTask(result).Select(str => new ValueTask(int.Parse(str)));
Task<Result<Exception, int>> extracted = new ValueTask(result).Select(str => Task.FromResult(int.Parse(str)));

Task<Result<Exception, int>> extracted = Task.FromResult(result).Select(str => int.Parse(str));
Task<Result<Exception, int>> extracted = Task.FromResult(result).Select(async str => await Task.FromResult(int.Parse(str)));
Task<Result<Exception, int>> extracted = Task.FromResult(result).Select(str => new ValueTask(int.Parse(str)));
Task<Result<Exception, int>> extracted = Task.FromResult(result).Select(str => Task.FromResult(int.Parse(str)));
```
</details>

## Do notation

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

## Do notation with async extensions
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
* `B` and `E` (`where` clause) may return `bool`, `ValueTask<bool>` or `Task<bool>`.
* `C`, `D`, `F` and `G`  (subsequent `from`/`in` clause) may return one of `Optional<T>`, `ValueTask<T>`, `ValueTask<Optional<T>>`, `Task<T>` or `Task<Optional<T>>`. Subsequent expressions may depend on previous expressions (`F` and `G` for example) or may not depend on previous expressions (`C` and `D` for example). The total count of `B`, `C`, `D`, `E`, `F` and `G`-like statements is efficiently unlimited.
* `H` (`select` clause) should return  `TResult`, `Optional<TResult>`, `ValueTask<TResult>`, `ValueTask<Optional<TResult>>`, `Task<TResult>` or `Task<Optional<TResult>>`.

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
* `B` and `E` (`where` clause) may return `Result<TFault>`, `ValueTask<Result<TFault>>` or `Task<Result<TFault>>`.
* `C`, `D`, `F` and `G` (subsequent `from`/`in` clause) may return one of `Result<TFault, TValue>`, `ValueTask<TValue>`, `ValueTask<Result<TFault, TValue>>`, `Task<TValue>` or `Task<Result<TFault, TValue>>`. Subsequent expressions may depend on previous expressions (`F` and `G` for example) or may not depend on previous expressions (`C` and `D` for example). The total count of `B`, `C`, `D`, `E`, `F` and `G`-like statements is efficiently unlimited.
* `H` (`select` clause) should return  `TResult`, `Result<TFault>`, `Result<TFault, TResult>`, `ValueTask<TResult>`, `ValueTask<Result<TFault>>`, `ValueTask<Result<TFault, TResult>>`, `Task<TResult>`, `Task<Result<TFault>>` or `Task<Result<TFault, TResult>>`.

`TFault` should be identical in all clauses. Use `MapFault` to convert different `TFault` to the same one.
