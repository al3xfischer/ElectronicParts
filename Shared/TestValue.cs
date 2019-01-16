namespace Shared
{
    public class TestValue : IValue
    {
        public object Value { get => true; set { } }

        public object Current { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
