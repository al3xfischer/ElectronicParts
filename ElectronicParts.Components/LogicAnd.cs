namespace ElectronicParts.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using Shared;

    /// <summary>
    /// Represents an "logic AND-Gate" with one output pin as boolean.
    /// </summary>
    public class LogicAnd : INode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAnd"/> class.
        /// </summary>
        public LogicAnd()
        {
            this.Inputs = new List<IPin>();
            this.Outputs = new List<IPin>();
            this.Outputs.Add(new Pin<bool>());
        }

        /// <summary>
        /// Gets the input pins of this gate.
        /// </summary>
        /// <value>The input pins of this gate.</value>
        public ICollection<IPin> Inputs { get; private set; }

        /// <summary>
        /// Gets the output pins of this gate. There is only one pin - so use first pin of the Collection.
        /// </summary>
        /// <value>The output pins of this gate.</value>
        public ICollection<IPin> Outputs { get; private set; }

        /// <summary>
        /// Gets the Label of this gate.
        /// </summary>
        /// <value>The label of this gate.</value>
        public string Label => "AND";

        /// <summary>
        /// Gets the description of this gate.
        /// </summary>
        /// <value>The description of this gate.</value>
        public string Description => "Under Construction";

        /// <summary>
        /// Evaluates all set input pins and sets output pin to true if all input pins are true otherwise to false.
        /// </summary>
        public void Execute()
        {
            if (this.Inputs.Count == 0)
            {
                this.Outputs.First().Value.Current = false;
                return;
            }

            bool output = true;

            foreach (var input in this.Inputs)
            {
                if (!(bool)input.Value.Current)
                {
                    output = false;
                    break;
                }
            }

            this.Outputs.First().Value.Current = output;
        }
    }
}
