using Moq;
using Opdracht_testing_steffvanweereld;

namespace BudgetTrackerUnitTests
{
    public class UnitTests
    {
        private Tracker _tracker;
        private Mock<IBudgetHelper> _budgetHelper;
        private Mock<INotifier> _notifier;

        public UnitTests() 
        {
            _budgetHelper = new Mock<IBudgetHelper>();
            _notifier = new Mock<INotifier>();
            _tracker = new Tracker(_notifier.Object, _budgetHelper.Object);
        }

        [Fact]
        public void TestIfBudgetIsZero()
        {
            _budgetHelper.Setup(x => x.GetBudget()).Returns(100);
            _budgetHelper.Setup(x => x.GetAmount()).Returns(100);

            _tracker.HandlePurchase();

            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made! no more balance"), Times.Once);
            _notifier.Verify(x => x.SendNotification("Purchase made, you still have balance"), Times.Never);
            Assert.Equal(0, _tracker.FailedAttempts);
        }
        [Fact]
        public void TestIfBudgetRemains()
        {
            _budgetHelper.Setup(x => x.GetBudget()).Returns(200);
            _budgetHelper.Setup(x => x.GetAmount()).Returns(100);

            _tracker.HandlePurchase();

            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made! no more balance"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made, you still have balance"), Times.Once);
        }
        [Fact]
        public void TestIfNoBudgetRemains()
        {
            _budgetHelper.Setup(x => x.GetBudget()).Returns(50);
            _budgetHelper.Setup(x => x.GetAmount()).Returns(100);

            _tracker.HandlePurchase();

            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Once);
            _notifier.Verify(x => x.SendNotification("Purchase made! no more balance"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made, you still have balance"), Times.Never);
        }
        [Fact]
        public void TestForBlockedCard()
        {
            _budgetHelper.Setup(x => x.GetBudget()).Throws<Exception>();

            for (int i = 1; i < _tracker.maxFails; i++)
            {
                _tracker.HandlePurchase();
            }

            _tracker.HandlePurchase();

            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made! no more balance"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made, you still have balance"), Times.Never);
            _budgetHelper.Verify(x => x.BlockCard(), Times.Once);
            Assert.True(_tracker.InSafeMode);
        }
        [Fact]
        public void TestForBlockedCardReset()
        {
            _budgetHelper.Setup(x => x.GetBudget()).Throws<Exception>();

            for (int i = 1; i < _tracker.maxFails; i++)
            {
                _tracker.HandlePurchase();
            }

            _tracker.HandlePurchase();

            Assert.True(_tracker.InSafeMode);
            _budgetHelper.Verify(x => x.BlockCard(), Times.Once);

            _budgetHelper.Setup(x => x.GetAmount()).Returns(100);
            _budgetHelper.Setup(x => x.GetBudget()).Returns(100);
    
            _tracker.HandlePurchase();

            Assert.False(_tracker.InSafeMode);
            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made! no more balance"), Times.Once);
            _notifier.Verify(x => x.SendNotification("Purchase made, you still have balance"), Times.Never);
        }
        [Fact]
        public void TestStayInSafeMode()
        {
            _budgetHelper.Setup(x => x.GetBudget()).Throws<Exception>();

            for (int i = 1; i < _tracker.maxFails; i++)
            {
                _tracker.HandlePurchase();
            }

            _tracker.HandlePurchase();

            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made! no more balance"), Times.Never);
            _notifier.Verify(x => x.SendNotification("Purchase made, you still have balance"), Times.Never);
            _budgetHelper.Verify(x => x.BlockCard(), Times.Once);
            Assert.True(_tracker.InSafeMode);

            _budgetHelper.Setup(x => x.GetBudget()).Returns(50);
            _budgetHelper.Setup(x => x.GetAmount()).Returns(100);

            _tracker.HandlePurchase();

            _notifier.Verify(x => x.SendNotification("Unable to make purchase"), Times.Once);

            Assert.True(_tracker.InSafeMode);
        }
    }
}