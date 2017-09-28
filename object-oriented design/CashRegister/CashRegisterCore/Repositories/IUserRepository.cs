using CashRegisterCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Repositories
{
    interface IUserRepository
    {
        int getUserNum();

        List<User> getAllUsers();

        User getUserById(int id);

        User getUser(string username, string hash);

        void addUser(User user);

        void deleteUser(int userId);

    }
}
