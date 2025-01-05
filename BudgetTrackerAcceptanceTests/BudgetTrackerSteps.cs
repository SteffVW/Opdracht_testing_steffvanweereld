using Opdracht_testing_steffvanweereld;
using Xunit.Gherkin.Quick;

namespace BudgetTrackerAcceptanceTests;

    [FeatureFile("./Features/BudgetTracker.Feature")]
    public sealed class AcceptanceTests: Feature
    {
        private Tracker _tracker;
        private IBudgetHelper _budgetHelper;
        private INotifier _notifier;

        private const string mockoonBudget = "http://localhost:3001/budget";
        private const string mockoonAmount = "http://localhost:3001/amount";
        public AcceptanceTests()
        {
            _budgetHelper = new BudgetHelperApi();
            _notifier = new Notifier();
            _tracker = new Tracker(_notifier, _budgetHelper);
        }

        [Given(@"The budget is the same as the amount")]
        public void NotifyNoMoreBalance()
        {
            string budgetQuery = "?budget=100";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;
        }
        [Given(@"The budget is the higher than the amount")]
        public void NotifyRemainingBalance()
        {
            string budgetQuery = "?budget=200";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;
        }
        [Given(@"The budget is the lower than the amount")]
        public void NotifyPurhcaseFail()
        {
            string budgetQuery = "?budget=50";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;
        }
        [Given(@"safe mode is enabled and budget is refilled")]
        public void TestSafeModeReset()
        {
            string budgetQuery = "?budget=50";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;

            for (int i = 1; i < _tracker.maxFails; i++)
            {
                _tracker.HandlePurchase();
            }
            
            _tracker.HandlePurchase();

            string RefilbudgetQuery = "?budget=200";

            string Refilbudget = $"{mockoonBudget}{RefilbudgetQuery}";

            _budgetHelper.BudgetUrl = Refilbudget;
            _budgetHelper.AmountUrl = amount;
        }

        [When(@"Trying to make purchase")]
        public void AttemptPurchase()
        {
            _tracker.HandlePurchase();
        }

        [Then(@"Notify - ""Purchase made! no more balance""")]
        public void CheckNoRemainingBalance()
        {
            Assert.Equal("Purchase made! no more balance", _notifier.Message);
        }
        [Then(@"Notify - ""Purchase made, you still have balance""")]
        public void CheckRemainingBalance()
        {
            Assert.Equal("Purchase made, you still have balance", _notifier.Message);
        }
        [Then(@"Notify - ""Unable to make purchase""")]
        public void CheckNoBalance()
        {
            Assert.Equal("Unable to make purchase", _notifier.Message);
        }
        [Then(@"Disable safe mode")]
        public void CheckDisableSafeMode()
        {
            Assert.False(_tracker.InSafeMode);
        }
    }