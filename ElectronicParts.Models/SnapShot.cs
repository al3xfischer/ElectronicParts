﻿using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    public class SnapShot
    {
        public IEnumerable<NodeSnapShot> Nodes { get; set; }

        public IEnumerable<Connector> Connections { get; set; }
    }
}
