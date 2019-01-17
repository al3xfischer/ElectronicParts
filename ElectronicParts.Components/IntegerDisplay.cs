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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Shared;

    [Serializable]
    public class IntegerDisplay : IDisplayableNode
    {
        public IntegerDisplay()
        {
            this.Inputs = new List<IPin>();
            this.Inputs.Add(new Pin<int>());

            this.Outputs = new List<IPin>();

            this.Label = "IntegerDisplay";
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label { get; private set; }

        public string Description => "Displays an integer.";

        public NodeType Type => NodeType.Display;

        public Bitmap Picture => throw new NotImplementedException();

        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            this.Label = this.Inputs.ElementAt(0).Value.Current.ToString();
        }
    }
}
