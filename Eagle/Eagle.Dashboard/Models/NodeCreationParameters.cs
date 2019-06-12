using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Dashboard.Models
{
    public class NodeCreationParameters
    {
        public string Uri { get; set; }
        public string NodeName { get; set; }
        public string ClientSecret { get; set; }
        public int ExecutionIntervalInMinutes { get; set; } 
    }
}
