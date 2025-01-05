using System.Globalization;
using Opdracht_testing_steffvanweereld;

namespace BudgetTrackerIntegrationTests
{
    public class IntegrationTests
    {
        private Tracker _tracker;
        private IBudgetHelper _budgetHelper;
        private INotifier _notifier;

        private const string mockoonBudget = "http://localhost:3001/budget";
        private const string mockoonAmount = "http://localhost:3001/amount";
        public IntegrationTests()
        {
            _budgetHelper = new BudgetHelperApi();
            _notifier = new Notifier();
            _tracker = new Tracker(_notifier, _budgetHelper);
        }


        [Fact]
        public void TestWhenBudgetIsZero()
        {
            string budgetQuery = "?budget=100";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}"; 

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;

            _tracker.HandlePurchase();

            Assert.Equal(0, _tracker.FailedAttempts);
            Assert.Equal("Purchase made! no more balance", _notifier.Message);
        }
        [Fact]
        public void TestWhenBudgetRemains()
        {
            string budgetQuery = "?budget=200";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;

            _tracker.HandlePurchase();
  
            Assert.Equal(0, _tracker.FailedAttempts);
            Assert.Equal("Purchase made, you still have balance", _notifier.Message);
        }
        [Fact]
        public void TestWhenNoBudgetRemains()
        {
            string budgetQuery = "?budget=50";
            string amountQuery = "?amount=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;

            _tracker.HandlePurchase();

            Assert.Equal(1, _tracker.FailedAttempts);
            Assert.Equal("Unable to make purchase", _notifier.Message);
        }
        [Fact]
        public void TestForBlockedCard()
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

            Assert.Equal(3, _tracker.FailedAttempts);
            Assert.True(_tracker.InSafeMode);
        }
        [Fact]
        public void TestForBlockedCardReset()
        {
            string budgetQuery = "?budget=50";
            string amountQuery = "?amount=100";
            string refillBudgetQuery = "?budget=100";

            string budget = $"{mockoonBudget}{budgetQuery}";
            string amount = $"{mockoonAmount}{amountQuery}";

            _budgetHelper.BudgetUrl = budget;
            _budgetHelper.AmountUrl = amount;

            for (int i = 1; i < _tracker.maxFails; i++)
            {
                _tracker.HandlePurchase();
            }

            _tracker.HandlePurchase();

            Assert.True( _tracker.InSafeMode);
            Assert.Equal(3, _tracker.FailedAttempts);

            _budgetHelper.BudgetUrl = $"{mockoonBudget}{refillBudgetQuery}";

            _tracker.HandlePurchase();

            Assert.False(_tracker.InSafeMode);
            Assert.Equal("Purchase made! no more balance", _notifier.Message);
        }
        [Fact]
        public void TestStayInSafeMode()
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

            Assert.Equal(3, _tracker.FailedAttempts);
            Assert.True(_tracker.InSafeMode);

            _tracker.HandlePurchase();
            Assert.Equal("Unable to make purchase", _notifier.Message);

            Assert.Equal(4, _tracker.FailedAttempts);
            Assert.True(_tracker.InSafeMode);
        }
    }
}