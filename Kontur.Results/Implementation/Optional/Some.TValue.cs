using System;

namespace Kontur.Results
{
    internal sealed class Some<TValue> : Optional<TValue>
    {
        private readonly TValue value;

        internal Some(TValue value)
        {
            this.value = value;
        }

        public override TResult Match<TResult>(Func<TResult> onNone, Func<TValue, TResult> onSome)
        {
            return onSome(value);
        }
    }
}