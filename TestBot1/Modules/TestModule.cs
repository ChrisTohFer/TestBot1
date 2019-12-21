using Discord.Commands;
using System.Threading.Tasks;

namespace TestBot1.Modules
{
    class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("pingpingping");
        }
    }
}
