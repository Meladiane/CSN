using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSN;
using System.Collections.Generic;
using System.Linq;

namespace CSN_TESTS
{
    [TestClass]
    public class Store_Tests    
    {

        #region CONTEXT

        IStore store;

        #endregion


        #region INTIALIZER

        [TestInitialize]
        public void TestInitialize()
        {
            // pStore = new ST
            store = new Store();
            store.Import(@"C:\Users\yc786\source\repos\CSN\file2.json");

        }

        #endregion

        #region TESTS METHODS

        #region RECUPERATION DES INFORMATIONS

        /// <summary>
        /// Consulter, à partir du titre d’un livre, le stock disponible. Cela sera
        /// fait à partir de la méthode suivante
        /// Quantity computation test
        /// </summary>
        [TestMethod]
        public void Retrieve_quantity_by_book_name()
        {
            // ARRANGE
            string name = "Ayn Rand - FountainHead";
            int expectedQuantity = 10;
            // ACT
            int quantity = store.Quantity(name);
            // ASSERT
            Assert.AreEqual(expectedQuantity, quantity);
        }

        #endregion


        #region CALCUL DES PRIX DES PANIERS

        /// <summary>
        /// Règle 1
        /// L’achat d’un livre seul se paye au prix du livre
        /// fourni dans le catalogue.
        /// </summary>
        [TestMethod]
        public void When_client_buy_on_book_he_pays_at_the_price()
        {
            // ARRANGE
            double expectedPrice = 24.0;
            // ACT
            double price = store.Buy("J.K Rowling - Goblet Of fire", "Isaac Asimov - Foundation");
            // ASSERT
            Assert.AreEqual(expectedPrice, price);
        }



        /// <summary>
        /// Règle 3
        /// Seul le premier exemplaire de chaque livre a le droit à la réduction
        /// </summary>
        [TestMethod]
        public void Only_the_first_copy_of_each_book_has_the_right_to_reduction()
        {
            // ARRANGE
            double expectedPrice = 30;
            // ACT
            double price = store.Buy(
                "J.K Rowling - Goblet Of fire", "Robin Hobb - Assassin Apprentice",
                "Robin Hobb - Assassin Apprentice");
            // ASSERT
            Assert.AreEqual(expectedPrice, price);
        }


        /// <summary>
        /// Règle 4
        /// Si un panier n’est pas valide car le catalogue ne contient pas assez d’ouvrage
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotEnoughInventoryException))]
        public void If_basket_invalid_because_catalog_does_not_contain_enough_books_Scenario1()
        {
            // ARRANGE
            // ACT
            double price = store.Buy(
                "Isaac Asimov - Foundation", "Isaac Asimov - Foundation");
        }

        /// <summary>
        /// Règle 4
        /// Si un panier n’est pas valide car le catalogue ne contient pas assez d’ouvrage        
        /// /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotEnoughInventoryException))]
        public void If_basket_invalid_because_catalog_does_not_contain_enough_books_Scenario2()
        { 
        
            // ARRANGE
            // ACT
            double price = store.Buy("Isaac Asimov - Robot series",
                "Isaac Asimov - Foundation", "Isaac Asimov - Foundation");
        }



        /// <summary>
        /// Règle 4
        /// Si un panier n’est pas valide car le catalogue ne contient pas assez d’ouvrage        
        /// /// </summary>
        [TestMethod]
        public void If_basket_invalid_because_catalog_does_not_contain_enough_books_Scenario3()
        {

            // ARRANGE
            try{
            // ACT
            double price = store.Buy("Isaac Asimov - Robot series", 
            "Isaac Asimov - Robot series", "Isaac Asimov - Foundation", "Isaac Asimov - Foundation");

            }
            catch (NotEnoughInventoryException ex)
            {
                // ASSERT
                Assert.AreEqual("Isaac Asimov - Robot series", ex.Missing[0].Name);
                Assert.AreEqual("Isaac Asimov - Foundation", ex.Missing[1].Name);
            }
        }


        #endregion



        #endregion



        /// <summary>
        /// Regrouper les livres dans panier en calculant occurence
        /// de chacun
        /// fourni dans le catalogue.
        /// </summary>
        [TestMethod]
        public void ComputeOccurencesBookInChart()
        {
var clientChart = new string []{ "J.K Rowling - Goblet Of fire", "Isaac Asimov - Foundation",
             "J.K Rowling - Goblet Of fire"};

            var groupByBookName = clientChart.GroupBy(x  => x);
        }

    }
}
