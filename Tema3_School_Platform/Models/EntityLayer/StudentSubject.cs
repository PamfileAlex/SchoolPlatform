using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class StudentSubject
    {
        public int ID { get; set; }
        public User Student { get; set; }
        public Subject Subject { get; set; }
        public StudentSubject() { }
        public StudentSubject(StudentSubject other)
        {
            this.ID = other.ID;
            this.Student = other.Student;
            this.Subject = other.Subject;
        }
    }
}
