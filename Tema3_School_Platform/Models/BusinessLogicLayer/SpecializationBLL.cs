using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tema3_School_Platform.Models.DataAccessLayer;
using Tema3_School_Platform.Models.EntityLayer;
using Tema3_School_Platform.Exceptions;

namespace Tema3_School_Platform.Models.BusinessLogicLayer
{
    class SpecializationBLL
    {
        public static SpecializationBLL Instance { get; } = new SpecializationBLL();
        public ObservableCollection<Specialization> Specializations { get; }
        static SpecializationBLL() { }
        private SpecializationBLL()
        {
            Specializations = SpecializationDAL.GetSpecializations();
        }

        public void Init() { }

        public Specialization GetSpecialization(int id)
        {
            return Specializations.First(specialization => specialization.ID == id);
        }

        public void AddSpecialization(Specialization specialization)
        {
            CheckForSpecializationExistence(specialization);
            SpecializationDAL.AddSpecialization(specialization);
            Specialization fromDB = SpecializationDAL.GetSpecialization(specialization.Name);
            if (fromDB == null)
                throw new SchoolPlatformException("Add Specialization failed");
            Specializations.Add(fromDB);
        }

        public void RemoveSpecialization(Specialization specialization)
        {
            if (!Specializations.Remove(specialization))
                throw new SchoolPlatformException("Remove Specialization failed");
            SpecializationDAL.RemoveSpecialization(specialization);
            SubjectSpecializationBLL.Instance.UpdateSubjectSpecializations();
        }

        public void ModifySpecialization(Specialization specialization, int selectedIndex)
        {
            if (!specialization.Name.Equals(Specializations[selectedIndex].Name))
                CheckForSpecializationExistence(specialization);
            Specializations[selectedIndex] = specialization;
            SpecializationDAL.ModifySpecialization(specialization);
        }

        public void CheckForSpecializationExistence(Specialization specialization)
        {
            if (SpecializationDAL.CheckForSpecializationExistence(specialization))
                throw new SchoolPlatformException("Name is taken");
        }
    }
}
