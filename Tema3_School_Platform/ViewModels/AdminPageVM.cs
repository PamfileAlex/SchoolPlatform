using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using Tema3_School_Platform.Commands;
using Tema3_School_Platform.Utils;
using Tema3_School_Platform.Views;

namespace Tema3_School_Platform.ViewModels
{
    class AdminPageVM : BasePropertyChanged
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

        public AdminPageVM()
        {
            MenuCommand = new RelayCommand<String>(MenuAction);
        }

        private void MenuAction(String option)
        {
            int control = Convert.ToInt32(option);
            if (control == 0)
                ViewNavigator.ChangePage(ViewNavigator.MainPage.Login);
            CurrentPage = ViewNavigator.AdminPageControl(control);
        }
    }
}
