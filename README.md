# Uitleg
Met dit project ga ik een budgetTracker testen,
De functionaliteit van de budgetTracker zorgt ervoor dat je meldingen krijgt of blokeert naarmate je uitgave.

# Structuur
- *Tracker*
Dit is de klasse die de beslissingen maakt zoals het goedkeuren van de betaling en het blokkeren van de kaart

- *Notifier*
Deze klasse geeft een notificatie door aan de beslissingsmodulle dit zorgt ervoor dat het makkelijk te testen is,
deze klasse implementeert de interface INotifier.

- *BudgetHelper*
Deze klasse verzamelt de informatie en geeft de info door aan de beslissingsmodule.
- haalt het budget op
- Haalt de kosten op
- Past het budget aan
Deze klasse implementeert de interface IBudgetHelper

- *BudgethelperApi*
Ik heb een aparte Klasse gemaakt om met mockoon te werken,
Met de hulp van DI worden BudgetHelperApi en BudgetHelper correct gebruikt.
Deze klasse implementeert de interface IbudgetHelper


Met deze structuur is elke klasse verantwoordelijk voor zijn eigen taak,
hierdoor blijft het project overzichtelijker en eenvoudiger.
Door het gebruik van DI en interfaces is het eenvoudig verschillende zaken te mocken.
Dit zorgt ervoor dat je geen andere code of fuctionaliteit moet aanpassen en een aangename werkflow.

# Unit Testen
*TestIfBudgetIsZero*
Deze test controleert wanneer het budget gelijk is aan het bedrag van de aankoop.
De verwachte uitkomst is dat de aankoop door gaat en een notificatie word verzonden met de melding "Purchase made! no more balance".
Het aantal mislukte pogingen blijft 0.

*TestIfBudgetRemains*
Deze test controleert wanneer het budget meer is dan het bedrag van de aankoop.
De verwachte uitkomst is dat de aankoop door gaat en een notificatie word verzonden met de melding "Purchase made, you still have balance".
Het aantal mislukte pogingen blijft 0.

*TestIfNoBudgetRemains*
Deze test controleert wanneer het budget minder is dan het bedrag van de aankoop.
De verwachte uitkomst is dat de aankoop niet door gaat en een notificatie word verzonden met de melding "Unable to make purchase".
Het aantal mislukte pogingen is 1

*TestForBlockedCard*
Deze test controleert wanneer er een fout optreedt bij het ophalen van het budget.
De verwachte uitkomst is dat de kaart na 3 fails geblokkeerd wordt en in safe modus gaat.
Het aantal mislukte pogingen is 3

*TestForBlockedCardReset*
Deze test controleert wanneer er een fout oprteedt bij het ophalen van budget gevolgd door een reset van safe mode
De verwachte uitkomst is dat na 3 fails de kaart blokkeerd en in safe modus gaat.
Wanneer er een geldig budget wordt opgehaald gaat safe modus af en gaat de betaling door,
er wordt een notificatie verzonden: "Purchase made! no more balance".

*TestStayInSafeMode*
Deze test controleert wanneer er een fout oprteedt bij het ophalen van budget gevolgd door een aankoop met een te laag budget.
De verwachte uitkomst is dat na 3 fails de kaart blokkeerd en in safe modus gaat.
Waneer je een aankoop wil doen dat boven je budget is blijft safe modus aan.
er wordt een notificatie verzonden: "Unable to make purchase".

# Integration Testen
De integratietesten lijken sterk op de unittesten, maar maken gebruik van Mockoon om een mock-API te simuleren.
Met Mockoon wordt de API nagebootst, zodat de applicatie data kan ophalen alsof deze van een echte backend komt.

# Acceptance testen
Deze testen zijn dezelfde als de unitttesten en de integration testen,
maar maken gebruik van Gherkin en feature bestanden om test scenario's in een duidelijke en leesbare structuur te beschrijven.
Deze features maken gebruik van Given, When, Then.
Door het importeren van de feature files worden de testen geautomatiseerd
