using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Web
{
    public class TestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerialNumber { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Result { get; set; }

        [Required]
        public DateTime StartingTime { get; set; }

        [Required]
        public DateTime FinishingTime { get; set; }

        [Required]
        public double DurationInMs { get; set; }
    }
}