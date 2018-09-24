using System;

namespace HmiExample.Models
{
    public class Setting : BaseModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
