using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Dashboard.Database
{
    public class DbNodePackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerialNumber { get; set; }

        public string NodeName { get; set; }

        public string Package { get; set; }
    }
}