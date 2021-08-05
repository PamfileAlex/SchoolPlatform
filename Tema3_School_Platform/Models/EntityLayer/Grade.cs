using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class Grade
    {
        public int ID { get; set; }
        public StudentSubject StudentSubject { get; set; }
        public bool Semester { get; set; }
        public float Value { get; set; }
        public bool Thesis { get; set; }

        public String SemesterFormated { get { return Semester ? "Sem II" : "Sem I"; } }
        public String ThesisFormated { get { return Thesis ? "YES" : "NO"; } }

        public Grade() { }
        public Grade(Grade other)
        {
            this.ID = other.ID;
            this.StudentSubject = other.StudentSubject;
            this.Semester = other.Semester;
            this.Value = other.Value;
            this.Thesis = other.Thesis;
        }
    }
}
