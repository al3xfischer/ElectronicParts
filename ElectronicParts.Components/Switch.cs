// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="Switch.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the Switch class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Shared;

    /// <summary>
    /// Represents an <see cref="Switch"/> with one input and one output pin as boolean.
    /// </summary>
    public class Switch : IDisplayableNode
    {
        /// <summary>
        /// Contains a value indicating whether the <see cref="Switch"/> is switched on.
        /// </summary>
        private bool switchedOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        public Switch()
        {
            this.Inputs = new List<IPin>();
            this.Inputs.Add(new Pin<bool>());

            this.Outputs = new List<IPin>();
            this.Outputs.Add(new Pin<bool>());
        }

        /// <summary>
        /// Event to be called when picture has changed.
        /// </summary>
        public event EventHandler PictureChanged;

        /// <summary>
        /// Gets the input pins of this gate. There is only one input pin - so use first pin of collection.
        /// </summary>
        /// <value>The input pins of this gate.</value>
        public ICollection<IPin> Inputs { get; private set; }

        /// <summary>
        /// Gets the output pins of this gate. There is only one output pin - so use first pin of the Collection.
        /// </summary>
        /// <value>The output pins of this gate.</value>
        public ICollection<IPin> Outputs { get; private set; }

        /// <summary>
        /// Gets the label of this gate.
        /// </summary>
        /// <value>The label of this gate.</value>
        public string Label => "Switch";

        /// <summary>
        /// Gets the description of this node.
        /// </summary>
        /// <value>The description of this node.</value>
        public string Description => "A switch to interrupt power.";

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>The type of the node.</value>
        public NodeType Type => NodeType.Switch;

        /// <summary>
        /// Gets the current picture of this node.
        /// </summary>
        /// <value>The current picture of this node.</value>
        public Bitmap Picture => throw new NotImplementedException();
        
        /// <summary>
        /// Toggles between switched on and off.
        /// </summary>
        public void Activate()
        {
            this.switchedOn = !this.switchedOn;

            this.Execute();

            //// TODO: change picture

            this.PictureChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Evaluates whether switched on or off and sets output pin to corresponding state.
        /// </summary>
        public void Execute()
        {
            if (this.switchedOn)
            {
                this.Outputs.ElementAt(0).Value.Current = this.Inputs.ElementAt(0).Value.Current;
            }
            else
            {
                this.Outputs.ElementAt(0).Value.Current = false;
            }
        }
    }
}
