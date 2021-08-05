using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class Class : BasePropertyChanged
    {
        public int ID { get; }
        private Specialization specialization;
        public Specialization Specialization
        {
            get { return specialization; }
            set
            {
                specialization = value;
                NotifyPropertyChanged("Specialization");
            }
        }

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

        private String year;
        public String Year
        {
            get { return year; }
            set
            {
                year = value;
                NotifyPropertyChanged("Year");
            }
        }

        public Class(int id)
        {
            this.ID = id;
        }

        public Class(Class other)
        {
            this.ID = other.ID;
            this.Specialization = other.Specialization;
            this.Name = other.Name == null ? String.Empty : String.Copy(other.Name);
            this.Year = other.Year == null ? String.Empty : String.Copy(other.Year);
        }

        public Class(int id, Class other) : this(other)
        {
            this.ID = id;
        }
    }
}
