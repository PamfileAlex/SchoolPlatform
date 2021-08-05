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
    class UserPageVM : BaseVM
    {
        public ObservableCollection<User> Users { get { return UserBLL.Instance.Users; } }
        public ObservableCollection<Class> Classes { get { return ClassBLL.Instance.Classes; } }
        public ObservableCollection<Class> NoHeadTeacherClasses
        {
            get
            {
                ObservableCollection<Class> noHeadTeacherClasses = new ObservableCollection<Class>();
                if (User.Class != null)
                    noHeadTeacherClasses.Add(User.Class);
                foreach (var classObj in ClassBLL.Instance.Classes)
                {
                    if (Users.Where(user => user.Role == User.UserRole.Professor && user.Class != null && user.Class.ID == classObj.ID).Count() == 0)
                        noHeadTeacherClasses.Add(classObj);
                }
                return noHeadTeacherClasses;
            }
        }
        private User user;
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                NotifyPropertyChanged("User");
            }
        }

        private int dataGridSelectedIndex;
        public int DataGridSelectedIndex
        {
            get { return dataGridSelectedIndex; }
            set
            {
                dataGridSelectedIndex = value;
                NotifyPropertyChanged("SelectedIndex");
                ErrorMessage = String.Empty;
                if (DataGridSelectedIndex == -1) { User = new User(0); return; }
                User = new User(UserBLL.Instance.Users[DataGridSelectedIndex]);
                NotifyPropertyChanged("NoHeadTeacherClasses");
            }
        }

        public List<User.UserRole> UserRoles { get; }

        public ICommand AddCommand { get; }
        public ICommand ModifyCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand ClearClassCommand { get; }

        public UserPageVM()
        {
            Clear();
            UserRoles = Enum.GetValues(typeof(User.UserRole)).Cast<User.UserRole>().ToList();
            this.AddCommand = new RelayCommand<User>(user => ErrorWrapper(() => { UserBLL.Instance.AddUser(user); Clear(); }));
            this.ModifyCommand = new RelayCommand<User>(user => ErrorWrapper(() => { UserBLL.Instance.ModifyUser(user, DataGridSelectedIndex); Clear(); }));
            this.RemoveCommand = new RelayCommand<User>(user => ErrorWrapper(() => { UserBLL.Instance.RemoveUser(UserBLL.Instance.Users[DataGridSelectedIndex]); Clear(); }));
            this.ClearCommand = new ActionCommand(Clear);
            this.ClearClassCommand = new ActionCommand(ClearClass);
        }

        private void Clear()
        {
            DataGridSelectedIndex = -1;
        }

        private void ClearClass()
        {
            User.Class = null;
        }
    }
}
