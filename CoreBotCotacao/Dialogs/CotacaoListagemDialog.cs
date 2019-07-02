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
    public class CotacaoListagemDialog : CancelAndHelpDialog
    {
        public CotacaoListagemDialog()
            : base(nameof(CotacaoListagemDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                CotacaoListagemStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> CotacaoListagemStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string mensagem = string.Empty;
            var servico = new CotacaoMoeda();
            try
            {
                var cotacoesSolicitadas = await servico.Listagem();
                mensagem = cotacoesSolicitadas.Count > 1 ? "Cotações" : "Cotação";
                mensagem += ":\n\n";

                foreach (var cotacao in cotacoesSolicitadas)
                {
                    mensagem += $"**{cotacao.name} ({cotacao.code})**:  \n" +
                        $"Valor de Compra R$ {cotacao.bid} \n" +
                        $"Valor de Venda R$ {cotacao.ask} \n" +
                        $"Variação R$ {cotacao.varBid} \n" +
                        $"Porcentagem da Variação R$ {cotacao.pctChange} \n" +
                        $"Máximo R$ {cotacao.high} \n" +
                        $"Mínimo R$ {cotacao.low} \n";
                }
            }
            catch (Exception ex)
            {
                mensagem = "Desculpe, não consegui buscar essa informação no momento. Se importa de tentar novamente?";
            }

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
