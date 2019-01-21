using ElectronicParts.ViewModels.Commands;
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
        private int top;

        private int left;

        public NodeViewModel(IDisplayableNode node, ICommand deleteCommand, ICommand inputPinCommand, ICommand OutputPinCommand)
        {
            this.Node = node ?? throw new ArgumentNullException(nameof(node));
            this.DeleteCommand = deleteCommand ?? throw new ArgumentNullException(nameof(deleteCommand));
            this.Inputs = node.Inputs?.Select(n => new PinViewModel(n, inputPinCommand)).ToObservableCollection();
            this.Outputs = node.Outputs?.Select(n => new PinViewModel(n, OutputPinCommand)).ToObservableCollection();
            this.Top = 18;
            this.Left = 20;

            this.ActivateCommand = new RelayCommand(arg =>
            {
                this.Node.Activate();
            });

            this.Node.PictureChanged += NodePictureChanged;
        }
        
        public void RemoveDelegate()
        {
            this.Node.PictureChanged -= NodePictureChanged;
        }

        public void AddDeleage()
        {
            this.Node.PictureChanged -= NodePictureChanged;
            this.Node.PictureChanged += NodePictureChanged;
        }

        private void NodePictureChanged(object sender, EventArgs e)
        {
            this.FirePropertyChanged(nameof(Picture));
        }

        public int Top
        {
            get => this.top;

            set
            {
                Set(ref this.top, value);
                this.UpdateTop(this.Inputs?.Select((p, i) => Tuple.Create(p, i)), this.Top);
                this.UpdateTop(this.Outputs?.Select((p, i) => Tuple.Create(p, i)), this.Top);
            }
        }

        public int Left
        {
            get => this.left;

            set
            {
                Set(ref this.left, value);
                this.UpdateLeft(this.Inputs, this.left);
                if (this.Inputs is null || this.Inputs.Count == 0)
                {
                    this.UpdateLeft(this.Outputs, this.Left + 63);
                }
                else
                {
                    this.UpdateLeft(this.Outputs, this.Left + 73);
                }
            }
        }

        /// <summary>
        /// Snaps to grid.
        /// </summary>
        /// <param name="gridSize">Size of the grid.</param>
        /// <param name="floor">if set to true will floor value else will ceil value.</param>
        public void SnapToNewGrid(int gridSize, bool floor)
        {
            int leftOffset;
            leftOffset = this.Inputs.Count > 0 ? 10 : 0;

            if (floor)
            {
                this.Left = Math.Max(this.Left.FloorTo(gridSize) - leftOffset, 0);
                this.Top = Math.Max(this.Top.FloorTo(gridSize) - 2, 0);
            }
            else
            {
                this.Left = Math.Max(this.Left.CeilingTo(gridSize) + (gridSize - leftOffset), 0);
                this.Top = Math.Max(this.Top.CeilingTo(gridSize) - 2, 0);

            }
        }

        /// <summary>
        /// Snaps to grid.
        /// Will round to the next possible value.
        /// </summary>
        /// <param name="gridSize">Size of the grid.</param>
        public void SnapToNewGrid(int gridSize)
        {
            int leftOffset;
            leftOffset = this.Inputs.Count > 0 ? 10 : 0;


            this.Left = Math.Max(this.Left.RoundTo(gridSize) - leftOffset, 0);
            this.Top = Math.Max(this.Top.RoundTo(gridSize) - 2, 0);
        }

        public ObservableCollection<PinViewModel> Inputs { get; }

        public ObservableCollection<PinViewModel> Outputs { get; }

        public Bitmap Picture { get => this.Node.Picture; }

        public string Label { get => this.Node.Label; }

        public string Description { get => this.Node.Description; }

        public IDisplayableNode Node { get; }
        public ICommand DeleteCommand { get; }

        public ICommand ActivateCommand { get; }

        public void Update()
        {
            this.FirePropertyChanged(nameof(Picture));
        }

        public int MaxPins
        {
            get
            {
                return this.Inputs.Count >= this.Outputs.Count ? this.Inputs.Count : this.Outputs.Count;
            }
        }

        private void UpdateLeft(IEnumerable<PinViewModel> pins, int value)
        {
            if (pins is null)
            {
                return;
            }

            foreach (var pin in pins)
            {
                pin.Left = value;
            }
        }

        private void UpdateTop(IEnumerable<Tuple<PinViewModel, int>> pins, int value)
        {
            if (pins is null)
            {
                return;
            }

            foreach (var pin in pins)
            {
                pin.Item1.Top = (pin.Item2 * 20) + value + 11;
            }
        }
    }
}
