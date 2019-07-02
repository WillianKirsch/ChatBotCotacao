// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.3.0

using System;
using System.Threading;
using System.Threading.Tasks;
using CoreBotCotacao.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBotCotacao.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        protected readonly IConfiguration Configuration;
        protected readonly ILogger Logger;

        public MainDialog(IConfiguration configuration, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            Configuration = configuration;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new SaudacaoDialog());
            AddDialog(new SobreDialog());
            AddDialog(new CotacaoDialog());
            AddDialog(new CotacaoListagemDialog());
            AddDialog(new NaoReconhecidoDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // O diálogo filho inicial a ser executado.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Configuration["LuisAppId"]) || string.IsNullOrEmpty(Configuration["LuisAPIKey"]) || string.IsNullOrEmpty(Configuration["LuisAPIHostName"]))
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("ATENÇÃO: O LUIS não está configurado. Para ativa-lo, adicione 'LuisAppId', 'LuisAPIKey' e 'LuisAPIHostName' no arquivo appsettings.json."), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(stepContext, cancellationToken);
                //return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Como posso ajudá-lo?\nMe pergunte algo como \"Qual a cotação do dólar hoje\"") }, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Chama o LUIS e reúne todas as possíveis intenções. (Observe que o TurnContext tem a resposta para o prompt).
            var LUISResponse = stepContext.Result != null
                    ? await LuisHelper.ExecuteLuisQuery(Configuration, Logger, stepContext.Context, cancellationToken)
                    : new LUISResponse();

            if (LUISResponse.Intencao == Intencao.conversa_saudacao)
                return await stepContext.BeginDialogAsync(nameof(SaudacaoDialog), cancellationToken);
            else if (LUISResponse.Intencao == Intencao.conversa_sobre)
                return await stepContext.BeginDialogAsync(nameof(SobreDialog), cancellationToken);
            else if (LUISResponse.Intencao == Intencao.moeda_cotacao)
                return await stepContext.BeginDialogAsync(nameof(CotacaoDialog), LUISResponse, cancellationToken);
            else if (LUISResponse.Intencao == Intencao.moeda_listagem)
                return await stepContext.BeginDialogAsync(nameof(CotacaoListagemDialog), LUISResponse, cancellationToken);
            
            return await stepContext.BeginDialogAsync(nameof(NaoReconhecidoDialog), cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled or the user failed to confirm, the Result here will be null.
            if (stepContext.Result != null)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(stepContext.Result.ToString()), cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Obrigado."), cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
