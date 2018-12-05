using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionEquipmentControlSoftware.Models
{
    public class Product : BaseModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(128)]
        [Index(IsUnique = true)]
        public string Code { get; set; }    // unique
    }
}
