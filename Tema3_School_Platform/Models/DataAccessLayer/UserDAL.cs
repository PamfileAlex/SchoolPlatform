using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tema3_School_Platform.Models.EntityLayer;

namespace Tema3_School_Platform.Models.DataAccessLayer
{
    static class UserDAL
    {
        public static User UserLogin(String email, String password)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("UserLogin", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@email", email));
                command.Parameters.Add(new SqlParameter("@password", password));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                User user = null;
                if (reader.Read())
                {
                    user = new User(reader.GetInt32(0))
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Password = reader.GetString(4),
                        Role = (User.UserRole)reader.GetInt32(5),
                        Class = reader.IsDBNull(6) ? null : DALHelper.GetClass(reader.GetInt32(6))
                    };
                }
                reader.Close();
                return user;
            }
        }

        public static ObservableCollection<User> GetAllUsers()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetAllUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ObservableCollection<User> users = new ObservableCollection<User>();
                while (reader.Read())
                {
                    users.Add(new User(reader.GetInt32(0))
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Password = reader.GetString(4),
                        Role = (User.UserRole)reader.GetInt32(5),
                        Class = reader.IsDBNull(6) ? null : DALHelper.GetClass(reader.GetInt32(6))
                    });
                }
                reader.Close();
                return users;
            }
        }

        public static void AddUser(User user)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@firstName", user.FirstName),
                    new SqlParameter("@lastName", user.LastName),
                    new SqlParameter("@email", user.Email),
                    new SqlParameter("@password", user.Password),
                    new SqlParameter("@role", (int)user.Role),
                });
                if (user.Class == null)
                    command.Parameters.Add(new SqlParameter("@classID", DBNull.Value));
                else
                    command.Parameters.Add(new SqlParameter("@classID", user.Class.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveUser(User user)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RemoveUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", user.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void ModifyUser(User user)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("ModifyUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@id",user.ID),
                    new SqlParameter("@firstName", user.FirstName),
                    new SqlParameter("@lastName", user.LastName),
                    new SqlParameter("@email", user.Email),
                    new SqlParameter("@password", user.Password),
                    new SqlParameter("@role", (int)user.Role)
                });
                if (user.Class == null)
                    command.Parameters.Add(new SqlParameter("@classID", DBNull.Value));
                else
                    command.Parameters.Add(new SqlParameter("@classID", user.Class.ID));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static bool CheckForEmailExistence(User user)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("CheckForEmailExistenceInUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@email", user.Email));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                bool emailExistence = false;
                if (reader.Read())
                {
                    emailExistence = true;
                }
                reader.Close();
                return emailExistence;
            }
        }
    }
}
