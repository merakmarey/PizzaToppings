using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PizzaToppings
{
    public class PizzaDataProcessor
    {
        public List<Pizza> getPizzaData()
        {
            // Our return vessel is ready!
            List<Pizza> pizzaList;

            // Let me get the URL from App.config 
            string url;
            try
            {
                url = ConfigurationManager.AppSettings["URLPizzaSourceJson"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Configuration error..using default value of http://files.olo.com/pizzas.json for server url..");
                url = @"http://files.olo.com/pizzas.json";
            }
            // Let's get them boy!
            HttpWebRequest httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;


            // Try to fetch data from remote
            using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
            {
                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                    throw new Exception(string.Format("Server returned an error: (HTTP {0}: {1}).", httpWebResponse.StatusCode, httpWebResponse.StatusDescription));

                try
                {
                    // Gimme Gimme..
                    Stream stream = httpWebResponse.GetResponseStream();
                    string json = new StreamReader(stream).ReadToEnd();
                    stream.Close();

                    // jSon to Object
                    pizzaList = JsonConvert.DeserializeObject<List<Pizza>>(json);
                }

                // Uh oh...something went wrong...
                catch (OutOfMemoryException ex)
                {
                    Console.WriteLine("Out of Memory");
                    throw ex;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("I/O Exception");
                    throw ex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General Exception");
                    throw ex;
                }
            }

            // Your pie..list...is ready?
            return pizzaList;
        }
    

        public List<ToppingCombination> GetToppingCombinations(List<Pizza> pizzas)
        {
            // Sort toppings 
            var sortedPizzaToppings = pizzas.Select(p => p.Toppings.OrderBy(t => t));

            // create same unique topping combination for each pie..
            IEnumerable<string> aggregatedToppings = sortedPizzaToppings.Select((toppings => toppings.Aggregate((a, b) => a.ToLower().Trim() + "," + b.ToLower().Trim())));

            // group and count them..
            List<ToppingCombination> groupedToppings = aggregatedToppings
                .GroupBy(toppingsGroup => toppingsGroup)
                .Select(toppingsGroup => new ToppingCombination()
                {
                    toppings = toppingsGroup.Key,
                    count = toppingsGroup.Count()
                }).ToList();

            return groupedToppings;
        }
    }
}
