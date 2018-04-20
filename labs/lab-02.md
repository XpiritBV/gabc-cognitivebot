# Lab 02: Identify criminals

> This lab continues on the knowledge of lab 01 where we are going to use the face API to identify persons.

1. Clone This Repo
```
git clone https://github.com/XpiritBV/gabc-cognitivebot.git
```

2. Checkout branch for this lab called labs/lab02
```
git checkout -f labs/lab02
```

3. Open the solution in Visual Studio 2017, VS Code or Visual Studio for Mac: 
```
cognitivebot\cognitivebot.sln
```

4. Browse the code to find the place to add face recognition logic used by the bot "Identify suspects" method.


5. Use the [FaceClient](https://www.nuget.org/packages/Microsoft.ProjectOxford.Face/) from cognitive services and implement the method to return the name of a person who was identified.

> HINT: You can use the API key from the master branch because this already has a trained model with suspects. (Hint try to take a picture of one of the Xpirit employees. They look like criminals to me.)

> HINT2: Identifying persons consists of 3 steps.
> 1. Retrieve a faceId
> 2. Get the personId based on a faceId.
> 3. Get person name and details based on personId


## References / Hints
> Master branch has a working version implemented.

> Face API Docs: https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp
