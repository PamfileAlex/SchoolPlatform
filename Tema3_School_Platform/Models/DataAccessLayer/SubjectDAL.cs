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
    static class SubjectDAL
    {
        public static ObservableCollection<Subject> GetSubjects()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetSubjects", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Subject> subjects = new ObservableCollection<Subject>();
                while (reader.Read())
                {
                    subjects.Add(new Subject(reader.GetInt32(0))
                    {
                        Name = reader.GetString(1)
                    });
                }
                reader.Close();
                return subjects;
            }
        }

        public static Subject GetSubject(String name)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetSubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", name));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Subject subject = null;
                if (reader.Read())
                {
                    subject = new Subject(reader.GetInt32(0))
                    {
                        Name = reader.GetString(1)
                    };
                }
                reader.Close();
                return subject;
            }
        }

        public static void AddSubject(Subject subject)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddSubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", subject.Name));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveSubject(Subject subject)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveSubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", subject.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void ModifySubject(Subject subject)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("ModifySubject", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@id",subject.ID),
                    new SqlParameter("@name", subject.Name),

                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static bool CheckForSubjectExistence(Subject subject)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("CheckForSubjectExistence", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", subject.Name));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                bool subjectExistence = false;
                if (reader.Read())
                {
                    subjectExistence = true;
                }
                reader.Close();
                return subjectExistence;
            }
        }
    }
}
