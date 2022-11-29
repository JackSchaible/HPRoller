import inquirer, { Question } from "inquirer";

const HitDiceRegex = /([0-9]+)d([0-9]+)((?:\+|-)([0-9]+))?/;
const questions: Question[] = [
  {
    type: "number",
    name: "numMonsters",
    message: "How many monsters are you rolling for?",
    default: 1,
    validate: (input: number) => {
      if (input > 0) {
        return true;
      }
      return "You must roll for at least one monster";
    },
  },
  {
    type: "input",
    name: "hitDice",
    message: "What is the hit dice of the monster?",
    default: "1d8",
    validate: (input: string) => {
      if (HitDiceRegex.test(input)) {
        return true;
      }
      return "Invalid hit dice";
    },
  },
  {
    type: "number",
    name: "initiative",
    message: "What is the initiative bonus of the monster?",
    default: 0,
  },
];
let quit = false;
async function main() {
  while (!quit) {
    await run();

    const answers = await inquirer.prompt({
      type: "confirm",
      name: "again",
      message: "Roll again?",
      default: true,
    });

    quit = !answers.again;
  }
}

async function run() {
  const answers = await inquirer.prompt(questions);
  const numMonsters = answers["numMonsters"] as number;
  const hitDice = answers["hitDice"] as string;
  const initMod = answers["initiative"] as number;

  const match = hitDice.match(HitDiceRegex);

  if (match) {
    const numDice = parseInt(match[1] ?? "0");
    const diceSize = parseInt(match[2] ?? "0");
    const modifier = match[3] ? parseInt(match[3]) : 0;

    const results: result[] = [];
    for (let i = 0; i < numMonsters; i++) {
      let hp = 0;
      for (let j = 0; j < numDice; j++) {
        let roll = 0;
        while (roll <= 1) {
          roll = Math.floor(Math.random() * diceSize) + 1;
        }

        hp += roll;
        console.log("Total HP: " + hp);
      }

      hp += modifier;

      const initiative = Math.floor(Math.random() * 20) + 1 + initMod;

      results.push({
        "HP Rolled": hp,
        Initiative: initiative,
      });
    }

    results.sort((a, b) => b.Initiative - a.Initiative);

    console.table(results, ["HP Rolled", "Initiative"]);
  }
}

interface result {
  "HP Rolled": number;
  Initiative: number;
}

main();
