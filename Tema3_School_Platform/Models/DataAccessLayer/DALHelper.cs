using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_School_Platform.Models.BusinessLogicLayer;
using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.Models.DataAccessLayer
{
    static class DALHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public static Subject GetSubject(int id)
        {
            return SubjectBLL.Instance.GetSubject(id);
        }

        public static Specialization GetSpecialization(int id)
        {
            return SpecializationBLL.Instance.GetSpecialization(id);
        }

        public static Class GetClass(int id)
        {
            return ClassBLL.Instance.GetClass(id);
        }

        public static User GetUser(int id)
        {
            return UserBLL.Instance.GetUser(id);
        }

        public static StudentSubject GetStudentSubject(int id)
        {
            return StudentSubjectBLL.Instance.GetStudentSubject(id);
        }

        public static TeacherSubjectClass GetTeacherSubjectClass(int id)
        {
            return TeacherSubjectClassBLL.Instance.GetTeacherSubjectClass(id);
        }
    }
}
