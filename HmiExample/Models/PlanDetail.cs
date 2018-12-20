using System;
using System.ComponentModel.DataAnnotations;

namespace ProductionEquipmentControlSoftware.Models
{
    public class PlanDetail : BaseModel
    {
        public Guid Id { get; set; }

        public Guid PlanId { get; set; }

        public Guid? EmployeeId { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid ProductId { get; set; }

        public int? ActualQuantity { get; set; }

        public int? NotGoodQuantity { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        #region Relationship
        public virtual Employee Employee { get; set; }
        public virtual Product Product { get; set; }
        #endregion
    }
}
