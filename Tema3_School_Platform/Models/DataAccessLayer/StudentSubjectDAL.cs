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
    static class StudentSubjectDAL
    {
        public static ObservableCollection<StudentSubject> GetStudentSubjectList()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetStudentSubjectList", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<StudentSubject> studentSubjectList = new ObservableCollection<StudentSubject>();
                while (reader.Read())
                {
                    studentSubjectList.Add(new StudentSubject()
                    {
                        ID = reader.GetInt32(0),
                        Student = DALHelper.GetUser(reader.GetInt32(1)),
                        Subject = DALHelper.GetSubject(reader.GetInt32(2))
                    });
                }
                reader.Close();
                return studentSubjectList;
            }
        }

        public static StudentSubject GetStudentSubject(int studentID, int subjectID)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetStudentSubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@studentID", studentID),
                    new SqlParameter("@subjectID", subjectID)
                });
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                StudentSubject studentSubject = null;
                if (reader.Read())
                {
                    studentSubject = new StudentSubject()
                    {
                        ID = reader.GetInt32(0),
                        Student = DALHelper.GetUser(reader.GetInt32(1)),
                        Subject = DALHelper.GetSubject(reader.GetInt32(2))
                    };
                }
                reader.Close();
                return studentSubject;
            }
        }

        public static void AddStudentSubject(StudentSubject studentSubject)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddStudentSubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@studentID", studentSubject.Student.ID),
                    new SqlParameter("@subjectID", studentSubject.Subject.ID)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveStudentSubject(StudentSubject studentSubject)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveStudentSubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", studentSubject.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //public static void LockStudentSubject(StudentSubject studentSubject)
        //{
        //    using (SqlConnection connection = DALHelper.Connection)
        //    {
        //        SqlCommand command = new SqlCommand("LockStudentSubject", connection)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        command.Parameters.AddRange(new SqlParameter[]
        //        {
        //            new SqlParameter("@id", studentSubject.ID),
        //            new SqlParameter("@firstSemester", studentSubject.FirstSemester),
        //            new SqlParameter("@secondSemester", studentSubject.SecondSemester)
        //        });
        //        connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}
    }
}
