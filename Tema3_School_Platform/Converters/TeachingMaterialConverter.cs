using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Tema3_School_Platform.Models.BusinessLogicLayer;
using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.Converters
{
    class TeachingMaterialConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Subject subject = values[0] as Subject;
            Class classObj = values[1] as Class;
            if (subject == null || classObj == null)
                return null;
            TeacherSubjectClass tscResult = null;
            try
            {
                tscResult = TeacherSubjectClassBLL.Instance.TeacherSubjectClassList.First(tsc => tsc.Subject.ID == subject.ID
                && tsc.Teacher.ID == UserBLL.Instance.CurrentUser.ID && tsc.Class.ID == classObj.ID);
            }
            catch { return null; }
            return new TeachingMaterial()
            {
                ID = 0,
                TeacherSubjectClass = tscResult,
                Name = values[2].ToString(),
                Semester = (bool)values[3],
                Path = String.Empty
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
