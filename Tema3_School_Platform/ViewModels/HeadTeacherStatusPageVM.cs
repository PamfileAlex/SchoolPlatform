using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tema3_School_Platform.Commands;
using Tema3_School_Platform.Models.BusinessLogicLayer;
using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.ViewModels
{
    class HeadTeacherStatusPageVM : BaseVM
    {
        private List<User> Students { get; }
        public List<String> HierarchyList { get; }
        public List<String> Mentions { get; }
        public List<String> Corigents { get; }
        public List<String> Repeaters { get; }
        public List<String> Exmatriculated { get; }

        public String First { get; set; }
        public String Second { get; set; }
        public String Third { get; set; }

        private bool hierarchy;
        public bool Hierarchy
        {
            get { return hierarchy; }
            set
            {
                hierarchy = value;
                NotifyPropertyChanged("Hierarchy");
            }
        }

        private bool status;
        public bool Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private bool exmatriculation;
        public bool Exmatriculation
        {
            get { return exmatriculation; }
            set
            {
                exmatriculation = value;
                NotifyPropertyChanged("Exmatriculation");
            }
        }

        public ICommand MenuCommand { get; }

        public HeadTeacherStatusPageVM()
        {
            Students = UserBLL.Instance.Users.Where(user => user.Role == User.UserRole.Student && user.Class.ID == UserBLL.Instance.CurrentUser.Class.ID).ToList();
            MenuCommand = new RelayCommand<String>(MenuAction);
            HierarchyList = GetStudentList();
            Mentions = new List<String>();
            Corigents = new List<String>();
            Repeaters = new List<String>();
            Exmatriculated = GetExmatriculated();
            HonorStudents();
            CorigentsAndRepeaters();
        }

        private void MenuAction(String option)
        {
            int control = Convert.ToInt32(option);
            switch (control)
            {
                case 0:
                    Hierarchy = true;
                    Status = false;
                    Exmatriculation = false;
                    break;
                case 1:
                    Hierarchy = false;
                    Status = true;
                    Exmatriculation = false;
                    break;
                case 2:
                    Hierarchy = false;
                    Status = false;
                    Exmatriculation = true;
                    break;
                default:
                    break;
            }
        }

        private List<String> GetStudentList()
        {
            List<Tuple<String, float>> inter = new List<Tuple<string, float>>();
            foreach (var student in Students)
            {
                List<int> studentSubjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss => ss.Student.ID == student.ID).Select(ss => ss.ID).ToList();
                float value = CalculateGeneralGrade(studentSubjects);
                inter.Add(new Tuple<string, float>(student.Name, (float)Math.Round(value, 2)));
            }
            inter.Sort((item1, item2) => item2.Item2.CompareTo(item1.Item2));
            return inter.Select(item => item.Item1 + " - " + item.Item2).ToList();
        }

        private float CalculateGeneralGrade(List<int> studentSubjectIDs)
        {
            float value;
            try
            {
                value = FinalGradeBLL.Instance.FinalGrades.Where(finalGrade => studentSubjectIDs.Contains(finalGrade.StudentSubject.ID)).Select(fg => fg.Value).Average();
            }
            catch { value = 0; }
            return value;
        }

        private void HonorStudents()
        {
            for (int index = 0; index < HierarchyList.Count(); ++index)
            {
                if (index == 0)
                    First = HierarchyList[index];
                else if (index == 1)
                    Second = HierarchyList[index];
                else if (index == 2)
                    Third = HierarchyList[index];
                else if (index > 5)
                    break;
                if (index < 3)
                    continue;
                Mentions.Add(HierarchyList[index]);
            }
        }

        private void CorigentsAndRepeaters()
        {
            foreach (var student in Students)
            {
                List<StudentSubject> studentSubjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss => ss.Student.ID == student.ID).ToList();
                List<String> corigent = new List<String>();
                float value = CalculateGeneralGrade(studentSubjects.Select(ss => ss.ID).ToList());
                foreach (var ss in studentSubjects)
                {
                    try
                    {
                        if (FinalGradeBLL.Instance.FinalGrades.Where(finalGrade => finalGrade.StudentSubject.ID == ss.ID).Select(fg => fg.Value).Average() < 5)
                        {
                            corigent.Add(ss.Subject.Name);
                        }
                    }
                    catch { }
                }
                if (corigent.Count == 0)
                    continue;
                if (corigent.Count > 2)
                {
                    Repeaters.Add(student.Name + " - " + Math.Round(value, 2));
                    return;
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(student.Name + " - " + Math.Round(value, 2));
                foreach (var cor in corigent)
                {
                    stringBuilder.Append("\n" + cor);
                }
                Corigents.Add(stringBuilder.ToString());
            }
        }

        private List<String> GetExmatriculated()
        {
            int permitedAbsences = Convert.ToInt32(ConfigurationManager.AppSettings["PermitedAbsences"]); ;
            List<String> exmat = new List<String>();
            foreach (var student in Students)
            {
                List<int> studentSubjects = StudentSubjectBLL.Instance.StudentSubjectList.Where(ss => ss.Student.ID == student.ID).Select(ss => ss.ID).ToList();
                int sem1 = AbsenceBLL.Instance.Absences.Where(abs => abs.Type != Absence.AbsenceType.Motivated && abs.Semester == false && studentSubjects.Contains(abs.StudentSubject.ID)).Count();
                int sem2 = AbsenceBLL.Instance.Absences.Where(abs => abs.Type != Absence.AbsenceType.Motivated && abs.Semester == true && studentSubjects.Contains(abs.StudentSubject.ID)).Count();
                if (sem1 > permitedAbsences || sem2 > permitedAbsences)
                    exmat.Add(student.Name);
            }
            return exmat;
        }
    }
}
