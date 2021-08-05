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
    class ClassBLL
    {
        public static ClassBLL Instance { get; } = new ClassBLL();
        public ObservableCollection<Class> Classes { get; }
        static ClassBLL() { }
        private ClassBLL()
        {
            SpecializationBLL.Instance.Init();
            Classes = ClassDAL.GetClasses();
        }

        public void Init() { }

        public Class GetClass(int id)
        {
            return Classes.First(classObj => classObj.ID == id);
        }

        public void AddClass(Class classObj)
        {
            CheckClassFields(classObj);
            CheckForClassExistence(classObj);
            ClassDAL.AddClass(classObj);
            Class fromDB = ClassDAL.GetClass(classObj.Name);
            if (fromDB == null)
                throw new SchoolPlatformException("Add Class failed");
            Classes.Add(fromDB);
        }

        public void RemoveClass(Class classObj)
        {
            if (!Classes.Remove(classObj))
                throw new SchoolPlatformException("Remove Class failed");
            ClassDAL.RemoveClass(classObj);

            TeacherSubjectClassBLL.Instance.UpdateTeacherSubjectClassList();
        }

        public void ModifyClass(Class classObj, int selectedIndex)
        {
            CheckClassFields(classObj);
            if (!classObj.Name.Equals(Classes[selectedIndex].Name))
                CheckForClassExistence(classObj);
            if (classObj.Specialization.ID != Classes[selectedIndex].Specialization.ID)
                StudentSubject(classObj);
            Classes[selectedIndex] = classObj;
            ClassDAL.ModifyClass(classObj);
        }

        private void CheckForClassExistence(Class classObj)
        {
            if (Classes.Where(classIn => classIn.Name.Equals(classObj.Name)).Count() != 0)
                throw new SchoolPlatformException("Name is taken");
        }

        private void CheckClassFields(Class classObj)
        {
            if (String.IsNullOrEmpty(classObj.Name) || String.IsNullOrEmpty(classObj.Year) || classObj.Specialization == null)
                throw new SchoolPlatformException("Please fill all fields");
        }

        private void StudentSubject(Class classObj)
        {
            foreach (var student in UserBLL.Instance.Users)
            {
                if (student.Role != User.UserRole.Student) { continue; }
                if (student.Class.ID != classObj.ID) { continue; }
                foreach (var ss in SubjectSpecializationBLL.Instance.SubjectSpecializations)
                {
                    if (classObj.Specialization.ID != ss.Specialization.ID) { continue; }
                    try
                    {
                        StudentSubjectBLL.Instance.AddStudentSubject(new StudentSubject()
                        {
                            ID = 0,
                            Student = student,
                            Subject = ss.Subject
                        });
                    }
                    catch { }
                }
            }
        }
    }
}
