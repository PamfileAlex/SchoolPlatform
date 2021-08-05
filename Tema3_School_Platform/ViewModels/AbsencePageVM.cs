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
    class AbsencePageVM : BaseVM
    {
        private ObservableCollection<Absence> absences;
        public ObservableCollection<Absence> Absences
        {
            get { return absences; }
            set
            {
                absences = value;
                NotifyPropertyChanged("Absences");
            }
        }
        public ObservableCollection<Subject> Subjects
        {
            get
            {
                return SubjectBLL.Instance.Subjects.Where(subject => TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.Where(tsc
                    => tsc.Subject.ID == subject.ID && tsc.Teacher.ID == UserBLL.Instance.CurrentUser.ID).Count() != 0).ToObservableCollection();
            }
        }
        public ObservableCollection<User> Students
        {
            get
            {
                if (Subject == null)
                    return null;
                return UserBLL.Instance.Users.Where(user => StudentSubjectBLL.Instance.StudentSubjectList.Where(ss
                        => ss.Subject.ID == Subject.ID && ss.Student.ID == user.ID).Count() != 0).ToObservableCollection();
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
                SelectedAbsence = null;
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
                NotifyPropertyChanged("Students");
                SelectedAbsence = null;
            }
        }

        private Absence.AbsenceType type;
        public Absence.AbsenceType Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
                SelectedAbsence = null;
            }
        }

        private bool semester;
        public bool Semester
        {
            get { return semester; }
            set
            {
                semester = value;
                NotifyPropertyChanged("Semester");
                SelectedAbsence = null;
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
                NotifyPropertyChanged("Modify");
            }
        }

        public bool Modify { get { return SelectedAbsence != null && SelectedAbsence.Type != Absence.AbsenceType.Unmotivatable; } }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ModifyCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        public AbsencePageVM()
        {
            Clear();
            AbsenceTypes = Enum.GetValues(typeof(Absence.AbsenceType)).Cast<Absence.AbsenceType>().ToList();
            this.AddCommand = new RelayCommand<Absence>(absences => ErrorWrapper(() => { AbsenceBLL.Instance.AddAbsence(absences); Update(); }));
            this.RemoveCommand = new RelayCommand<Absence>(absences => ErrorWrapper(() => { AbsenceBLL.Instance.RemoveAbsence(absences); Clear(); }));
            this.ModifyCommand = new RelayCommand<Absence>(absences => ErrorWrapper(() => { AbsenceBLL.Instance.ModifyAbsence(absences); Clear(); }));
            this.SearchCommand = new ActionCommand(Search);
            this.ClearCommand = new ActionCommand(Clear);
        }

        private void Search()
        {
            Absences = AbsenceBLL.Instance.Absences.Where(absence => absence.StudentSubject.Student.ID == Student.ID
            && absence.StudentSubject.Subject.ID == Subject.ID && absence.Semester == Semester).ToObservableCollection();
            ErrorMessage = String.Empty;
        }

        private void Update()
        {
            Absences = AbsenceBLL.Instance.Absences.Where(absence => TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.Where(tsc
                   => tsc.Subject.ID == absence.StudentSubject.Subject.ID && tsc.Teacher.ID == UserBLL.Instance.CurrentUser.ID).Count() != 0).ToObservableCollection();
            ErrorMessage = String.Empty;
        }

        private void Clear()
        {
            //Grades = GradeBLL.Instance.Grades.Where(grade => TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.Where(tsc
            //    => tsc.Subject.ID == grade.StudentSubject.Subject.ID && tsc.Teacher.ID == UserBLL.Instance.CurrentUser.ID &&
            //    grade.Semester == Semester).Count() != 0).ToObservableCollection();
            Update();
            Type = Absence.AbsenceType.None;
            Student = null;
            Subject = null;
            SelectedAbsence = null;
            ErrorMessage = String.Empty;
        }
    }
}
