# Lab 05: Training the API for known murder weapon types

In this lab we'll take a look at the basics of the Custom Vision apis in Cognitive Services. We'll take a look at implementing basic object recognition. Object recognition does not work without first training your custom vision model with predefined objects. In this lab we will upload and train our Custom Vision model.

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

4. Browse the code of the TrainCustomVision console application. Look at the code provides in the ```Program.cs``` class.


4. Browse the code to find the place to add face recognition logic used by the bot "Person Details" method.

5. You'll need the Face API from cognitive services in Azure. Create a new Face API in Azure and retrieve the API Key from the portal

![](/labs/img/5.png)

6. Use the [FaceClient](https://www.nuget.org/packages/Microsoft.ProjectOxford.Face/) from cognitive services and implement the method to return details from identified persons.



## References / Hints
> Master branch has a working version implemented.

> Face API Docs: https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp
