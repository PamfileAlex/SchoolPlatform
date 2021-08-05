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

namespace Tema3_School_Platform.ViewModels
{
    class ClassPageVM : BaseVM
    {
        public ObservableCollection<Class> Classes { get { return ClassBLL.Instance.Classes; } }
        public ObservableCollection<Specialization> Specializations { get { return SpecializationBLL.Instance.Specializations; } }
        private Class classObj;
        public Class Class
        {
            get { return classObj; }
            set
            {
                classObj = value;
                NotifyPropertyChanged("Class");
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                NotifyPropertyChanged("SelectedIndex");
                ErrorMessage = String.Empty;
                if (SelectedIndex == -1) { Class = new Class(0); return; }
                Class = new Class(ClassBLL.Instance.Classes[SelectedIndex]);
            }
        }

        public ICommand AddCommand { get; }
        public ICommand ModifyCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ClearCommand { get; }

        public ClassPageVM()
        {
            Clear();
            this.AddCommand = new RelayCommand<Class>(classObj => ErrorWrapper(() => { ClassBLL.Instance.AddClass(classObj); Clear(); }));
            this.ModifyCommand = new RelayCommand<Class>(classObj => ErrorWrapper(() => { ClassBLL.Instance.ModifyClass(classObj, SelectedIndex); Clear(); }));
            this.RemoveCommand = new RelayCommand<Class>(classObj => ErrorWrapper(() => { ClassBLL.Instance.RemoveClass(Classes[SelectedIndex]); Clear(); }));
            this.ClearCommand = new ActionCommand(Clear);
        }

        private void Clear()
        {
            SelectedIndex = -1;
            ErrorMessage = String.Empty;
        }
    }
}
