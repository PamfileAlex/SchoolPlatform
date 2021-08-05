using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Tema3_School_Platform.Commands;
using Tema3_School_Platform.Models.BusinessLogicLayer;

namespace Tema3_School_Platform.ViewModels
{
    class LoginPageVM : BaseVM
    {
        public ICommand LoginCommand { get; }

        public LoginPageVM()
        {
            //LoginCommand = new RelayCommand<String[]>(UserBLL.Instance.UserLogin);
            LoginCommand = new RelayCommand<String[]>(info => ErrorWrapper(() => UserBLL.Instance.UserLogin(info)));
        }
    }
}
