using System;

namespace Kontur.Results
{
    internal sealed class None<TValue> : Optional<TValue>
    {
        private static readonly Lazy<None<TValue>> Provider = new(() => new());

        private None()
        {
        }

        internal static Optional<TValue> Instance => Provider.Value;

        public override TResult Match<TResult>(Func<TResult> onNone, Func<TValue, TResult> onSome)
        {
            return onNone();
        }
    }
}