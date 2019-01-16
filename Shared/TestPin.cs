namespace Shared
{
    public class TestPin : IPin
    {
        private IValue test;
        public string Label { get { return "test"; } set { } }
        public IValue Value { get { return new TestValue(); } set { this.test = value; } }
    }
}
