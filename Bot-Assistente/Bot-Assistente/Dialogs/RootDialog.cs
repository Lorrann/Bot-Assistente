using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using BotBuilder.Instrumentation.Dialogs;
using Microsoft.Bot.Builder.Luis;

namespace Bot_Assistente.Dialogs
{
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        public RootDialog(LuisService service) : base(service) { }

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
            await context.PostAsync("Sou Alice, um bot que te ajuda a escolher destinos para viagens!\n\n\nVocê pode perguntar coisas como Quero " +
                "Viajar para EUROPA,\nQueri ir para uma praia tranquila,\nQuero conhecer lugares frios,\nEntão vamos lá, pra onde deseja viajar? ");

            context.Wait(MessageReceived);
        }

		[LuisIntent("Viagem")]
		public async Task Viagem(IDialogContext context, LuisResult result)
		{
			await context.PostAsync("Diga um destino para onde deseja viajar?");

			context.Wait(MessageReceived);
		}

		#endregion
	}
}