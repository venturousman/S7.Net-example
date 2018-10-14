using System;

namespace HmiExample.Models
{
    public class Plan : BaseModel
    {
        public Guid Id { get; set; }

        public string Machine { get; set; }
        public string Employee { get; set; }
        public string Product { get; set; }

        public int ExpectedQuantity { get; set; }
        public int ActualQuantity { get; set; }
    }
}
