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
    class HeadTeacherAbsencePageVM : BaseVM
    {
        public ObservableCollection<User> Students
        {
            get { return UserBLL.Instance.Users.Where(user => user.Role == User.UserRole.Student && user.Class.ID == UserBLL.Instance.CurrentUser.Class.ID).ToObservableCollection(); }
        }
        public ObservableCollection<Absence> Absences
        {
            get
            {
                List<int> studentSubjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss
                    => Students.Select(student => student.ID).Contains(ss.Student.ID)
                    && (Student == null || ss.Student.ID == Student.ID)).Select(ss => ss.ID).ToList();
                return AbsenceBLL.Instance.Absences.Where(absence => studentSubjects.Contains(absence.StudentSubject.ID)
                && (AbsenceType == Absence.AbsenceType.None || absence.Type == AbsenceType)).ToObservableCollection();
            }
        }
        public List<Absence.AbsenceType> AbsenceTypes { get; }

        private User student;
        public User Student
        {
            get { return student; }
            set
            {
                student = value;
                NotifyPropertyChanged("Student");
                NotifyPropertyChanged("Absences");
            }
        }

        private Absence.AbsenceType absenceType;
        public Absence.AbsenceType AbsenceType
        {
            get { return absenceType; }
            set
            {
                absenceType = value;
                NotifyPropertyChanged("AbsenceType");
                NotifyPropertyChanged("Absences");
            }
        }

        private Absence selectedAbsence;
        public Absence SelectedAbsence
        {
            get { return selectedAbsence; }
            set
            {
                selectedAbsence = value;
                NotifyPropertyChanged("SelectedAbsence");
            }
        }

        public ICommand ModifyCommand { get; }
        public ICommand ClearCommand { get; }

        public HeadTeacherAbsencePageVM()
        {
            Clear();
            AbsenceTypes = Enum.GetValues(typeof(Absence.AbsenceType)).Cast<Absence.AbsenceType>().ToList();
            this.ModifyCommand = new RelayCommand<Absence>(absences => ErrorWrapper(() => { AbsenceBLL.Instance.ModifyAbsence(absences); Clear(); }));
            this.ClearCommand = new ActionCommand(Clear);
        }

        private void Clear()
        {
            Student = null;
            AbsenceType = Absence.AbsenceType.None;
            SelectedAbsence = null;
            ErrorMessage = String.Empty;
        }
    }
}
