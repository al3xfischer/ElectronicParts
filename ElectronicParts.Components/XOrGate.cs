// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="XOrGate.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the XOrGate class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Shared;

    /// <summary>
    /// Represents an <see cref="XOrGate"/> with one output pin as boolean.
    /// </summary>
    [Serializable]
    public class XOrGate : IDisplayableNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XOrGate"/> class with two input pins and one output pin.
        /// </summary>
        public XOrGate()
        {
            this.Inputs = new List<IPin>() { new Pin<bool>(), new Pin<bool>() };

            this.Outputs = new List<IPin>() { new Pin<bool>() };
        }

        /// <summary>
        /// Event to be called when picture has changed.
        /// </summary>
        public event EventHandler PictureChanged;

        /// <summary>
        /// Gets the input pins of this gate.
        /// </summary>
        /// <value>The input pins of this gate.</value>
        public ICollection<IPin> Inputs { get; }

        /// <summary>
        /// Gets the output pins of this gate. There is only one output pin - so use first pin of the Collection.
        /// </summary>
        /// <value>The output pins of this gate.</value>
        public ICollection<IPin> Outputs { get; }

        /// <summary>
        /// Gets the Label of this gate.
        /// </summary>
        /// <value>The label of this gate.</value>
        public string Label => "XOR-Gate";

        /// <summary>
        /// Gets the description of this gate.
        /// </summary>
        /// <value>The description of this gate.</value>
        public string Description => "An XOr-Gate.";

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>The type of the node.</value>
        public NodeType Type => NodeType.Logic;

        /// <summary>
        /// Gets the current picture of this node.
        /// </summary>
        /// <value>The current picture of this node.</value>
        public Bitmap Picture => Properties.Resources.ExOr_Gate;

        /// <summary>
        /// Empty Method. <see cref="XOrGate"/> is always active.
        /// </summary>
        public void Activate()
        {
            return;
        }

        /// <summary>
        /// Evaluates the two input pins and sets output pin to true if one is true and the other false, otherwise false.
        /// </summary>
        public void Execute()
        {
            if (this.Inputs.Count == 0)
            {
                this.Outputs.ElementAt(0).Value.Current = false;
                return;
            }

            bool input1 = (bool)this.Inputs.ElementAt(0).Value.Current;
            bool input2 = (bool)this.Inputs.ElementAt(1).Value.Current;

            if ((input1 && !input2) || (!input1 && input2))
            {
                this.Outputs.ElementAt(0).Value.Current = true;
            }
            else
            {
                this.Outputs.ElementAt(0).Value.Current = false;
            }
        }
    }
}
