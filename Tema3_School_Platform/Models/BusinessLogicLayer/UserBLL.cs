using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tema3_School_Platform.Exceptions;
using Tema3_School_Platform.Models.DataAccessLayer;
using Tema3_School_Platform.Models.EntityLayer;
using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.Models.BusinessLogicLayer
{
    class UserBLL : BasePropertyChanged
    {
        public static UserBLL Instance { get; } = new UserBLL();
        public ObservableCollection<User> Users { get; }
        public User CurrentUser { get; set; }
        static UserBLL() { }
        private UserBLL()
        {
            ClassBLL.Instance.Init();
            Users = UserDAL.GetAllUsers();
        }

        public void Init() { }

        public User GetUser(int id)
        {
            return Users.First(user => user.ID == id);
        }

        public void UserLogin(String[] loginInfo)
        {
            CurrentUser = UserDAL.UserLogin(loginInfo[0], loginInfo[1]);
            if (CurrentUser == null)
                throw new SchoolPlatformException("Incorrect Email or Password");
            switch (CurrentUser.Role)
            {
                case User.UserRole.Admin:
                    ViewNavigator.ChangePage(ViewNavigator.MainPage.Admin);
                    break;
                case User.UserRole.Professor:
                    ViewNavigator.ChangePage(ViewNavigator.MainPage.Professor);
                    break;
                case User.UserRole.Student:
                    ViewNavigator.ChangePage(ViewNavigator.MainPage.Student);
                    break;
                default:
                    break;
            }
        }

        public void AddUser(User user)
        {
            AdminCheck(user);
            CheckUserFields(user);
            CheckForEmailExistence(user);
            UserDAL.AddUser(user);
            User userFromDB = UserDAL.UserLogin(user.Email, user.Password);
            if (userFromDB == null)
                throw new SchoolPlatformException("Add User failed");
            Users.Add(userFromDB);
            StudentSubject(userFromDB);
        }

        public void RemoveUser(User user)
        {
            AdminCheck(user);
            //if (!Users.Contains(user)) { return; }
            User.UserRole userRole = user.Role;
            if (!Users.Remove(user))
                throw new SchoolPlatformException("Remove User failed");
            UserDAL.RemoveUser(user);

            if (userRole == User.UserRole.Student)
                StudentSubjectBLL.Instance.UpdateStudentSubjectList();
            else if (userRole == User.UserRole.Professor)
                TeacherSubjectClassBLL.Instance.UpdateTeacherSubjectClassList();
        }

        public void ModifyUser(User user, int selectedIndex)
        {
            if (Users[selectedIndex].Role == User.UserRole.Admin)
                throw new SchoolPlatformException("Cannot Modify Admin");
            AdminCheck(user);
            CheckUserFields(user);
            if (!user.Email.Equals(Users[selectedIndex].Email))
                CheckForEmailExistence(user);
            if (user.Class.ID != Users[selectedIndex].Class.ID)
                StudentSubject(user);
            Users[selectedIndex] = user;
            UserDAL.ModifyUser(user);
        }

        private void AdminCheck(User user)
        {
            if (user.Role == User.UserRole.Admin)
                throw new SchoolPlatformException("Cannot set Admin role");
        }

        private void CheckUserFields(User user)
        {
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName)
                || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
                throw new SchoolPlatformException("Please fill all inputs");
            if (user.Role == User.UserRole.None)
                throw new SchoolPlatformException("Please select a role");
            if (user.Role == User.UserRole.Student && user.Class == null)
                throw new SchoolPlatformException("Please select a class");
        }

        private void CheckForEmailExistence(User user)
        {
            if (UserDAL.CheckForEmailExistence(user))
                throw new SchoolPlatformException("Email address is taken");
        }

        private void StudentSubject(User user)
        {
            foreach (var ss in SubjectSpecializationBLL.Instance.SubjectSpecializations)
            {
                if (user.Role != User.UserRole.Student) { continue; }
                if (user.Class.Specialization.ID != ss.Specialization.ID) { continue; }
                try
                {
                    StudentSubjectBLL.Instance.AddStudentSubject(new StudentSubject()
                    {
                        ID = 0,
                        Student = user,
                        Subject = ss.Subject
                    });
                }
                catch { }
            }
            //foreach (var tsc in TeacherSubjectClassBLL.Instance.TeacherSubjectClassList)
            //{
            //    if (user.Class.ID != tsc.Class.ID) { continue; }
            //    try
            //    {
            //        StudentSubjectBLL.Instance.AddStudentSubject(new StudentSubject()
            //        {
            //            ID = 0,
            //            Student = user,
            //            Subject = tsc.Subject,
            //            FirstSemester = false,
            //            SecondSemester = false
            //        });
            //    }
            //    catch { }
            //}
        }
    }
}
