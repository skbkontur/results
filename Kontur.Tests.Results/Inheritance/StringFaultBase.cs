namespace Kontur.Tests.Results.Inheritance
{
    public class StringFaultBase
    {
        public StringFaultBase(string fault)
        {
            this.Fault = fault;
        }

        public string Fault { get; }

        public sealed override string ToString()
        {
            return this.Fault;
        }

        public sealed override bool Equals(object? obj)
        {
            return obj is StringFaultBase other && Equals(this.Fault, other.Fault);
        }

        public sealed override int GetHashCode()
        {
            return (this.GetType(), this.Fault).GetHashCode();
        }
    }
}
