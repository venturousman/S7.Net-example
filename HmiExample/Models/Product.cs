using System;

namespace HmiExample.Models
{
    public class Product : BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }    // unique
    }
}
