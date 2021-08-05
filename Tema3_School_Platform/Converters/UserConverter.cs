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
    sealed class UserConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Contains(null)) { return null; }
            return new User(-1)
            {
                FirstName = values[0].ToString(),
                LastName = values[1].ToString(),
                Email = values[2].ToString(),
                Password = values[3].ToString()
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
