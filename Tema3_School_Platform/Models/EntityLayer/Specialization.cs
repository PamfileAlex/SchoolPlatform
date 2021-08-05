using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class Specialization : BasePropertyChanged
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

        public Specialization(int id)
        {
            this.ID = id;
        }

        public Specialization(Specialization other)
        {
            this.ID = other.ID;
            this.Name = other.Name == null ? String.Empty : String.Copy(other.Name);
        }

        public Specialization(int id, Specialization other) : this(other)
        {
            this.ID = id;
        }
    }
}
