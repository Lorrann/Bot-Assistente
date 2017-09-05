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
using System.Linq;
using Microsoft.Bot.Builder.Luis;

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
            var atributes = new LuisModelAttribute(
                    ConfigurationManager.AppSettings["LuisId"],
                    ConfigurationManager.AppSettings["LuisKey"]);

            var service = new LuisService(atributes);

            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog(service));
            }
            else
            {
                await HandleSystemMessageAsync(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {

            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded.All(o => o.Id != message.Recipient.Id))
                    return null;

                var connector = new ConnectorClient(new Uri(message.ServiceUrl));
                var reply = message.CreateReply();
                reply.Text = "Olá, sou Alice em que posso ajudar?";

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {

            }
            else if (message.Type == ActivityTypes.Typing)
            {

            }
            else if (message.Type == ActivityTypes.Ping)
            {

            }

            return null;
        }
    }
}