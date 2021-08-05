using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class Absence
    {
        public enum AbsenceType { None, Motivated, Unmotivated, Unmotivatable }
        public int ID { get; set; }
        public StudentSubject StudentSubject { get; set; }
        public bool Semester { get; set; }
        public AbsenceType Type { get; set; }

        public String SemesterFormated { get { return Semester ? "Sem II" : "Sem I"; } }

        public Absence() { }
        public Absence(Absence other)
        {
            this.ID = other.ID;
            this.StudentSubject = other.StudentSubject;
            this.Semester = other.Semester;
            this.Type = other.Type;
        }
    }
}
