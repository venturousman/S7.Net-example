using System;

namespace HmiExample.Models
{
    public class User : BaseModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string PhoneNumber { get; set; }        
    }
}
