using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;

namespace EduPlus.Models
{
    public class Teacher
    {
        
        public int TeacherId { get; set; }

      
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
