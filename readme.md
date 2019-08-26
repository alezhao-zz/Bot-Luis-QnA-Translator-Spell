### **Overview**

Microsoft Bot Framework is a comprehensive offering to build and deploy high quality bots for your users to enjoy in their favorite conversation experiences. Developers writing bots all face the same problems: bots require basic I/O; they must have language and dialog skills; they must be performant, responsive and scalable; and they must connect to users – ideally in any conversation experience and language the user chooses. Bot Framework provides just what you need to build, connect, manage and publish intelligent bots that interact naturally wherever your users are talking – from text/SMS to Skype, Slack, Facebook Messenger, Office 365 mail and other popular services. This lab is to setup a collaboration reserve Azure bot with language understanding, Q&A, translator and spell check service. User could chat with bot with more than 60 languages e.g. Chinese, English, Japanese etc. to reserve the restaurant and check reviews. 

![](https://iothubstorageaccts.blob.core.windows.net/botpic/overview.png)

## **System requirements**

You must have the following to complete this lab:

- Azure Subscription
- Windows 10
- Microsoft Visual Studio 2017 Community (latest update) 
- Bot Framework Emulator (latest update)

## **Exercises**

This Hands-on lab includes the following exercises:

1. ###### Create Azure Service
2. ###### Setup local bot project with Bot Framework SDK
3. ###### Use the Bot Framework Emulator to test your Bot
4. ###### Deploy your local bot on Azure
5. ###### Use web chat to verity your Azure bot service

Estimated time to complete this lab:  **90** **minutes**.

### Exercise 1: Create Azure  Service

1. Login Azure portal with provide Azure subscription

2. Create a notepad file, copy and paste below notes, save as config.txt at C:\lab\config.txt.

   ```xml
   <appSettings>
       <!-- Your BotId, Microsoft App Id and your Microsoft App Password-->
       <add key="BotId" value="" />
       <add key="MicrosoftAppId" value="" />
       <add key="MicrosoftAppPassword" value="" />
       <!-- Azure bing spell check -->
       <add key="IsSpellCorrectionEnabled" value="true" />
       <add key="BingSpellCheckApiKey" value="" />
       <!-- Azure translator -->
       <add key="AzureTranslatorApiKey" value="" />
       <!-- Azure bot storage to trigger message -->
       <add key="AzureWebJobsStorage" value="" />
       <!-- QnA maker -->
       <add key="QnAmakerUriBase" value="" />
       <add key="KnowledgeBaseId" value="" />
       <add key="AuthorizationKey" value="" />
       <!-- Luis -->
       <add key="LuisApplicationID" value="" />
       <add key="LuisAuthoringKey" value="" />
   </appSettings>
   ```

3. Create a Web App Bot Service

4. - Create a new **Resource Groups** `BOTRG` click Add at top panel. (remember `BOTRG`, please create all of services within this resource group)

   - Then entry `Web App bot`. A new blade will open with information about the `Web App Bot`

   - In the **Bot Service** blade, provide the requested information about your bot as specified in the table below the image.

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/2.png)

     - Entry your **[Bot Service Name],** entry **[App Name]** in **App name** section, choose lab provided **[Azure Subscription]** in **Subscription** section, select **East Asia** at **Location** section, click **Bot template** section, select **SDKv3** with **Basic C#** template, click **Select.** Click and create new app service plan in **App service plan/Location** section, select **East Asia** at **Location** section at output panel.	

       ![](https://iothubstorageaccts.blob.core.windows.net/botpic/3.png) 

     - Create New **[App Storage]** in **App Storage** section

       ![](https://iothubstorageaccts.blob.core.windows.net/botpic/4.png)

     - Unable **Application Insights** currently

       ![](https://iothubstorageaccts.blob.core.windows.net/botpic/5.png)

     - Click **Create**

   - Click upper **Notification** link of right-hand, if resource created successfully, click **Go To Resource** to open service you just created.

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/6.png)

   - Click **Application Settings** under **App Service Settings** section, copy value of `AzureWebJobsStorage` to C:\lab\config.txt 

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/7.png)

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/40.png)


