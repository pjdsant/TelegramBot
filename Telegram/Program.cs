using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using System.Threading;
using CSharp_LogErros;
using MyProg;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text.RegularExpressions;
using ADHelperLib;
using static Telegram.EnumStateMachine;
using Telegram;

namespace TelegramEcho



{
    class Program
    {
        static ITelegramBotClient botClient;
        static int interactionCount = 0;
        static State stateMachine = State.Initial;
        static Step step = Step.None;
        static string userMain;
        static string respAttributeOne;
        static string respAttributeTwo;

        static string domain = AppSettings.Domain;
        static string container = AppSettings.Container;
        static string admin = AppSettings.Admin;
        static string adminPassword = AppSettings.AdminPassword;
        static string attributeOne = AppSettings.AttributeOne;
        static string attributeTwo = AppSettings.AttributeTwo;

        static async Task Main(string[] args)
        {
            try
            {
                LdapConnection connection = new LdapConnection("domain.com");
                NetworkCredential credential = new NetworkCredential("admin", "password");
                connection.Credential = credential;
                connection.Bind();
                Console.WriteLine("logged in active directory -- ");
            }
            catch (LdapException lexc)
            {
                Console.WriteLine(lexc);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }




            var MyIni = new IniFile();
            var TeleKey = MyIni.Read("TeleKey");

            //botClient = new TelegramBotClient("key"); //key original
            try
            {
                botClient = new TelegramBotClient(TeleKey); //key no arquivo ini

                GravaLog gravaLog = new GravaLog();


                //var MyIni = new IniFile(@"C:\temp\Settings.ini");
                //MyIni.Write("DefaultVolume", "100");                  //Escreve Arquino INI
                // MyIni.Write("HomePage", "http://www.google.com");    //Escreve Arquino INI
                //var DefaultVolume = MyIni.Read("DefaultVolume");      //le arquivos INI
                //var HomePage = MyIni.Read("HomePage");                //le arquivos INI

                var me = botClient.GetMeAsync().Result;
                Console.Title = me.Username;
                Console.WriteLine($"\n Olá, eu sou o {me.Username} meu ID é {me.Id} e meu nome é {me.FirstName}.");
                Console.WriteLine($"\n Criado log - Telegram.log.");
                Console.WriteLine($"\n Lendo INI - Telegram.ini.");

                gravaLog.grava("Serviço iniciado (" + me.Username + ") - meu ID é " + me.Id + " - e meu nome é (" + me.FirstName + me.LastName + "). Usuario " + System.Environment.UserName);
                gravaLog.grava("Criado Telegram.log");
                gravaLog.grava("Lendo Telegram.ini");


                botClient.OnMessage += Bot_OnMessage;
                botClient.StartReceiving();
                Console.WriteLine("\n\n Programa em execução..... \n\n");
                Console.WriteLine("\n\n Digite qualquer tecla para sair. \n\n");
                Console.ReadKey();


                botClient.StopReceiving();
                gravaLog.grava(" Saindo - " + System.Environment.UserName + "--------------------------------------------");

            }
            catch (Exception e)
            {
                GravaLog gravaLog = new GravaLog();
                Console.WriteLine("Erro!!!");
                Console.WriteLine(e.Message);
                gravaLog.grava("ERROO!!!!");
                gravaLog.grava(e.Message);
                Console.ReadKey();

            }
        }


        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var MyIni = new IniFile();
            GravaLog gravaLog = new GravaLog();

            Regex rx;

            if (stateMachine == State.Initial)
            {
                if ((e.Message.From.Id == 1118826046) || (e.Message.From.Id == 1392841920))
                {
                    var blacklist1 = MyIni.Read("BlackNumber1", "BlackList");                //le arquivos INI
                    var blacklist2 = MyIni.Read("BlackNumber2", "BlackList");                //le arquivos INI
                    Console.WriteLine("Numero em BlackLit - " + blacklist1);
                    gravaLog.grava("Numero em BlackLit - " + blacklist1);
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: " Telefone em Blacklist - Desculpe");

                }
                else if (e.Message.Text.Length >= 1)
                {
                    // Verifica opção Telefones
                    rx = new Regex(@"\b(Telefon(e|es)|1)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Qual telefone esta precisando?\n "
                                                                                            +
                                                                                                "---------------------------------------\n" +
                                                                                                "4Help\n" +
                                                                                                "Produção\n" +
                                                                                                "Recepção\n" +
                                                                                                "RH\n" +
                                                                                                "---------------------------------------\n"
                                                                                                );
                    }

