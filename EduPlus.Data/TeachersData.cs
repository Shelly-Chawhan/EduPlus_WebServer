using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using EduPlus.Models;
using System.Data.SqlClient;

namespace EduPlus.Data
{
    public static class TeachersData
    {
            public static void Insert(Teacher teacher, SqlConnection cn)
            {
                if (cn.State == ConnectionState.Closed) cn.Open();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers WHERE Email=@Email", cn))
                {
                    cmd.Parameters.AddWithValue("Email", teacher.Email);
                    if ((int)cmd.ExecuteScalar() > 0)
                        throw new Exception("This email address has already been used.");
                }

                using (var cmd = new SqlCommand("INSERT INTO Teachers (FullName, Email) VALUES (@FullName, @Email)", cn))
                {
                    cmd.Parameters.AddWithValue("FullName", teacher.FullName);
                    cmd.Parameters.AddWithValue("Email", teacher.Email);
                    cmd.ExecuteNonQuery();
                }
            }

            public static void Update(Teacher teacher, SqlConnection cn)
            {
                if (cn.State == ConnectionState.Closed) cn.Open();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers WHERE TeacherId<>@TeacherId AND Email=@Email", cn))
                {
                    cmd.Parameters.AddWithValue("TeacherId", teacher.TeacherId);
                    cmd.Parameters.AddWithValue("Email", teacher.Email);
                    if ((int)cmd.ExecuteScalar() > 0)
                        throw new Exception("This email address has already been used.");
                }

                using (var cmd = new SqlCommand("UPDATE Teachers SET FullName=@FullName, Email=@Email WHERE TeacherId=@TeacherId", cn))
                {
                    cmd.Parameters.AddWithValue("TeacherId", teacher.TeacherId);
                    cmd.Parameters.AddWithValue("FullName", teacher.FullName);
                    cmd.Parameters.AddWithValue("Email", teacher.Email);
                    cmd.ExecuteNonQuery();
                }
            }

            public static Teacher GetTeacher(int teacherId, SqlConnection cn)
            {
                Teacher result = null;
                using (var cmd = new SqlCommand("SELECT * FROM Teachers WHERE TeacherId=@TeacherId", cn))
                {
                    cmd.Parameters.AddWithValue("TeacherId", teacherId);

                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            result = new Teacher()
                            {
                                TeacherId = Convert.ToInt32(dr["TeacherId"]),
                                FullName = Convert.ToString(dr["FullName"]),
                                Email = Convert.ToString(dr["Email"])
                            };
                            dr.Close();
                        }
                    }
                }
                return result;
            }

            public static List<Teacher> GetList(SqlConnection cn)
            {
                var result = new List<Teacher>();
                using (var cmd = new SqlCommand("SELECT * FROM Teachers", cn))
                {
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var teacher = new Teacher()
                        {
                            TeacherId = Convert.ToInt32(dr["TeacherId"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            Email = Convert.ToString(dr["Email"])
                        };
                        result.Add(teacher);
                    }
                    dr.Close();
                }
                return result;
            }

            public static void Delete(Teacher teacher, SqlConnection cn)
            {
                using (var cmd = new SqlCommand("DELETE Teachers WHERE TeacherId=@TeacherId", cn))
                {
                    cmd.Parameters.AddWithValue("TeacherId", teacher.TeacherId);
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            public static bool HasLectures(Teacher teacher, SqlConnection cn)
            {
                bool result = false;
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TeacherLectures WHERE TeacherId=@TeacherId", cn))
                {
                    cmd.Parameters.AddWithValue("TeacherId", teacher.TeacherId);
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    result = (int)cmd.ExecuteScalar() > 0;
                }
                return result;
            }
        }
    }
