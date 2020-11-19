using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

/*
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
*/

namespace TelegramBot
{
    class Program
    {
        static ITelegramBotClient botClient;
        static double contadorSI = 0;
        static double contadorNO = 0;
        private static readonly log4net.ILog log = LogHelper.GetLoggger();
        static void Main()
        {

            //Token del bot de la clase: 1405453733:AAHu9DKWnQCIymcrRKwiObzwrdbFfNDl0do
            botClient = new TelegramBotClient("1073045363:AAGj36rMsGOP_DfyxMUC-ihNeEjCaA_ra3M");

            var me = botClient.GetMeAsync().Result;
            Console.Title = me.Username;
            Console.WriteLine(
              $"botClient>> Hola! Me llamo {me.FirstName}."
            );
            log.Info("Bot Started");
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnCallbackQuery += BotOnCallbackQueryRecieved;
            botClient.OnReceiveError += BotOnReceiveError;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        static void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            Console.WriteLine($"botClient>> Error recibido: " + e.ApiRequestException.Message);
        }

        public static async void BotOnCallbackQueryRecieved(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            Console.WriteLine($"botClient:>> @{callbackQuery.Message.Chat.Username} seleccionó {callbackQuery.Data}");
            log.Info($"User response {callbackQuery.Data}");
            

            //CONDICIONALES DEL MENU INICIAL
            if (callbackQuery.Data == "Circulacion")
            {

                DiaCirculacionAsync(callbackQuery);

            }
            else if (callbackQuery.Data == "AutoEvaluate")
            {

                CuestionarioSintomas(callbackQuery);

            }
            else if (callbackQuery.Data == "/help")
            {

                await botClient.SendTextMessageAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  text: "Comandos:\n" +
                      "/start - ejecuta los comandos COVID 19\n" +
                      "/circulacion - muestra los dias de circulación\n" +
                      "/stats - muestra las estadisticas de COVID 19\n" +
                      "/evaluate - muestra una serie de preguntas sobre los sintomas que padeces\n" +
                      "/recomendaciones - muestra una serie de recomendaciones para prevenir el COVID 19\n"
                );

            }
            else if (callbackQuery.Data == "prevenir")
            {

                await botClient.SendTextMessageAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  text: "Consejos para prevenir COVID-19\n" +
                        "1. Utiliza constantemente alcohol en gel\n" +
                        "2. Toma abundante agua y cuida tu alimentación para que mantengas tu sistema inmunologico fortalecido\n" +
                        "3. Si tienes algún sintoma busca un medio y comunicate con tu supervisor\n" +
                        "4. No saludes de mano o beso a las personas\n" +
                        "5. Lávate las manos frecuentemente con agua y jabón\n" +
                        "6. Limpia y desinfecta las superficies y objetos de uso común\n" +
                        "7. Evita tocar tus ojos, nariz y boca sin haberte lavado las manos\n" +
                        "8. Cubre tu nariz y boca con el antebrazo o con un pañuelo desechable al estornudar o toser"
                );

            }
            else
            {

                await botClient.AnswerCallbackQueryAsync(
                  callbackQueryId: callbackQuery.Id,
                  text: ""
                );

            }

