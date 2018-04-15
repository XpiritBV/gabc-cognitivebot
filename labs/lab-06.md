# Lab 06: Deploy the bot to Azure and connect it to channels

In this lab we will deploy our bot to Azure. We will first create a Web App Bot in Azure and then deploy our application to it.

1. Create a Resource group that contains all Bot resources

* Click on the + Create new resource groups and give it a name
![](/labs/img/1.png)

* Wait untill the resource group is created
2. Now we can add our bot to the newly created resource group. Click **+ Create a Resource** and search for **"Web App Bot"**
![](/labs/img/2.png)

3. Fill in all properties to create a web app bot
![](/labs/img/3.png)

4. Wait untill the deployment finishes. now we can navigate to our web app bot.
![](/labs/img/4.png)

5. Try to find the URL of the web app bot and connect the bot emulator to your deployed bot.

now we have a working Bot running on Azure but it's only echo-ing our messages. we need to deploy the Detective bot application to Azure.

5. Deploy the Detectivebot to the web app through Visual Studio

