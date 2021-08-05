using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tema3_School_Platform.Views;

namespace Tema3_School_Platform.Utils
{
    static class ViewNavigator
    {
        public enum MainPage
        {
            None, Login, Admin, Professor, Student, HeadTeacher
        }
        private static MainWindow mainWindow;
        public static MainWindow MainWindow
        {
            set
            {
                //if (mainWindow != null) { return; }
                mainWindow = value;
            }
        }

        public static void ChangePage(MainPage page)
        {
            if (mainWindow == null) { return; }
            switch (page)
            {
                case MainPage.None:
                    return;
                case MainPage.Login:
                    mainWindow.Content = new LoginPage();
                    break;
                case MainPage.Admin:
                    mainWindow.Content = new AdminPage();
                    break;
                case MainPage.Professor:
                    mainWindow.Content = new ProfessorPage();
                    break;
                case MainPage.Student:
                    mainWindow.Content = new StudentPage();
                    break;
                case MainPage.HeadTeacher:
                    mainWindow.Content = new HeadTeacherPage();
                    break;
            }
        }

        public static Page AdminPageControl(int option)
        {
            switch (option)
            {
                case 1:
                    return new UserPage();
                case 2:
                    return new SubjectSpecializationPage();
                case 3:
                    return new ClassPage();
                case 4:
                    return new TeacherSubjectClassPage();
                default:
                    return null;
            }
        }

        public static Page ProfessorPageControl(int option)
        {
            switch (option)
            {
                case 2:
                    return new GradePage();
                case 3:
                    return new AbsencePage();
                case 4:
                    return new TeachingMaterialPage();
                default:
                    return null;
            }
        }

        public static Page StudentPageControl(int option)
        {
            switch (option)
            {
                case 1:
                    return new StudentGradePage();
                case 2:
                    return new StudentAbsencePage();
                case 3:
                    return new StudentTeachingMaterialPage();
                case 4:
                    return new StudentFinalGradePage();
                default:
                    return null;
            }
        }

        public static Page HeadTeacherPageControl(int option)
        {
            switch (option)
            {
                case 1:
                    return new HeadTeacherAbsencePage();
                case 2:
                    return new HeadTeacherFinalGradePage();
                case 3:
                    return new HeadTeacherStatusPage();
                default:
                    return null;
            }
        }
    }
}
