using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Opdracht_testing_steffvanweereld
{
    public class BudgetHelper: IBudgetHelper
    {
        private double budget = 100;

        private string budgetUrl;

        public string BudgetUrl
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        private string amountUrl;

        public string AmountUrl
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public double GetBudget()
        {
            return budget;
        }
        public double UpdateBudget(double amount)
        {
            return budget - amount;
        }
        public void BlockCard()
        {
            throw new NotImplementedException();
        }
        public double GetAmount()
        {
            throw new NotImplementedException();
        }
    }
}
