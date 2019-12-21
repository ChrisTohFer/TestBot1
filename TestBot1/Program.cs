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
        const string                BOT_TOKEN   = "NjU1NTI2MTE3Nzc1NzY5NjMx.Xf4U3g.dRpB4IIWop5UWHcsTICm1puYy1M";
        const ulong                 CLIENT_ID   = 655526117775769631;
        
        DiscordSocketClient         dsclient;
        CommandService              commands;

        //
        static void Main(string[] args)
            => new Program().DoBotStuffAsync().GetAwaiter().GetResult();

        //Asyncrhonous main method
        async Task DoBotStuffAsync()
        {
            //Initialize client and command service objects
            dsclient = new DiscordSocketClient();
            commands = new CommandService();

            var handler = new CommandHandler(dsclient, commands);
            await handler.InstallCommandsAsync();

            //Login
            await dsclient.LoginAsync(TokenType.Bot, BOT_TOKEN);
            await dsclient.StartAsync();

            //Require user input for program to close
            Console.Read();
        }
    }

    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        //Construction
        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
        }

        //
        public async Task InstallCommandsAsync()
        {
            _client.Log += Log;
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        //Logging function for asynchonous use
        Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        //
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            //Return if system message or bot message
            var message = messageParam as SocketUserMessage;
            if (message == null || message.Author.IsBot)
                return;

            //Record message in log
            Console.WriteLine("Message received:\t" + messageParam.Content);

            //Check if command and save argpos
            var argpos = 0;
            if (!message.HasCharPrefix('!', ref argpos))
                return;

            //Create command context based on message
            var context = new SocketCommandContext(_client, message);

            //Execute command
            var result = await _commands.ExecuteAsync(context, argpos, null);

            if (!result.IsSuccess)
                Console.WriteLine(result.ErrorReason);
        }
    }
}
