namespace Kontur.Tests.Results.Inheritance
{
    public static class StringFaultResult
    {
        public static StringFaultResult<TValue> Fail<TValue>(StringFault fault)
        {
            return new(fault);
        }

        public static StringFaultResult<TValue> Succeed<TValue>(TValue value)
        {
            return new(value);
        }
    }
}
