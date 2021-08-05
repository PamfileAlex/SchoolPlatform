using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tema3_School_Platform.Utils;
using Tema3_School_Platform.Exceptions;

namespace Tema3_School_Platform.ViewModels
{
    abstract class BaseVM : BasePropertyChanged
    {
        protected String errorMessage;
        public String ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        protected delegate void wrapperDelegate();

        protected void ErrorWrapper(wrapperDelegate myDelegate)
        {
            try
            {
                myDelegate();
            }
            catch (SchoolPlatformException exc)
            {
                ErrorMessage = exc.Message;
            }
        }
    }
}
