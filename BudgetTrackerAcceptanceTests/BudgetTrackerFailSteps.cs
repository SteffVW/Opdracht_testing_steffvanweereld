using Opdracht_testing_steffvanweereld;
using Xunit.Gherkin.Quick;

namespace BudgetTrackerAcceptanceTests;

    [FeatureFile("./Features/BudgetTrackerFailure.Feature")]
    public sealed class BudgetTrackerFailSteps: Feature
    {
        private Tracker _tracker;
        private IBudgetHelper _budgetHelper;
        private INotifier _notifier;

        private const string mockoonBudget = "http://localhost:3001/budget";
        private const string mockoonAmount = "http://localhost:3001/amount";
        public BudgetTrackerFailSteps()
        {
            _budgetHelper = new BudgetHelperApi();
            _notifier = new Notifier();
            _tracker = new Tracker(_notifier, _budgetHelper);
        }

    [Given(@"Fail a purchase 3 times")]
    public void InSafeMode()
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
    }
    [Given(@"Fail a purhcase in safe mode")]
    public void StayInSafeMode()
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


        string RefilbudgetQuery = "?budget=25";

        string Refilbudget = $"{mockoonBudget}{RefilbudgetQuery}";

        _budgetHelper.BudgetUrl = Refilbudget;
    }

    [When(@"Trying to make purchase")]
    public void AttemptPurchase()
    {
        _tracker.HandlePurchase();
    }

    [Then(@"Go in safe mode")]
    [Then(@"Stay in safe mode")]
    public void CheckDisableSafeMode()
    {
        Assert.True(_tracker.InSafeMode);
        Assert.Equal("Unable to make purchase", _notifier.Message);
    }
}
