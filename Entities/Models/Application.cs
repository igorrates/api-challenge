using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Application")]
    public class Application
    {
        
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string PathLocal { get; set; }
        public bool DebuggingMode { get; set; }


    }
}
