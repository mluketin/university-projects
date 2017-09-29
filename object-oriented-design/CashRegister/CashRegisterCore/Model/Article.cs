using System;

namespace CashRegisterCore.Model
{
    /// <summary>
    /// Represents article in cash register.
    /// Article has id, name, price and vat (price entered has already included VAT).
    /// name cannot be null or empty;
    /// id, price and vat cannot be lesser than zero
    /// </summary>
    public class Article
    {
        private int _id;
        private string _name;
        private double _price;
        private double _vat;

        /// <summary>
        /// If any of arguments is invalid, throws exception
        /// </summary>
        /// <param name="id">cannot be null or empty</param>
        /// <param name="name">cannot be null or empty</param>
        /// <param name="price">cannot be lesser than 0</param>
        /// <param name="vat">cannot be lesser than 0</param>
        public Article(int id, string name, double price, double vat)
        {
            IdValidation(id);
            NameValidation(name);
            PriceValidation(price);
            VatValidation(vat);
            _id = id;
            _name = name;
            _price = price;
            _vat = vat;
        }

        /// <summary>
        /// Validates Article properties.
        /// If Article is invalid, throws exception
        /// </summary>
        public void Validate()
        {
            IdValidation(_id);
            NameValidation(_name);
            PriceValidation(_price);
            VatValidation(_vat);   
        }

        private void IdValidation(int id)
        {
            if(id < 0) throw new ArgumentException("Article id cannot be lesser than 0");
        }

        private void NameValidation(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Article name cannot be null or empty");
        }

        private void PriceValidation(double price)
        {
            if (price < 0) throw new ArgumentException("Article price cannot be lesser than 0");
        }

        private void VatValidation(double vat)
        {
            if (vat < 0) throw new ArgumentException("Article vat cannot be lesser than 0");
        }

        public int Id
        {
            get { return _id; }
            set
            {
                IdValidation(value);
                _id = value;
            }
        }

        /// <summary>
        /// Returns name of article.
        /// Setting a new name that is invalid throws exception.
        /// </summary>
        public string Name
        { 
            get {return _name; }
            set
            {
                NameValidation(value);
                _name = value;
            }
        }

        /// <summary>
        /// Returns price of article.
        /// Setting a new price that is invalid throws exception.
        /// </summary>
        public double Price
        {
            get { return _price; }
            set
            {
                PriceValidation(value);
                _price = value;
            }
        }

        /// <summary>
        /// Returns vat of article.
        /// Setting a new vat that is invalid throws exception.
        /// </summary>
        public double Vat
        {
            get { return _vat; }
            set
            {
                VatValidation(value);
                _vat = value;
            }
        }

        /// <summary>
        /// Return vat amount from price of article.
        /// Example. if price is 10, and vat is 25, vat amount is 8.
        /// (Base cost is 8 + 8*0.25 = 10)
        /// </summary>
        /// <returns></returns>
        public double GetVatAmount()
        {
            return _price / (1 + 100/_vat);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Article article = (Article) obj;
            return _id == article.Id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Article({0}, {1}, {2}, {3})", _id, _name, _price, _vat);
        }
    }
}
