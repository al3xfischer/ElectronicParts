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
            var yyyy = config["StringColor"];

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
