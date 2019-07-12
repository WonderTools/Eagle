using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Dashboard.Database
{
    public class DbNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NodeName { get; set; }

        public string Uri { get; set; }

        public string ClientSecret { get; set; }

        public int ExecutionIntervalInMinutes { get; set; }

    }
}