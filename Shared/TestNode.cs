using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shared
{
    [Serializable]
    public class TestNode : IDisplayableNode
    {
        public TestNode()
        {
            this.Inputs = new List<IPin>() {
                  new TestPin(),
                  new TestPin(),
                  new TestPin()
            };
            this.Outputs = new List<IPin>()
            {
                new TestPin(),
                new TestPin()
            };
        }
        public ICollection<IPin> Inputs { get; set; }

        public ICollection<IPin> Outputs { get; set; }

        public string Label { get { return "hupfasdfasdfasdasdasdasdfasdfasdfasd"; } }

        public string Description { get { return "du W"; } }

        public Bitmap Picture { get { return null; } } // new Bitmap(@"C:\Users\alexfh\Desktop\GetPersonaPhoto.jpg"); } }

        public NodeType Type => (NodeType) 0;

        public event EventHandler PictureChanged;

        public void Activate()
        {
        }

        public void Execute()
        {
        }
    }
}
