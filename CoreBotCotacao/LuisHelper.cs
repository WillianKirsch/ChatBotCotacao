// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.3.0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBotCotacao.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreBotCotacao
{
    public static class LuisHelper
    {
        public static async Task<LUISResponse> ExecuteLuisQuery(IConfiguration configuration, ILogger logger, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            LUISResponse response = new LUISResponse();
            try
            {
                // Cria as configurações do LUIS a partir da configuração.
                var luisApplication = new LuisApplication(
                    configuration["LuisAppId"],
                    configuration["LuisAPIKey"],
                    "https://" + configuration["LuisAPIHostName"]
                );

                var recognizer = new LuisRecognizer(luisApplication);

                // Chama o LUIS
                var recognizerResult = await recognizer.RecognizeAsync(turnContext, cancellationToken);

                var (intent, score) = recognizerResult.GetTopScoringIntent();
                Enum.TryParse(intent, out Intencao intencao);
                response.Intencao = intencao;

                switch (intencao)
                {
                    case Intencao.conversa_saudacao:
                    break;

                    case Intencao.conversa_sobre:
                    break;

                    case Intencao.moeda_cotacao:
                        response.Entidade = recognizerResult.Entities["Moeda"]?.FirstOrDefault().ToString();
                        break;

                    case Intencao.moeda_listagem:
                    break;
                }
            }
            catch (Exception e)
            {
                logger.LogWarning($"Exceção LUIS: {e.Message} Verifique sua configuração LUIS.");
            }

            return response;
        }
    }
}
