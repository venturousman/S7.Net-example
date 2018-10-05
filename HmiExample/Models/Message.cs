using System;

namespace HmiExample.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string MachineCode { get; set; }
        public string Supervisor { get; set; }
        public string Status { get; set; }
    }
}
