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
    class GradeBLL
    {
        public static GradeBLL Instance { get; } = new GradeBLL();
        public ObservableCollection<Grade> Grades { get; private set; }
        static GradeBLL() { }
        private GradeBLL()
        {
            StudentSubjectBLL.Instance.Init();
            Grades = GradeDAL.GetGrades();
        }

        public void UpdateGrades()
        {
            Grades = GradeDAL.GetGrades();
        }

        public void AddGrade(Grade grade)
        {
            CheckFields(grade);
            CheckSemesters(grade);
            GradeDAL.AddGrade(grade);
            Grade fromDB = GradeDAL.GetGrade(grade.StudentSubject, grade.Semester, grade.Value, grade.Thesis);
            if (fromDB == null)
                throw new SchoolPlatformException("Add Grade failed");
            Grades.Add(fromDB);
        }

        public void RemoveGrade(Grade grade)
        {
            CheckSemesters(grade);
            if (!Grades.Remove(grade))
                throw new SchoolPlatformException("Remove Grade failed");
            GradeDAL.RemoveGrade(grade);
        }

        private void CheckFields(Grade grade)
        {
            if (grade == null || grade.StudentSubject == null || grade.Value == default)
                throw new SchoolPlatformException("Please fill all fields");
            if (grade.Value > 10)
                throw new SchoolPlatformException("Invalid grade value");
            if (!grade.Thesis) { return; }
            if (Grades.Where(item => item.Thesis && item.Semester == grade.Semester
            && item.StudentSubject.Student.ID == grade.StudentSubject.Student.ID
             && item.StudentSubject.Subject.ID == grade.StudentSubject.Subject.ID).Count() != 0)
                throw new SchoolPlatformException("Thesis already exists");
        }

        private void CheckSemesters(Grade grade)
        {
            if (FinalGradeBLL.Instance.FinalGrades.Where(fg => fg.StudentSubject.ID == grade.StudentSubject.ID && fg.Semester == grade.Semester).Count() != 0)
            {
                throw new SchoolPlatformException("StudentSubject is closed\nfor selected Semester");
            }
        }

        //private void CheckSemesters(Grade grade)
        //{
        //    if (!grade.Semester)
        //    {
        //        if (grade.StudentSubject.FirstSemester)
        //            throw new SchoolPlatformException("StudentSubject is closed\nfor First Semester");
        //    }
        //    else
        //    {
        //        if (grade.StudentSubject.SecondSemester)
        //            throw new SchoolPlatformException("StudentSubject is closed\nfor Second Semester");
        //    }
        //}
    }
}
