# Inheritance

You can create your own `Result` type by inheriting provided types.

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
