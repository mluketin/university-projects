using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Model
{
   [Serializable]
   public class ArticleDoesntExistException: Exception
   { }

    [Serializable]
    public class ReceiptDoesntExistException : Exception
    { }

    [Serializable]
    public class UserNotLoggedInException : Exception
    { }

    [Serializable]
    public class UserNotAdminException : Exception
    { }

    [Serializable]
    public class PreviousReceiptNotClosedException: Exception
    { }

    [Serializable]
    public class ReceiptNotOpenedException : Exception
    { }

}
