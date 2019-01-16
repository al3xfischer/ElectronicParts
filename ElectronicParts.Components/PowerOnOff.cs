// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="PowerOnOff.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the PowerOnOff class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Shared;

    /// <summary>
    /// Represents an <see cref="PowerOnOff"/> node with one output pin as boolean.
    /// </summary>
    public class PowerOnOff : IDisplayableNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerOnOff"/> class.
        /// </summary>
        public PowerOnOff()
        {
            this.Inputs = new List<IPin>();

            this.Outputs = new List<IPin>();
            this.Outputs.Add(new Pin<bool>());
        }

        /// <summary>
        /// Event to be called when picture has changed.
        /// </summary>
        public event EventHandler PictureChanged;

        /// <summary>
        /// Gets the input pins of this gate. List is empty because <see cref="PowerOnOff"/> has no inputs.
        /// </summary>
        /// <value>The input pins of this gate (Empty).</value>
        public ICollection<IPin> Inputs { get; private set; }

        /// <summary>
        /// Gets the output pins of this gate. There is only one pin - so use first pin of the Collection.
        /// </summary>
        /// <value>The output pins of this gate.</value>
        public ICollection<IPin> Outputs { get; private set; }

        /// <summary>
        /// Gets the label of this node.
        /// </summary>
        /// <value>The label of this node.</value>
        public string Label => nameof(PowerOnOff);

        /// <summary>
        /// Gets the description of this node.
        /// </summary>
        /// <value>The description of this node.</value>
        public string Description => "Switches power on and off.";

        /// <summary>
        /// Gets the current picture of this node.
        /// </summary>
        /// <value>The current picture of this node.</value>
        public Bitmap Picture
        {
            get
            {
                if ((bool)this.Outputs.ElementAt(0).Value.Current)
                {
                    return Properties.Resources.PowerOn;
                }
                else
                {
                    return Properties.Resources.PowerOff;
                }
            }
        }

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>The type of the node.</value>
        public NodeType Type => NodeType.Source;

        /// <summary>
        /// This execute method is empty. To change state of <see cref="PowerOnOff"/> use activate.
        /// </summary>
        public void Execute()
        {
            return;
        }

        /// <summary>
        /// Toggles the output value between true and false.
        /// </summary>
        public void Activate()
        {
            this.Outputs.ElementAt(0).Value.Current = !(bool)this.Outputs.ElementAt(0).Value.Current;

            this.PictureChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
