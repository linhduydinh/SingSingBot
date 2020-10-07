using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using SingSingBot.Models;
using SingSingBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SingSingBot.Bots
{
    public class GreetingBot : ActivityHandler
    {

        #region Variables
        private readonly StateService _stateService;
        #endregion

        protected override Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            return base.OnMessageActivityAsync(turnContext, cancellationToken);
        }


        protected override Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            return base.OnMembersAddedAsync(membersAdded, turnContext, cancellationToken);
        }

        private async  Task GetName(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            UserProfile userProfile = await _stateService.UserProfileAccessor.GetAsync(turnContext, () => new UserProfile());
            ConversationData conversationData = await _stateService.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData());


            if(!string.IsNullOrEmpty(userProfile.Name))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(string.Format("Hi {0}. How can I help you today?", userProfile.Name)), cancellationToken);
            }
            else
            {
                if(conversationData.PromptedUserForName)
                {
                    // Set the name to what the user provided
                    userProfile.Name = turnContext.Activity.Text?.Trim();

                    // Acknowledge that we got their name
                    await turnContext.SendActivityAsync(MessageFactory.Text(string.Format("Thanks {0}. How can I help you today?", userProfile.Name)), cancellationToken);

                    // Reset the flag to allow the bot to go though the cycle again
                    conversationData.PromptedUserForName = false;

                }
                else
                {

                }
            }
        }

    }
}
