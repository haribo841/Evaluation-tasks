using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserXmlService
    {
        private string dataFilePath = "users.xml";
        public User GetUserById(int id)
        {
            List<User> users = ReadUsersFromXML();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public void UpdateUser(User updatedUser)
        {
            List<User> users = ReadUsersFromXML();
            User existingUser = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser != null)
            {
                // update
                existingUser.Name = updatedUser.Name;
                existingUser.Surname = updatedUser.Surname;
                existingUser.Gender = updatedUser.Gender;
                existingUser.BirthDate = updatedUser.BirthDate;
                existingUser.TelephoneNumber = updatedUser.TelephoneNumber;
                existingUser.Position = updatedUser.Position;
                existingUser.ShoeSize = updatedUser.ShoeSize;

                WriteUsersToXML(users);
            }
        }

        public void DeleteUser(int id)
        {
            List<User> users = ReadUsersFromXML();
            User userToDelete = users.FirstOrDefault(u => u.Id == id);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                WriteUsersToXML(users);
            }
        }
        public List<User> ReadUsersFromXML()
        {
            try
            {
                List<User> users;
                if (File.Exists(dataFilePath))
                {
                    using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                        users = (List<User>)serializer.Deserialize(fileStream);
                    }
                }
                else
                {
                    users = new List<User>();
                }
                return users;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Błąd odczytu danych XML");
                return new List<User>();
            }
        }
        private void WriteUsersToXML(List<User> users)
        {
            using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                serializer.Serialize(fileStream, users);
            }
        }
        public void WriteUsersToXML(User user)
        {
            List<User> users = ReadUsersFromXML();
            users.Add(user);

            using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                serializer.Serialize(fileStream, users);
            }
        }
    }
}