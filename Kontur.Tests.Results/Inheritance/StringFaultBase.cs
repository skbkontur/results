namespace Kontur.Tests.Results.Inheritance
{
    public class StringFaultBase
    {
        public StringFaultBase(string fault)
        {
            Fault = fault;
        }

        public string Fault { get; }

        public sealed override string ToString()
        {
            return Fault;
        }

        public sealed override bool Equals(object? obj)
        {
            return obj is StringFaultBase other && Equals(Fault, other.Fault);
        }

        public sealed override int GetHashCode()
        {
            return (GetType(), Fault).GetHashCode();
        }
    }
}
