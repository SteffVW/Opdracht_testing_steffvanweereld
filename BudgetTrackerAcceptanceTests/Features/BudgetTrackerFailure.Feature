Feature: Abnormal working of BudgetTracker

Abnormal working of the BudgetTracker. We have 2 scenario's:
Scenario 1: Fail a purchase 3 times: Go in safe mode
Scenario 2: Fail a purchase in safe mode: Stay in safe mode

Scenario: 1 - Go in safe mode after failing after failing a purchase 3 times - Go in safe mode
	Given Fail a purchase 3 times
	When Trying to make purchase
	Then  Go in safe mode


Scenario: 2 - Stay in safe mode after failing purchase - Stay in safe mode
	Given Fail a purhcase in safe mode
	When Trying to make purchase
	Then  Stay in safe mode