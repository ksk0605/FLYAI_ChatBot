using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;

using System.Collections.Generic;
using GreatWall.Dialogs;

namespace GreatWall
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string WelcomeMessage = "안녕하세요 만리장석 봇입니다. 1.주문 2.FAQ 중에 선택하세요";

        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(WelcomeMessage);

            var message = context.MakeMessage();

            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1.주문", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2.FAQ", Value = "2", Type = ActionTypes.ImBack });


            message.Attachments.Add(
                new HeroCard
                {
                    Title = "원하는 기능을 선택하세요",
                    Buttons = actions
                }.ToAttachment()
            );

            await context.PostAsync(message);

            context.Wait(SendWelcomeMessageAsync);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string selected = activity.Text.Trim();

            if (selected == "1")
            {
                await context.PostAsync("음식 주문 메뉴 입니다. 원하시는 음식을 입력해 주십시오.");
                context.Call(new OrderDialog(), DialogResumeAfter);
            }
            else if (selected == "2")
            {
                await context.PostAsync("FAQ 서비스 입니다. 질문을 입력해 주십시오.");
                context.Call(new FAQDialog(), DialogResumeAfter);

            }
            else
            {
                await context.PostAsync("잘못 선택하셨습니다. 다시 선택해 주십시오");
                context.Wait(SendWelcomeMessageAsync);
            }

        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string message = await result;

                await this.MessageReceivedAsync(context, result);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("오류가 생겼습니다. 죄송합니다.");
            }
        }


        /*public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            var activity = await argument as Activity;

            string message = activity.Text;

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }*/
        /*public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            *//*var message = await argument;

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                context.Wait(MessageReceivedAsync);
            }*//*
            var activity = await argument as Activity;

            string message = string.Format("당신의 {0} 주문 감사하다 :)", activity.Text);

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }*/

    }
}