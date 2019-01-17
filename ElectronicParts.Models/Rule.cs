using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    [DataContract]
    public class Rule<T>
    {

        [DataMember]
        public T Value { get; set; }

        [DataMember]
        public string Color { get; set; }

        public Rule(T value, string color)
        {
            this.Value = value;
            this.Color = color;
        }
    }
}
