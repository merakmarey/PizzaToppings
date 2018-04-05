using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PizzaToppings
{
    class Program
    {
        static void Main(string[] args)
        {

            // Letme know what you are doing..
            Console.WriteLine("Getting pie information from remote server..");

            var pizzaProcessor = new PizzaDataProcessor();

            var pizzaList = pizzaProcessor.getPizzaData();

            if ((pizzaList != null) && (pizzaList.Count>0)) // There is some pizza in there!..
            {
                
                Console.WriteLine("Getting toppings combinations information from remote server..");

                // As I said...
                var ToppingCombinations = pizzaProcessor.GetToppingCombinations(pizzaList);


                // Get top list size from App.Config
                int topCount;

                try
                {
                    if (!Int32.TryParse(ConfigurationManager.AppSettings["TopListCount"].ToString(), out topCount))
                        topCount = 20;
                }
                catch (Exception ex) {
                    Console.WriteLine("Configuration error..using default value of 20 for top list..");
                    topCount = 20;
                }

                // Don't get too short, or too far..
                // Adecuate output to data amount by ensuring top count is greater than the combinations count;

                topCount = (topCount <= ToppingCombinations.Count() ? topCount : ToppingCombinations.Count());

               
                
                Console.WriteLine("Sorting..");



                // Now take the most X popular toppings from the list
                var popularToppings = ToppingCombinations.OrderByDescending(t => t.count).Take(topCount).ToList();


                Console.WriteLine("Here's the results..");
                
                // Output is ready!
                
                // Adjustment for loop..
                topCount--;

                for (int i = 0; i < topCount; i++)
                {
                    Console.WriteLine(String.Format("Rank #{0}",i));
                    Console.WriteLine(String.Format("Ordered: {0} times", popularToppings[i].count));
                    Console.WriteLine(String.Format("Toppings: {0}", popularToppings[i].toppings));
                    Console.WriteLine("---------------------");


                }
            }
            else
            {
                // You want bread?
                Console.WriteLine("No pie for you!");
            }
            Console.WriteLine("Bye!");
        }
       
    }
}
