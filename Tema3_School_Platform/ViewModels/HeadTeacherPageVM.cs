using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tema3_School_Platform.Commands;
using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.ViewModels
{
    class HeadTeacherPageVM : BaseVM
    {
        private Page currentPage;
        public Page CurrentPage
        {
            get { return currentPage; }
            private set
            {
                currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        public ICommand MenuCommand { get; }

        public HeadTeacherPageVM()
        {
            MenuCommand = new RelayCommand<String>(MenuAction);
        }

        private void MenuAction(String option)
        {
            int control = Convert.ToInt32(option);
            if (control == 0)
                ViewNavigator.ChangePage(ViewNavigator.MainPage.Professor);
            CurrentPage = ViewNavigator.HeadTeacherPageControl(control);
        }
    }
}
