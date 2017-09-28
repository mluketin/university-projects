using CashRegisterCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashRegisterCore.Model;
using System.IO;
using Newtonsoft.Json;

namespace CashRegisterCore.DAL
{
    class ReceiptRepository : IReceiptRepository
    {
        private static string DATABASE_FILE_PATH = @"DatabaseFiles/receipts.json";
        private static ReceiptRepository _instance;
        private readonly List<Receipt> _listReceipts = new List<Receipt>();
        private int currentId;

        private ReceiptRepository()
        {
            string articleJsonString = File.ReadAllText(DATABASE_FILE_PATH);
            ListReceipts receipts = JsonConvert.DeserializeObject<ListReceipts>(articleJsonString);
            _listReceipts = receipts.receipts;
            currentId = receipts.IdCounter;
        }

        public static ReceiptRepository getInstance()
        {
            return _instance ?? (_instance = new ReceiptRepository());
        }

        public void addReceipt(Receipt receipt)
        {
            receipt.Id = currentId + 1;
            currentId += 1;
            _listReceipts.Add(receipt);
            SaveChanges();
        }

        public List<Receipt> getAllReceipts()
        {
            return _listReceipts;
        }

        public List<Receipt> getAllReceiptsByDate(string date)
        {
            List<Receipt> list = new List<Receipt>();
            foreach(var item in _listReceipts)
            {
                var purchaseDate = item.PurchaseDate.Split(' ')[0];
                if ( purchaseDate == date)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public Receipt getReceiptById(int id)
        {
            foreach(var item in _listReceipts)
            {
                if(item.Id == id)
                {
                    return item;
                }
            }
            throw new ReceiptDoesntExistException();
        }

        public void deleteReceipt(int id)
        {
            Receipt receipt = getReceiptById(id);
            _listReceipts.Remove(receipt);
            SaveChanges();
        }


        public void modifyReceipt(int id, Receipt receipt)
        {
            Receipt oldReceipt = getReceiptById(id);
            oldReceipt.PurchasedArticles = receipt.PurchasedArticles;
            oldReceipt.PurchaseDate = receipt.PurchaseDate;
            oldReceipt.TotalAmount = receipt.TotalAmount;
            oldReceipt.VatAmount = receipt.VatAmount;
            SaveChanges();
        }

        private void SaveChanges()
        {
            ListReceipts newList = new ListReceipts(_listReceipts, currentId);
            string newJsonString = JsonConvert.SerializeObject(newList, Formatting.Indented);
            Console.WriteLine(newJsonString);
            File.WriteAllText(DATABASE_FILE_PATH, newJsonString);
        }

     

        private class ListReceipts
        {
            public ListReceipts(List<Receipt> list, int idCounter)
            {
                IdCounter = idCounter;
                receipts = list;
            }

            public int IdCounter { get; set; }

            public List<Receipt> receipts
            {
                get;
                set;
            }
        }
    }
}
