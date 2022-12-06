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
        return;
    }
    if (messageText == "Проверка")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Проверка бота: работа корректна",
        cancellationToken: cancellationToken);

    }
    if (messageText == "Привет")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Привет",
        cancellationToken: cancellationToken);

    }
    if (messageText == "Пока")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Пока",
        cancellationToken: cancellationToken);

    }
    if (messageText == "как дела?")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "нормально",
        cancellationToken: cancellationToken);

    }
    if (messageText == "что делаешь?")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "что-то",
        cancellationToken: cancellationToken);

    }
    if (messageText == "как настроение?")
    {

        await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "отличное",
        cancellationToken: cancellationToken);


    }

    if (messageText == "Фотка")
    {

        Message messag = await botClient.SendPhotoAsync(
chatId: chatId,
photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
parseMode: ParseMode.Html,
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

