using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class TeacherSubjectClass
    {
        public int ID { get; set; }
        public User Teacher { get; set; }
        public Subject Subject { get; set; }
        public Class Class { get; set; }

        public TeacherSubjectClass() { }
        public TeacherSubjectClass(TeacherSubjectClass other)
        {
            this.ID = other.ID;
            this.Teacher = other.Teacher;
            this.Subject = other.Subject;
            this.Class = other.Class;
        }
    }
}
