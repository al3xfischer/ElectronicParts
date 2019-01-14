namespace Shared
{
    public interface IValueGeneric<T> : IValue
    {
        new T Value { get; set; }
    }
}
