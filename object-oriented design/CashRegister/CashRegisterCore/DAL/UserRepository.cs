using CashRegisterCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using System.Threading.Tasks;
using Newtonsoft.Json;
using CashRegisterCore.Model;

namespace CashRegisterCore.DAL
{
    class UserRepository : IUserRepository
    {
        private static UserRepository _instance;
        private readonly List<User> _listUsers = new List<User>();

        private UserRepository()
        {
            string usersJsonString = File.ReadAllText(@"DatabaseFiles/users.json");
            ListUsers users = JsonConvert.DeserializeObject<ListUsers>(usersJsonString);
            _listUsers = users.users;
        }

        public static UserRepository getInstance()
        {
            return _instance ?? (_instance = new UserRepository());
        }

        public void addUser(User user)
        {
            throw new NotImplementedException();
        }

        public void deleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> getAllUsers()
        {
            return _listUsers;
        }

        /// <summary>
        /// Return user if username and hash maches user in database.
        /// Returns null if there is no match.
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public User getUser(string username, string hash)
        {
            foreach(var user in _listUsers)
            {
                if(user.Username == username && user.Hash == hash)
                {
                    return user;
                } 
            }
            return null;
        }

        public User getUserById(int id)
        {
            foreach(var user in _listUsers)
            {
                if(user.Id == id)
                {
                    return user;
                }
            }
            return null;
        }

        public int getUserNum()
        {
            return _listUsers.Count;
        }

        /// <summary>
        /// Exists for purpose of Newtonsoft.Json
        /// </summary>
        private class ListUsers
        {
            public int Index { get; set; }

            public List<User> users
            {
                get;
                set;
            }
        }
    }
}
