using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Unit
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitId { get; set; }
        [Required]
        [ForeignKey("Metric")]
        public int MetricId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }

        public Unit(int unitId, int metricId, string name, string code, string description)
        {
            UnitId = unitId;
            MetricId = metricId;
            Name = name;
            Code = code;
            Description = description;
        }

        public Unit()
        {
        }
    }
}
