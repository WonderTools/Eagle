using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Dashboard.Models
{
    public class NodeUpsertParameters
    {
        [Required]
        public string Uri { get; set; }
        [Required]
        public string NodeName { get; set; }
        public string ClientSecret { get; set; }
        public int ExecutionIntervalInMinutes { get; set; } 
    }
}
