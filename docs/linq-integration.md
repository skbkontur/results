# LINQ Query Syntax Integration with IEnumerable

There is no preferred way of combining the `IEnumerable` and `Optional` or `Result`.
You can choose a variant that suits your needs and implement it in your project.
Some variants are found in the tests folder.

Some examples:

## Treat `Result` and `Optional` as containers with single or no elements

This is the simplest variant. But using it results in losing fault details.

### Optional
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
Optional<int> optional = Optional.Some(10);

IEnumerable<int> extracted =
  from value in optional
  from i1 in new [] { 1, 2 }
  from i2 in Array.Empty<int>()
  select value + i1 + i2;

IEnumerable<int> extracted =
  from i1 in new [] { 1, 2 }
  from value in optional
  from i2 in Array.Empty<int>()
  select i1 + value + i2;

// extracted is empty.
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

<details>
  <summary>To implement it you can use the code</summary>

```csharp
public static IEnumerable<TResult> SelectMany<TValue, TItem, TResult>(
  this IOptional<TValue> optional,
  Func<TValue, IEnumerable<TItem>> collectionSelector,
  Func<TValue, TItem, TResult> resultSelector)
{
  return optional.GetValues().SelectMany(collectionSelector, resultSelector);
}

public static IEnumerable<TResult> SelectMany<TItem, TValue, TResult>(
  this IEnumerable<TItem> collection,
  Func<TItem, IOptional<TValue>> optionSelector,
  Func<TItem, TValue, TResult> resultSelector)
{
  return collection.SelectMany(value => optionSelector(value).GetValues(), resultSelector);
}
```
</details>

### Result
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
Result<Exception, int> result = Result.Succeed(10);

IEnumerable<int> extracted =
  from value in result
  from i1 in new [] { 1, 2 }
  from i2 in Array.Empty<int>()
  select value + i1 + i2;

IEnumerable<int> extracted =
  from i1 in new [] { 1, 2 }
  from value in result
  from i2 in Array.Empty<int>()
  select i1 + value + i2;

// extracted is empty.
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

<details>
  <summary>To implement it you can use the code</summary>

```csharp
public static IEnumerable<TResult> SelectMany<TFault, TValue, TItem, TResult>(
  this IResult<TFault, TValue> result,
  Func<TValue, IEnumerable<TItem>> collectionSelector,
  Func<TValue, TItem, TResult> resultSelector)
{
  return result.GetValues().SelectMany(collectionSelector, resultSelector);
}

public static IEnumerable<TResult> SelectMany<TItem, TFault, TValue, TResult>(
  this IEnumerable<TItem> collection,
  Func<TItem, IResult<TFault, TValue>> selector,
  Func<TItem, TValue, TResult> resultSelector)
{
  return collection.SelectMany(value => selector(value).GetValues(), resultSelector);
}
```
</details>

## Returning the first found fault

This is is the most useful way of combining data.

### Optional

```csharp
Optional<int> optional = Optional.Some(10);

Optional<IEnumerable<int>> extracted =
  from value in optional
  from i1 in new [] { 1, 2 }
  from i2 in new [] { 100, 200 }
  select value + i1 + i2;

Optional<IEnumerable<int>> extracted =
  from i1 in new [] { 1, 2 }
  from value in optional
  from i2 in new [] { 100, 200 }
  select i1 + value + i2;

// extracted is [111, 112, 211, 212].
```

```csharp
Optional<int> optional = Optional.Some(10);

Optional<IEnumerable<int>> extracted =
  from value in optional
  from i1 in new [] { 1, 2 }
  from i2 in Array.Empty<int>()
  select value + i1 + i2;

Optional<IEnumerable<int>> extracted =
  from i1 in new [] { 1, 2 }
  from value in optional
  from i2 in Array.Empty<int>()
  select i1 + value + i2;

// extracted is empty.
```

```csharp
Optional<int> optional = Optional.None();

Optional<IEnumerable<int>> extracted =
  from value in optional
  from i in new [] { 1, 2 }
  select value + i;

Optional<IEnumerable<int>> extracted =
  from i in new [] { 1, 2 }
  from value in optional
  select value + i;

// extracted is `None`.

```

<details>
  <summary>To implement it, four `SelectMany` methods can be used</summary>

