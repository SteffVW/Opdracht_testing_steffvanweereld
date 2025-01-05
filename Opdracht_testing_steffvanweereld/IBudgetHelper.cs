using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht_testing_steffvanweereld
{
    public interface IBudgetHelper
    {
        string BudgetUrl { get; set; }
        string AmountUrl { get; set; }

        double GetBudget();
        double UpdateBudget(double amount);
        void BlockCard();
        double GetAmount();
    }
}
