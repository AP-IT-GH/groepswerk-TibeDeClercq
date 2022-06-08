# Groepswerk: Zeeslag VR

<b> 2ITSOF2 2021 - 2022 | Evert Bekaert, Tibe De Clercq, Ruben Vorsselmans, Bjorn Felix, Jeroen Roelant </b>

Wij maken een VR versie van het bordspel "Zeeslag". Je bent in de controlekamer van een oorlogsschip en speelt tegen ML Agent. Deze schepen worden willekeurig geplaatst op het veld.
Voor jou krijg je een holografisch speelveld te zien. Dit is grid is van je tegenspeler. Hierop kan je oftewel wijzen met je vingers (hand tracking) of gebruik maken van de controllers om een vak aan te duiden op het grid. Je zal hier dan op schieten. We werken niet turn-based maar eerder met een schiet-cooldown.

## Samenvatting

In dit document wordt uitgelegd welke stappen er werden gedaan om dit project te ontwikkelen.

- Installatie
- Verloop van het spel
- Observaties, mogelijke acties en beloningen
- Objecten
- One pager
- Resultaten
- Conclusie
- Bronvermelding

## Installatie

Voor dit project gebruiken wij Unity versie 2020.3.24f1. De plugins die gebruikt worden zijn als volgt: <br>
![plugins](/img/Picture1.png "List van plugins")

## Verloop van het spel

Zowel de speler als de agent kunnen schieten wanneer ze willen. Na elk schot is er wel een cooldown van x aantal seconden voordat er weer geschoten mag worden. De eerste speler dat al de schepen van de tegenstander raakt wint.
Je speelbord zal visueel aantonen of je schot raak is. Blauw is ongekende waters, wit is een gemist schot en rood is een raak schot.
Wanneer het spel is afgelopen krijgt de speler de keuze om opnieuw te spelen. De schepen op de speelvelden worden dan opnieuw random gegenereerd.

## Observaties, mogelijke acties en beloningen

De tegenstander in het spel is een ML Agent die werd getrained aan de hand van reinforcement learning. De agent maakt gebruik van rays om observaties te maken. De agent maakt gebruik van zijn vorig schot om naar de schepen te zoeken. De aantal gimiste schoten worden ook bijgehouden. Deze worden ook gebruikt bij het berekenen van de reward.

| Aties                                         | Beloning (floats)                                                       |
| -------------------------------| ------------------------------------------------------|
| StepCount onder 2000 *     |                            -1                                                   |
| Game winnen                         | (watertileCount-missedcount)/waterTileCount |

- In de OnActionReceived wordt gecontroleerd of de game completed is. Indien dit waar is en de agent de winaar is krijgt de agent een reward afhankelijk van zijn "precisie".
Deze reward kan maximaal 1.0 zijn in geval dat de agent nooit gemist heeft.
- Elke keer dat de OnActionReceived methode wordt uitgevoerd wordt de stepcount verhoogd. Wanneer de Stepcount meer dan 2000 is zal de agent een reward van -1f krijgen en zal de episode beindigd worden. Dit zorgt er voor dat de agent niet stopt met schieten.

## Objecten

Het project is opgebouwd uit verschillende core componenten. <br>
![Game Objecten](/img/gameObjects.png "List van game objecten")

- Fields: Dit zijn de velden (Grids) dat alle schepen van een speler bevat. Er zijn dus twee fields, van jou en van de tegenspeler.
- Schepen: Hebben een default behavior zodat het lijkt dat ze dobberen.
- Missile: Een raket detecteert met welk object hij collision heeft. Als hij het water raakt is er een plons, raakt het een schip, dan is er een explosie.
- Observatiegrid: Deze grids bestaan uit meerdere cubes waarmee de speler kan interageren. Zowel de agent als de speler maken hier gebruik van. Hier op kan je zien op welke coördinaten werden geschoten, en waar een vijandelijk schip werd geraakt.
- Agent: Dit is de getrainde agent die de tegenspeler gebruikt. Deze gebruikt zoals eerder gezegd het observatiegrid.
- Menu: Dit bevat elke canvas dat wordt gebruikt voor de UI van het spel.
- Scene objecten: Er wordt gebruik gemaakt van verschillende objecten voor de scene te maken. Dit bevat onder andere:
  - Water: Een custom water shader die beweegt en interactie heeft met de schepen en kogels.
  - Mist: De oceaan is bedekt met een laag dikke mist. Deze werkt aan de hand van custom particles.
  - Regen: Dit wordt bereikt door een prefab te gebruiken van een extern pakket "Rain maker".
  - Schepen: schepen gebaseerd op assets van de Unity asset store
  - Bridge: Compositie van assets voor de omgeving waar de speler zich bevindt.
  - Kanonnen: Delen van schepen die bewegen op basis van de acties van de speler.

## One pager

````
Je zit in de wapenkamer van een oorlogsschip. Er zijn twee holografische borden in de kamer. Een hiervan toont jouw schepen terwijl het andere bord het veld van de vijand voorstelt. Hierop moet je een coördinaat aanduiden waar je een artillerieaanval op wil uitvoeren. We werken met cooldowns van 3-5 seconden na elk schot, tegenover de originele turn-based manier van spelen. We denken dat dit het leuker en spannender zou maken. Het schip dat als eerste de vloot van de tegenstander vernietigt, wint het spel.

