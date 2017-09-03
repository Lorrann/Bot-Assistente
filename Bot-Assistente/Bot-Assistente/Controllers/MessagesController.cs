using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Bot_Assistente.Dialogs;
using System;
using Bot_Assistente.Controllers;

namespace Bot_Assistente
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity.Type == ActivityTypes.Message)
            {
                await InstrumentationHelper.DefaultInstrumentation.TrackActivity(activity);

                await Conversation.SendAsync(activity, () => new RootDialog(
                    ConfigurationManager.AppSettings["LuisId"],
                    ConfigurationManager.AppSettings["LuisKey"]));
            }
            else
            {
                var reply = activity.CreateReply();
                reply.Text = "Olá, eu sou o **BotInho**.\n\n\nPor hora a única coisa que eu faço é jogar um **RPG** com você.\n\n\nMas lembre-se, eu sou o master. É só você seguir os meus comandos que vamos ter um bom jogo.\n\n\n**PS:** Eu estou na v0.1.1, então pega leve tá?!?";

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //private Activity HandleSystemMessage(Activity message)
        //{
        //    if (message.Type == ActivityTypes.DeleteUserData)
        //    {
        //        // Implement user deletion here
        //        // If we handle user deletion, return a real message
        //    }
        //    else if (message.Type == ActivityTypes.ConversationUpdate)
        //    {
        //        // Handle conversation state changes, like members being added and removed
        //        // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
        //        // Not available in all channels
        //    }
        //    else if (message.Type == ActivityTypes.ContactRelationUpdate)
        //    {
        //        // Handle add/remove from contact lists
        //        // Activity.From + Activity.Action represent what happened
        //    }
        //    else if (message.Type == ActivityTypes.Typing)
        //    {
        //        // Handle knowing tha the user is typing
        //    }
        //    else if (message.Type == ActivityTypes.Ping)
        //    {
        //    }

        //    return null;
        //}
    }
}