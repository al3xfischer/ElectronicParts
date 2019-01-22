// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="IntegerDisplay.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the IntegerDisplay class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Shared;

    [Serializable]
    public class IntegerDisplay : IDisplayableNode
    {
        public IntegerDisplay()
        {
            this.Inputs = new List<IPin>() { new Pin<int>() };

            this.Outputs = new List<IPin>();
            this.SetNewPicture("int");
            this.Label = "IntegerDisplay";
        }

        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label { get; private set; }

        public string Description => "Displays an integer.";

        public NodeType Type => NodeType.Display;

        private Bitmap picture;

        /// <summary>
        /// Gets the current picture of this node.
        /// </summary>
        /// <value>The current picture of this node.</value>
        public Bitmap Picture
        {
            get => this.picture;
            set
            {
                if (!(value is null))
                {
                    this.picture = value;
                    this.PictureChanged?.Invoke(this, null);
                }
            }
        }
        
        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            this.SetNewPicture(this.Inputs.ElementAt(0).Value.Current.ToString());
            this.Label = this.Inputs.ElementAt(0).Value.Current.ToString();
        }

        private void SetNewPicture(string value)
        {
            var bitmap = Properties.Resources.IntegerDisplay;
            DrawingVisual visual = new DrawingVisual();
            DrawingContext context = visual.RenderOpen();
            context.DrawText(
                new FormattedText(this.Inputs.ElementAt(0).Value.Current.ToString(),
                System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Verdana"), 50, System.Windows.Media.Brushes.White), new System.Windows.Point(0, 0));
            context.Close();
            RenderTargetBitmap newBmp = new RenderTargetBitmap(value.Length * 200, bitmap.Height, 500, 800, PixelFormats.Default);
            newBmp.Render(visual);
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(newBmp));
            encoder.Save(stream);
            this.Picture = new Bitmap(stream);
        }
    }
}
