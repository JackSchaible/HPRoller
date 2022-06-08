using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Linq;
using System.Text.RegularExpressions;

namespace HP_Roller
{
    internal class Program
    {
        private static readonly Regex HitDiceRegex = new(@"([0-9]+)d([0-9]+)((?:\+|\-)([0-9]+))?");
        private static readonly Random Random = new();
        private static readonly string[] Columns = {
            "Monster #",
            "Hp Rolled",
            "Initiative"
        };

        private static int Main(string[] args)
        {
            var numMonstersArg = new Argument<int>(
                "num-monsters", "Number of monsters to roll for");
            numMonstersArg.AddValidator(result =>
            {
                if (result.GetValueForArgument(numMonstersArg) < 1)
                {
                    result.ErrorMessage = "Must be greater than 0";
                }
            });
            numMonstersArg.SetDefaultValue(1);

            var dexModArg = new Argument<int>(
                "dex-mod", "The dexterity modifier of the monsters to roll for");

            var hpArg = new Argument<string>(
                "hp", "The Hp dice of the monsters to roll");
            hpArg.AddValidator(result =>
            {
                Match match = HitDiceRegex.Match(result.GetValueForArgument(hpArg));
                if (!match.Success)
                {
                    result.ErrorMessage = "HP Dice isn't valid. It must be in the format {n}d{n}[+/-]{n}, so '1d8+4', for example.";
                }
            });
            
            var cmd = new RootCommand
            {
                numMonstersArg,
                dexModArg,
                hpArg,
            };
            cmd.Description = "A command-line utility to roll HP for 5e D&D monsters!";
            cmd.Handler = CommandHandler.Create<int, int, string, IConsole>(HandleRoot);

            return cmd.Invoke(args);
        }

        private static void HandleRoot(int numMonsters, int dexMod, string hp, IConsole console)
        {

            // Validation is done already so we can assume the regex is valid
            Match match = HitDiceRegex.Match(hp);
            int diceCount = int.Parse(match.Groups[1].Value),
                hitDice = int.Parse(match.Groups[2].Value),
                hpNo = int.Parse(match.Groups[3].Value);
            
            ProcessInput(numMonsters, dexMod, diceCount, hitDice, hpNo, console);
        }

        private static void ProcessInput(int numMonsters, int dexMod, int diceCount,
            int hitDice, int hp, IConsole console)
        {
            PrintTableHeading(console);

            for (int i = 0; i < numMonsters; i++)
            {
                int rolledHp = hp, initiative = dexMod;

                for (int j = 0; j < diceCount; j++)
                {
                    if (j == 0)
                        rolledHp += hitDice;
                    else
                        rolledHp += Random.Next(2, hitDice + 1);
                }

                initiative += Random.Next(1, 20 + 1);

                PrintTableRow(console, i + 1, rolledHp, initiative);
            }
        }

        private static void PrintTableHeading(IConsole console)
        {
            console.WriteLine(string.Join(" | ", Columns));
            PrintDivider(console, 2);
        }

        private static void PrintDivider(IConsole console, int times = 1)
        {
            int dashes = Columns.Select(s => s.Length).Sum() + (Columns.Length - 1) * 3;

            for (int i = 0; i < times; i++)
                console.WriteLine(new string('-', dashes));
        }

        private static void PrintTableRow(IConsole console, int monsterNumber, int hp, int initiative)
        {
            string hpLine = PrintInCenter(Columns[0].Length + 1, monsterNumber.ToString());
            hpLine += "|";
            hpLine += PrintInCenter(Columns[1].Length + 2, hp.ToString());
            hpLine += "|";
            hpLine += PrintInCenter(Columns[2].Length + 3, initiative.ToString());

            console.WriteLine(hpLine);
            PrintDivider(console);
        }

        private static string PrintInCenter(int spaces, string text)
        {
            string result = "";
            int textLength = text.Length;

            for (int j = 0; j < spaces; j++)
            {
                if (j == spaces / 2 - textLength)
                {
                    result += text;
                    j += textLength - 1;
                }
                else
                    result += " ";
            }

            return result;
        }
    }
}
