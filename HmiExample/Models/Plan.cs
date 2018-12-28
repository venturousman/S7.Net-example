using System;
using System.ComponentModel.DataAnnotations;

namespace ProductionEquipmentControlSoftware.Models
{
    public class Plan : BaseModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid MachineId { get; set; }

        //[Required(ErrorMessage = "*")]
        public Guid? EmployeeId { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid ProductId { get; set; }

        //[Required(ErrorMessage = "*")]
        public int? ExpectedQuantity { get; set; }

        public int? ActualQuantity { get; set; }

        public int? NotGoodQuantity { get; set; }

        public double TotalMilliseconds { get; set; }

        //public DateTime? StartTime { get; set; }

        //public DateTime? EndTime { get; set; }

        public bool IsProcessed { get; set; }

        #region Relationship
        public virtual Machine Machine { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Product Product { get; set; }
        #endregion
    }
}
