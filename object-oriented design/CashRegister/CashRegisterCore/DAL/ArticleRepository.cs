using System.Collections.Generic;
using System.Linq;
using CashRegisterCore.Repositories;
using System.IO;
using Newtonsoft.Json;
using CashRegisterCore.Model;
using System;
using Newtonsoft.Json.Linq;

namespace CashRegisterCore.DAL
{
    class ArticleRepository : IArticleRepository
    {
        private static string DATABASE_FILE_PATH = @"DatabaseFiles/articles.json";
        private static ArticleRepository _instance;
        private readonly Dictionary<Article, Article> _dictArticle = new Dictionary<Article, Article>();

        private ArticleRepository()
        { 
            string articleJsonString = File.ReadAllText(DATABASE_FILE_PATH); 
            ListArticles articles = JsonConvert.DeserializeObject<ListArticles>(articleJsonString);
            foreach(var article in articles.articles)
            {
                _dictArticle.Add(article, article);
            }
        }

        public static ArticleRepository getInstance()
        {
            return _instance ?? (_instance = new ArticleRepository());
        }


      
        /// <summary>
        /// Adds article to collection if there is no article with the same id.
        /// If there is no article with same id, returns true, otherwise false.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool addArticle(Article article)
        {
            if (_dictArticle.Keys.Contains(article))
            {
                return false;
            }
            _dictArticle.Add(article, article);
            SaveChanges();
            return true;
        }
        
      public Dictionary<Article, Article> getAllArticles()
      {
          return _dictArticle;
      }

        public List<Article> getAllArticlesAsList()
        {
            return new List<Article>(_dictArticle.Keys);
        }

        public Article getArticleById(int id)
      {
            Article article;
            bool exists = _dictArticle.TryGetValue(new Article(id, "x", 0, 0), out article);
            if (exists)
            {
                return article;
            }
            throw new ArticleDoesntExistException();
      }

      /// <summary>
      /// Adds article to collection.
      /// If there is already article with same name, new article will replace it.
      /// </summary>
      /// <param name="article"></param>
      public void putArticle(Article article)
      {
          _dictArticle.Add(article, article);
            SaveChanges();
      }

        private void SaveChanges()
        {
            ListArticles newList = new ListArticles();
            newList.articles = new List<Article>(_dictArticle.Values);
            string newJsonString = JsonConvert.SerializeObject(newList, Formatting.Indented);
            File.WriteAllText(DATABASE_FILE_PATH, newJsonString);

        }

        private class ListArticles
        {
            public List<Article> articles
            {
                get;
                set;
            }
        }
    }
}