De werking hiervan is als volgt. Het zeeslag spel speelt zich niet af in de 3d omgeving, maar eerder in de game manager, waar velden worden voorgesteld als 2-dimensionale char arrays. Daarop kan jij je schepen zetten.

De vijand zal in dit geval de ML agent zijn. Deze zal leren om patronen te herkennen, in de plaats van telkens op een willekeurige locatie te bombarderen. De AI zal beloond worden wanneer hij een van jouw schepen raakt. Hij wordt gestraft wanneer hij een coördinaat raakt waar al eerder op werd geschoten. Er wordt ook een kleine straf gegeven wanneer hij water raakt na een schip te raken.

De omgeving waar de speler zich bevind is het interieur van een oorlogsschip.

Op het einde van het spel zal je een scherm te zien met de uitslag, het aantal schoten en de tijd die van het spel.
````

Het project bleef grotendeels het concept dat we hebben gebruikt in de one pager volgen, met uitzondering van de boten die de speler zelf plaatst. Deze worden nu random geplaatst wanneer je het spel start. 
Verder werd het systeem van beloningen aangeast. De agent wordt nu beloond/bestraft op basis van het aantal gemiste schoten.
De implementatie van het overzichtscherm dat je te zien krijgt na het spel is ook aangepast.

## Resultaten

Training:  

![Cumulatieve rewards](/img/training_1.png "Cumulatieve rewards")

De training is voltooid in stappen (lessons) met behulp van curriculum training. De agent begint met trainen op een veld van 5x5 in lesson 1 en leert uiteindelijk spelen op een veld van 10x10. Elke lesson wordt het speelveld met 1x1 verhoogd als de agent een bepaalde score bereikt ( 0.6 ).

![Lesson Number / Field size](/img/training_2.png "Lesson Number / Field size")

De agent heeft de eerste lesson voltooid na +-1.8m steps, de tweede na +-3.7m steps, de derde is snel bereikt na +-3.8m steps. De voorlaatste lesson duurde langer om te leren omdat er veel meer water is om op te schieten waardoor de score verlaagt wordt ( 9x9 ). Deze is voltooid na +-8.7m steps.

![Episode Length](/img/training_3.png "Lengte van episodes")

De episode length gaat in het begin naar beneden aangezien de agent leert om correct te spelen, hierna verhoogt hij een beetje bij elke lesson, aangezien het veld steeds groter wordt.

![Policy loss](/img/training_4.png "Policy loss")

De policy loss is zo klein dat het verwaarloosbaar is.

![Value loss](/img/training_5.png "Value loss")

De value loss verlaagt verder in de training, dit is een teken van een goede training.

Opmerking: Soms stopt de agent met schieten en kan de training niet doorgaan, om dit tegen te gaan wordt de agent bestraft als hij stopt met schieten. Dit zorgt voor de grote dalingen in de grafiek (afbeelding 1)

## Conclusie

Ons project is een VR variant van het bekende bordspel zeeslag waarin je tegenstander een ML-Agent is.

### Overzicht resultaten

De training is succesvol verlopen met behulp van curriculum training. Zonder curriculum training liep de agent vast bij te grote velden. Door de agent in stappen te laten training liep hij niet meer vast omdat de moeilijkheid in kleine stappen verhoogt.

### Persoonlijke visie resultaten

Zoals eerder gezegd is de training succesvol verlopen. Wij vinden dat onze aanpak een goede aanpak is om dit soort agent te trainen. We hebben geprobeerd om de agent te trainen aan de hand van verschillende observatiemethoden, waaronder camera, rays en 2D arrays. Uiteindelijk hebben we gekozen om rays te gebruiken. Door het gebruik van stappen kon de agent telkens op een groter veld gaan trainen op het moment dat de agent een bepaalde reward behaald heeft. In het begin waren we direct begonnen met trainen op een groot veld, maar dit was niet succesvol.

### Verbetering naar toekomst

- Overzicht scherm op het einde van het spel met o.a. aantal schoten etc
- Moeilijkheid setting die verschillende niveaus van getrainde agents gebruiken
- Mooie effecten wanneer je wint/verliest (regenboog/nuke)

## Bronvermelding

<a id="1">[1]</a>
Etredal (15/08/2021) <br>
<a href="https://youtu.be/UllkvfMR96s">Volumetric Fog in Unity using Particles (Any Rendering Pipeline)</a>

<a id="2">[2]</a>
Jeff Johnson (17/05/2022) <br>
<a href="https://assetstore.unity.com/packages/vfx/particles/environment/rain-maker-2d-and-3d-rain-particle-system-for-unity-34938#description">Rain Maker - 2D and 3D Rain Particle System for Unity</a>

<a id="3">[3]</a>
IgniteCoders (19/03/2021) <br>
<a href="https://assetstore.unity.com/packages/2d/textures-materials/water/simple-water-shader-urp-191449">Simple Water Shader URP</a>

<a id="4">[4]</a>
Oculus (31/05/2022) <br>
<a href="https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022">Oculus integration</a>
