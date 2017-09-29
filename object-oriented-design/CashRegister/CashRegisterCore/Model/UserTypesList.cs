using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Model
{
    class UserTypesList
    {
        public enum UserTypesEnum : int
        {
            ADMIN = 1,
            CASHIER = 2
        };
    }
}
