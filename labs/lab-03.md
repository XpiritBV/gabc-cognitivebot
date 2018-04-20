# Lab 03: Training the API for known murder weapon types

In this lab we'll take a look at the basics of the Custom Vision apis in Cognitive Services. We'll take a look at implementing basic object recognition. Object recognition does not work without first training your custom vision model with predefined objects. In this lab we will upload and train our Custom Vision model.

1. Clone This Repo
```
git clone https://github.com/XpiritBV/gabc-cognitivebot.git
```

2. Checkout branch for this lab called labs/lab03
```
git checkout -f labs/lab03
``` 

3. Open the solution in Visual Studio 2017, VS Code or Visual Studio for Mac: 
```
cognitivebot\cognitivebot.sln
```

4. Browse the code of the TrainCustomVision console application. Look at the code provides in the ```Program.cs``` class.

5. Fill in your Custom Vision api key and add the code to create or retrieve a custom vision project. You can create a project using code or create one in the [Custom Vision portal](https://customvision.ai/)

![](/labs/img/customvision.png)

6. Complete the code to upload your images and train the Custom Vision model.

## References / Hints
> Master branch has a working version implemented.

> Custom Vision API Docs: 
https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/home