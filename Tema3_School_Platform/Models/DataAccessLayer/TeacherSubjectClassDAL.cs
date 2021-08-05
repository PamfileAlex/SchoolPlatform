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
    static class TeacherSubjectClassDAL
    {
        public static ObservableCollection<TeacherSubjectClass> GetTeacherSubjectClassList()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetTeacherSubjectClassList", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<TeacherSubjectClass> teacherSubjectClasses = new ObservableCollection<TeacherSubjectClass>();
                while (reader.Read())
                {
                    teacherSubjectClasses.Add(new TeacherSubjectClass()
                    {
                        ID = reader.GetInt32(0),
                        Teacher = DALHelper.GetUser(reader.GetInt32(1)),
                        Subject = DALHelper.GetSubject(reader.GetInt32(2)),
                        Class = DALHelper.GetClass(reader.GetInt32(3))
                    });
                }
                reader.Close();
                return teacherSubjectClasses;
            }
        }

        public static TeacherSubjectClass GetTeacherSubjectClass(int teacherID, int subjectID, int classID)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetTeacherSubjectClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@teacherID", teacherID),
                    new SqlParameter("@subjectID", subjectID),
                    new SqlParameter("@classID", classID)
                });
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                TeacherSubjectClass teacherSubjectClass = null;
                if (reader.Read())
                {
                    teacherSubjectClass = new TeacherSubjectClass()
                    {
                        ID = reader.GetInt32(0),
                        Teacher = DALHelper.GetUser(reader.GetInt32(1)),
                        Subject = DALHelper.GetSubject(reader.GetInt32(2)),
                        Class = DALHelper.GetClass(reader.GetInt32(3))
                    };
                }
                reader.Close();
                return teacherSubjectClass;
            }
        }

        public static void AddTeacherSubjectClass(TeacherSubjectClass teacherSubjectClass)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddTeacherSubjectClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@teacherID", teacherSubjectClass.Teacher.ID),
                    new SqlParameter("@subjectID", teacherSubjectClass.Subject.ID),
                    new SqlParameter("@classID", teacherSubjectClass.Class.ID)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveTeacherSubjectClass(TeacherSubjectClass teacherSubjectClass)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveTeacherSubjectClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", teacherSubjectClass.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
