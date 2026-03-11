using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    /// <summary>Factory methods for creating <see cref="Result{TFault}"/> and <see cref="Result{TFault, TValue}"/> instances.</summary>
    public static class Result
    {
        /// <summary>Creates a failure result containing the specified fault. For success, use <see cref="Succeed()"/> or <see cref="Succeed{TValue}(TValue)"/>.</summary>
        [Pure]
        public static ResultFailure<TFault> Fail<TFault>(TFault fault)
        {
            return new(fault);
        }

        /// <summary>Creates a success marker that implicitly converts to <see cref="Result{TFault}"/>. For failure, use <see cref="Fail{TFault}(TFault)"/>.</summary>
        [Pure]
        public static SuccessMarker Succeed()
        {
            return default;
        }

        /// <inheritdoc cref="Result{TFault}.Succeed()"/>
        [Pure]
        public static Result<TFault> Succeed<TFault>()
        {
            return Result<TFault>.Succeed();
        }

        /// <summary>Creates a success marker that implicitly converts to <see cref="Result{TFault, TValue}"/>. For failure, use <see cref="Fail{TFault}(TFault)"/>. For a simpler overload, use <see cref="Succeed()"/>.</summary>
        [Pure]
        public static SuccessMarker<TValue> Succeed<TValue>(TValue value)
        {
            return new(value);
        }
    }
}
