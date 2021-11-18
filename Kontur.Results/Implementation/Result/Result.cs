using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class Result
    {
        [Pure]
        public static ResultFailure<TFault> Fail<TFault>(TFault fault)
        {
            return new(fault);
        }

        [Pure]
        public static SuccessMarker Succeed()
        {
            return default;
        }

        [Pure]
        public static Result<TFault> Succeed<TFault>()
        {
            return Result<TFault>.Succeed();
        }

        [Pure]
        public static SuccessMarker<TValue> Succeed<TValue>(TValue value)
        {
            return new(value);
        }
    }
}