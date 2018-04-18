# Lab 04: Training the API for known suspects

In this lab we'll add functionality train our API for known suspects. In real life this could be done through a police database of known suspects. This lab will continue where lab-02 finishes. if you have a working version of lab-02 you can continue with step 4 and skip step 1-3.

1. Clone This Repo
```
git clone https://github.com/XpiritBV/gabc-cognitivebot.git
```

2. Checkout branch for this lab called labs/lab01
```
git checkout -f labs/lab04
```

3. Open the solution in Visual Studio 2017, VS Code or Visual Studio for Mac: 
```
cognitivebot\cognitivebot.sln
```

5. Use the [FaceClient](https://www.nuget.org/packages/Microsoft.ProjectOxford.Face/) from cognitive services and implement methods to train new persons. This contains the following steps
* After upload of an image check if it contains a face
* if image contains a face do we already know it?
* if yes -> add it to the existing person to improve recognition
* if no -> ask for a name and create a new person

## References / Hints
> Master branch has a working version implemented.

> Face API Docs: https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp
