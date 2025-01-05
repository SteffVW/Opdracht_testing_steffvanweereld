using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Opdracht_testing_steffvanweereld
{
    public class BudgetHelperApi : IBudgetHelper
    {
        private string budgetUrl;

        public string BudgetUrl
        {
            get { return budgetUrl; }
            set { budgetUrl = value; }
        }
        private string amountUrl;

        public string AmountUrl
        {
            get { return amountUrl; }
            set { amountUrl = value; }
        }


        public double GetBudget()
        {
            using (var httpClient = new HttpClient())
            {
                var httpRespone = httpClient.GetAsync(budgetUrl).GetAwaiter().GetResult();
                var response = httpRespone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Budget>(response).budget;
            }
        }
        public double UpdateBudget(double amount)
        {
            return GetBudget() - amount;
        }
        public void BlockCard()
        {
            Console.WriteLine("card blocked");
        }
        public double GetAmount()   
        {
            using (var httpClient = new HttpClient())
            {
                var httpRespone = httpClient.GetAsync(amountUrl).GetAwaiter().GetResult();
                var response = httpRespone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Amount>(response).amount;
            }
        }
       
    }
}
