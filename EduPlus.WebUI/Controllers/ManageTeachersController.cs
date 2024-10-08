using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Text;
using EduPlus.Data;
using EduPlus.Models;

namespace EduPlus.WebUI.Controllers
{
    public class ManageTeachersController : CommonBaseClass
    {
        // GET: ManageTeachers
        public ActionResult Index()
        {
            var listOfTeachers = new List<Teacher>();
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    listOfTeachers = TeachersData.GetList(cn);
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }
            return View(listOfTeachers);
        }

        public ActionResult AddOrUpdate(int? id)
        {
            if (id == null) return View();

            Teacher teacher = null;
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    teacher = TeachersData.GetTeacher((int)id, cn);
                    if (teacher == null)
                        return RedirectToAction("Index", "NotFound", new { entity = "Teacher", backUrl = "/ManageTeachers/" });
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate(Teacher teacher)
        {
            if (!ModelState.IsValid)
                return (teacher.TeacherId == 0) ? View() : View(teacher);

            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    if (teacher.TeacherId == 0)
                        TeachersData.Insert(teacher, cn);
                    else
                        TeachersData.Update(teacher, cn);

                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
                return (teacher.TeacherId == 0) ? View() : View(teacher);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(Teacher teacher)
        {
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    if (TeachersData.HasLectures(teacher, cn))
                    {
                        //throw new Exception("This Teacher cannot be removed because it has been associated with one or more lectures. Remove all associations first.");
                        var sb = new StringBuilder();
                        var lectures = TeacherLecturesData.GetAssociatedLectureList(teacher.TeacherId, cn);
                        foreach (var lecture in lectures)
                        {
                            if (sb.ToString().Length > 0)
                                sb.Append(", ");

                            sb.Append(lecture.Subject);
                        }
                        throw new Exception($"This Teacher cannot be removed because it has been associated with These Subjects: {sb.ToString()}. Remove all associations first.");
                    }

                    else
                        TeachersData.Delete(teacher, cn);
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
