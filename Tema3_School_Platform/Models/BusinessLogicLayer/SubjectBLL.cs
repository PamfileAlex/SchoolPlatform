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
    class SubjectBLL
    {
        public static SubjectBLL Instance { get; } = new SubjectBLL();
        public ObservableCollection<Subject> Subjects { get; }
        static SubjectBLL() { }
        private SubjectBLL()
        {
            Subjects = SubjectDAL.GetSubjects();
        }

        public void Init() { }

        public Subject GetSubject(int id)
        {
            return Subjects.First(subject => subject.ID == id);
        }

        public void AddSubject(Subject subject)
        {
            CheckForSubjectExistence(subject);
            SubjectDAL.AddSubject(subject);
            Subject fromDB = SubjectDAL.GetSubject(subject.Name);
            if (fromDB == null)
                throw new SchoolPlatformException("Add Subject failed");
            Subjects.Add(fromDB);
        }

        public void RemoveSubject(Subject subject)
        {
            if (!Subjects.Remove(subject))
                throw new SchoolPlatformException("Remove Subject failed");
            SubjectDAL.RemoveSubject(subject);

            SubjectSpecializationBLL.Instance.UpdateSubjectSpecializations();
            StudentSubjectBLL.Instance.UpdateStudentSubjectList();
            TeacherSubjectClassBLL.Instance.UpdateTeacherSubjectClassList();
        }

        public void ModifySubject(Subject subject, int selectedIndex)
        {
            if (!subject.Name.Equals(Subjects[selectedIndex].Name))
                CheckForSubjectExistence(subject);
            Subjects[selectedIndex] = subject;
            SubjectDAL.ModifySubject(subject);
        }

        private void CheckForSubjectExistence(Subject subject)
        {
            if (SubjectDAL.CheckForSubjectExistence(subject))
                throw new SchoolPlatformException("Name is taken");
        }
    }
}
