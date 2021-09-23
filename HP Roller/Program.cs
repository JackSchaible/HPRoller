using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HP_Roller
{
    class Program
    {
        private static readonly Regex HitDiceRegex = new Regex(@"([0-9]+)d([0-9]+)(\s(?:\+|\-)\s([0-9]+))?");
        private static readonly Random Random = new Random();
        private static string[] Columns = new[]
        {
            "Monster #",
            "HP Rolled",
            "Initiative"
        };

        static void Main()
        {
            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.Q)
            {
                Console.Clear();

                Console.WriteLine("Enter number of monsters to roll for (default 1)");
                string input = Console.ReadLine()?.Trim() ?? "";
                if (!int.TryParse(input, out var monsters))
                {
                    monsters = 1;
                }

                Console.WriteLine("Enter Dexterity modifier");
                input = Console.ReadLine()?.Trim() ?? "";
                int dexMod, hp;

                if (!int.TryParse(input, out dexMod))
                {
                    dexMod = 0;
                }

                Match match;
                do
                {
                    Console.WriteLine("Enter HP to roll");
                    input = Console.ReadLine()?.Trim() ?? "";

                    match = HitDiceRegex.Match(input);
                    if (match.Success)
                    {
                        Console.Clear();
                        var diceCount = int.Parse(match.Groups[1].Value);
                        var hitDice = int.Parse(match.Groups[2].Value);
                        int.TryParse(match.Groups[3].Value, out hp);

                        PrintTableHeading();

                        for (int i = 0; i < monsters; i++)
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

                            PrintTableRow(i + 1, rolledHp, initiative);
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            "HP Not Understood. Please write in format of [number]d[Hit Dice][+/-][Constitution Modifier]");
                    }
                } while (!match.Success);

                Console.WriteLine("\r\nPress q to quit or any other character.");
                key = Console.ReadKey().Key;
            }
        }

        private static void PrintTableHeading()
        {
            Console.WriteLine(string.Join(" | ", Columns));
            PrintDivider(2);
        }

        private static void PrintDivider(int times = 1)
        {
            int dashes = Columns.Select(s => s.Length).Sum() + (Columns.Length - 1) * 3;

            for(int i = 0; i < times; i++)
                Console.WriteLine(new string('-', dashes));
        }

        private static void PrintTableRow(int monsterNumber, int hp, int initiative)
        {
            string hpLine = PrintInCenter(Columns[0].Length + 1, monsterNumber.ToString());
            hpLine += "|";
            hpLine += PrintInCenter(Columns[1].Length + 2, hp.ToString());
            hpLine += "|";
            hpLine += PrintInCenter(Columns[2].Length + 3, initiative.ToString());

            Console.WriteLine(hpLine);
            PrintDivider();
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
