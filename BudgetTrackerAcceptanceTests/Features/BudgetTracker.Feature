Feature: Normal working of BudgetTracker

Normal working of the BudgetTracker. We are testing 4 features:
Scenario 1: There is no budget left after purchase: Notify "Purchase made! no more balance"
Scenario 2: There is budget left after purchase: Notify "Purchase made, you still have balance"
Scenario 3: There is no budget left when when attempting purhcase: Notify "Unable to make purchase"
Scenario 4: Refill budget after safe mode: Disable safe mode  

Scenario: 1 - There is no budget left in the wallet when attempting purchase - Notify "Purchase made! no more balance"
	Given The budget is the same as the amount
	When Trying to make purchase
	Then Notify - "Purchase made! no more balance"

Scenario: 2 - There is budget left in the wallet when attempting purchase - Notify "Purchase made, you still have balance"
	Given The budget is the higher than the amount
	When Trying to make purchase
	Then Notify - "Purchase made, you still have balance" 


Scenario: 3 - There is no budget left in the wallet when attempting purchase - Notify "Unable to make purchase"
	Given The budget is the lower than the amount
	When Trying to make purchase
	Then Notify - "Unable to make purchase"
	

Scenario: 4 - Disable safe mode when budget is refilled - disable safe mode
	Given safe mode is enabled and budget is refilled
	When Trying to make purchase
	Then  Disable safe mode