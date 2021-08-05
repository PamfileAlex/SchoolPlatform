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
    static class ClassDAL
    {
        public static ObservableCollection<Class> GetClasses()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetClasses", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Class> classes = new ObservableCollection<Class>();
                while (reader.Read())
                {
                    classes.Add(new Class(reader.GetInt32(0))
                    {
                        Specialization = DALHelper.GetSpecialization(reader.GetInt32(1)),
                        Name = reader.GetString(2),
                        Year = reader.GetString(3)
                    });
                }
                reader.Close();
                return classes;
            }
        }

        public static Class GetClass(String name)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@name", name));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Class classObj = null;
                if (reader.Read())
                {
                    classObj = new Class(reader.GetInt32(0))
                    {
                        Specialization = DALHelper.GetSpecialization(reader.GetInt32(1)),
                        Name = reader.GetString(2),
                        Year = reader.GetString(3)
                    };
                }
                reader.Close();
                return classObj;
            }
        }

        public static void AddClass(Class classObj)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("specializationID", classObj.Specialization.ID),
                    new SqlParameter("@name", classObj.Name),
                    new SqlParameter("@year", classObj.Year)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveClass(Class classObj)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", classObj.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void ModifyClass(Class classObj)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("ModifyClass", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@id", classObj.ID),
                    new SqlParameter("specializationID", classObj.Specialization.ID),
                    new SqlParameter("@name", classObj.Name),
                    new SqlParameter("@year", classObj.Year)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
