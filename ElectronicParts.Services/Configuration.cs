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
        public string StringValue { get; set; }
        [DataMember]
        public Color StringColor { get; set; }

        [DataMember]
        public int IntValue { get; set; }
        [DataMember]
        public Color IntColor { get; set; }

        [DataMember]
        public bool BoolValue { get; set; }
        [DataMember]
        public Color BoolColor { get; set; }

        public Configuration(IConfiguration config)
        {
            this.StringColor = (Color)ColorConverter.ConvertFromString(config["StringColor"]);

            this.IntColor = (Color)ColorConverter.ConvertFromString(config["IntColor"]);

            this.BoolColor = (Color)ColorConverter.ConvertFromString(config["BoolColor"]);

            this.StringValue = config["StringValue"];

            int.TryParse(config["IntValue"], out int intValue);

            this.IntValue = intValue;

            this.BoolValue = config["BoolValue"] == "True";
        }
    }
}
