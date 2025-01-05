using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Opdracht_testing_steffvanweereld
{
    public class Tracker
    {
        public int maxFails = 3;

        private int failedAttempts = 0;

        public int FailedAttempts
        {
            get { return failedAttempts; }
            set { failedAttempts = value; }
        }

        public bool InSafeMode
        {
            get { return (failedAttempts < maxFails) ? false : true; }
        }


        private readonly INotifier notifier;
        private readonly IBudgetHelper budgetHelper;

        public Tracker (INotifier notifier, IBudgetHelper budgetHelper)
        {
            this.notifier = notifier;
            this.budgetHelper = budgetHelper;
        }

        public void HandlePurchase()
        {

            try
            {
                double amount = budgetHelper.GetAmount();
                double budget = budgetHelper.GetBudget();

                if (budget > amount)
                {
                    budgetHelper.UpdateBudget(amount);
                    notifier.SendNotification("Purchase made, you still have balance");
                    failedAttempts = 0;
                }
                else if(budget == amount)
                {
                    budgetHelper.UpdateBudget(amount);
                    notifier.SendNotification("Purchase made! no more balance");
                    failedAttempts = 0;
                }
                else if(budget < amount)
                {
                    notifier.SendNotification("Unable to make purchase");
                    throw new Exception();
                }
            }
            catch
            {
                FailedAttempts++;
                if(failedAttempts == maxFails)
                {
                    budgetHelper.BlockCard();
                }
                
            }
        }
    }
}
