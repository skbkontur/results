namespace Kontur.Results
{
    /// <summary>Marker type carrying a value, used with implicit conversion to construct a successful <see cref="Result{TFault, TValue}"/>.</summary>
    /// <typeparam name="TValue">The type of the success value.</typeparam>
    public sealed class SuccessMarker<TValue>
    {
        internal SuccessMarker(TValue value)
        {
            this.Value = value;
        }

        internal TValue Value { get; }
    }
}
