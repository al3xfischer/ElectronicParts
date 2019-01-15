namespace Shared
{
    public interface IValueGeneric<T> : IValue
    {
        new T Current { get; set; }
    }
}
