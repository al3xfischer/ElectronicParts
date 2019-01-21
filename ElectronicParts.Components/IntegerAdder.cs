// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="IntegerAdder.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the IntegerAdder class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Shared;

    [Serializable]
    public class IntegerAdder : IDisplayableNode
    {
        public IntegerAdder()
        {
            this.Inputs = new List<IPin>() { new Pin<int>(), new Pin<int>() };

            this.Outputs = new List<IPin>() { new Pin<int>() };
        }

        public ICollection<IPin> Inputs { get; private set; }

        public ICollection<IPin> Outputs { get; private set; }

        public string Label => "Integer Adder";

        public string Description => "Adds all input integers.";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => Properties.Resources.IntegerAdder;
        
        public event EventHandler PictureChanged;

        /// <summary>
        /// Empty method. <see cref="IntegerAdder"/> is always active.
        /// </summary>
        public void Activate()
        {
            return;
        }

        /// <summary>
        /// Adds all input pin values and writes it to output pin.
        /// </summary>
        public void Execute()
        {
            int result = 0;

            foreach (var pin in this.Inputs)
            {
                if (int.TryParse(pin.Value.Current.ToString(), out int input))
                {
                    result += input;
                }
            }

            this.Outputs.ElementAt(0).Value.Current = result;
        }
    }
}
