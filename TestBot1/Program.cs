using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace TestBot1
{
    class Program
    {
        //
        const string                BOT_TOKEN   = "NjU1NTI2MTE3Nzc1NzY5NjMx.XfoiQA.zdvLruvct3LP6zZDU0WnXI_F6uI";
        const ulong                 CLIENT_ID   = 655526117775769631;
        
        DiscordSocketClient         dsclient;
        CommandService              commands;
        IServiceProvider            serviceProvider;

        //
        static void Main(string[] args)
            => new Program().DoBotStuffAsync().GetAwaiter().GetResult();

        //Asyncrhonous main method
        async Task DoBotStuffAsync()
        {
            //Initialize client and command service objects
            dsclient = new DiscordSocketClient();
            commands = new CommandService();

            serviceProvider = new ServiceCollection()
                .AddSingleton(dsclient)
                .AddSingleton(commands)
                .BuildServiceProvider();

            //Register commands
            dsclient.Log += Log;
            dsclient.LoggedIn += OnLogin;
            dsclient.LoggedOut += OnLogout;
            dsclient.MessageReceived += OnMessageRecieved;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);

            //Login
            await dsclient.LoginAsync(TokenType.Bot, BOT_TOKEN);
            await dsclient.StartAsync();

            //Require user input for program to close
            Console.Read();
        }

        //Logging function for asynchonous use
        Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        async Task OnLogin()
        {

        }
        async Task OnLogout()
        {

        }
        async Task OnMessageRecieved(SocketMessage arg)
        {
            //Ensure that nothing in this method would cause recursion
            Console.WriteLine(arg.ToString());

            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot)
                return;

            int argpos = 0;
            if (!message.HasCharPrefix('!', ref argpos))
                return;

            var context = new SocketCommandContext(dsclient, message);

            var result = await commands.ExecuteAsync(context, argpos, serviceProvider);

            if(!result.IsSuccess)
            {
                Console.WriteLine(argpos);
                Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
