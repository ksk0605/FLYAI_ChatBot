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
        string WelcomeMessage = "�ȳ��ϼ��� �����弮 ���Դϴ�. 1.�ֹ� 2.FAQ �߿� �����ϼ���";

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

            actions.Add(new CardAction() { Title = "1.�ֹ�", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2.FAQ", Value = "2", Type = ActionTypes.ImBack });


            message.Attachments.Add(
                new HeroCard
                {
                    Title = "���ϴ� ����� �����ϼ���",
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
                await context.PostAsync("���� �ֹ� �޴� �Դϴ�. ���Ͻô� ������ �Է��� �ֽʽÿ�.");
                context.Call(new OrderDialog(), DialogResumeAfter);
            }
            else if (selected == "2")
            {
                await context.PostAsync("FAQ ���� �Դϴ�. ������ �Է��� �ֽʽÿ�.");
                context.Call(new FAQDialog(), DialogResumeAfter);

            }
            else
            {
                await context.PostAsync("�߸� �����ϼ̽��ϴ�. �ٽ� ������ �ֽʽÿ�");
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
                await context.PostAsync("������ ������ϴ�. �˼��մϴ�.");
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

            string message = string.Format("����� {0} �ֹ� �����ϴ� :)", activity.Text);

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }*/

    }
}