﻿using Shared;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class NodeViewModel : BaseViewModel
    {
<<<<<<< HEAD
        internal readonly IDisplayableNode node;

=======
>>>>>>> UI
        private int top;

        private int left;

        private int pinTop;

        private int pinLeft;

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
                var point = this.GetPintPositions();
                this.PinTop = point.Y;
            }
        }

        public int Left
        {
            get => this.left;

            set
            {
                Set(ref this.left, value);
                var point = this.GetPintPositions();
                this.PinLeft = point.X;
            }
        }
        public int PinLeft { get => this.pinLeft; set { Set(ref this.pinLeft, value); } }
        public int PinTop { get => this.pinTop; set { Set(ref this.pinTop, value); } }

        public ObservableCollection<PinViewModel> Inputs { get; }

        public ObservableCollection<PinViewModel> Outputs { get; }

        public Bitmap Picture { get => this.Node.Picture; }

        public string Label { get => this.Node.Label; }

        public string Description { get => this.Node.Description; }

        public IDisplayableNode Node { get; }

        public ICommand DeleteCommand { get; }

        private Point GetPintPositions()
        {
            var pinX = this.Left - 20;
            var pinY = this.Top - 20;

            return new Point(pinX, pinY);
        }

    }
}
