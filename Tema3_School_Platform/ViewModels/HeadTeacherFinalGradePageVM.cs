using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tema3_School_Platform.Commands;
using Tema3_School_Platform.Models.BusinessLogicLayer;
using Tema3_School_Platform.Models.EntityLayer;
using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.ViewModels
{
    class HeadTeacherFinalGradePageVM : BaseVM
    {
        public ObservableCollection<FinalGrade> FinalGrades
        {
            get
            {
                List<int> studentSubjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss
                    => Students.Select(student => student.ID).Contains(ss.Student.ID)
                    && (Student == null || ss.Student.ID == Student.ID)).Select(ss => ss.ID).ToList();
                return FinalGradeBLL.Instance.FinalGrades.Where(finalGrade => studentSubjects.Contains(finalGrade.StudentSubject.ID)).ToObservableCollection();
            }
        }
        public ObservableCollection<User> Students
        {
            get { return UserBLL.Instance.Users.Where(user => user.Role == User.UserRole.Student && user.Class.ID == UserBLL.Instance.CurrentUser.Class.ID).ToObservableCollection(); }
        }

        private User student;
        public User Student
        {
            get { return student; }
            set
            {
                student = value;
                NotifyPropertyChanged("Student");
                NotifyPropertyChanged("FinalGrades");
                NotifyPropertyChanged("GeneralGrade");
            }
        }

        public String GeneralGrade
        {
            get
            {
                if (FinalGrades.Count() == 0)
                    return String.Empty;
                return Math.Round(FinalGrades.Select(finalGrade => finalGrade.Value).Average(), 2).ToString();
            }
        }


        public ICommand ClearCommand { get; }

        public HeadTeacherFinalGradePageVM()
        {
            this.ClearCommand = new ActionCommand(Clear);
        }

        private void Clear()
        {
            Student = null;
            ErrorMessage = String.Empty;
        }
    }
}
