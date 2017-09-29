using System;
using CashRegisterCore.Model;

namespace ObjektnoOblikovanjeDz1
{
    class EntryPoint
    {
        static CashRegisterModel _cashRegisterModel;

        static void Main(string[] args)
        {
            //username is 'admin', and 'cashier' depending on who uses cash register
            //Password for existing accounts is "aaaa" (in users.json, only hash is seen)
            _cashRegisterModel = new CashRegisterModel();
            LoginLoop();
        }

        private static void LoginLoop()
        {
            bool end = false;
            while (!end)
            {
                Console.WriteLine("Available actions");
                Console.WriteLine("  1: Login");
                Console.WriteLine("  0: Exit");
                string raw = Console.ReadLine().Trim();
                int choice;
                bool parseSuccessful = Int32.TryParse(raw, out choice);
                if (parseSuccessful)
                {
                    switch (choice)
                    {
                        case 0:
                            end = true;
                            break;

                        case 1:
                            LoginPrompt();
                            break;

                        default:
                            Console.WriteLine("Please enter a choice from 0 to 1");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number!");
                }
            }
        }

        private static void LoginPrompt()
        {
            Console.Write("  username: ");
            string username = Console.ReadLine().Trim();
            Console.Write("  password: ");
            string password = Console.ReadLine();
            Console.WriteLine();
            bool loginSuccess = _cashRegisterModel.Login(username, password);
            if(loginSuccess)
            {
                if (_cashRegisterModel.isAdminLogged())
                {
                    PromptAdminLoop();
                }
                else
                {
                    PromptCashierLoop();
                }
            }
        }

        private static void PromptAdminLoop()
        {
            bool end = false;
            while (!end)
            {
                Console.WriteLine();
                Console.WriteLine("Available actions");
                Console.WriteLine("  1: Define new article");
                Console.WriteLine("  2: Create new receipt");
                Console.WriteLine("  3: Delete old receipt");
                Console.WriteLine("  4: Modify old receipt");
                Console.WriteLine("  5: Print daily report");
                Console.WriteLine("  6: Print article report");
                Console.WriteLine("  0: Exit");
                string raw = Console.ReadLine().Trim();
                int choice;
                bool parseSuccessful = Int32.TryParse(raw, out choice);
                if (parseSuccessful)
                {
                    try
                    {
                        switch (choice)
                        {
                            case 0:
                                end = true;
                                break;

                            case 1:
                                DefineNewArticlePrompt();
                                break;

                            case 2:
                                CreateNewReceipt();
                                break;

                            case 3:
                                DeleteOldReceipt();
                                break;

                            case 4:
                                Console.Write("Id: ");
                                string rawId = Console.ReadLine().Trim();
                                int id;
                                if (!Int32.TryParse(rawId, out id))
                                {
                                    Console.WriteLine("Please enter number!");
                                    break;
                                }
                                _cashRegisterModel.SetReceiptAsActive(id);
                                ModifyOldReceipt();
                                break;

                            case 5:
                                PrintDailyReport();
                                break;

                            case 6:
                                PrintArticleReport();
                                break;


                            default:
                                Console.WriteLine("Please enter a choice from 0 to 6");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number!");
                }
            }
        }

        private static void ModifyOldReceipt()
        {
            bool end = false;
            while (!end)
            {
                Console.WriteLine();
                Console.WriteLine("Available actions");
                Console.WriteLine("  1: Remove article");
                Console.WriteLine("  2: Add article");
                Console.WriteLine("  3: Get list of all articles");
                Console.WriteLine("  4: See Receipt Output");
                Console.WriteLine("  0: Exit");
                string raw = Console.ReadLine().Trim();
                int choice;
                bool parseSuccessful = Int32.TryParse(raw, out choice);
                if (parseSuccessful)
                {

                    try
                    {
                        switch (choice)
                        {
                            case 0:
                                end = true;
                                break;

                            case 1:
                                Console.Write("  Article Position: ");
                                string rawArticlePosition = Console.ReadLine().Trim();
                                int articlePosition;
                                if (!Int32.TryParse(rawArticlePosition, out articlePosition))
                                {
                                    Console.WriteLine("Id should be valid number.");
                                    break;
                                }
                                _cashRegisterModel.RemoveReceiptArticleEntry(articlePosition);
                                break;

                            case 2:
                                Console.Write("  ArticleId: ");
                                string rawId = Console.ReadLine().Trim();
                                Console.Write("  Article Amount: ");
                                string rawAmount = Console.ReadLine().Trim();

                                int id;
                                if (!Int32.TryParse(rawId, out id))
                                {
                                    Console.WriteLine("Id should be number.");
                                    break;
                                }

                                double amount;
                                if (!double.TryParse(rawAmount, out amount))
                                {
                                    Console.WriteLine("Amount should be number.");
                                    break;
                                }
                                _cashRegisterModel.AddArticleToReceipt(id, amount);
                                break;

                            case 3:
                                Console.WriteLine("Article(id, name, price, vat)");
                                foreach (var article in _cashRegisterModel.getAllArticles())
                                {
                                    Console.WriteLine(article.ToString());
                                }
                                break;

                            case 4:
                                _cashRegisterModel.SeeReceiptOutput();
                                break;


                            default:
                                Console.WriteLine("Please enter a choice from 0 to 4");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number!");
                }
            }
        }

        private static void DeleteOldReceipt()
        {
            bool end = false;
            while (!end)
            {
                Console.WriteLine("Delete Receipt - Available actions");
                Console.WriteLine("  1: Enter receipt id");
                Console.WriteLine("  0: Exit");
                string raw = Console.ReadLine().Trim();
                int choice;
                bool parseSuccessful = Int32.TryParse(raw, out choice);
                if (parseSuccessful)
                {
                    try
                    {
                        switch (choice)
                        {
                            case 0:
                                end = true;
                                break;

                            case 1:
                                Console.Write("Id: ");
                                string rawId = Console.ReadLine().Trim();
                                int id;
                                if (!Int32.TryParse(rawId, out id))
                                {
                                    Console.WriteLine("Please enter number!");
                                    break;
                                }
                                else
                                {
                                    _cashRegisterModel.DeleteReceipt(id);
                                    Console.WriteLine("Receipt deleted");
                                    end = true; 
                                }
                                break;

                            default:
                                Console.WriteLine("Please enter a choice from 0 to 1");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number!");
                }
            }
        }

        private static void DefineNewArticlePrompt()
        {
            Console.Write("Id: ");
            string rawId = Console.ReadLine().Trim();
            Console.Write("Name: ");
            string rawName = Console.ReadLine().Trim();
            Console.Write("Price: ");
            string rawPrice = Console.ReadLine().Trim();
            Console.Write("Vat: ");
            string rawVat = Console.ReadLine().Trim();

            int id;
            bool isIdOk = Int32.TryParse(rawId, out id); 
            if(!isIdOk)
            {
                Console.WriteLine("Id should be number.");
                return;
            }

            if(string.IsNullOrEmpty(rawName))
            {
                Console.WriteLine("Name should not be null or empty.");
                return;
            }

            double price;
            bool isPriceOk = double.TryParse(rawPrice, out price);
            if (!isPriceOk)
            {
                Console.WriteLine("Price should be number.");
                return;
            }

            double vat;
            bool isVatOk = double.TryParse(rawVat, out vat);
            if (!isVatOk)
            {
                Console.WriteLine("Vat should be number.");
                return;
            }

            try
            {
                _cashRegisterModel.DefineNewArticle(id, rawName, price, vat);
                Console.WriteLine("New Article defined in cash register!");
            }
            catch(ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void PromptCashierLoop()
        {
            bool end = false;
            while (!end)
            {
                Console.WriteLine();
                Console.WriteLine("Available actions");
                Console.WriteLine("  1: Create new receipt");
                Console.WriteLine("  2: Print daily report");
                Console.WriteLine("  3: Print article report");
                Console.WriteLine("  0: Exit");
                string raw = Console.ReadLine().Trim();
                int choice;
                bool parseSuccessful = Int32.TryParse(raw, out choice);
                if (parseSuccessful)
                {
                    try
                    {
                        switch (choice)
                        {
                            case 0:
                                end = true;
                                break;

                            case 1:
                                CreateNewReceipt();
                                break;

                            case 2:
                                PrintDailyReport();
                                break;

                            case 3:
                                PrintArticleReport();
                                break;

                            default:
                                Console.WriteLine("Please enter a choice from 0 to 3");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number!");
                }
            }
        }

        private static void PrintArticleReport()
        {
            bool end = false;
            while (!end)
            {
                Console.WriteLine();
                Console.WriteLine("Available actions");
                Console.WriteLine("  1: Article Report By Total Number Sold");
                Console.WriteLine("  2: Article Report By Total Value Sold");
                Console.WriteLine("  0: Exit");
                string raw = Console.ReadLine().Trim();
                int choice;
                bool parseSuccessful = Int32.TryParse(raw, out choice);
                if (parseSuccessful)
                {
                    try
                    {
                        switch (choice)
                        {
                            case 0:
                                end = true;
                                break;

                            case 1:
                                _cashRegisterModel.ArticleReportByTotalNumberSold();
                                break;

                            case 2:
                                _cashRegisterModel.ArticleReportByTotalValueSold();
                                break;

                            default:
                                Console.WriteLine("Please enter a choice from 0 to 2");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else 
                {
                    Console.WriteLine("Please enter a number!");
                }
            }
        }

        private static void PrintDailyReport()
        {
            try
            {
                _cashRegisterModel.PrintDailyReport();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void CreateNewReceipt()
        {
            try
            {
                _cashRegisterModel.OpenNewReceipt();
                bool end = false;
                while (!end)
                {
                    Console.WriteLine("Available actions");
                    Console.WriteLine("  1: Add article to receipt");
                    Console.WriteLine("  2: Get list of all articles");
                    Console.WriteLine("  3: Print receipt");
                    Console.WriteLine("  4: Cancel receipt");
                    Console.WriteLine("  0: Exit");
                    string raw = Console.ReadLine().Trim();
                    int choice;
                    bool parseSuccessful = Int32.TryParse(raw, out choice);
                    if (parseSuccessful)
                    {
                        try
                        {
                            switch (choice)
                            {
                                case 0:
                                    if (_cashRegisterModel.isReceiptOpen())
                                    {
                                        Console.WriteLine("Cannot exit. Receipt is still open.");
                                    }
                                    else
                                    {
                                        end = true;
                                    }
                                    break;

                                case 1:
                                    Console.Write("  ArticleId: ");
                                    string rawId = Console.ReadLine().Trim();
                                    Console.Write("  Article Amount: ");
                                    string rawAmount = Console.ReadLine().Trim();

                                    int id;
                                    if (!Int32.TryParse(rawId, out id))
                                    {
                                        Console.WriteLine("Id should be number.");
                                        break;
                                    }

                                    double amount;
                                    if (!double.TryParse(rawAmount, out amount))
                                    {
                                        Console.WriteLine("Amount should be number.");
                                        break;
                                    }
                                    _cashRegisterModel.AddArticleToReceipt(id, amount);
                                    break;

                                case 2:
                                    Console.WriteLine("Article(id, name, price, vat)");
                                    foreach (var article in _cashRegisterModel.getAllArticles())
                                    {
                                        Console.WriteLine(article.ToString());
                                    }
                                    break;

                                case 3:
                                    _cashRegisterModel.PrintReceipt();
                                    end = true;
                                    break;

                                case 4:
                                    _cashRegisterModel.CancelReceipt();
                                    break;

                                default:
                                    Console.WriteLine("Please enter a choice from 0 to 4");
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
