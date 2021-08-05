using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class TeachingMaterial
    {
        public int ID { get; set; }
        public TeacherSubjectClass TeacherSubjectClass { get; set; }
        public bool Semester { get; set; }
        public String Name { get; set; }
        public String Path { get; set; }

        public String SemesterFormated { get { return Semester ? "Sem II" : "Sem I"; } }

        public TeachingMaterial() { }
        public TeachingMaterial(TeachingMaterial other)
        {
            this.ID = other.ID;
            this.TeacherSubjectClass = other.TeacherSubjectClass;
            this.Semester = other.Semester;
            this.Name = other.Name == null ? String.Empty : String.Copy(other.Name);
            this.Path = other.Path == null ? String.Empty : String.Copy(other.Path);
        }
    }
}
