using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class FinalGrade
    {
        public int ID { get; set; }
        public StudentSubject StudentSubject { get; set; }
        public bool Semester { get; set; }
        public float Value { get; set; }

        public String SemesterFormated { get { return Semester ? "Sem II" : "Sem I"; } }

        public FinalGrade() { }
        public FinalGrade(FinalGrade other)
        {
            this.ID = other.ID;
            this.StudentSubject = other.StudentSubject;
            this.Semester = other.Semester;
            this.Value = other.Value;
        }
    }
}
