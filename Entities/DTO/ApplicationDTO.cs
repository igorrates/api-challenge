using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class ApplicationDTO
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? PathLocal { get; set; }
        public bool? DebuggingMode { get; set; }
    }
}
