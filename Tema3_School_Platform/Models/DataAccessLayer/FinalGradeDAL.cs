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
    class FinalGradeDAL
    {
        public static ObservableCollection<FinalGrade> GetFinalGrades()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetFinalGrades", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<FinalGrade> finalGrades = new ObservableCollection<FinalGrade>();
                while (reader.Read())
                {
                    finalGrades.Add(new FinalGrade()
                    {
                        ID = reader.GetInt32(0),
                        StudentSubject = DALHelper.GetStudentSubject(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Value = reader.GetFloat(3)
                    });
                }
                reader.Close();
                return finalGrades;
            }
        }

        public static FinalGrade GetFinalGrade(StudentSubject studentSubject, bool semester)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetFinalGrade", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("studentSubjectID", studentSubject.ID),
                    new SqlParameter("@semester", Convert.ToInt32(semester))
                });
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                FinalGrade finalGrade = null;
                if (reader.Read())
                {
                    finalGrade = new FinalGrade()
                    {
                        ID = reader.GetInt32(0),
                        StudentSubject = DALHelper.GetStudentSubject(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Value = reader.GetFloat(3)
                    };
                }
                reader.Close();
                return finalGrade;
            }
        }

        public static void AddFinalGrade(FinalGrade finalGrade)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddFinalGrade", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("studentSubjectID", finalGrade.StudentSubject.ID),
                    new SqlParameter("@semester", Convert.ToInt32(finalGrade.Semester)),
                    new SqlParameter("@value", finalGrade.Value)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
