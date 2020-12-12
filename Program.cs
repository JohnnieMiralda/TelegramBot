using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Http;
using Newtonsoft.Json;
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
        static readonly HttpClient client = new HttpClient();
        
        static void Main()
        {

            //Token del bot de la clase: 1405453733:AAHu9DKWnQCIymcrRKwiObzwrdbFfNDl0do
            botClient = new TelegramBotClient("1405453733:AAHu9DKWnQCIymcrRKwiObzwrdbFfNDl0do");

            var me = botClient.GetMeAsync().Result;
            Console.Title = me.Username;
            
            log.Info("Bot Started");
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnCallbackQuery += BotOnCallbackQueryRecieved;
            botClient.OnReceiveError += BotOnReceiveError;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
            log.Info("Bot Stopped");

        }

        static void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            log.Error($"Error recibido: " + e.ApiRequestException.Message);
        }

        static async void EstadisticaAsync(CallbackQuery callbackQuery)
        {


            var Estadisticas = new InlineKeyboardMarkup(new[]{
              new []{
                InlineKeyboardButton.WithCallbackData(
                  text:"Internacionales",
                  callbackData: "EInter"),
                  InlineKeyboardButton.WithCallbackData(
                    text:"Nacionales",
                    callbackData: "ENac")
              }
              });
            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Que desea ver?", replyMarkup: Estadisticas);
        }
        public static async void BotOnCallbackQueryRecieved(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            log.Info($"User response {callbackQuery.Data}");
            

            //CONDICIONALES DEL MENU INICIAL
            if (callbackQuery.Data == "Circulacion")
            {

                DiaCirculacionAsync(callbackQuery);
                log.Warn("Llamado a dia de circulacion");

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
                      "/evaluate - muestra una serie de preguntas sobre los síntomas que padeces\n" +
                      "/recomendaciones - muestra una serie de recomendaciones para prevenir el COVID 19\n"
                );

            }
            else if (callbackQuery.Data == "prevenir")
            {

                // Manda la imagen de prevencion de covid
              await botClient.SendPhotoAsync(
                chatId:  callbackQuery.Message.Chat,
                photo: "https://lh3.googleusercontent.com/proxy/lksd8u7-Ie9m8LSj5ck1eBHHSYqBNtylQQqaPuerMSOE2PeL0anfDrvalemgzQ4ZbnZ84ImzcWgXPN07ecnwS8cRtZIFuePe9w",
                caption: ""
              );

              // Muestra las otras opciones de prevencion
              NewMenu(callbackQuery);

            }
            if (callbackQuery.Data == "EInter")
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.covid19api.com/summary");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(responseBody);
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                        "Estadisticas Internacionales" +
                        "\nNuevos Casos Confirmados: " + obj.Global.NewConfirmed +
                        "\nTotal Casos Confirmados: " + obj.Global.TotalConfirmed +
                        "\nNuevos Muertos: " + obj.Global.NewDeaths +
                        "\nTotal Muertos: " + obj.Global.TotalDeaths +
                        "\nNuevos Recuperados: " + obj.Global.NewRecovered +
                        "\nTotal Recuperados: " + obj.Global.TotalRecovered);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    log.Error(e.Message);
                }
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "https://news.google.com/covid19/map?hl=es-419&gl=US&ceid=US%3Aes-419");
            }
            else if (callbackQuery.Data == "ENac")
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.covid19api.com/summary");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(responseBody);
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                        "Estadisticas Nacionales" +
                        "\nNuevos Casos Confirmados: " + obj.Countries[73].NewConfirmed +
                        "\nTotal Casos Confirmados: " + obj.Countries[73].TotalConfirmed +
                        "\nNuevos Muertos: " + obj.Countries[73].NewDeaths +
                        "\nTotal Muertos: " + obj.Countries[73].TotalDeaths +
                        "\nNuevos Recuperados: " + obj.Countries[73].NewRecovered +
                        "\nTotal Recuperados: " + obj.Countries[73].TotalRecovered);

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    log.Error(e.Message);
                }
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "https://covid19honduras.org/");
            }

            else if (callbackQuery.Data == "Estadisticas")
            {
                EstadisticaAsync(callbackQuery);
            }

            // Condicionales para llevar la cuenta de las respuestas de doctor covid
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


            // Condicionales para llevar la cuenta de las respuestas de doctor covid
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
            // Muestra las preguntas a medida se van respondiendo
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

            if(callbackQuery.Data == "WashHands"){
                
                await botClient.SendPhotoAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  photo: "https://scontent.fsap7-1.fna.fbcdn.net/v/t1.0-9/89233379_1442355212599590_8952188415965659136_o.jpg?_nc_cat=105&ccb=2&_nc_sid=730e14&_nc_ohc=tCPGqwB9C7AAX8qS_Bm&_nc_ht=scontent.fsap7-1.fna&oh=1b43eb029d58c6a93ef6beece3844d73&oe=5FF318DC",
                  caption: ""
                );

              }else if(callbackQuery.Data == "Contagio"){

                await botClient.SendPhotoAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  photo: "https://pbs.twimg.com/media/ETFQB13WoAAiI6S?format=jpg&name=large",
                  caption: ""
                );

              }else if(callbackQuery.Data == "hogar"){

                await botClient.SendPhotoAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  photo: "https://pbs.twimg.com/media/ETerAI5XQAYTU7E?format=jpg&name=900x900",
                  caption: ""
                );

              }else if(callbackQuery.Data == "cincoPasos"){

                  await botClient.SendPhotoAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  photo: "https://lh3.googleusercontent.com/proxy/cvrtuZCVUr2tMZ5ITPwz5t0NfIe5g2aXw8Cc93zIGkKB5vW3bukqjSg1vYvWsRKkLPP0SFMBrj8Cu22CC7xkRWozkQYKXeMHzg",
                  caption: ""
                );

              }


        }

        static async void DiaCirculacionAsync(CallbackQuery callbackQuery)
        {

            await botClient.SendTextMessageAsync(
              chatId: callbackQuery.Message.Chat.Id,
              text: "Estamos en mantenimiento intentalo mas tarde"
            );

        }

        
        static async void CuestionarioSintomas(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

        new[]{

          InlineKeyboardButton.WithCallbackData(
            text:"Bien \U0001F603",
            callbackData: "bien"
          ),
          InlineKeyboardButton.WithCallbackData(
            text:"Mal \U0001F625	",
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
           text: "Sí \U0001F44D",
           callbackData: "si1"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no1"
         )

       }

     });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Has viajado en los ultimos 14 dias fuera del país/estado?", replyMarkup: respuestas);
        }

        static async void TerceraPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si2"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no2"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Has tenido contacto directo con una persona diagnosticada con COVID-19?", replyMarkup: respuestas);
        }

        static async void CuartaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si3"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no3"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes fiebre mayor a 37.5 grados?", replyMarkup: respuestas);
        }

        static async void QuintaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si4"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no4"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Te duele la garganta?", replyMarkup: respuestas);
        }

        static async void SextaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si5"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no5"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes tos seca y persistente?", replyMarkup: respuestas);
        }

        static async void SeptimaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si6"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no6"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Te cuest trabajo respirar?", replyMarkup: respuestas);
        }

        static async void OctavaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si7"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no7"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes dolor muscular, de cabeza, y/o de articulaciones?", replyMarkup: respuestas);
        }

        static async void NovenaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si8"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no8"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes perdida de sentido del gusto u olfato?", replyMarkup: respuestas);
        }

        static async void DecimaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si9"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no9"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Tienes diarrea, nausea o vomito?", replyMarkup: respuestas);
        }

        static async void OnceavaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si10"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
            callbackData: "no10"
         )
       }

      });

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "¿Te has hecho la prueba de COVID-19 \n (PCR, IgG, IgM)?", replyMarkup: respuestas);
        }

        static async void DoceavaPregunta(CallbackQuery callbackQuery)
        {

            var respuestas = new InlineKeyboardMarkup(new[]{

       new[]{
         InlineKeyboardButton.WithCallbackData(
           text: "Sí \U0001F44D",
           callbackData: "si11"
         ),
         InlineKeyboardButton.WithCallbackData(
            text: "No \U0001F44E",
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

                log.Info($"Received a text message from @{e.Message.Chat.Username}:" + e.Message.Text);

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
                InlineKeyboardButton.WithCallbackData(
                  text:"Estadísticas \U0001F4C8",
                  callbackData: "Estadisticas"),
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
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Bienvenid@ {e.Message.Chat.Username}\n Selecciona el comando a ejecutar", replyMarkup: BotonesHYD);
                }
                else
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Bienvenid@ {e.Message.Chat.Username}, porfavor usa el comando /start");

                }
            }
        }

        static async void calcularRiesgo(CallbackQuery callbackQuery)
        {

            double positivo = (contadorSI / 12) * 100;
            positivo = Math.Round(positivo, 2);
            double negativo = (contadorNO / 12) * 100;
            negativo = Math.Round(negativo, 2);

          //Ponderacion del test
            if(positivo >= negativo){

              await botClient.SendPhotoAsync(
                chatId:  callbackQuery.Message.Chat,
                photo: "https://hhp-blog.s3.amazonaws.com/2020/08/GettyImages-1216575896-300x200.jpg",
                caption: $"Tienes un {positivo}% de que no estes contagiado. Sigue cuidandote!"
              );

            }else{

                log.Info($"Cantidad de negativo: {negativo}");
              await botClient.SendPhotoAsync(
                chatId:  callbackQuery.Message.Chat,
                photo: "https://e00-expansion.uecdn.es/assets/multimedia/imagenes/2020/06/15/15922327273752.jpg",
                caption: $"Tienes un {negativo}% de que no estes contagiado. Sigue cuidandote!"
              );
            }

          contadorNO = 0;
          contadorSI = 0;

        }

            static async void NewMenu(CallbackQuery callbackQuery){

            var respuestas = new InlineKeyboardMarkup(new[]{

              new[]{ InlineKeyboardButton.WithCallbackData(text: "¿Como lavarte las manos? \U0001F9FC \U0001F44F ", callbackData: "WashHands") },
              new[]{ InlineKeyboardButton.WithCallbackData(text: "¿Como me puedo contagiar? \U0001F912", callbackData: "Contagio") },
              new[]{ InlineKeyboardButton.WithCallbackData(text: "¿Como protego mi hogar? \U0001F3E0", callbackData: "hogar") },
              new[]{ InlineKeyboardButton.WithCallbackData(text: "Los 5 pasos para protegerse\U0001F637 ", callbackData: "cincoPasos") },

            });

          await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Prevención de COVID-19 \U0001F9A0", replyMarkup: respuestas);
          
        }

    }
}

