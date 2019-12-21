using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestBot1.Modules
{
    //Module provides functions to interact with a stored list of names
    public class GameChoiceModule : ModuleBase<SocketCommandContext>
    {
        const string FILE_NAME = "games.txt";

        [Command("addgame")]
        public Task AddGame(string gameArg)
        {
            //Populate games list using old list and argument provided
            string[] newGamesList;
            if (System.IO.File.Exists(FILE_NAME))
            {
                string[] games = System.IO.File.ReadAllLines(FILE_NAME);
                var nOfGames = games.Length;
                newGamesList = new string[nOfGames + 1];

                for (int i = 0; i < nOfGames; ++i)
                {
                    newGamesList[i] = games[i];
                }
            }
            else
            {
                newGamesList = new string[1];
            }
            newGamesList[newGamesList.Length - 1] = gameArg;

            //Save list and inform user
            System.IO.File.WriteAllLines(FILE_NAME, newGamesList);
            ReplyAsync("*TestBot will remember that game.*");

            return Task.CompletedTask;
        }

        [Command("removegame")]
        public Task RemoveGame(string game)
        {
            //Check list exists
            if (!System.IO.File.Exists(FILE_NAME))
            {
                ReplyAsync("*Testbot doesn't know any games. You can teach it using !addgame*");
                return Task.CompletedTask;
            }
            var games = System.IO.File.ReadAllLines(FILE_NAME);

            //Check if the game is in the list
            int foundGameTimes = 0;
            for (int i = 0, j = 0; i < games.Length; ++i)
            {
                if (games[i] == game)
                {
                    ++foundGameTimes;
                }
                else
                {
                    ++j;
                }
            }

            //Create list of appropriate size and add all games besides the chosen one
            var newGamesList = new string[games.Length - foundGameTimes];
            for(int i = 0, j = 0; i < games.Length; ++i)
            {
                if(games[i] == game)
                {
                    ++foundGameTimes;
                }
                else
                {
                    newGamesList[j] = games[i];
                    ++j;
                }
            }
            System.IO.File.WriteAllLines(FILE_NAME, newGamesList);

            //Inform user
            if(foundGameTimes > 0)
            {
                ReplyAsync("*Testbot will forget that game.*");
            }
            else
            {
                ReplyAsync("*Testbot doesn't know that game.*");
            }

            return Task.CompletedTask;
        }

        [Command("choosegame")]
        public Task ChooseGame(params string[] gamesArg)
        {
            string[] games;
            if (gamesArg.Length != 0)
            {
                //Select from list provided by user
                games = gamesArg;
            }
            else
            {
                //Select from stored games

                //Check file exists
                if (!System.IO.File.Exists(FILE_NAME))
                {
                    ReplyAsync("*Testbot doesn't know any games. You can teach it using !addgame*");
                    return Task.CompletedTask;
                }
                games = System.IO.File.ReadAllLines(FILE_NAME);
            }
            
            
            //Select a random game and inform user
            var rand = new Random();
            var chosenGame = games[rand.Next() % games.Length];
            ReplyAsync("*Testbot thinks you should play \"" + chosenGame + "\"!*");

            return Task.CompletedTask;
        }

        [Command("listgames")]
        public Task ListGames()
        {
            //Check list exists
            if(!System.IO.File.Exists(FILE_NAME))
            {
                ReplyAsync("*Testbot doesn't know any games. You can teach it using !addgame*");
                return Task.CompletedTask;
            }

            //Create string and send to user
            string outstring = "*Testbot knows these games:*";
            string[] games = System.IO.File.ReadAllLines(FILE_NAME);
            for(int i = 0; i < games.Length; ++i)
            {
                outstring += "\n*" + games[i] + "*";
            }
            ReplyAsync(outstring);

            return Task.CompletedTask;
        }
    }
}
