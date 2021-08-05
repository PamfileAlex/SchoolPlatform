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
    static class SpecializationDAL
    {
        public static ObservableCollection<Specialization> GetSpecializations()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetSpecializations", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Specialization> specializations = new ObservableCollection<Specialization>();
                while (reader.Read())
                {
                    specializations.Add(new Specialization(reader.GetInt32(0))
                    {
                        Name = reader.GetString(1)
                    });
                }
                reader.Close();
                return specializations;
            }
        }

        public static Specialization GetSpecialization(String name)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", name));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Specialization specialization = null;
                if (reader.Read())
                {
                    specialization = new Specialization(reader.GetInt32(0))
                    {
                        Name = reader.GetString(1)
                    };
                }
                reader.Close();
                return specialization;
            }
        }

        public static void AddSpecialization(Specialization specialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", specialization.Name));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveSpecialization(Specialization specialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveSpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", specialization.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void ModifySpecialization(Specialization specialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("ModifySpecialization", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@id",specialization.ID),
                    new SqlParameter("@name", specialization.Name),

                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static bool CheckForSpecializationExistence(Specialization specialization)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("CheckForSpecializationExistence", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", specialization.Name));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                bool specializationExistence = false;
                if (reader.Read())
                {
                    specializationExistence = true;
                }
                reader.Close();
                return specializationExistence;
            }
        }
    }
}
