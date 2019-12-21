using Discord.Commands;
using System.Threading.Tasks;

namespace TestBot1.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong");
        }

        //Command to provide random number selection
        [Command("roll")]
        public async Task Roll(int sides = 6)
        {
            if (sides < 0)
            {
                await ReplyAsync("*Testbot attempts to find a dice with " + sides + " sides but gets confused* :pleading_face:");
                return;
            }

            var rand = new System.Random();

            switch (sides)
            {
                case 0:
                    await ReplyAsync("*Testbot rolls an imaginary dice, but can't see what it landed on.*");
                    return;
                case 1:
                    await ReplyAsync("*Testbot rolls a sphere on the table.*");
                    return;
                case 2:
                    string headsOrTails;
                    if (rand.Next() % 2 == 1)
                        headsOrTails = "heads";
                    else
                        headsOrTails = "tails";
                    await ReplyAsync("*Testbot flips a coin. It lands on " + headsOrTails + ".*");
                    return;
                default:
                    var number = rand.Next() % sides;
                    await ReplyAsync("*Testbot rolls a " + sides + " sided dice. It lands on "+ (number + 1) + ".*");
                    return;
            }
        }
    }
}
