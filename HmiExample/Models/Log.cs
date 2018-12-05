using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionEquipmentControlSoftware.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(255)]
        public string Thread { get; set; }

        [MaxLength(50)]
        public string Level { get; set; }

        [MaxLength(255)]
        public string Logger { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
