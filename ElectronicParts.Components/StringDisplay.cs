using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ElectronicParts.Components
{
    public class StringDisplay : IDisplayableNode
    {
        public StringDisplay()
        {
            this.Inputs = new List<IPin> { new Pin<string>() };
            this.Outputs = new List<IPin>();
            this.SetNewPicture(this.Inputs.First().Value.Current?.ToString() ?? string.Empty);
        }

        public ICollection<IPin> Inputs { get; private set; }

        public ICollection<IPin> Outputs { get; private set; }

        public string Label { get => "String Display"; }

        public string Description { get => "Displays the input value."; }

        public NodeType Type { get => NodeType.Display; }

        public Bitmap Picture { get; private set; }

        public event EventHandler PictureChanged;

        /// <summary>
        /// Empty method. <see cref="IntegerAdder"/> is always active.
        /// </summary>
        public void Activate()
        {
        }


        /// <summary>
        /// Displays the current input value.
        /// </summary>
        public void Execute()
        {
            if (!(this.Inputs is null) && this.Inputs.Count != 1)
            {
                return;
            }

            this.SetNewPicture(this.Inputs.First().Value.Current.ToString());
            this.PictureChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the new picture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">Is thrown if the value is <see cref="null"/></exception>
        private void SetNewPicture(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bitmap = Properties.Resources.IntegerDisplay;
            DrawingVisual visual = new DrawingVisual();
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            RenderTargetBitmap newBmp = new RenderTargetBitmap(bitmap.Width, bitmap.Height, 500, 800, PixelFormats.Default);

            DrawingContext context = visual.RenderOpen();
            context.DrawText(
                new FormattedText(value,
                System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Verdana"), 50, System.Windows.Media.Brushes.White), new System.Windows.Point(0, 0));
            context.Close();
            newBmp.Render(visual);
            encoder.Frames.Add(BitmapFrame.Create(newBmp));
            encoder.Save(stream);
            this.Picture = new Bitmap(stream);
        }
    }
}
