using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shared
{
    public class TestNode : IDisplayableNode
    {
        public TestNode()
        {
            this.Inputs = new List<IPin>() {
                  new TestPin(),
                  new TestPin(),
                  new TestPin()
            };
            this.Outputs = new List<IPin>();
        }
        public ICollection<IPin> Inputs { get; set; }

        public ICollection<IPin> Outputs { get; set; }

        public string Label { get { return "hupf"; } }

        public string Description { get { return "du W"; } }

        public Bitmap Picture { get { return new Bitmap(@"C:\Users\alexfh\Desktop\GetPersonaPhoto.jpg"); } }

        public event EventHandler PictureChanged;

        public void Execute()
        {
        }
    }
}
