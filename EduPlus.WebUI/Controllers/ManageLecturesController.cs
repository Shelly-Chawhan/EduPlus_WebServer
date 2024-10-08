using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EduPlus.Data;
using EduPlus.Models;

namespace EduPlus.WebUI.Controllers
{
    public class ManageLecturesController : CommonBaseClass
    {
        // GET: ManageLectures
        public ActionResult Index()
        {
            var list = new List<Lecture>();
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    list = LecturesData.GetList(cn);
                }
            }
            catch (Exception ex)
            {

                TempData["DangerMessage"] = ex.Message;
            }
            return View(list);
        }

        public ActionResult AddOrUpdate(int? id)
        {
            if (id == null) return View();

            Lecture lecture = null;
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    lecture = LecturesData.GetLecture((int)id, cn);
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }

            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate(Lecture lecture)
        {
            if (!ModelState.IsValid)
                return (lecture.LectureId == 0) ? View() : View(lecture);

            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    if (lecture.LectureId == 0)
                        LecturesData.Insert(lecture, cn);
                    else
                        LecturesData.Update(lecture, cn);
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
                return (lecture.LectureId == 0) ? View() : View(lecture);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(Lecture lecture)
        {
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    if (LecturesData.HasTeachers(lecture, cn))
                        throw new Exception("This lecture is associated with one or more teachers. Remove all the associations first");
                    else
                        LecturesData.Delete(lecture, cn);

                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
                return (lecture.LectureId == 0) ? View() : View(lecture);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
