﻿// ***********************************************************************
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
    using ElectronicParts.Models;
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
                this.StringRules.Add(new Rule<string>(value, color));
            }

            foreach (var rule in intRules)
            {
                int.TryParse(rule["Value"], out int value);
                string color = rule["Color"];
                this.IntRules.Add(new Rule<int>(value, color));
            }

            foreach (var rule in boolRules)
            {
                bool value = rule["Value"] == "True";
                string color = rule["Color"];
                this.BoolRules.Add(new Rule<bool>(value, color));
            }
        }

        /// <summary>
        /// Gets or sets the string rules used for the color of connections with type string.
        /// </summary>
        /// <value>All string rules.</value>
        [DataMember]
        public List<Rule<string>> StringRules { get; set; }

        /// <summary>
        /// Gets or sets the integer rules used for the color of connections with type integer.
        /// </summary> 
        /// <value>All integer rules.</value>
        [DataMember]
        public List<Rule<int>> IntRules { get; set; }

        /// <summary>
        /// Gets or sets the boolean rules used for the color of connections with type boolean.
        /// </summary>
        /// <value>All boolean rules.</value>
        [DataMember]
        public List<Rule<bool>> BoolRules { get; set; }       
    }
}