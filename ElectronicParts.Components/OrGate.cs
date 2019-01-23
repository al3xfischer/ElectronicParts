// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="OrGate.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the OrGate class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Shared;

    /// <summary>
    /// Represents an <see cref="OrGate"/> with one output pin as boolean.
    /// </summary>
    [Serializable]
    public class OrGate : IDisplayableNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrGate"/> class with two input pins and one output pin.
        /// </summary>
        public OrGate()
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
        /// Gets the label of this gate.
        /// </summary>
        /// <value>The label of this gate.</value>
        public string Label => "OR";

        /// <summary>
        /// Gets the description of this gate.
        /// </summary>
        /// <value>The description of this gate.</value>
        public string Description => "A logic Or-gate.";

        /// <summary>
        /// Gets the current picture of this node.
        /// </summary>
        /// <value>The current picture of this node.</value>
        public Bitmap Picture => Properties.Resources.Or_Gate;

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>The type of the node.</value>
        public NodeType Type => NodeType.Logic;

        /// <summary>
        /// Empty Method. <see cref="OrGate"/> is always active.
        /// </summary>
        public void Activate()
        {
            return;
        }

        /// <summary>
        /// Evaluates all set input pins and sets output pin to true if at least one input pin is true, otherwise to false.
        /// </summary>
        public void Execute()
        {
            bool output = false;

            foreach (var input in this.Inputs.Where(pin => pin.Value.Current.GetType() == typeof(bool)))
            {
                if ((bool)input.Value.Current)
                {
                    output = true;
                    break;
                }
            }

            this.Outputs.First().Value.Current = output;
        }
    }
}