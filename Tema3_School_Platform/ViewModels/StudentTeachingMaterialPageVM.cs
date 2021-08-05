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
    class StudentTeachingMaterialPageVM : BaseVM
    {
        public ObservableCollection<TeachingMaterial> TeachingMaterials
        {
            get
            {
                return TeachingMaterialBLL.Instance.TeachingMaterials.Where(tm => tm.TeacherSubjectClass.Class.ID == UserBLL.Instance.CurrentUser.Class.ID
                && (Subject == null || Subject.ID == tm.TeacherSubjectClass.Subject.ID)).ToObservableCollection();
                //List<int> tscList = TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.Where(tsc => (Subject == null || tsc.Subject.ID == Subject.ID)
                //& tsc.Class.ID == UserBLL.Instance.CurrentUser.Class.ID).Select(tsc => tsc.ID).ToList();
                //return TeachingMaterialBLL.Instance.TeachingMaterials.Where(tm => tscList.Contains(tm.TeacherSubjectClass.ID)).ToObservableCollection();
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
                NotifyPropertyChanged("TeachingMaterials");
            }
        }

        public ICommand ClearCommand { get; }
        public ICommand CopyCommand { get; }

        public StudentTeachingMaterialPageVM()
        {
            this.ClearCommand = new ActionCommand(Clear);
            this.CopyCommand = new RelayCommand<TeachingMaterial>(tm => ErrorWrapper(() => { TeachingMaterialBLL.Instance.CopyTeachingMaterial(tm); Clear(); }));
        }

        private void Clear()
        {
            Subject = null;
            ErrorMessage = String.Empty;
        }
    }
}
