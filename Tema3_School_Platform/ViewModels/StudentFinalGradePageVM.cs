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
    class StudentFinalGradePageVM : BaseVM
    {
        public ObservableCollection<FinalGrade> FinalGrades
        {
            get
            {
                List<int> studentSubjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss
                    => ss.Student.ID == UserBLL.Instance.CurrentUser.ID
                    && (Subject == null || ss.Subject.ID == Subject.ID)).Select(ss => ss.ID).ToList();
                return FinalGradeBLL.Instance.FinalGrades.Where(finalGrade => studentSubjects.Contains(finalGrade.StudentSubject.ID)).ToObservableCollection();
            }
        }
        public ObservableCollection<Subject> Subjects
        {
            get
            {
                List<int> subjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss => ss.Student.ID == UserBLL.Instance.CurrentUser.ID).Select(ss => ss.Subject.ID).ToList();
                return SubjectBLL.Instance.Subjects.Where(subject => subjects.Contains(subject.ID)).ToObservableCollection();
            }
        }

        private Subject subject;
        public Subject Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                NotifyPropertyChanged("Subject");
                NotifyPropertyChanged("FinalGrades");
            }
        }

        public String GeneralGrade => Math.Round(FinalGrades.Select(finalGrade => finalGrade.Value).Average(), 2).ToString();

        public ICommand ClearCommand { get; }

        public StudentFinalGradePageVM()
        {
            this.ClearCommand = new ActionCommand(Clear);
        }

        private void Clear()
        {
            Subject = null;
            ErrorMessage = String.Empty;
        }
    }
}