                    // Verifica opção Endereços
                    rx = new Regex(@"\b(Endere[ç|c](o|os)|2)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");
                        await botClient.SendTextMessageAsync(
                          chatId: e.Message.Chat,
                              text: "Qual campus precisa do endereço?\n" +
                              "Lapa\n" +
                              "SGA\n" +
                              "Morumbi\n" +
                              "Higienopolis\n"
                              );
                    }

                    // Verifica opção Produção
                    rx = new Regex(@"\b(Produ[ç|c][ã|a]o)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        var Contato1 = MyIni.Read("Contato1");
                        var TelefoneContato1 = MyIni.Read("TelefoneContato1");
                        var LastNameContato1 = MyIni.Read("LastNameContato1");

                        await botClient.SendContactAsync(
                        chatId: e.Message.Chat.Id,
                        phoneNumber: TelefoneContato1, 
                        firstName: Contato1,
                        lastName: LastNameContato1);
                    }

                    // Verifica opção Recepção
                    rx = new Regex(@"\b(Recep[ç|c][ã|a]o)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendContactAsync(
                        chatId: e.Message.Chat.Id,
                        phoneNumber: "+55 11 999999999",
                        firstName: "Recepção",
                        lastName: "Empresa");
                    }


                    // Verifica opção RH
                    rx = new Regex(@"\b(Rh)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendContactAsync(
                        chatId: e.Message.Chat.Id,
                        phoneNumber: "+55 11 999999999",
                        firstName: "RH",
                        lastName: "Empresa");
                    }


                    // Verifica opção 4Help
                    rx = new Regex(@"\b((4|4 )Help)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendContactAsync(
                        chatId: e.Message.Chat.Id,
                        phoneNumber: "+55 11 9999999999",
                        firstName: "4Help - ServiceDesk",
                        lastName: "Empresa");
                    }

                    // Verifica opção Lapa
                    rx = new Regex(@"\b(Lapa)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendVenueAsync(
                         chatId: e.Message.Chat.Id,
                        latitude: -23.5126548f,
                        longitude: -46.7117332f,
                        title: "Empresa Lapa",
                        address: "Rua Lapa\n");
                    }

                    // Verifica opção SGA
                    rx = new Regex(@"\b(SGA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendVenueAsync(
                         chatId: e.Message.Chat.Id,
                        latitude: -5.7761384f,
                        longitude: -35.2548219f,
                        title: "Empresa SGA",
                        address: "Rua SGA\n");
                    }

                    // Verifica opção Morumbi
                    rx = new Regex(@"\b(Moru[m|n]bi)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendVenueAsync(
                         chatId: e.Message.Chat.Id,
                        latitude: -23.6194861f,
                        longitude: -46.7685963f,
                        title: "Empresa Morumbi",
                        address: "Rua Morumbi\n");
                    }

                    // Verifica opção Higienopolis
                    rx = new Regex(@"\b(Higien[ó|o]polis)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendVenueAsync(
                         chatId: e.Message.Chat.Id,
                        latitude: -23.5472037f,
                        longitude: -46.6811513f,
                        title: "Empresa Higienópolis",
                        address: "Rua Higienópolis\n");
                    }

                    // Verifica opção Desbloqueios
                    rx = new Regex(@"\b(Desbloquei(o|os)|3)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        stateMachine = State.Unlock;
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Infome seu login de rede:\n ");
                        userMain = "";
                        Thread.Sleep(1000);
                    }

                    // Verifica opção Muda Senha
                    rx = new Regex(@"\b((Mudar| )|( |Senha)|4)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        stateMachine = State.ChangePassword;
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Infome seu login de rede:\n ");
                        step = Step.User;
                        userMain = "";
                        Thread.Sleep(1000);
                    }

                    // Verifica opção Sair
                    rx = new Regex(@"\b(Sair|0)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (rx.Matches(e.Message.Text).Count >= 1)
                    {
                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Obrigado!\n ");
                        Thread.Sleep(1000);

                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Tchau!\n ");
                        interactionCount = 0;

                    }
                    else if (interactionCount == 0)
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Olá, tudo bem? \n");

                        Thread.Sleep(1000);
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Digite em palavras, o que posso lhe ajudar?\n\n"
                        +
                            "---------------------------------------\n" +
                            "1 - Telefones\n" +
                            "2 - Endereços\n" +
                            "3 - Desbloqueio\n" +
                            "4 - Mudar Senha\n" +
                            "0 - Sair\n" +
                            "---------------------------------------\n"
                            );

                        Console.WriteLine($"Received a text {e.Message.Text} - {e.Message.Date.ToLocalTime()} - From {e.Message.From.Id}.");
                        gravaLog.grava(" Received a text (" + e.Message.Text + ") - " + e.Message.Date.ToLocalTime() + " - From (" + e.Message.From.Id + ").");

                        interactionCount = interactionCount + 1;

                    }

                }
            }else if (stateMachine == State.Unlock)
            {
                // Verifica opção Sair
                rx = new Regex(@"\b(Sair|0)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (rx.Matches(e.Message.Text).Count >= 1)
                {
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Tchau!\n ");
                    interactionCount = 0;
                    stateMachine = State.Initial;
                }

                if (e.Message.Text.Length >= 1 )
                {
                    userMain = e.Message.Text;

                    ADHelper adhelper = new ADHelper();

                    if (adhelper.UnlockUser(domain, container, admin, adminPassword, userMain) == "Isvalid")
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Usuário Desbloqueado!\n " +
                                                                                            "Obrigado pelo contato. \n");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Não foi possível desbloquear o usuário " + userMain + "\n" +
                                                "Entre em contato com o Help Desk, Obrigado:\n ");
                    }

                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Tchau!\n ");
                    interactionCount = 0;
                    stateMachine = State.Initial;
                    
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Informe o seu login de rede ou digite 'Sair' a qualquer momento!\n ");
                }

                //Solicitar o usuario de rede

            }
            else if (stateMachine == State.ChangePassword)
            {
                // Verifica opção Sair
                rx = new Regex(@"\b(Sair|0)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (rx.Matches(e.Message.Text).Count >= 1)
                {
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Tchau!\n ");
                    interactionCount = 0;
                    stateMachine = State.Initial;
                }
                else if(step == Step.User && e.Message.Text.Length >= 1)
                {
                    userMain = e.Message.Text;

                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Entre com sua primeira frase secreta!\n ");
                    step = Step.SecretePhraseOne;
                }
                else if (step == Step.SecretePhraseOne && e.Message.Text.Length >= 1)
                {
                    respAttributeOne = e.Message.Text;

                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Entre com sua segunda frase secreta!\n ");
                    step = Step.SecretePhraseTwo;
                }
                else if (step == Step.SecretePhraseTwo && e.Message.Text.Length >= 1)
                {
                    respAttributeTwo = e.Message.Text;

                    ADHelper adhelper = new ADHelper();

                    var valida = adhelper.ValidateExAttributes(domain, container, admin, adminPassword, userMain, attributeOne, attributeTwo, respAttributeOne, respAttributeTwo);

                    if (valida == "IsValid")
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Entre com sua nova Senha!\n ");
                        step = Step.Password;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "As respostas secretas não conferem!\n "
                                                                             +   "Entre em contato com o Help Desk, Obrigado:\n ");

                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Tchau!\n ");
                        interactionCount = 0;
                        stateMachine = State.Initial;
                    }
                }
                else if (step == Step.Password && e.Message.Text.Length >= 1)
                {
                    var userMainPassword = e.Message.Text;
                    step = Step.None;

                    ADHelper adhelper = new ADHelper();

                    if (adhelper.ChangePassword(domain,container,admin,adminPassword, userMain, userMainPassword) == "PasswordChanged")
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Senha Alterada com Sucesso!\n "
                                                                                            + "Obrigado pelo contato. \n");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Não foi possivel alterar sua senha!\n "
                                                                                     + "Entre em contato com o Help Desk, Obrigado:\n ");
                    }

                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Tchau!\n ");
                    interactionCount = 0;
                    stateMachine = State.Initial;
                }
            }
        }

    }
}