4. Create a Translator Text service

   - Click **Create New Resource** link found on the upper left-hand corner of the Azure portal, entry `Translator Text` in filter panel, click enter keyboard, select **Translator Text** service, click **Create**

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/8.png)

   - Entry `New Translator Service Name` of your service, choose lab provided **Azure Subscription** in **Subscription** section, select **S1** in **Pricing Tier** section, select your resource group at **Resource Group** section

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/9.png)

   - Click **Create**

   - Click upper **Notification** link of right-hand, if resource created successfully, click **Go to resource** to open service you just created.

   - Click **Keys** in **RESOURCE MANAGEMENT** section, copy **KEY 1** into C:\lab\config.txt

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/41.png)

5. Create a Bing Spell Check service

   - Click **Create new resource** link found on the
     upper left-hand corner of the Azure portal, entry **Bing Spell** in filter panel, click **enter** keyboard, select **Bing Spell Check v7** service, click **Create**

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/10.png)

   - Entry `New Spell Check Service Name` of your service, Entry `New Translator Service Name` of your service, select **S1** in **Pricing tier** section, select your resource group at **Resource group** section, check on **Confirm** condition.

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/11.png)

   - Click **Create**

   - Click upper **Notification** link of right-hand, if resource created successfully, click **To to resource** to open service you just created.

   - Click **Keys** in **RESOURCE MANAGEMENT** section, copy **KEY 1** into C:\lab\config.txt

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/13.png)

6. Create a LUIS service

   - Open LUIS portal <http://www.luis.ai>, click **Log****in,** entry lab provided **[Azure Subscription]** and **[Password]** to login

   - Click **Create new** **app** link found on the upper left-hand corner of the LUIS portal, entry `LUIS name` in **Name** section, Click **Done**

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/14.png)

   - Click **Prebuilt domains** link found on the bottom left-hand corner of the LUIS portal, entry `res` in the filter panel, click **Add Domain** of output `RestaurantReservation` domain

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/15.png)

   - Entry `pla` in the filter panel, click **Add Domain** of output **Places** domain

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/16.png)

   - Click **Intents** link found in left panel, find `Places.GetReviews` and click, entry `starbucks reviews` at utterance entries to add new utterance, click **enter** keyboard

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/17.png)

   - Click `starbucks` word in new utterance added, entry `placename` at pop-up window filter panel, click `Places.PlaceName` 

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/18.png)

   - Click **Train** link found on the upper right-hand corner of the LUIS portal, once status shows complete, click **publish** to on the upper right-hand corner to publish LUIS app to production

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/19.png)

   - Create a notepad, save as config.txt.

   - Click **MANAGE** tab at upper panel, save **Application ID** at left-hand **Application Information** panel to C:\lab\config.txt, save **Authoring Key** at left-hand **Keys and Endpoint** panel to C:\lab\config.txt 

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/20.png)

7. Create a QnA maker service

   - Click **Create new resource** link found on the upper left-hand corner of the Azure portal, entry **QnA** in filter panel, click **enter** keyboard, select **QnA Maker** service, click **Create**

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/21.png)

   - Entry `QnA Maker Service Name` in **Name** section, choose lab provided `Azure Subscription` in **Subscription** section, select **West US** in **Location** section, select **S0** in **Management price tier** section, select **S** in **Pricing tier** section, entry `App Name` in **App name** section, select **West
     US** at **Search location** section.

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/22.png)

   - Click **Create**

   - Open QnA maker portal <https://www.qnamaker.ai/>, click Create a knowledge base at upper tab.

   - Under section **Step 2**, select **Microsoft** in **Microsoft Azure Directory ID** section, choose lab provided `Azure Subscription` in **Azure subscription name** section, select **QnA maker Service Name** which created by pervious step in **Azure QnA service** section.

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/23.png)

   - Under section **Step 3**, entry your `Knowledge Base Name` in **Name your KB** section.

   - Under section **Step 4**, click c, select **C:\lab\qnamaker.xlsx** to upload

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/24.png)

   - Click **Create your KB** at the bottom. 

   - Under your created QnA Maker App, click **Save and train** at upper of right-hand corner.

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/25.png)

   - After train completed, select **PUBLISH** at upper panel, click **Publish**

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/26.png)

   - After publish completed, go to SETTING panel, scroll down to the bottom, copy `Knowledge Based ID`, `QnAmakerUriBase` and `AuthorizationKey` found at POST /knowledgebases/`Knowledge Based ID`/generateAnswer 

     Host: `QnAmakerUriBase` 

     Authorization: `AuthorizationKey` 

     into C:\lab\config.txt, save config.txt

     ![](https://iothubstorageaccts.blob.core.windows.net/botpic/27.png)

8. Verify C:\lab\config.txt, to check if below keys are all setup.

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/28.png)

