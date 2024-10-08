using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Antlr.Runtime.Misc;
using EduPlus.Models;


namespace EduPlus.WebUI.Models
{
    public class TeacherLecturesViewModel
    {
        public Teacher Teacher { get; set; }
        public List<Lecture> AssociatedLectures { get; set; }
        public List<Lecture> NonAssociatedLectures { get; set; }
    }
}