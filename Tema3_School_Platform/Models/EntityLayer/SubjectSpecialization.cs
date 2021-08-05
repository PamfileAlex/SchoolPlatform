using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class SubjectSpecialization : BasePropertyChanged
    {
        public int ID { get; set; }
        public Subject Subject { get; set; }
        public Specialization Specialization { get; set; }
        private bool thesis;
        public bool Thesis
        {
            get { return thesis; }
            set
            {
                thesis = value;
                NotifyPropertyChanged("Thesis");
            }
        }

        public String Format
        {
            get { return Specialization.Name + " - " + Subject.Name; }
        }

        public SubjectSpecialization() { }
        public SubjectSpecialization(SubjectSpecialization other)
        {
            this.ID = other.ID;
            this.Subject = other.Subject;
            this.Specialization = other.Specialization;
            this.Thesis = other.Thesis;
        }
    }
}
