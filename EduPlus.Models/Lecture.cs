using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;

namespace EduPlus.Models
{
    public class Lecture
    {
        public int LectureId { get; set; }


        public string Subject { get; set; } = string.Empty;

        public int MinMinutes { get; set; }
        public int MaxMinutes { get; set; }
    }
}
