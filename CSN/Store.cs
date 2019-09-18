using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSN
{

    public class Store : IStore
    {

        public NotEnoughInventoryException InventoryException { get; set; }

        private RootObject BooksStore { get; set; }

        public Store()
        {
            InventoryException = new NotEnoughInventoryException();
            InventoryException.Missing = new List<NameQuantity>();
        }



        public void Import(string catalogAsJson)
        {
            var json = File.ReadAllText(catalogAsJson);
            BooksStore = JsonConvert.DeserializeObject<RootObject>(json);

        }

        public int Quantity(string name)
        {
            return BooksStore.Catalog.Find(book => book.Name == name).Quantity;
        }


        public double Buy(params string[] basketByNames)
        {

            double price = 0;
            if (basketByNames != null)
            {
                int length = basketByNames.Length;

                if (length == 1)
                {
                    return BooksStore.Catalog.Find(book => book.Name == basketByNames[0]).Price;
                }
                else
                {
                    var groupClientChartByBookName = basketByNames.GroupBy(bookName => bookName).Select(
                        book => new
                        {
                            BookName = book,
                            NumberOfCopies = book.ToList().Count,
                            BookData = BooksStore.Catalog.Find(b => b.Name == book.Key),
                        }

                        ).ToList();

                    var groupByCategory = BooksStore.Category.Join(groupClientChartByBookName,
                     cat => cat.Name, x => x.BookData.Category
                     , (Category, x) => new
                     {
                         Category,
                         Data = x
                     })
                     .GroupBy(Categorie => Categorie.Category).ToList();


                    foreach (var category in groupByCategory)
                    {
                        var booksSelections = category.ToList();
                        var categoryDiscount = category.Key.Discount;
                        var numberOfBooksboughtInCategory = booksSelections.Count;

                        if (numberOfBooksboughtInCategory == 1)
                        {
                            var bookData = booksSelections.First().Data;
                            var numberOfCopiesAskedInChart = bookData.NumberOfCopies;
                            //  Si un panier n’est pas valide car le catalogue ne contient pas assez d’ouvrage
                            var numberOfCopiesInSock = bookData.BookData.Quantity;
                            if (numberOfCopiesAskedInChart > numberOfCopiesInSock)
                            {
                                InventoryException.Missing.Add(
                                    new NameQuantity
                                    {
                                        Name = bookData.BookData.Name,
                                        Quantity = bookData.BookData.Quantity
                                    }
                                    );
                            }

                            price += bookData.BookData.Price;


                        }
                        else if (numberOfBooksboughtInCategory > 1)
                        {
                            foreach (var bookSelection in booksSelections)
                            {
                                var numberOfBooksInStock = bookSelection.Data.BookData.Quantity;
                                var numberOfBooksAsked = bookSelection.Data.NumberOfCopies;

                                if (numberOfBooksAsked > numberOfBooksInStock)
                                {
                                    InventoryException.Missing.Add(
                                                         new NameQuantity
                                                         {
                                                             Name = bookSelection.Data.BookData.Name,
                                                             Quantity = bookSelection.Data.BookData.Quantity
                                                         }
                                   );
                                }

                                var bookPrice = bookSelection.Data.BookData.Price;
                                var numberOfCopies = bookSelection.Data.NumberOfCopies;

                                if (bookSelection.Data.NumberOfCopies > 1)
                                {
                                    price += bookPrice * +(1 - categoryDiscount) + bookPrice * (numberOfCopies - 1);
                                }
                                if (bookSelection.Data.NumberOfCopies == 1)
                                {
                                    price += bookPrice * (1 - categoryDiscount);
                                }

                            }

                        }


                    }
                }
            }

            int numberOfBooksMissing = InventoryException.Missing.Count();
            if (numberOfBooksMissing > 0 )
            {
                throw InventoryException;
            }
            return price;
        }

        private class RootObject
        {
            public List<NameQuantity> Catalog { get; set; }
            public List<Category> Category { get; set; }

        }

    }

    public class Bookstore
    {
        public List<NameQuantity> Books { get; set; }
    }
}
