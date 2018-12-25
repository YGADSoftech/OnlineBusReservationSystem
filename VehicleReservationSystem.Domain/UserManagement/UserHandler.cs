using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace VehicleReservationSystem.Domain.UserManagement
{
    public class UserHandler
    {
        public List<User> GetUsers()
        {
            using (var context = new VehiclesContext())
            {
                return context.Users.Include(a => a.RememberPassword).ToList();


            }
        }

        public User GetUserforLoginId(string loginid, string password)
        {
            using (var context = new VehiclesContext())
            {
                return (from u in context.Users
                        .Include("RememberPassword")


                        where u.Email == loginid && u.Password == password
                        select u).FirstOrDefault();
            }
        }

        public User GetUserforEmail(string emailId)
        {
            using (var context = new VehiclesContext())
            {
                return context.Users.Include(a => a.RememberPassword).Where(a => a.Email == emailId).FirstOrDefault();
            }
        }

        public User GetUser(int id)
        {
            using (var context = new VehiclesContext())
            {
                return context.Users.Include(a => a.RememberPassword)
                    .Where(a => a.Id == id).FirstOrDefault();
            }
        }

        public void AddUser(User user)
        {
            using (var context = new VehiclesContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (var context = new VehiclesContext())
            {
                var found = context.Users.Find(id);
                context.Users.Remove(found);
                context.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            using (var context = new VehiclesContext())
            {
                var found = context.Users.Include(a => a.RememberPassword).Where(a => a.Id == user.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(user.FistName))
                {
                    found.FistName = user.FistName;
                }
                if (!string.IsNullOrWhiteSpace(user.LastName))
                {
                    found.LastName = user.LastName;
                }

                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    found.Email = user.Email;
                }

                if (!string.IsNullOrWhiteSpace(user.LoginId))
                {
                    found.LoginId = user.LoginId;
                }
                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    found.Password = user.Password;
                }

                if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    found.PhoneNumber = user.PhoneNumber;
                }

                found.Is_Active = user.Is_Active;

                context.SaveChanges();

            }
        }


        public void UpdateAccount(User user)
        {
            using (var context = new VehiclesContext())
            {
                var found = context.Users.Find(user.Id);
                found.FistName = user.FistName;
                found.LastName = user.LastName;
                found.PhoneNumber = user.PhoneNumber;
                context.SaveChanges();
            }
        }

        public User GetUserForRenewSession(int id)
        {
            using (var context = new VehiclesContext())
            {
                return (from u in context.Users
                        .Include(a => a.RememberPassword)
                        where u.Id == id
                        select u).FirstOrDefault();
            }
        }

        public void ChangePassword(string password, int id)
        {
            using (var context = new VehiclesContext())
            {
                User found = context.Users.Find(id);
                if (found.Password != password)
                {
                    found.Password = password;
                }
                context.SaveChanges();

            }
        }

        public User ForgotPassword(string emailID)
        {
            using (var context = new VehiclesContext())
            {
                User found = context.Users
                    .Where(a => a.Email == emailID).FirstOrDefault();
                return found;
            }
        }

        public void AddContactUser(Contact c)
        {
            using (var context = new VehiclesContext())
            {
                context.Contacts.Add(c);
                context.SaveChanges();
            }
        }

    }
}
