using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CashRegisterCore.DAL;
using CashRegisterCore.Repositories;
using CashRegisterCore.Model;

namespace CashRegisterCore.Model
{
    public class CashRegisterModel
    { 
        /// <summary>
        /// Current user that is using cash register
        /// </summary>
        private User _activeUser = null;
        private Receipt _activeReceipt = null;
        private IUserRepository _userRepository;
        private IArticleRepository _articleRepository;
        private IReceiptRepository _receiptRepository; 

        public CashRegisterModel()
        {
            _userRepository = UserRepository.getInstance();
            _articleRepository = ArticleRepository.getInstance();
            _receiptRepository = ReceiptRepository.getInstance();  
        }


        public bool Login(string username, string password)
        {
            string hash = Sha1Util.SHA1HashStringForUTF8String(password);
            User user = _userRepository.getUser(username, hash);
            if(user == null)
            {
                return false;
            }
            _activeUser = user;
            return true;
        }

        public bool isAdminLogged()
        {
            ValidateActiveUser();
            if (_activeUser.Role == "admin") return true;
            return false;
        }

        public bool isReceiptOpen()
        {
            ValidateActiveUser();

            if (_activeReceipt == null) return false;
            return true;
        }

        /// <summary>
        /// Defines new article for cash register
        /// </summary>
        /// <param name="id">Unique id for article</param>
        /// <param name="name">Name for article</param>
        /// <param name="price">Article total price (vat amount included)</param>
        /// <param name="vat">Article price percentage (eq if vat is 25%, method argument is 25)</param>
        public void DefineNewArticle(int id, string name, double price, double vat)
        {
            ValidateAdmin();
            
            Article article = new Article(id, name, price, vat);
            if(!_articleRepository.addArticle(article))
            {
                throw new ArgumentException("Article already exists in register");
            }
        }

        public void OpenNewReceipt()
        {
            ValidateActiveUser();

            if(_activeReceipt != null)
            {
                throw new PreviousReceiptNotClosedException();
            }
            _activeReceipt = new Receipt();
        }

        public void CancelReceipt()
        {
            ValidateActiveUser();

            _activeReceipt = null;
        }

        public void AddArticleToReceipt(int articleId, double amount)
        {
            AddArticleToReceipt(_articleRepository.getArticleById(articleId), amount);
        }

        public void AddArticleToReceipt(Article article, double amount)
        {
            ValidateActiveUser();
            ValidateActiveReceipt();
            _activeReceipt.AddArticle(article, amount);
        }

        public void PrintReceipt()
        {
            ValidateActiveUser();
            ValidateActiveReceipt();

            _receiptRepository.addReceipt(_activeReceipt);
            Console.WriteLine(_activeReceipt.ToString());
            _activeReceipt = null;
        }

        public void SeeReceiptOutput()
        {
            ValidateActiveUser();
            ValidateActiveReceipt();
            Console.WriteLine(_activeReceipt.ToString());
        }

        public void SetReceiptAsActive(int receiptId)
        {
            if (_activeReceipt != null)
                throw new PreviousReceiptNotClosedException();

            _activeReceipt = _receiptRepository.getReceiptById(receiptId);
        }

        public void RemoveReceiptArticleEntry(int articlePosition)
        {
            _activeReceipt.RemoveArticleEntry(articlePosition);
        }

        public void ModifyReceipt(int id, Receipt receipt)
        {

            _receiptRepository.modifyReceipt(id, receipt);
        }

        public void DeleteReceipt(int id)
        {
            _receiptRepository.deleteReceipt(id);
        }

        public void PrintDailyReport()
        {
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
            var list = _receiptRepository.getAllReceiptsByDate(currentDate);
            
            Console.WriteLine("DAILY REPORT: ");
            Console.WriteLine("  Receipts: ");
            double totalAmount = 0;
            foreach (var item in list)
            {
                Console.WriteLine("    " + item.ToReportString());
                totalAmount += item.TotalAmount;
            }
            Console.WriteLine("  Number of receipts: " + list.Count);
            Console.WriteLine("  Total amount: " + totalAmount);
        }

        public void ArticleReportByTotalValueSold()
        {
            Dictionary<Article, double> dict = new Dictionary<Article, double>();
            var receipts = _receiptRepository.getAllReceipts();
            foreach(var receipt in receipts)
            {
                var purchasedArticles = receipt.PurchasedArticles;
                foreach(var purchasedArticle in purchasedArticles)
                {
                    var article = purchasedArticle.Article;
                    var amount = purchasedArticle.Amount;

                    if(dict.ContainsKey(article))
                    {
                        dict[article] += article.Price * amount;
                    }
                    else
                    {
                        dict.Add(article, article.Price * amount);
                    }
                }
            }
            var reportList = dict.ToList();
            reportList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            Console.WriteLine("Article report by total value sold (inlude vat)");
            Console.WriteLine("Value, Article(id, name, price, vat)");
            foreach (var item in reportList)
            {
                Console.WriteLine("  " + item.Value + " " + item.Key);
            }
        }

        public void ArticleReportByTotalNumberSold()
        {
            Dictionary<Article, double> dict = new Dictionary<Article, double>();
            var receipts = _receiptRepository.getAllReceipts();
            foreach (var receipt in receipts)
            {
                var purchasedArticles = receipt.PurchasedArticles;
                foreach (var purchasedArticle in purchasedArticles)
                {
                    var article = purchasedArticle.Article;
                    var amount = purchasedArticle.Amount;

                    if (dict.ContainsKey(article))
                    {
                        dict[article] += amount;
                    }
                    else
                    {
                        dict.Add(article, amount);
                    }
                }
            }
            var reportList = dict.ToList();
            reportList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            Console.WriteLine("Article report by total amount sold (inlude vat)");
            Console.WriteLine("Amount, Article(id, name, price, vat)");
            foreach (var item in reportList)
            {
                Console.WriteLine("  " + item.Value + " " + item.Key);
            }
        }

        private void ValidateAdmin()
        {
            ValidateActiveUser();
            if(_activeUser.Role != "admin")
            {
                throw new UserNotAdminException();
            }
        }

        private void ValidateActiveUser()
        {
            if (_activeUser == null)
            {
                throw new UserNotLoggedInException();
            }
        }

        private void ValidateActiveReceipt()
        {
            if(_activeReceipt == null)
            {
                throw new ReceiptNotOpenedException();
            }
        }

        #region additional functionality
        public List<Article> getAllArticles()
        {
            return new List<Article>(_articleRepository.getAllArticles().Values);
        }

        #endregion



    }
}
