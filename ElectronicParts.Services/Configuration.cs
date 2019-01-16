using ElectronicParts.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ElectronicParts.Services
{
    [DataContract]
    public class Configuration
    {
        [DataMember]
        public List<Rule<string>> StringRules { get; set; }

        [DataMember]
        public List<Rule<int>> IntRules { get; set; }

        [DataMember]
        public List<Rule<bool>> BoolRules { get; set; }

        [DataMember]
        public string StringValue { get; set; }
        [DataMember]
        public string StringColor { get; set; }

        [DataMember]
        public int IntValue { get; set; }
        [DataMember]
        public string IntColor { get; set; }

        [DataMember]
        public bool BoolValue { get; set; }
        [DataMember]
        public string BoolColor { get; set; }

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

            this.StringColor = config["StringColor"];

            this.IntColor = config["IntColor"];

            this.BoolColor = config["BoolColor"];

            this.StringValue = config["StringValue"];

            int.TryParse(config["IntValue"], out int intValue);

            this.IntValue = intValue;

            this.BoolValue = config["BoolValue"] == "True";
        }
    }
}
