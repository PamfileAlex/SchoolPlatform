using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class Subject : BasePropertyChanged
    {
        public int ID { get; }

        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public Subject(int id)
        {
            this.ID = id;
        }

        public Subject(Subject other)
        {
            this.ID = other.ID;
            this.Name = other.Name == null ? String.Empty : String.Copy(other.Name);
        }

        public Subject(int id, Subject other) : this(other)
        {
            this.ID = id;
        }
    }
}
