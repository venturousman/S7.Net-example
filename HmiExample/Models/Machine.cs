using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionEquipmentControlSoftware.Models
{
    public class Machine : BaseModel
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(128)]
        [Index(IsUnique = true)]
        public string Code { get; set; }    // unique

        public int CumulativeCount { get; set; } // so lan dap khuon tich luy

        [DefaultValue(-1)]
        public int TagIndex { get; set; } // vd: M2{1}.0
    }
}
