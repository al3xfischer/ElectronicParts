using Shared;
using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ElectronicParts.ViewModels
{
    public class NodeViewModel : BaseViewModel
    {
        internal readonly IDisplayableNode node;

        private int top;

        private int left;

        public NodeViewModel(IDisplayableNode node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
            this.Top = 20;
            this.Left = 20;
        }

        public int Top { get => this.top; set { Set(ref this.top, value); } }

        public int Left { get => this.left; set { Set(ref this.left, value); } }


        public ObservableCollection<IPin> Inputs { get => this.node.Inputs.ToObservableCollection(); }
        public ObservableCollection<IPin> Outputs { get => this.node.Outputs.ToObservableCollection(); }
        public Bitmap Picture { get => this.node.Picture; }

    }
}
