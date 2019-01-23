// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="Configuration.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Configuration class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents the Configuration class of the ElectronicParts program.
    /// </summary>
    [DataContract]
    public class Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            this.StringRules = new List<Rule<string>>();
            this.IntRules = new List<Rule<int>>();
            this.BoolRules = new List<Rule<bool>>();

            this.BoolRules.Add(new Rule<bool>(
                true,
                "Black",
                (value) =>
                    {
                        return !this.BoolRules.Any(rule => rule.Value == value);
                    }));

            this.BoolRules.Add(new Rule<bool>(
                false,
                "Black",
                (value) =>
                    {
                        return !this.BoolRules.Any(rule => rule.Value == value);
                    }));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="config">The IConfiguration instance used for setting up the starting configurations.</param>
        public Configuration(IConfiguration config)
        {
            this.StringRules = new List<Rule<string>>();
            this.IntRules = new List<Rule<int>>();
            this.BoolRules = new List<Rule<bool>>();

            var stringRules = config.GetSection("StringRules").GetChildren().AsEnumerable();
            var intRules = config.GetSection("IntRules").GetChildren().AsEnumerable();
            var boolRules = config.GetSection("BoolRules").GetChildren().AsEnumerable();

            foreach (var rule in stringRules)
            {
                string value = rule["Value"];
                string color = rule["Color"];
                this.StringRules.Add(new Rule<string>(
                    value,
                    color,
                    (newValue) =>
                        {
                            return !this.StringRules.Any(existingRule => existingRule.Value == newValue);
                        }));
            }

            foreach (var rule in intRules)
            {
                int.TryParse(rule["Value"], out int value);
                string color = rule["Color"];
                this.IntRules.Add(new Rule<int>(
                    value,
                    color,
                    (newValue) =>
                        {
                            return !this.IntRules.Any(existingRule => existingRule.Value == newValue);
                        }));
            }

            foreach (var rule in boolRules)
            {
                bool value = rule["Value"] == "True";
                string color = rule["Color"];
                this.BoolRules.Add(new Rule<bool>(
                    value, 
                    color, 
                    (newValue) =>
                        {
                            return !this.BoolRules.Any(existingRule => existingRule.Value == newValue);
                        }));
            }

            if (this.BoolRules.Count != 2)
            {
                this.BoolRules.Clear();
                this.BoolRules.Add(new Rule<bool>(
                    true, 
                    "Black", 
                    (value) =>
                        {
                            return !this.BoolRules.Any(rule => rule.Value == value);
                        }));

                this.BoolRules.Add(new Rule<bool>(
                    false, 
                    "Black", 
                    (value) =>
                        {
                            return !this.BoolRules.Any(rule => rule.Value == value);
                        }));
            }

            int.TryParse(config["BoardHeight"] ?? string.Empty, out int boardHeight);
            int.TryParse(config["BoardWidth"] ?? string.Empty, out int boardWidth);
            this.BoardHeight = boardHeight;
            this.BoardWidth = boardHeight;
        }

        /// <summary>
        /// Gets or sets the string rules used for the color of connections with type string.
        /// </summary>
        /// <value>The string rules used for the color of connections with type string.</value>
        [DataMember]
        public List<Rule<string>> StringRules { get; set; }

        /// <summary>
        /// Gets or sets the integer rules used for the color of connections with type integer.
        /// </summary>
        /// <value>The integer rules used for the color of connections with type integer.</value>
        [DataMember]
        public List<Rule<int>> IntRules { get; set; }

        /// <summary>
        /// Gets or sets the boolean rules used for the color of connections with type boolean.
        /// </summary>
        /// <value>The boolean rules used for the color of connections with type boolean.</value>
        [DataMember]
        public List<Rule<bool>> BoolRules { get; set; }

        /// <summary>
        /// Gets or sets the width of the board.
        /// </summary>
        /// <value>The width of the board.</value>
        [DataMember]
        public int BoardWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the board.
        /// </summary>
        /// <value>The height of the board.</value>
        [DataMember]
        public int BoardHeight { get; set; }
    }
}
