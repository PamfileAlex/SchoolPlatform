using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Exceptions
{
    class SchoolPlatformException : ApplicationException
    {
        public SchoolPlatformException(string message) : base(message) { }
    }
}
