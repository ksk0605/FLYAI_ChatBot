using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using QnAMakerDialog.Models;
using QnAMakerDialog;


namespace GreatWall.Dialogs
{
    [Serializable]
    [QnAMakerService("https://labuser4qnamakerapp.azurewebsites.net/qnamaker", "b1f0df1b-6bc1-40d3-a193-5c3f38159e05", "7f7d2b75-f194-446b-afeb-f47bdd3f8500", MaxAnswers = 5)]
    public class FAQDialog : QnAMakerDialog<string>
    {
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'.");
            context.Wait(MessageReceived);
        }

        public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            if (originalQueryText == "그만")
            {
                context.Done("");
                return;
            }

            await context.PostAsync(result.Answers.First().Answer);

            context.Wait(MessageReceived);
        }

        [QnAMakerResponseHandler(0.5)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help...{result.Answers.First().Answer}.";
            await context.PostAsync(messageActivity);

            context.Wait(MessageReceived);
        }
    }
}