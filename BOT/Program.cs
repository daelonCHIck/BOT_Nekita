using BOT.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

Dictionary<long, BOT.Models.User> Users = new Dictionary<long, BOT.Models.User>();
Dictionary<long, string> people = new Dictionary<long, string>();



var botClient = new TelegramBotClient("5842355288:AAEZImZBbhdPxUXWcebfNP6zTkSeCM7aGO8");
using CancellationTokenSource cts = new();
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};
botClient.StartReceiving(
updateHandler: HandleUpdateAsync,
pollingErrorHandler: HandlePollingErrorAsync,
receiverOptions: receiverOptions,
cancellationToken: cts.Token
);
var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();



async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{


    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;
    var chatId = message.Chat.Id;
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (people.ContainsKey(chatId))
    {
        string command = people[chatId];
        if (command == "Регистрация1")
        {
            people[chatId] = "Регистрация2";
            await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Введите ваш город",
        cancellationToken: cancellationToken);
            Users[chatId].Age = int.Parse(messageText);
        }

        if (command == "Регистрация2")
        {
            people[chatId] = "Регистрация3";
            await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Введите вашу страну",
        cancellationToken: cancellationToken);
            Users[chatId].City = messageText;
        }

        if (command == "Регистрация3")
        {
            people.Remove(chatId);
            
            Users[chatId].Country = messageText;
            
                 await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: $"Спасибо за регистрацию! Вот ваши данные:\n{Users[chatId].Age}, {Users[chatId].Country}, {Users[chatId].City}",
        cancellationToken: cancellationToken);
            BotABWContext context = new BotABWContext();
            context.Add(Users[chatId]);
            context.SaveChanges();

        }

        return;
    }
    if (messageText == "Проверка")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Проверка бота: ошибок не найдено.",
        cancellationToken: cancellationToken);

    }
    

    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
        new KeyboardButton[] { "Регистрация", "FAQ" },
    })
    {
        ResizeKeyboard = true
    };

    //Message sentMessage = await botClient.SendTextMessageAsync(
    //    chatId: chatId,
    //    text: "Choose a response",
    //    replyMarkup: replyKeyboardMarkup,
    //    cancellationToken: cancellationToken);

    if (messageText == "Регистрация")
    {
        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Введите ваш возраст",
        cancellationToken: cancellationToken);
        people.Add(chatId, "Регистрация1");

        Users.Add(chatId, new BOT.Models.User());
    }
   
    if (messageText == "FAQ")
    {
        await botClient.SendTextMwssageAsync(
        chatId: chatId,
        text: "Тут должен быть FAQ, который автор еще не составил.",
        cancellationToken: cancellationToken);
    }
}
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

