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
    class SubjectSpecializationBLL
    {
        public static SubjectSpecializationBLL Instance { get; } = new SubjectSpecializationBLL();
        public ObservableCollection<SubjectSpecialization> SubjectSpecializations { get; private set; }
        static SubjectSpecializationBLL() { }
        private SubjectSpecializationBLL()
        {
            SubjectBLL.Instance.Init();
            SpecializationBLL.Instance.Init();
            SubjectSpecializations = SubjectSpecializationDAL.GetSubjectSpecializations();
        }

        public void UpdateSubjectSpecializations()
        {
            SubjectSpecializations = SubjectSpecializationDAL.GetSubjectSpecializations();
        }

        public void AddSubjectSpecialization(SubjectSpecialization subjectSpecialization)
        {
            //Verificare daca exista deja
            SubjectSpecializationDAL.AddSubjectSpecialization(subjectSpecialization);
            SubjectSpecialization fromDB = SubjectSpecializationDAL.GetSubjectSpecialization(subjectSpecialization.Subject.ID, subjectSpecialization.Specialization.ID);
            if (fromDB == null)
                throw new SchoolPlatformException("Add SubjectSpecialization failed");
            SubjectSpecializations.Add(fromDB);
            StudentSubject(fromDB);
        }

        public void RemoveSubjectSpecialization(SubjectSpecialization subjectSpecialization)
        {
            if (!SubjectSpecializations.Remove(subjectSpecialization))
                throw new SchoolPlatformException("Remove SubjectSpecialization failed");
            SubjectSpecializationDAL.RemoveSubjectSpecialization(subjectSpecialization);

            TeacherSubjectClassBLL.Instance.RemovedSubjectSpecialization(subjectSpecialization.Subject.ID, subjectSpecialization.Specialization.ID);
            StudentSubjectBLL.Instance.RemovedSubjectSpecialization(subjectSpecialization.Subject.ID, subjectSpecialization.Specialization.ID);
        }

        public void ModifySubjectSpecialization(SubjectSpecialization subjectSpecialization)
        {
            SubjectSpecializationDAL.ModifySubjectSpecialization(subjectSpecialization);
        }

        public int IndexOfSubjectSpecialization(int subjectID, int specializationID)
        {
            try
            {
                SubjectSpecialization subjectSpecializationItem = SubjectSpecializations.First(subjectSpecialization => subjectSpecialization.Subject.ID == subjectID && subjectSpecialization.Specialization.ID == specializationID);
                return SubjectSpecializations.IndexOf(subjectSpecializationItem);
            }
            catch
            {
                return -1;
            }
        }

        private void StudentSubject(SubjectSpecialization subjectSpecialization)
        {
            List<int> classIDs = new List<int>();
            foreach (var classObj in ClassBLL.Instance.Classes)
            {
                if (classObj.Specialization.ID != subjectSpecialization.Specialization.ID) { continue; }
                classIDs.Add(classObj.ID);
            }
            foreach (var user in UserBLL.Instance.Users)
            {
                if (user.Role != User.UserRole.Student) { continue; }
                if (!classIDs.Contains(user.Class.ID)) { continue; }
                try
                {
                    StudentSubjectBLL.Instance.AddStudentSubject(new StudentSubject()
                    {
                        ID = 0,
                        Student = user,
                        Subject = subjectSpecialization.Subject
                    });
                }
                catch { }
            }
        }
    }
}
