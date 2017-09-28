using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Model
{
    /// <summary>
    /// Represents receipt in cash register.
    /// Contains collection of articles, time and date of purchase,
    /// total amount and vat amount.
    /// </summary>
    public class Receipt
    {
        private int _id;
        private List<PurchasedArticle> _listPurchasedArticle;
        private string _purchaseDate;
        private double _totalAmount;
        private double _vatAmount;

        public Receipt()
        {
            _listPurchasedArticle = new List<PurchasedArticle>();
            _totalAmount = 0;
            _vatAmount = 0;
        }

        public int Id {
            get { return _id; }
            set { _id = value; }
        }


        public List<PurchasedArticle> PurchasedArticles
        {
            get { return _listPurchasedArticle; }
            set { _listPurchasedArticle = value; }
        }

        public string PurchaseDate
        {
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }

        public double TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; }
        }

        public double VatAmount
        {
            get { return _vatAmount; }
            set { _vatAmount = value; }
        }

        public void RemoveArticleEntry(int entryIndex)
        {
            _listPurchasedArticle.RemoveAt(entryIndex);
        }

        /// <summary>
        /// Adds article to receipt
        /// </summary>
        /// <param name="article"></param>
        public void AddArticle(Article article, double amount)
        {
            article.Validate();
            if (amount < 0) throw new ArgumentException("AddArticle amount cannot be lesser then 0");

            PurchasedArticle purchasedArticle = new PurchasedArticle(article, amount);
            _listPurchasedArticle.Add(purchasedArticle);
           
            _totalAmount += Math.Round(article.Price * amount, 2);
            _vatAmount += Math.Round(article.GetVatAmount() * amount, 2);
            _purchaseDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public override string ToString()
        {
            Console.WriteLine("HERE");
            string sb = "";
            sb += "Receipt(\n";
            sb += "  id: " + _id + "\n";
            foreach(var item in _listPurchasedArticle)
            {
                sb += "    " + item.ToString() + ", amount:" + item.Amount + ", " + "total:" + item.Amount * item.Article.Price + "\n";
            }
            sb += "  Total: " + _totalAmount + "\n";
            sb += "  VAT: " + _vatAmount + "\n";
            sb += "  DATE: " + _purchaseDate + "\n)";
            return sb;
        }

        public string ToReportString()
        {
            return "(" + _purchaseDate + ", " + _listPurchasedArticle.Count + ", " + _totalAmount + ")";
        }

        public class PurchasedArticle
        {
            public PurchasedArticle(Article article, double amount)
            {
                Article = article;
                Amount = amount;
            }

            public Article Article
            {
                get;set;
            }

            public double Amount
            {
                get;set;
            }

            public override string ToString()
            {
                return Article.ToString() + "; amount: " + Amount;             
            }
        }
    }
}
