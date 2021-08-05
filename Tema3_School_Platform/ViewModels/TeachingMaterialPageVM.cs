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
    class TeachingMaterialPageVM : BaseVM
    {
        public ObservableCollection<TeachingMaterial> Materials
        {
            get
            {
                return TeachingMaterialBLL.Instance.TeachingMaterials.Where(tm => tm.TeacherSubjectClass.Teacher.ID == UserBLL.Instance.CurrentUser.ID
                && (Subject == null || Subject.ID == tm.TeacherSubjectClass.Subject.ID) && (Class == null || Class.ID == tm.TeacherSubjectClass.Class.ID)).ToObservableCollection();
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
        public ObservableCollection<Class> Classes
        {
            get
            {
                if (Subject == null)
                    return null;
                return TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.Where(tsc => tsc.Teacher.ID == UserBLL.Instance.CurrentUser.ID
                && tsc.Subject.ID == Subject.ID).Select(tsc => tsc.Class).Distinct().ToObservableCollection();
                //return TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.Where(tsc => tsc.Teacher.ID == UserBLL.Instance.CurrentUser.ID
                //&& Subjects.Select(s => s.ID).Contains(tsc.Subject.ID)).Select(tsc => tsc.Class).Distinct().ToObservableCollection();
            }
        }

        private Class classObj;
        public Class Class
        {
            get { return classObj; }
            set
            {
                classObj = value;
                NotifyPropertyChanged("Class");
                NotifyPropertyChanged("Materials");
                SelectedMaterial = null;
                ErrorMessage = String.Empty;
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
                NotifyPropertyChanged("Classes");
                NotifyPropertyChanged("Materials");
                SelectedMaterial = null;
                ErrorMessage = String.Empty;
            }
        }

        private TeachingMaterial selectedMaterial;
        public TeachingMaterial SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                selectedMaterial = value;
                NotifyPropertyChanged("SelectedMaterial");
            }
        }

        private String materialName;
        public String MaterialName
        {
            get { return materialName; }
            set
            {
                materialName = value;
                NotifyPropertyChanged("MaterialName");
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
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ClearCommand { get; }

        public TeachingMaterialPageVM()
        {
            this.AddCommand = new RelayCommand<TeachingMaterial>(tm => ErrorWrapper(() => { TeachingMaterialBLL.Instance.AddTeachingMaterial(tm); Clear(); }));
            this.RemoveCommand = new RelayCommand<TeachingMaterial>(tm => ErrorWrapper(() => { TeachingMaterialBLL.Instance.RemoveTeachingMaterial(tm); Clear(); }));
            this.ClearCommand = new ActionCommand(Clear);
        }

        private void Clear()
        {
            Subject = null;
            Class = null;
            NotifyPropertyChanged("Materials");
            SelectedMaterial = null;
            MaterialName = String.Empty;
            ErrorMessage = String.Empty;
        }
    }
}