## Exercise 2: Setup local bot project with Bot Framework SDK

1. Double click **Microsoft.Bot.Sample.SimpleEchoBot.sln**, open Bot solution with Visual Studio 2017 community.                   

2. Double click **Web.config** under project **Microsoft.Bot.Sample.SimpleEchoBot,** replace 

   `< appSettings…</ appSettings>`
   with values in C:\lab\config.txt         

    ![](https://iothubstorageaccts.blob.core.windows.net/botpic/29.png)   

3. Entry **Root** at upper of right-hand filter, double click **RootLuisDialog.cs** from filter result, replace `LuisModel("[Application ID]", "[Authoring Key]")` with values in C:\lab\config.txt, **Ctrl+S** to save, clear **filter**. 

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/30.png)

4. Right click project name **Microsoft.Bot.Sample.SimpleEchoBot** at upper of right-hand corner, select **Build**

## Exercise 3: Use the Bot Framework Emulator to test your local Bot

1. Click debug with **IIS Express (Microsoft Edge)** upper of middle, copy opened **[Local URL]** with port e.g. <http://localhost:3984/>

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/31.png)

2. Open **Bot Emulator** at Desktop, click **Create a new bot configuration,** entry `Bot Name` entry `Local URL/api/messages` in Endpoint URL section. E.g [http://localhost:3984//api/messages](http://localhost:3984/api/messages), click **Save and connect**, save configuration file under C:\lab\\`Bot Name`.bot

3. Verify if Bot Emulator return 200 at LOG section with **Welcome User!**

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/32.png)

4. Entry some utterance to check e.g. `KFC reviews`. If return like below then success. Now your local bot
   is working, and connected to LUIS, translator, spell check and QnA maker. Next step we need deploy your local bot onto Azure.

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/33.png)

5. Stop debug by click **Stop** **Debugging** at middle of upper panel. 

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/34.png)

## Exercise 4: Deploy your local bot on Azure

1. Right click project name **Microsoft.Bot.Sample.SimpleEchoBot** at upper of right-hand corner, select **Publish**

2. Select **Microsoft Azure App Service** in left panel **Publish** section, select **Select Existing,** click **Publsih,** login with lab provided `Azure Subscription` and `Password` select `Azure Subscription` select your resource group, click `Bot Service Name` created in pervious step, click **OK**

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/35.png)

3. Verify if publish succeeded.

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/36.png)

## Exercise 5: Use Web Chat to test your Azure bot 

1. Login Azure portal with provide `Azure subscription`

2. Click **All resources** at left-hand panel, entry `Bot Service Name` to find your bot service and click, select **Test in Web Chat** under **Bot management** section.

3. Entry test utterance (multiple languages) to verify your bot service on Azure

   e.g. 

   `“I wang to book a room at KFC”`

   `“KFC reviews”`

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/37.png)

   Click **Channels** under **Bot management** section, click **Edit** of **Web Chat,** copy first **Secret Keys** at the top and paste to a new notepad, copy and paste URL value in SRC of **Embed code** to notepad, replace **YOUR_SECRET_HERE** with **Secret Keys** just saved

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/38.png)

   Open **Microsoft Edge**, put whole URL to URL section and search, you will find a web chat bot is ready, it also could integrated as a frame. 

4. Entry test utterance (multiple languages) to verify your web chat bot. 

   ![](https://iothubstorageaccts.blob.core.windows.net/botpic/39.png)
