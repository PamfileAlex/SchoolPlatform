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
    static class AbsenceDAL
    {
        public static ObservableCollection<Absence> GetAbsences()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetAbsences", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Absence> absences = new ObservableCollection<Absence>();
                while (reader.Read())
                {
                    absences.Add(new Absence()
                    {
                        ID = reader.GetInt32(0),
                        StudentSubject = DALHelper.GetStudentSubject(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Type = (Absence.AbsenceType)reader.GetInt32(3)
                    });
                }
                reader.Close();
                return absences;
            }
        }

        public static Absence GetAbsence(StudentSubject studentSubject, bool semester, Absence.AbsenceType type)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetAbsence", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("studentSubjectID", studentSubject.ID),
                    new SqlParameter("@semester", Convert.ToInt32(semester)),
                    new SqlParameter("@type", type)
                });
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Absence absence = null;
                if (reader.Read())
                {
                    absence = new Absence()
                    {
                        ID = reader.GetInt32(0),
                        StudentSubject = DALHelper.GetStudentSubject(reader.GetInt32(1)),
                        Semester = reader.GetBoolean(2),
                        Type = (Absence.AbsenceType)reader.GetInt32(3)
                    };
                }
                reader.Close();
                return absence;
            }
        }

        public static void AddAbsence(Absence absence)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddAbsence", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("studentSubjectID", absence.StudentSubject.ID),
                    new SqlParameter("@semester", Convert.ToInt32(absence.Semester)),
                    new SqlParameter("@type", (int)absence.Type)
                });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveAbsence(Absence absence)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveAbsence", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", absence.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void ModifyAbsence(Absence absence)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("ModifyAbsence", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", absence.ID));
                command.Parameters.Add(new SqlParameter("@type", (int)absence.Type));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
