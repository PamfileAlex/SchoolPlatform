using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tema3_School_Platform.Utils;

namespace Tema3_School_Platform.Models.EntityLayer
{
    class User : BasePropertyChanged
    {
        public enum UserRole : int
        {
            None, Admin, Professor, Student
        }

        public int ID { get; }

        private String firstName;
        public String FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        private String lastName;
        public String LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                NotifyPropertyChanged("LastName");
            }
        }

        private String email;
        public String Email
        {
            get { return email; }
            set
            {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        private String password;
        public String Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged("Password");
            }
        }

        private UserRole role;
        public UserRole Role
        {
            get { return role; }
            set
            {
                role = value;
                NotifyPropertyChanged("Role");
            }
        }

        private Class classObj;
        public Class Class
        {
            get { return classObj; }
            set
            {
                classObj = value;
                NotifyPropertyChanged("Class");
            }
        }

        public String Name
        {
            get { return FirstName + " " + LastName; }
        }

        public static User NullUser { get; } = new User(0);
        public User(int id)
        {
            this.ID = id;
        }

        public User(User other)
        {
            this.ID = other.ID;
            Copy(other);
        }

        public User(int id, User other) : this(other)
        {
            this.ID = id;
        }

        public void Copy(User other)
        {
            this.FirstName = other.FirstName == null ? String.Empty : String.Copy(other.FirstName);
            this.LastName = other.LastName == null ? String.Empty : String.Copy(other.LastName);
            this.Email = other.Email == null ? String.Empty : String.Copy(other.Email);
            this.Password = other.Password == null ? String.Empty : String.Copy(other.Password);
            this.Role = other.Role;
            this.Class = other.Class;
        }
    }
}
