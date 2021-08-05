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
    class GradeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            User student = values[0] as User;
            Subject subject = values[1] as Subject;
            if (student == null || subject == null) { return null; }
            float value;
            try
            {
                value = float.Parse(values[3] as String);
            }
            catch { value = default; }
            StudentSubject studentSubject;
            try
            {
                studentSubject = StudentSubjectBLL.Instance.GetStudentSubject(student.ID, subject.ID);
            }
            catch { studentSubject = null; }
            return new Grade()
            {
                ID = 0,
                StudentSubject = studentSubject,
                Semester = (bool)values[2],
                Value = value,
                Thesis = (bool)values[4]
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
