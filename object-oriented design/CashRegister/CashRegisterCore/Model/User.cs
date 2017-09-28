using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Model
{
    class User
    {
        /*
        public int id;
        public string username;
        public string hash;
        public string name;
        public string surename;
        public string role;
        /*
        private string _name;
        private string _surename;
        private UserTypesList.UserTypesEnum _userType;
        /*
         * */
        public int Id
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Hash
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Surename
        {
            get;
            set;
        }

        public string Role
        {
            get;
            set;
        }

        /*
        public UserTypesList.UserTypesEnum UserType
        {
            get;
            set;
        }
        */

        public override string ToString()
        {
            return string.Format("User({0}, {1}, {2})", Name, Surename, Role);
        }

    }
}