            //PERDON INGE POR SER LA VERGA HACIENDO CODIGO!! FIJO ESTA MIERDA NO ES MANTENIBLE
            if (callbackQuery.Data == "si1")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si2")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si3")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si4")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si5")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si6")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si7")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si8")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si9")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si10")
            {
                contadorSI++;
            }
            else if (callbackQuery.Data == "si11")
            {
                contadorSI++;
            }

            if (callbackQuery.Data == "no1")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no2")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no3")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no4")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no5")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no6")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no7")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no8")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no9")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no10")
            {
                contadorNO++;
            }
            else if (callbackQuery.Data == "no11")
            {
                contadorNO++;
            }

            // CONDIICONALES DEL DOCTOR COVID
            if (callbackQuery.Data == "bien" || callbackQuery.Data == "mal")
            {
                SegundaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si1" || callbackQuery.Data == "no1")
            {

                TerceraPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si2" || callbackQuery.Data == "no2")
            {

                CuartaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si3" || callbackQuery.Data == "no3")
            {

                QuintaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si4" || callbackQuery.Data == "no4")
            {

                SextaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si5" || callbackQuery.Data == "no5")
            {

                SeptimaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si6" || callbackQuery.Data == "no6")
            {

                OctavaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si7" || callbackQuery.Data == "no7")
            {

                NovenaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si8" || callbackQuery.Data == "no8")
            {

                DecimaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si9" || callbackQuery.Data == "no9")
            {

                OnceavaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si10" || callbackQuery.Data == "no10")
            {

                DoceavaPregunta(callbackQuery);
            }
            else if (callbackQuery.Data == "si11" || callbackQuery.Data == "no11")
            {

                calcularRiesgo(callbackQuery);
                contadorSI = 0;
                contadorNO = 0;

            }

        }

        static async void DiaCirculacionAsync(CallbackQuery callbackQuery)
        {

            await botClient.SendTextMessageAsync(
              chatId: callbackQuery.Message.Chat.Id,
              text: "¡¡¡Aqui va la informacion de los dias de circulación!!!"
            );

        }

        static async void CuestionarioSintomas(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

        new[]{

          InlineKeyboardButton.WithCallbackData(
            text:"Bien",
            callbackData: "bien"
          ),
          InlineKeyboardButton.WithCallbackData(
            text:"Mal",
            callbackData:"mal"
          )
        }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Como te sientes hoy?", replyMarkup: respuestas);
        }

        static async void SegundaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si1"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no1"
         )

       }

     });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Has viajado en los ultimos 14 dias fuera del país/estado?", replyMarkup: respuestas);
            //TerceraPregunta(callbackQuery);
        }

        static async void TerceraPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si2"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no2"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Has tenido contacto directo con una persona diagnosticada con COVID-19?", replyMarkup: respuestas);
            //CuartaPregunta(callbackQuery);
        }

        static async void CuartaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si3"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no3"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes fiebre mayor a 37.5 grados?", replyMarkup: respuestas);
            //QuintaPregunta(callbackQuery);
        }

        static async void QuintaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si4"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no4"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Te duele la garganta?", replyMarkup: respuestas);
            //SextaPregunta(callbackQuery);
        }

        static async void SextaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si5"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no5"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes tos seca y persistente?", replyMarkup: respuestas);
            //SeptimaPregunta(callbackQuery);
        }

        static async void SeptimaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si6"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no6"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Te cuest trabajo respirar?", replyMarkup: respuestas);
            //OctavaPregunta(callbackQuery);
        }

        static async void OctavaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si7"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no7"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes dolor muscular, de cabeza, y/o de articulaciones?", replyMarkup: respuestas);
            //NovenaPregunta(callbackQuery);
        }

        static async void NovenaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si8"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no8"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes perdida de sentido del gusto u olfato?", replyMarkup: respuestas);
            //DecimaPregunta(callbackQuery);
        }

        static async void DecimaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si9"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no9"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes diarrea, nausea o vomito?", replyMarkup: respuestas);
            //OnceavaPregunta(callbackQuery);
        }

        static async void OnceavaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si10"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no10"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Te has hecho la prueba de COVID-19 \n (PCR, IgG, IgM)?", replyMarkup: respuestas);
            //DoceavaPregunta(callbackQuery);
        }

        static async void DoceavaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí",
           callbackData: "si11"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No",
            callbackData: "no11"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Usted se encuentra en alguna de las siguientes condiciones?\n" +
                    "* Mayor a 60 años\n" +
                    "* Enfermedades cardiovasculares\n" +
                    "* Hipertensión arterial\n" +
                    "* Diabetes\n" +
                    "* Enfermedades respiratorias (pulmonar, cronica, asma)\n" +
                    "* Insuficiencia renal cronica\n" +
                    "* Cancer\n" +
                    "* Obesidad\n" +
                    "* Enfermedad o tratamiento immunosupresor", replyMarkup: respuestas
            );
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {

                Console.WriteLine($"botClient:>> Received a text message from @{e.Message.Chat.Username}:" + e.Message.Text);

                if (e.Message.Text == "/start")
                {

                    var BotonesHYD = new InlineKeyboardMarkup(new[]{
              new []{
                InlineKeyboardButton.WithCallbackData(
                  text:"Días de Circulación HN\U0001F699",
                  callbackData: "Circulacion"),
                  InlineKeyboardButton.WithCallbackData(
                    text:"Doctor COVID-19 \U0001F9D1",
                    callbackData: "AutoEvaluate")
              },
              new []{
                InlineKeyboardButton.WithUrl(
                  text:"Estadísticas \U0001F4C8",
                  url: "https://www.google.com/search?q=coronavirus+statistics&oq=coronavirus+st&aqs=chrome.0.0i67j69i57j0l6.6211j0j4&sourceid=chrome&ie=UTF-8"),
                InlineKeyboardButton.WithCallbackData(
                  text:"Prevenir COVID-19\U0001F637",
                  callbackData: "prevenir")
              },
              new[]{
                InlineKeyboardButton.WithCallbackData(
                  text:"Ayuda \U0001F6A8",
                  callbackData:"/help")
              }
          });

                    await botClient.SendPhotoAsync(
                      chatId: e.Message.Chat,
                      photo: "https://image.freepik.com/vector-gratis/coronavirus-covid-19-luchadores-02_126288-23.jpg",
                      caption: ""
                    );
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Bienvenid@ al BOT COVID19HN \n Selecciona el comando a ejecutar", replyMarkup: BotonesHYD);
                }

            }
        }

        static async void calcularRiesgo(CallbackQuery callbackQuery)
        {

            double positivo = (contadorSI / 12) * 100;
            positivo = Math.Round(positivo, 2);
            double negativo = (contadorNO / 12) * 100;
            negativo = Math.Round(negativo, 2);

            // Console.WriteLine($"Cantidad de si {contadorSI}");
            // Console.WriteLine($"Cantidad de no {contadorNO}");
            // Console.WriteLine($"Cantidad de positivo {positivo}");
            // Console.WriteLine($"Cantidad de negativo {negativo}");

            //Ponderacion del test
            if (positivo >= negativo)
            {

                Console.WriteLine($"Cantidad de positivo: {positivo}");

                await botClient.SendPhotoAsync(
                  chatId: callbackQuery.Message.Chat,
                  photo: "https://hhp-blog.s3.amazonaws.com/2020/08/GettyImages-1216575896.jpg",
                  caption: $"Tienes un {positivo}% de probabilidad que estes contagiado. Por favor atiende a tu medico mas cercano."
                );

            }
            else
            {

                Console.WriteLine($"Cantidad de negativo: {negativo}");

                await botClient.SendPhotoAsync(
                  chatId: callbackQuery.Message.Chat,
                  photo: "https://e00-expansion.uecdn.es/assets/multimedia/imagenes/2020/06/15/15922327273752.jpg",
                  caption: $"Tienes un {negativo}% de que no estes contagiado. Sigue cuidandote!"
                );

            }

            contadorNO = 0;
            contadorSI = 0;

        }

    }
}