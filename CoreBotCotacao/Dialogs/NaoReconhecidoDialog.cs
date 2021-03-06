// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.3.0

using System;
using System.Threading;
using System.Threading.Tasks;
using CoreBotCotacao.Models;
using CoreBotCotacao.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBotCotacao.Dialogs
{
    public class NaoReconhecidoDialog : CancelAndHelpDialog
    {
        public NaoReconhecidoDialog()
            : base(nameof(NaoReconhecidoDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                NaoReconhecidoStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> NaoReconhecidoStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mensagem = "Me desculpe, como eu ainda sou muito jovem, n�o consegui lhe compreender, poderias tentar de uma maneira diferente?";
           
            return await stepContext.NextAsync(mensagem, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result != null)
            {
                return await stepContext.EndDialogAsync(stepContext.Result.ToString(), cancellationToken);
            }
            else
            {
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }
    }
}
