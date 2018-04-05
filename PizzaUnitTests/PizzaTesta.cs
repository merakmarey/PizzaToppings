using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PizzaToppings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PizzaUnitTests
{
    [TestClass]
    public class PizzaTesta
    {
        [TestMethod]
        public void TestGetToppingCombinations()
        {
            var pizza = new List<Pizza>();
            pizza.Add(new Pizza() { Toppings = new List<string>() { "pePEr", "cHeesE" } });
            pizza.Add(new Pizza() { Toppings = new List<string>() { "CHeEse ", "PePER" } });
            pizza.Add(new Pizza() { Toppings = new List<string>() { "pePEr", "cHeesE" } });

            pizza.Add(new Pizza() { Toppings = new List<string>() { "beEF", "sAusaGe" } });
            pizza.Add(new Pizza() { Toppings = new List<string>() { "SauSaGE", "BeEf" } });

            pizza.Add(new Pizza() { Toppings = new List<string>() { "pepperoni" } });

            var pizzaProcessor = new PizzaDataProcessor();

            var result = pizzaProcessor.GetToppingCombinations(pizza);

            Assert.IsTrue(result[0].count==3);

        }
    }
}
