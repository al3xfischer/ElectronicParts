using System;

namespace Shared
{
    [Serializable]
    public class TestValue : IValue
    {
        public object Current { get => true; set { } }
    }
}
