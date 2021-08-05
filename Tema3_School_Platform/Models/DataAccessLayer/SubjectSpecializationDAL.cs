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
    static class SubjectSpecializationDAL
    {
        public static ObservableCollection<SubjectSpecialization> GetSubjectSpecializations()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetSubjectSpecializations", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<SubjectSpecialization> subjectSpecializations = new ObservableCollection<SubjectSpecialization>();
                while (reader.Read())
                {
                    subjectSpecializations.Add(new SubjectSpecialization()
                    {
                        ID = reader.GetInt32(0),
                        Subject = DALHelper.GetSubject(reader.GetInt32(1)),
                        Specialization = DALHelper.GetSpecialization(reader.GetInt32(2)),
                        Thesis = reader.GetBoolean(3)
                    });
                }
                reader.Close();
                return subjectSpecializations;
            }
        }

        public static SubjectSpecialization GetSubjectSpecialization(int subjectID, int specializatinID)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetSubjectSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@subjectID", subjectID));
                command.Parameters.Add(new SqlParameter("@specializationID", specializatinID));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                SubjectSpecialization subjectSpecialization = null;
                if (reader.Read())
                {
                    subjectSpecialization = new SubjectSpecialization()
                    {
                        ID = reader.GetInt32(0),
                        Subject = DALHelper.GetSubject(reader.GetInt32(1)),
                        Specialization = DALHelper.GetSpecialization(reader.GetInt32(2)),
                        Thesis = reader.GetBoolean(3)
                    };
                }
                reader.Close();
                return subjectSpecialization;
            }
        }

        public static void AddSubjectSpecialization(SubjectSpecialization subjectSpecialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddSubjectSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@subjectID", subjectSpecialization.Subject.ID),
                    new SqlParameter("@specializationID", subjectSpecialization.Specialization.ID),
                    new SqlParameter("@thesis", Convert.ToInt32(subjectSpecialization.Thesis)),
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveSubjectSpecialization(SubjectSpecialization subjectSpecialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveSubjectSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", subjectSpecialization.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void ModifySubjectSpecialization(SubjectSpecialization subjectSpecialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("ModifySubjectSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", subjectSpecialization.ID));
                command.Parameters.Add(new SqlParameter("@thesis", Convert.ToInt32(subjectSpecialization.Thesis)));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
