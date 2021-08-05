using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.Models.DataAccessLayer
{
    static class GradeDAL
    {
        public static ObservableCollection<Grade> GetGrades()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetGrades", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Grade> grades = new ObservableCollection<Grade>();
                while (reader.Read())
                {
                    grades.Add(new Grade()
                    {
                        ID = reader.GetInt32(0),
                        StudentSubject = DALHelper.GetStudentSubject(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Value = reader.GetFloat(3),
                        Thesis = reader.GetBoolean(4)
                    });
                }
                reader.Close();
                return grades;
            }
        }

        public static Grade GetGrade(StudentSubject studentSubject, bool semester, float value, bool thesis)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetGrade", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("studentSubjectID", studentSubject.ID),
                    new SqlParameter("@semester", Convert.ToInt32(semester)),
                    new SqlParameter("@value", value),
                    new SqlParameter("@thesis", Convert.ToInt32(thesis))
                });
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Grade grade = null;
                if (reader.Read())
                {
                    grade = new Grade()
                    {
                        ID = reader.GetInt32(0),
                        StudentSubject = DALHelper.GetStudentSubject(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Value = reader.GetFloat(3),
                        Thesis = reader.GetBoolean(4)
                    };
                }
                reader.Close();
                return grade;
            }
        }

        public static void AddGrade(Grade grade)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddGrade", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("studentSubjectID", grade.StudentSubject.ID),
                    new SqlParameter("@semester", Convert.ToInt32(grade.Semester)),
                    new SqlParameter("@value", grade.Value),
                    new SqlParameter("@thesis", Convert.ToInt32(grade.Thesis))
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveGrade(Grade grade)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveGrade", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", grade.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
