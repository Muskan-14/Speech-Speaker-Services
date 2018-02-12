using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;
using System.Text;

namespace Bot_Application2_using_luis.Dialogs
{
    [LuisModel("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        //Entities
        public const string marketplace_productname = "marketplace.productname";
        public const string marketplace_timeperiod = "marketplace.timeperiod";
        public const string marketplace_ProductCategory = "marketplace.ProductCategory";
    
    
        // Intents
        public const string marketplace_Categories = "marketplace_Categories"
        public const string marketplace_None = "None";
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }

 

        [LuisIntent(marketplace_Categories)]
        public async Task OnCategories(IDialogContext context, LuisResult result)
        {
           
            await this.ShowLuisResult(context, result);
        
        // Entities found in result

        public string BotEntityRecognition(string intentName, LuisResult result)
        {
            IList<EntityRecommendation> listOfEntitiesFound = result.Entities;
            StringBuilder entityResults = new StringBuilder();

            foreach (EntityRecommendation item in listOfEntitiesFound)
            {
               
                entityResults.Append(item.Type.Replace("marketplace.", "") + "=" + item.Entity + ",");
            }
            // remove last comma
            entityResults.Remove(entityResults.Length - 1, 1);

            return entityResults.ToString();
        }
        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            // get recognized entities
            string entities = this.BotEntityRecognition(marketplace_None, result);

            // round number
            await context.PostAsync($" Your request '{ result.Query}' has been initiated: {result.Query}, **Intent**: {result.Intents[0].Intent},  **Entities**: {entities}");
            context.Wait(MessageReceived);
        }
    }
}
