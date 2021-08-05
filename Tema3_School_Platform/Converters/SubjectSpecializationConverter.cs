using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.Converters
{
    sealed class SubjectSpecializationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null) { return null; }
            return new SubjectSpecialization()
            {
                Subject = values[0] as Subject,
                Specialization = values[1] as Specialization
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
