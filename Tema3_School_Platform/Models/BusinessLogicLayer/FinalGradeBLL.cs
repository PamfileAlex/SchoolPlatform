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
    class FinalGradeBLL
    {
        public static FinalGradeBLL Instance { get; } = new FinalGradeBLL();
        public ObservableCollection<FinalGrade> FinalGrades { get; private set; }
        static FinalGradeBLL() { }
        private FinalGradeBLL()
        {
            StudentSubjectBLL.Instance.Init();
            FinalGrades = FinalGradeDAL.GetFinalGrades();
        }

        public void UpdateFinalGrades()
        {
            FinalGrades = FinalGradeDAL.GetFinalGrades();
        }

        public void AddFinalGrade(FinalGrade finalGrade)
        {
            FinalGradeDAL.AddFinalGrade(finalGrade);
            FinalGrade fromDB = FinalGradeDAL.GetFinalGrade(finalGrade.StudentSubject, finalGrade.Semester);
            if (fromDB == null)
                throw new SchoolPlatformException("Add FinalGrade failed");
            FinalGrades.Add(fromDB);
        }
    }
}
