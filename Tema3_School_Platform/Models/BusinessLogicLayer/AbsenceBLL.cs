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
    class AbsenceBLL
    {
        public static AbsenceBLL Instance { get; } = new AbsenceBLL();
        public ObservableCollection<Absence> Absences { get; private set; }
        static AbsenceBLL() { }
        private AbsenceBLL()
        {
            StudentSubjectBLL.Instance.Init();
            Absences = AbsenceDAL.GetAbsences();
        }

        public void UpdateAbsences()
        {
            Absences = AbsenceDAL.GetAbsences();
        }

        public void AddAbsence(Absence absence)
        {
            CheckFields(absence);
            CheckSemesters(absence);
            AbsenceDAL.AddAbsence(absence);
            Absence fromDB = AbsenceDAL.GetAbsence(absence.StudentSubject, absence.Semester, absence.Type);
            if (fromDB == null)
                throw new SchoolPlatformException("Add Absence failed");
            Absences.Add(fromDB);
        }

        public void RemoveAbsence(Absence absence)
        {
            CheckSemesters(absence);
            if (!Absences.Remove(absence))
                throw new SchoolPlatformException("Remove Absence failed");
            AbsenceDAL.RemoveAbsence(absence);
        }

        public void ModifyAbsence(Absence absence)
        {
            CheckSemesters(absence);
            switch (absence.Type)
            {
                case Absence.AbsenceType.Motivated:
                    absence.Type = Absence.AbsenceType.Unmotivated;
                    break;
                case Absence.AbsenceType.Unmotivated:
                    absence.Type = Absence.AbsenceType.Motivated;
                    break;
                case Absence.AbsenceType.Unmotivatable:
                    throw new SchoolPlatformException("Can't modify this absence");
                default:
                    break;
            }
            AbsenceDAL.ModifyAbsence(absence);
        }

        private void CheckFields(Absence absence)
        {
            if (absence == null || absence.StudentSubject == null || absence.Type == Absence.AbsenceType.None)
                throw new SchoolPlatformException("Please fill all fields");
        }

        private void CheckSemesters(Absence absence)
        {
            if (FinalGradeBLL.Instance.FinalGrades.Where(fg => fg.StudentSubject.ID == absence.StudentSubject.ID && fg.Semester == absence.Semester).Count() != 0)
            {
                throw new SchoolPlatformException("StudentSubject is closed\nfor selected Semester");
            }
        }

        //private void CheckSemesters(Absence absence)
        //{
        //    if (absence.Semester)
        //    {
        //        if (absence.StudentSubject.FirstSemester)
        //            throw new SchoolPlatformException("StudentSubject is closed\nfor First Semester");
        //    }
        //    else
        //    {
        //        if (absence.StudentSubject.SecondSemester)
        //            throw new SchoolPlatformException("StudentSubject is closed\nfor Second Semester");
        //    }
        //}
    }
}
