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
    /// <summary>
    /// Class IntTwoString.
    /// </summary>
    /// <seealso cref="Shared.IDisplayableNode" />
    public class IntToString : IDisplayableNode
    {
        public IntToString()
        {
            this.Inputs = new List<IPin> { new Pin<int>() };
            this.Outputs = new List<IPin> { new Pin<string>() };
        }

        public ICollection<IPin> Inputs { get; private set; }

        public ICollection<IPin> Outputs { get; private set; }

        public string Label { get => "Int 2 String"; }

        public string Description { get => "Translates an integer into a string."; }

        public NodeType Type { get => NodeType.Logic; }

        public Bitmap Picture { get => Properties.Resources.Converter; }

        public event EventHandler PictureChanged;

        /// <summary>
        /// Empty method. <see cref="IntegerAdder"/> is always active.
        /// </summary>
        public void Activate()
        {
        }


        /// <summary>
        /// Translate the input <see cref="int"/> into a <see cref="string"/>
        /// </summary>
        public void Execute()
        {
            if (!(this.Inputs is null) && this.Inputs.Count >= 1)
            {
                this.Outputs.First().Value.Current = this.Inputs?.FirstOrDefault()?.Value?.Current?.ToString() ?? string.Empty;
            }
        }
    }
}
