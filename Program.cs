using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    class Program
    {
        static TelegramBotClient bot = new TelegramBotClient("1265825402:AAFcRe8t6nYNHJrk9x-h-hcplyCQF5l26eo");
        //System.Reflection.MethodBase.GetCurrentMethod().DeclaringType gives you the class where it was call but may be really slow,
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog log = LogHelper.GetLoggger();
        static void Main(string[] args)
        {
            log.Info("Bot Started");
            bot.StartReceiving();
            bot.OnMessage += Bot_OnMessage;
            Console.ReadLine();
        }

        static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if(e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (e.Message.Text.Contains("Hola") || e.Message.Text.Contains("hola"))
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Adios " + e.Message.Chat.Username);
                    log.Info("Bot said bye");

                }
                else if(e.Message.Text.Contains("/Siu"))
                {
                    log.Warn("SIU doesnt have a profile picture");
                    bot.SendContactAsync(e.Message.Chat.Id, "+504 *5000", "Pizza Hut");
                }
                else
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Usage of commands: \n/Hola \n/Siu");
                    log.Warn("Bot needed to print usage");


                }
            }
            
        }
    }
}
