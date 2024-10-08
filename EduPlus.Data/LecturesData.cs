using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EduPlus.Models;


namespace EduPlus.Data
{
    public static class LecturesData
    {
        public static void Insert(Lecture lecture, SqlConnection cn)
        {
            using (var cmd = new SqlCommand("INSERT INTO Lectures (Subject, MinMinutes, MaxMinutes) VALUES (@Subject, @MinMinutes, @MaxMinutes)", cn))
            {
                cmd.Parameters.AddWithValue("Subject", lecture.Subject);
                cmd.Parameters.AddWithValue("MinMinutes", lecture.MinMinutes);
                cmd.Parameters.AddWithValue("MaxMinutes", lecture.MaxMinutes);
                if (cn.State == ConnectionState.Closed) cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void Update(Lecture lecture, SqlConnection cn)
        {
            using (var cmd = new SqlCommand("UPDATE Lectures SET Subject=@Subject, MinMinutes=@MinMinutes, MaxMinutes=@MaxMinutes WHERE LectureId=@LectureId", cn))
            {
                cmd.Parameters.AddWithValue("LectureId", lecture.LectureId);
                cmd.Parameters.AddWithValue("Subject", lecture.Subject);
                cmd.Parameters.AddWithValue("MinMinutes", lecture.MinMinutes);
                cmd.Parameters.AddWithValue("MaxMinutes", lecture.MaxMinutes);
                if (cn.State == ConnectionState.Closed) cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static Lecture GetLecture(int lectureId, SqlConnection cn)
        {
            Lecture result = null;
            using (var cmd = new SqlCommand("SELECT * FROM Lectures WHERE LectureId=@LectureId", cn))
            {
                cmd.Parameters.AddWithValue("LectureId", lectureId);
                if (cn.State == ConnectionState.Closed) cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = new Lecture()
                        {
                            LectureId = Convert.ToInt32(dr["LectureId"]),
                            Subject = Convert.ToString(dr["Subject"]),
                            MinMinutes = Convert.ToInt32(dr["MinMinutes"]),
                            MaxMinutes = Convert.ToInt32(dr["MaxMinutes"])
                        };
                        dr.Close();
                    }
                }
            }
            return result;
        }

        public static List<Lecture> GetList(SqlConnection cn)
        {
            var result = new List<Lecture>();
            using (var cmd = new SqlCommand("SELECT * FROM Lectures", cn))
            {
                if (cn.State == ConnectionState.Closed) cn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var lecture = new Lecture()
                    {
                        LectureId = Convert.ToInt32(dr["LectureId"]),
                        Subject = Convert.ToString(dr["Subject"]),
                        MinMinutes = Convert.ToInt32(dr["MinMinutes"]),
                        MaxMinutes = Convert.ToInt32(dr["MaxMinutes"])
                    };
                    result.Add(lecture);
                }
                dr.Close();
            }
            return result;
        }

        public static void Delete(Lecture lecture, SqlConnection cn)
        {
            using (var cmd = new SqlCommand("DELETE Lectures WHERE LectureId=@LectureId", cn))
            {
                cmd.Parameters.AddWithValue("LectureId", lecture.LectureId);
                if (cn.State == ConnectionState.Closed) cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static bool HasTeachers(Lecture lecture, SqlConnection cn)
        {
            bool result = false;
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TeacherLectures WHERE LectureId=@LectureId", cn))
            {
                cmd.Parameters.AddWithValue("LectureId", lecture.LectureId);
                if (cn.State == ConnectionState.Closed) cn.Open();
                result = (int)cmd.ExecuteScalar() > 0;
            }
            return result;
        }
    }
}
