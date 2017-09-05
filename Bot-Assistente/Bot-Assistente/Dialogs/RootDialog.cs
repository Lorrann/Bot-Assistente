using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using BotBuilder.Instrumentation.Dialogs;

namespace Bot_Assistente.Dialogs
{
    [Serializable]
    public class RootDialog : InstrumentedLuisDialog<object>
    {
        public RootDialog(string luisModelId, string luisSubscriptionKey) : base(luisModelId, luisSubscriptionKey) { }

        #region Intents
        
        [LuisIntent("")]
        public async Task NoneHandler(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe, não entendi");
        }

        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe, não consigo te entender, pode repetir de uma outra forma?");

            context.Wait(MessageReceived);
        }

        [LuisIntent("Saudar")]
        public async Task Saudar(IDialogContext context, LuisResult result)
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).TimeOfDay;
            string saudacao;

            if(now < TimeSpan.FromHours(12))
            {
                saudacao = "Bom dia!";
            }
            else if(now < TimeSpan.FromHours(18))
            {
                saudacao = "Boa tarde";
            }
            else
            {
                saudacao = "Boa noite";
            }

            await context.PostAsync($"{saudacao}! Em que posso ajudar?");
            context.Done("");
        }

        [LuisIntent("Ajudar")]
        public async Task Ajudar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Como posso te ajudar?");

            context.Wait(MessageReceived);
        }

        #endregion


        //public Task StartAsync(IDialogContext context)
        //{
        //    context.Wait(MessageReceivedAsync);

        //    return Task.CompletedTask;
        //}

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;

        //    // calculate something for us to return
        //    int length = (activity.Text ?? string.Empty).Length;

        //    // return our reply to the user
        //    await context.PostAsync($"You sent {activity.Text} which was {length} characters");

        //    context.Wait(MessageReceivedAsync);
        //}
    }
}