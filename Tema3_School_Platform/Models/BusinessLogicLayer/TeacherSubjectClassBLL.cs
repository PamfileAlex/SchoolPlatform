using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_School_Platform.Exceptions;
using Tema3_School_Platform.Models.DataAccessLayer;
using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.Models.BusinessLogicLayer
{
    class TeacherSubjectClassBLL
    {
        public static TeacherSubjectClassBLL Instance { get; } = new TeacherSubjectClassBLL();
        public ObservableCollection<TeacherSubjectClass> TeacherSubjectClassList { get; private set; }
        static TeacherSubjectClassBLL() { }
        private TeacherSubjectClassBLL()
        {
            UserBLL.Instance.Init();
            SubjectBLL.Instance.Init();
            ClassBLL.Instance.Init();
            TeacherSubjectClassList = TeacherSubjectClassDAL.GetTeacherSubjectClassList();
        }

        public void UpdateTeacherSubjectClassList()
        {
            TeacherSubjectClassList = TeacherSubjectClassDAL.GetTeacherSubjectClassList();
        }

        public TeacherSubjectClass GetTeacherSubjectClass(int id)
        {
            return TeacherSubjectClassList.First(tsc => tsc.ID == id);
        }

        public void RemovedSubjectSpecialization(int subjectID, int specializationID)
        {
            List<int> classIDs = new List<int>();
            foreach (var classObj in ClassBLL.Instance.Classes)
            {
                if (classObj.Specialization.ID != specializationID) { continue; }
                classIDs.Add(classObj.ID);
            }
            List<TeacherSubjectClass> toBeRemoved = new List<TeacherSubjectClass>();
            foreach (var tsc in TeacherSubjectClassList)
            {
                if (tsc.Subject.ID != subjectID || !classIDs.Contains(tsc.Class.ID)) { continue; }
                toBeRemoved.Add(tsc);
            }
            foreach (var tsc in toBeRemoved)
            {
                RemoveTeacherSubjectClass(tsc);
            }
        }

        public void AddTeacherSubjectClass(TeacherSubjectClass teacherSubjectClass)
        {
            CheckFields(teacherSubjectClass);
            CheckExistence(teacherSubjectClass);
            TeacherSubjectClassDAL.AddTeacherSubjectClass(teacherSubjectClass);
            TeacherSubjectClass fromDB = TeacherSubjectClassDAL.GetTeacherSubjectClass(teacherSubjectClass.Teacher.ID, teacherSubjectClass.Subject.ID, teacherSubjectClass.Class.ID);
            if (fromDB == null)
                throw new SchoolPlatformException("Add TeacherSubjectClass failed");
            TeacherSubjectClassList.Add(fromDB);
            //StudentSubject(fromDB);
        }

        public void RemoveTeacherSubjectClass(TeacherSubjectClass teacherSubjectClass)
        {
            if (!TeacherSubjectClassList.Remove(teacherSubjectClass))
                throw new SchoolPlatformException("Remove TeacherSubjectClass failed");
            TeacherSubjectClassDAL.RemoveTeacherSubjectClass(teacherSubjectClass);
            TeachingMaterialBLL.Instance.UpdateTeachingMaterials();
        }

        private void CheckFields(TeacherSubjectClass teacherSubjectClass)
        {
            if (teacherSubjectClass.Teacher == null || teacherSubjectClass.Subject == null || teacherSubjectClass.Class == null)
                throw new SchoolPlatformException("Please fill all fields");
        }

        private void CheckExistence(TeacherSubjectClass tscParam)
        {
            //if (TeacherSubjectClassList.Where(tsc => tsc.Teacher.ID == tscParam.Teacher.ID && tsc.Subject.ID == tscParam.Subject.ID && tsc.Class.ID == tscParam.Class.ID).Count() != 0)
            if (TeacherSubjectClassList.Where(tsc => tsc.Subject.ID == tscParam.Subject.ID && tsc.Class.ID == tscParam.Class.ID).Count() != 0)
                throw new SchoolPlatformException("Relation already exists");
        }

        private void StudentSubject(TeacherSubjectClass teacherSubjectClass)
        {
            foreach (var user in UserBLL.Instance.Users)
            {
                if (user.Role != User.UserRole.Student) { continue; }
                if (user.Class.ID != teacherSubjectClass.Class.ID) { continue; }
                try
                {
                    StudentSubjectBLL.Instance.AddStudentSubject(new StudentSubject()
                    {
                        ID = 0,
                        Student = user,
                        Subject = teacherSubjectClass.Subject
                    });
                }
                catch { }
            }
        }
    }
}
