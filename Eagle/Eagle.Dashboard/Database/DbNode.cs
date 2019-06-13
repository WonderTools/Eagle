using System.ComponentModel.DataAnnotations;

namespace Eagle.Dashboard.Database
{
    public class DbNode
    {
        [Key]
        public string NodeName { get; set; }

        public string Uri { get; set; }

    }
}