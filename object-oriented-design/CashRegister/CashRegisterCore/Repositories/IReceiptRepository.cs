using CashRegisterCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Repositories
{
    interface IReceiptRepository
    {
        List<Receipt> getAllReceipts();

        void addReceipt(Receipt receipt);

        List<Receipt> getAllReceiptsByDate(string date);

        void deleteReceipt(int id);

        Receipt getReceiptById(int id);

        void modifyReceipt(int id, Receipt receipt);

    }
}
