// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="IntegerSource.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the IntegerSource class of the ElectronicParts Programm</summary>
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
    public class IntegerSource : IDisplayableNode
    {
        private static Random random = new Random();

        public IntegerSource()
        {
            this.Inputs = new List<IPin>();

            this.Outputs = new List<IPin>() { new Pin<int>() };
        }
        public ICollection<IPin> Inputs { get; private set; }

        public ICollection<IPin> Outputs { get; private set; }

        public string Label => "Integer Source";

        public string Description => "Generates a random digit as output";

        public NodeType Type => NodeType.Source;

        public Bitmap Picture => Properties.Resources.IntegerSource;

        public event EventHandler PictureChanged;

        /// <summary>
        /// Empty method. Use Execute to generate a new value.
        /// </summary>
        public void Activate()
        {
            return;
        }

        /// <summary>
        /// Generates a new random digit and writes it to first (and only) output pin.
        /// </summary>
        public void Execute()
        {
            int newVal = random.Next(10);
            this.Outputs.ElementAt(0).Value.Current = newVal;
        }
    }
}
