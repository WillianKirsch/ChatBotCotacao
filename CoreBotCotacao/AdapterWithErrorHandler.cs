// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.3.0

using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

namespace CoreBotCotacao
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        public AdapterWithErrorHandler(ICredentialProvider credentialProvider, ILogger<BotFrameworkHttpAdapter> logger, ConversationState conversationState = null)
            : base(credentialProvider)
        {
            OnTurnError = async (turnContext, exception) =>
            {
                // Registre qualquer exceção perdida do aplicativo.
                logger.LogError($"Exceção capturada: {exception.Message}");

                // Envie um pedido de desculpas para o usuário.
                await turnContext.SendActivityAsync("Desculpe, parece que algo deu errado.");

                if (conversationState != null)
                {
                    try
                    {
                        // Exclua o conversationState da conversa atual para impedir que o
                        // bot de ficar preso em um loop de erro causado por estar em um estado ruim.
                        // ConversationState deve ser considerado como semelhante a "cookie-state" em páginas da Web.
                        await conversationState.DeleteAsync(turnContext);
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Exceção detectada ao tentar excluir o ConversationState: {e.Message}");
                    }
                }
            };
        }
    }
}