```csharp
static Optional<IEnumerable<TResult>> SelectMany<TItem, TValue, TResult>(
  this IEnumerable<TItem> collection,
  Func<TItem, Optional<TValue>> optionalSelector,
  Func<TItem, TValue, TResult> resultSelector)
{
  List<TResult> results = new();
  foreach (var item in collection)
  {
    if (!optionalSelector(item).TryGetValue(out var value))
      return Optional.None();
    results.Add(resultSelector(item, value));
  }
  return results;
}

static Optional<IEnumerable<TResult>> SelectMany<TItem1, TItem2, TResult>(
  this Optional<IEnumerable<TItem1>> optional,
  Func<TItem1, IEnumerable<TItem2>> collectionSelector,
  Func<TItem1, TItem2, TResult> resultSelector) =>
  optional.MapValue(values => values.SelectMany(value => collectionSelector(value).Select(item => resultSelector(value, item))));

static Optional<IEnumerable<TResult>> SelectMany<TValue, TItem, TResult>(
  this Optional<IEnumerable<TValue>> optional,
  Func<TValue, Optional<TItem>> collectionSelector,
  Func<TValue, TItem, TResult> resultSelector) =>
  optional.Match(
    Optional<IEnumerable<TResult>>.None,
    values => SelectMany(values, collectionSelector, resultSelector));

// To allow infinite chaining the following method could be placed in local namespace and other 3 methods can be placed in root namespace
static Optional<IEnumerable<TResult>> SelectMany<TValue, TItem, TResult>(
  this Optional<TValue> optional,
  Func<TValue, IEnumerable<TItem>> collectionSelector,
  Func<TValue, TItem, TResult> resultSelector) =>
  optional.MapValue(value => collectionSelector(value).Select(item => resultSelector(value, item)));
```
</details>

### Result
```csharp
Result<Exception, int> result = Result.Succeed(10);

Result<Exception, IEnumerable<int>> extracted =
  from value in result
  from i1 in new [] { 1, 2 }
  from i2 in new [] { 100, 200 }
  select value + i1 + i2;

Result<Exception, IEnumerable<int>> extracted =
  from i1 in new [] { 1, 2 }
  from value in result
  from i2 in new [] { 100, 200 }
  select i1 + value + i2;

// extracted is [111, 112, 211, 212].
```

```csharp
Result<Exception, int> result = Result.Succeed(10);

Result<Exception, IEnumerable<int>> extracted =
  from value in result
  from i1 in new [] { 1, 2 }
  from i2 in Array.Empty<int>()
  select value + i1 + i2;

Result<Exception, IEnumerable<int>> extracted =
  from i1 in new [] { 1, 2 }
  from value in result
  from i2 in Array.Empty<int>()
  select i1 + value + i2;

// extracted is empty.
```

```csharp
Result<Exception, int> result = Result.Fail(new Exception("fault"));

Result<Exception, IEnumerable<int>> extracted =
  from value in result
  from i in new [] { 1, 2 }
  select value + i;

Result<Exception, IEnumerable<int>> extracted =
  from i in new [] { 1, 2 }
  from value in result
  select value + i;

// extracted is Exception("fault").

```

<details>
  <summary>To implement it, four `SelectMany` methods can be used</summary>

```csharp
static Result<TFault, IEnumerable<TResult>> SelectMany<TItem, TFault, TValue, TResult>(
  this IEnumerable<TItem> collection,
  Func<TItem, Result<TFault, TValue>> selector,
  Func<TItem, TValue, TResult> resultSelector)
{
  List<TResult> results = new();
  foreach (var item in collection)
  {
    if (selector(item).TryGetFault(out var fault, out var value))
      return fault;
    results.Add(resultSelector(item, value));
  }
  return results;
}

static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TItem1, TItem2, TResult>(
  this Result<TFault, IEnumerable<TItem1>> result,
  Func<TItem1, IEnumerable<TItem2>> collectionSelector,
  Func<TItem1, TItem2, TResult> resultSelector) =>
  result.MapValue(values => values.SelectMany(value => collectionSelector(value).Select(item => resultSelector(value, item))));

static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TValue, TItem, TResult>(
  this Result<TFault, IEnumerable<TValue>> result,
  Func<TValue, Result<TFault, TItem>> collectionSelector,
  Func<TValue, TItem, TResult> resultSelector) =>
  result.Match(
    Result<TFault, IEnumerable<TResult>>.Fail,
    values => SelectMany(values, collectionSelector, resultSelector));

// To allow infinite chaining the following method could be placed in local namespace and other 3 methods can be placed in root namespace
static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TValue, TItem, TResult>(
            this Result<TFault, TValue> result,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector) =>
            result.MapValue(value => collectionSelector(value).Select(item => resultSelector(value, item)));
```
</details>
