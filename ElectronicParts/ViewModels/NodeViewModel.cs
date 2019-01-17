using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class NodeViewModel : BaseViewModel
    {
        internal readonly IDisplayableNode node;

        private int top;

        private int left;

        public NodeViewModel(IDisplayableNode node, ICommand deleteCommand, ICommand inputPinCommand, ICommand OutputPinCommand)
        {
            this.Node = node ?? throw new ArgumentNullException(nameof(node));
            this.DeleteCommand = deleteCommand ?? throw new ArgumentNullException(nameof(deleteCommand));
            this.Inputs = node.Inputs.Select(n => new PinViewModel(n, inputPinCommand)).ToObservableCollection();
            this.Outputs = node.Outputs.Select(n => new PinViewModel(n, OutputPinCommand)).ToObservableCollection();
            this.Top = 20;
            this.Left = 20;
        }

        public int Top
        {
            get => this.top;

            set
            {
                Set(ref this.top, value);
                this.updateTop(this.Inputs.Select((p, i) => Tuple.Create(p, i)),this.Top);
                this.updateTop(this.Outputs.Select((p, i) => Tuple.Create(p, i)),this.Top);
            }
        }

        public int Left
        {
            get => this.left;

            set
            {
                Set(ref this.left, value);
                this.updateLeft(this.Inputs, this.left);
                this.updateLeft(this.Outputs, this.Left + 73);
            }
        }
        public ObservableCollection<PinViewModel> Inputs { get; }

        public ObservableCollection<PinViewModel> Outputs { get; }

        public Bitmap Picture { get => this.Node.Picture; }

        public string Label { get => this.Node.Label; }

        public string Description { get => this.Node.Description; }

        public IDisplayableNode Node { get; }

        public ICommand DeleteCommand { get; }

        private void updateLeft(IEnumerable<PinViewModel> pins, int value)
        {
            foreach (var pin in pins)
            {
                pin.Left = value;
            }
        }

        private void updateTop(IEnumerable<Tuple<PinViewModel, int>> pins, int value)
        {
            foreach (var pin in pins)
            {
                pin.Item1.Top = (pin.Item2 * 22) + value + 13;
            }
        }
    }
}
