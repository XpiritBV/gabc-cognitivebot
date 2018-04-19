# Lab 05: Identify murder weapons

In this lab we'll take a look at the basics of the Custom Vision apis in Cognitive Services. We'll take a look at implementing basic object recognition. In this lab we will use our previous trained model to recognize the murder weapons.

1. Clone This Repo
```
git clone https://github.com/XpiritBV/gabc-cognitivebot.git
```

2. Checkout branch for this lab called labs/lab05
```
git checkout -f labs/lab05
``` 

3. Open the solution in Visual Studio 2017, VS Code or Visual Studio for Mac: 
```
cognitivebot\cognitivebot.sln
```

4. Browse the code to find the place to add Custom Vision logic used by the bot "Identify Murders Weapons" method.

5. Use the [CustomVision Prediction](https://www.nuget.org/packages/Microsoft.Cognitive.CustomVision.Prediction/) from cognitive services and implement the method to return the recognized weapon.

> HINT: Use your custom vision project ID and training endpoint that was created in lab 3. 

## References / Hints
> Master branch has a working version implemented.

> Custom Vision API Docs: 
https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/home