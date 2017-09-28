using CashRegisterCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterCore.Repositories
{
    interface IArticleRepository
    {
        bool addArticle(Article article);

        void putArticle(Article article);

        Dictionary<Article, Article> getAllArticles();

        List<Article> getAllArticlesAsList();

        Article getArticleById(int id);

    }
}
