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
    static class TeachingMaterialDAL
    {
        public static ObservableCollection<TeachingMaterial> GetTeachingMaterials()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetTeachingMaterials", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<TeachingMaterial> teachingMaterials = new ObservableCollection<TeachingMaterial>();
                while (reader.Read())
                {
                    teachingMaterials.Add(new TeachingMaterial()
                    {
                        ID = reader.GetInt32(0),
                        TeacherSubjectClass = DALHelper.GetTeacherSubjectClass(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Name = reader.GetString(3),
                        Path = reader.GetString(4)
                    });
                }
                reader.Close();
                return teachingMaterials;
            }
        }

        public static TeachingMaterial GetTeachingMaterial(int tscID, string name)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetTeachingMaterial", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@tscID", tscID));
                command.Parameters.Add(new SqlParameter("@name", name));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                TeachingMaterial teachingMaterial = null;
                if (reader.Read())
                {
                    teachingMaterial = new TeachingMaterial()
                    {
                        ID = reader.GetInt32(0),
                        TeacherSubjectClass = DALHelper.GetTeacherSubjectClass(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Name = reader.GetString(3),
                        Path = reader.GetString(4)
                    };
                }
                reader.Close();
                return teachingMaterial;
            }
        }

        public static void AddTeachingMaterial(TeachingMaterial teachingMaterial)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddTeachingMaterial", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("tscID", teachingMaterial.TeacherSubjectClass.ID),
                    new SqlParameter("@semester", Convert.ToInt32(teachingMaterial.Semester)),
                    new SqlParameter("@name", teachingMaterial.Name),
                    new SqlParameter("@path", teachingMaterial.Path)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveTeachingMaterial(TeachingMaterial teachingMaterial)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveTeachingMaterial", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", teachingMaterial.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
