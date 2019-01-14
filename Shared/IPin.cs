namespace Shared
{
    public interface IPin
    {
        string Label { get; set; }

        IValue Value { get; set; }
    }
}
