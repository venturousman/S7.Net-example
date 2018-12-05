using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionEquipmentControlSoftware.Models
{
    public class Employee : BaseModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(128)]
        [Index(IsUnique = true)]
        public string Code { get; set; }    // unique

        [Required(ErrorMessage = "*")]
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(255)]
        public string LastName { get; set; }

        public string Photo { get; set; } // ImagePath or Name
        public byte[] PhotoContent { get; set; }

        //[Required(ErrorMessage = "You must provide a phone number")]
        //[Display(Name = "Home Phone")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(255)]
        public string PhoneNumber { get; set; }
    }
}
