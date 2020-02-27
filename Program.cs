using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Text_based_game_oofer
{
    public class MainSpace
    {
        /* for all items make some mob exclusive by creating a different list for loot and then draw drops from the json file.
        /* for melee need new wepons, enemies give drops such as wolf pelt, implement armour, more armour, more chance enemies
         * attacks halve, more weapon types crimson blade, orb which increases magical damage, potions in battle, every stat x10, */

        //arrays, cos apparently they need global stuff now...
        public static int[,] map = new int[10, 10];

        //number variables
        public static int j = 2;
        public static int k = 0;
        public static int index = 0;
        public static int correct = 0;
        public static int itemID = 0;
        public static int playerSword = 10;
        public static int playerRanged = 10;
        public static int playerMagic = 10;
        public static int playerHP = 200;
        public static int playerMaxHP = 200;
        public static int playerMana = 120;
        public static int playerMaxMana = 120;
        public static int playerAmmo = 16;
        public static int randDamage = 0;
        public static int numOfEnemies = 0;
        public static int allowedToLeave = 0;
        public static double storyCounter = 0;
        public static int rndItem = 0;
        public static int yof1;
        public static int xof1;
        public static int storyCounterInt;

        //strings
        public static string input;

        //Items available for inventory
        public static List<string> menus = new List<string> { };
        public static List<string> inventory = new List<string> { "Exit" };
        public static List<string> bestiary = new List<string> { "Exit" };
        public static List<string> difficulty = new List<string> { "Easy", "Medium", "Hard", "Bravo Six, going dark" };
        public static List<int> inventoryIndex = new List<int> { };
        public static List<int> bestiaryIndex = new List<int> { };

        //bools
        public static bool foundItem = false;
        public static bool userMoron = false;
        public static bool tutorial = false;
        public static bool notUnderstood = false;
        public static bool escapeSucess = false;

        //story time!!
        public static List<string> story = new List<string> { "You are thrown into a firey hellhole with nothing but the mana in your veins and the weapons on your back. Your job? \nTo vanquish the demons that control this land and restore the peace and balance to this once great domain",
        "You find yourself alongside a stream of magma, intensely scorching the earth you walk. The only way ahead is forward, to keep walking - and fighting the monsters that plague this land when you must.",
        "Story1", "Story2", "Story3", "Story4", "Story5", "Story6" };

        //runs menu selector (not mine)
        private static string DrawMenu(List<string> menus)
        {
            do
            {
                Console.Clear();
                for (int i = 0; i < menus.Count; i++)
                {
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.WriteLine(menus[i]);
                    }
                    else
                    {
                        Console.WriteLine(menus[i]);
                    }
                    Console.ResetColor();
                }
                ConsoleKeyInfo ckey = Console.ReadKey();

                if (ckey.Key == ConsoleKey.DownArrow)
                {
                    if (index == menus.Count - 1)
                    {
                        index = 0; //Remove the comment to return to the topmost item in the list
                    }
                    else { index++; }
                }
                else if (ckey.Key == ConsoleKey.UpArrow)
                {
                    if (index <= 0)
                    {
                        index = menus.Count - 1; //Remove the comment to return to the item in the bottom of the list
                    }
                    else { index--; }
                }
                //mine, dropping items
                else if (ckey.Key == ConsoleKey.Q && menus[index] != "Exit" && !bestiary.Contains(menus[index]))
                {
                    menus.Remove(menus[index]);
                    index--;
                }
                else if (ckey.Key == ConsoleKey.Enter)
                {
                    return menus[index];
                }
                Console.Clear();
                return "";

            } while (correct == 0);
        }
        //combat system
        public static void Combat(bool tutorial, int[,] map)
        {
            if (userMoron == false)
            {
                MonsterFinder.monsters[j].enemyHP = MonsterFinder.monsters[j].defaultEnemyHP;
            }
            userMoron = false;
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.Black;
                bool finished = false;
                do
                {
                    Console.Title = "-=-COMBAT : " + MonsterFinder.monsters[j].enemyName.ToUpper() + "-=-";
                    correct = 0;
                    Console.Clear();
                    if (!bestiary.Contains(MonsterFinder.monsters[j].enemyName))
                    {
                        bestiary.Add(MonsterFinder.monsters[j].enemyName);
                        bestiaryIndex.Add(j);
                    }
                    Console.WriteLine("\n\n\n          Your Health is at {0} Points, Your Mana is at {1} Points, You have {2} arrows", playerHP, playerMana, playerAmmo);
                    Console.WriteLine("          The {0}s Health is at {1} Points", MonsterFinder.monsters[j].enemyName, MonsterFinder.monsters[j].enemyHP);
                    Console.WriteLine("          Enter the number for the type of attack that you wish to perform:");
                    Console.WriteLine("          1. Sword Attack");
                    Console.WriteLine("          2. Ranged Weapon Attack");
                    Console.WriteLine("          3. Magical Attack");
                    Console.WriteLine("          4. Run coward run");
                    Console.Write("          >> ");
                    int playerAttackOption = int.Parse(Console.ReadLine());
                    if (playerAttackOption == 1)
                    {
                        Random randDmg = new Random();
                        int randDamage = randDmg.Next(0, playerSword);
                        Console.WriteLine("          Damage inflicted: {0}", randDamage);
                        Random randCritChance = new Random();
                        int skillPointResult = randCritChance.Next(1, 101);
                        if (skillPointResult >= 90 && randDamage > 0)
                        {
                            randDamage += playerSword;
                            Console.WriteLine("          You did a critical hit, +{0} damage!", playerSword);
                        }
                        Console.WriteLine("          You did {0} Damage to the {1}", randDamage, MonsterFinder.monsters[j].enemyName);
                        MonsterFinder.monsters[j].enemyHP -= randDamage;
                    }
                    else if (playerAttackOption == 2)
                    {
                        if (playerAmmo == 0)
                        {
                            Console.WriteLine("\n\n\n          Not enough ammo!");
                            Console.ReadKey();
                            userMoron = true;
                            Combat(tutorial, map);
                        }
                        Random randDmg = new Random();
                        int randDamage = randDmg.Next(0, playerRanged);
                        Console.WriteLine("\n\n\n          Damage inflicted: {0}", randDamage);
                        Random randCritChance = new Random();
                        int skillPointResult = randCritChance.Next(1, 101);
                        if (skillPointResult >= 75 && randDamage > 0)
                        {
                            randDamage += playerRanged;
                            Console.WriteLine("          You did a critical hit, +{0} damage!", playerRanged);
                        }
                        Console.WriteLine("          You did {0} Damage to the {1}", randDamage, MonsterFinder.monsters[j].enemyName);
                        playerAmmo--;
                        MonsterFinder.monsters[j].enemyHP -= randDamage;
                    }
                    else if (playerAttackOption == 3)
                    {
                        if (playerMana == 0)
                        {
                            Console.WriteLine("\n\n\n          Not enough mana!");
                            Console.ReadKey();
                            userMoron = true;
                            Combat(tutorial, map);
                        }
                        Random randDmg = new Random();
                        int randDamage = randDmg.Next(0, playerMagic);
                        Console.WriteLine("\n\n\n          Damage inflicted: {0}", randDamage);
                        Random randCritChance = new Random();
                        int skillPointResult = randCritChance.Next(1, 101);
                        if (skillPointResult >= 50 && randDamage > 0)
                        {
                            randDamage += playerMagic;
                            Console.WriteLine("          You did a critical hit, +{0} damage!", playerMagic);
                        }
                        Console.WriteLine("          You did {0} Damage to the {1}", randDamage, MonsterFinder.monsters[j].enemyName);
                        playerMana -= 30;
                        MonsterFinder.monsters[j].enemyHP -= randDamage;
                    }
                    else if (playerAttackOption == 4)
                    {
                        Random randCoward = new Random();
                        int randCowardRun = randCoward.Next(1, 1001);
                        if (randCowardRun > 900)
                        {
                            if (tutorial)
                            {
                                Console.WriteLine("          This is the tutorial, don't be such a baby\n          Press any key...");
                                userMoron = true;
                                Console.ReadKey();
                                Combat(tutorial, map);
                            }
                            else
                            {
                                Console.WriteLine("\n\n\n          You escaped, congrats\n");
                                map[yof1, xof1] = j;
                                Console.ReadKey();
                                escapeSucess = true;
                                Start();
                            }

                        }
                        else
                        {
                            if (tutorial)
                            {
                                Console.WriteLine("          This is the tutorial, don't be such a baby\n          Press any key...");
                                userMoron = true;
                                Console.ReadKey();
                                Combat(tutorial, map);
                            }
                            else
                            {
                                Console.WriteLine("\n\n\n          Lmao you can't even do that properly, coward");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input\nPress any key...");
                        Console.ReadKey();
                        userMoron = true;
                        Combat(tutorial, map);
                    }

                    if (MonsterFinder.monsters[j].enemyHP > 0)
                    {
                        Random randEnemyDmg = new Random();
                        int enemyDmg = randEnemyDmg.Next(0, MonsterFinder.monsters[j].enemyMaxDmg);
                        Random randEnemyCritChance = new Random();
                        int enemyCritResult = randEnemyCritChance.Next(0, 100);
                        if (enemyCritResult >= 90 && randDamage > 0)
                        {
                            enemyDmg += MonsterFinder.monsters[j].enemyMaxDmg;
                            Console.WriteLine("\n\n\n          The enemy did a critical hit, +{0} damage!", MonsterFinder.monsters[j].enemyMaxDmg);
                        }
                        Console.WriteLine("\n\n\n          The {0} hits back, and deal {1} Points of Damage", MonsterFinder.monsters[j].enemyName, enemyDmg);
                        playerHP -= enemyDmg;
                        Console.ReadLine();
                    }
                    Console.WriteLine("\n\n\n          Your Health is at {0} Points, Your Mana is at {1} Points", playerHP, playerMana);
                    if (MonsterFinder.monsters[j].enemyHP < 0)
                    {
                        MonsterFinder.monsters[j].enemyHP = 0;
                    }
                    Console.WriteLine("          The {0}s Health is at {1} Points", MonsterFinder.monsters[j].enemyName, MonsterFinder.monsters[j].enemyHP);
                    if (MonsterFinder.monsters[j].enemyHP <= 0 || playerHP <= 0)
                    {
                        if (MonsterFinder.monsters[j].enemyHP > 0 && playerHP <= 0)
                        {
                            Console.WriteLine("");
                            Console.ReadKey();
                            Console.WriteLine("\n\n\n           - You died in battle -");
                            Console.ReadKey();
                            finished = true;
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\n\n\n           - You won the battle -");
                            Console.ReadKey();
                            Console.Title = "The Ember Isles";
                            finished = true;
                        }
                        if (numOfEnemies == 1)
                        {
                            storyCounter++;
                        }
                        else if (numOfEnemies == 2)
                        {
                            storyCounter += 0.5;
                        }
                        else if (numOfEnemies == 3)
                        {
                            storyCounter += 1 / 3;
                        }
                        else if (numOfEnemies == 4)
                        {
                            storyCounter += 0.25;
                        }
                        if (storyCounter % 1 == 0)
                        {
                            int storyCounterInt = Convert.ToInt32(storyCounter);
                            Console.WriteLine(story[storyCounterInt]);
                            Console.ReadKey();
                        }
                        if (storyCounterInt == 5)
                        {
                            Ending();
                        }
                    }

                } while (finished == false);
                finished = false;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                userMoron = true;
                Combat(tutorial, map);
            }
            Console.Clear();
            if (tutorial)
            {
                playerAmmo = 16;
                playerMana = 120;
                playerHP = 200;
            }
            //if player dies then it is permanent
            if (playerHP <= 0)
            {
                Console.WriteLine("Do you want to start again? (yes/no)");
                string startOver = Console.ReadLine().ToLower();
                if (startOver == "yes" || startOver == "y")
                {
                    inventory = new List<string> { "Exit" };
                    bestiary = new List<string> { "Exit" };
                    playerSword = 0;
                    playerRanged = 0;
                    playerMagic = 0;
                    playerAmmo = 16;
                    playerMana = 120;
                    playerHP = 200;
                    Main2(null);
                }
                else
                {
                    Console.WriteLine("Thanks for playing!");
                    Thread.Sleep(1200);
                    Environment.Exit(0);
                }
            }
        }
        public static void CharacterCreation()
        {
            //Strings:
            string playerGender;
            string playerRace;
            string playerClass;
            string playerName;

            //Character Creation:
            do
            {
                Console.Clear();
                Console.WriteLine("Please choose a sex:");
                Console.Write("Male / Female\n>> ");
                playerGender = Console.ReadLine().ToLower();
                if (playerGender == "male" || playerGender == "m")
                {
                    playerGender = "male";
                    correct = 1;
                }
                if (playerGender == "female" || playerGender == "f")
                {
                    playerGender = "female";
                    correct = 1;
                }
                if (playerGender == "jack")
                {
                    playerSword += 10000;
                    playerRanged += 10000;
                    playerMagic += 10000;
                    correct = 1;
                    Difficulty();
                    Parallel.Invoke(() => ManaAndHealthRegen(), () => Start());
                }
            } while (correct == 0);
            correct = 0;

            //playerRace Creation:
            do
            {
                Console.Clear();
                Console.WriteLine("Please choose a race:");
                Console.WriteLine("> Human");
                Console.WriteLine("> Dwarf");
                Console.WriteLine("> Elf");
                Console.Write(">> ");
                playerRace = Console.ReadLine().ToLower();
                switch (playerRace)
                {
                    case "human":
                        Console.WriteLine("\nThis race gives a bonus to the following stats:");
                        Console.WriteLine("Sword. 15 Point");
                        Console.WriteLine("Ranged : 15 Point");
                        Console.WriteLine("Magic : 10 Point");
                        Console.WriteLine("\nThe race of Men is the second race of beings created by the Erédin Bethídas. Because they awoke at the start of the First Age of the Timeless Inferno,\nwhile the Elves awoke three Ages before them, " +
                            "they are called the Secondborn by the Elves.\nMen awoke in a land located in the far east of the world, and are known for their great adaptable combat style, being equally gifted in the three forms of battle.");
                        Console.Write("\nIs this the race you wish to play? (yes/no)\n>> ");
                        input = Console.ReadLine().ToLower();
                        if (input == "yes" || input == "y")
                        {
                            //race bonuses
                            playerSword += 15;
                            playerRanged += 15;
                            playerMagic += 10;
                            correct = 1;
                        }
                        if (input == "no" || input == "n")
                        {
                            correct = 0;
                        }
                        break;
                    case "dwarf":
                        Console.WriteLine("\nThis race gives a bonus to the following stats:");
                        Console.WriteLine("Sword : 25 Points");
                        Console.WriteLine("Ranged : 10 Point");
                        Console.WriteLine("Magic : 5 Points");
                        Console.WriteLine("\nUnlike Elves and Men, the Dwarves are not counted among the Children of Bethídas. Their creator was Gilros, known as Wynren the Smith.\nWynren created the Five Fathers of Dwarves, from whom all other Dwarves are descended\nThis" +
                            " ancient race specialises in close combat and demonstrate this in battle");
                        Console.Write("\nIs this the race you wish to play? (yes/no)\n>> ");
                        input = Console.ReadLine().ToLower();
                        if (input == "yes" || input == "y")
                        {
                            //race bonuses
                            playerSword += 25;
                            playerRanged += 10;
                            playerMagic += 5;
                            correct = 1;
                        }
                        if (input == "no" || input == "n")
                        {
                            correct = 0;
                        }
                        break;
                    case "elf":
                        Console.WriteLine("\nThis race gives a bonus to the following stats:");
                        Console.WriteLine("Sword : 10 Points");
                        Console.WriteLine("Ranged : 10 Point");
                        Console.WriteLine("Magic : 20 Points");
                        Console.WriteLine("\nThe Elves were the first of the races of the Children of Bethídas, known also as the Firstborn for that reason. The Elves are distinguished from the other\ntwo races, the Men and the Dwarves, " +
                            "especially by the fact of their near immortality and remarkable magical talent in combat - at the cost of mana");
                        Console.Write("\nIs this the race you wish to play? (yes/no)\n>> ");
                        input = Console.ReadLine().ToLower();
                        //add in about magic and lore and stuff
                        if (input == "yes" || input == "y")
                        {
                            //race bonuses
                            playerSword += 10;
                            playerRanged += 10;
                            playerMagic += 20;
                            correct = 1;
                        }
                        if (input == "no" || input == "n")
                        {
                            correct = 0;
                        }
                        break;
                }
            } while (correct == 0);
            correct = 0;

            //playerClass Creation:
            do
            {
                Console.Clear();
                Console.WriteLine("Please choose a class:");
                Console.WriteLine("> Warrior");
                Console.WriteLine("> Mage");
                Console.WriteLine("> Thief");
                Console.Write(">> ");
                playerClass = Console.ReadLine().ToLower();
                switch (playerClass)
                {
                    case "warrior":
                        Console.WriteLine("\nThis class gives a bonus to the following stats:");
                        Console.WriteLine("Sword : 35 Points");
                        Console.WriteLine("Ranged : 5 Points");
                        Console.WriteLine("Magic : 0 Points");
                        Console.WriteLine("\nUnlike mages and thieves, the warrior is limited by nothing but the strength of his arm.\nHowever, this comes at a cost of having a smaller chance of doing extra damage");
                        Console.Write("\nIs this the class you wish to play? (yes/no)\n>> ");
                        input = Console.ReadLine().ToLower();
                        if (input == "yes" || input == "y")
                        {
                            //class bonuses
                            playerSword += 35;
                            playerRanged += 5;
                            correct = 1;
                        }
                        if (input == "no" || input == "n")
                        {
                            correct = 0;
                        }
                        break;
                    case "mage":
                        Console.WriteLine("\nThis class gives a bonus to the following stats:");
                        Console.WriteLine("Sword : 0 Points");
                        Console.WriteLine("Ranged : 15 Point");
                        Console.WriteLine("Magic : 25 Points");
                        Console.WriteLine("\nWhile limited by the cost of spells in mana, these abilities are far more likely to do extra damage than any other");
                        Console.Write("\nIs this the class you wish to play? (yes/no)\n>> ");
                        input = Console.ReadLine().ToLower();
                        if (input == "yes" || input == "y")
                        {
                            //class bonuses
                            playerRanged += 15;
                            playerMagic += 25;
                            correct = 1;
                        }
                        if (input == "no" || input == "n")
                        {
                            correct = 0;
                        }
                        break;
                    case "thief":
                        Console.WriteLine("\nThis class gives a bonus to the following stats:");
                        Console.WriteLine("Sword : 20 Point");
                        Console.WriteLine("Ranged : 20 Points");
                        Console.WriteLine("Magic : 0 Points");
                        Console.WriteLine("\nThe only limits an archer faces in battle is whether their quiver is full, should you find yourself out of arrows,\nmore can be found in random places you happen to stumble across");
                        Console.Write("\nIs this the class you wish to play? (yes/no)\n>> ");
                        input = Console.ReadLine().ToLower();
                        if (input == "yes" || input == "y")
                        {
                            //class bonuses
                            playerSword += 20;
                            playerRanged += 20;
                            correct = 1;
                        }
                        if (input == "no" || input == "n")
                        {
                            correct = 0;
                        }
                        break;
                }
            } while (correct == 0);
            correct = 0;
            do
            {
                Console.Clear();
                Console.Write("Name your character\n>> ");
                playerName = Console.ReadLine();
                if (playerName == null || playerName == "")
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Name");
                    Console.ReadKey();
                }
                else
                {
                    correct = 1;
                }

            } while (correct == 0);
            correct = 0;

            //Player Description
            Console.Clear();
            Console.WriteLine("Your full character description, is:");
            Console.WriteLine("A {0} {1} {2} called {3}", playerGender, playerRace, playerClass, playerName);
            Console.WriteLine("Sword skill points: {0}", playerSword);
            Console.WriteLine("Ranged skill points: {0}", playerRanged);
            Console.WriteLine("Magic skill points: {0}", playerMagic);
            Console.ReadKey();
            Console.Clear();
            Console.Write("Would you like to run through the tutorial?\n(For first time players this is strongly recommended)\n>> ");
            input = Console.ReadLine().ToLower();
            if (input == "yes" || input == "y")
            {
                Console.Clear();
                Tutorial();
            }
            else
            {
                Difficulty();
                Start();
            }
        }
        //runs tutorial
        public static void Tutorial()
        {
            bool tutorial = true;
            Console.Title = "Tutorial";
            Console.WriteLine("Welcome to the tutorial!\nTo begin, press any key...");
            Console.ReadKey();
            Console.Clear();
            do
            {
                //bestiary tut
                Console.Write("You can input 'B' to check the bestiary containing all monsters you have fought\n>> ");
                input = Console.ReadLine().ToLower();
                if (input == "b")
                {
                    Bestiary();
                    Console.Title = "Tutorial";
                    Console.WriteLine("You haven't fought any monsters on your way to the tutorial screen, so it appears empty\nFight more monsters to fill this page up");
                    correct = 1;
                    Console.ReadKey();
                }
                else
                {
                    correct = 0;
                    Console.Clear();
                }
            } while (correct == 0);
            correct = 0;
            do
            {
                //inventory tut
                Console.Clear();
                Console.Write("You can input 'I' to check your inventory of items\n>> ");
                input = Console.ReadLine().ToLower();
                if (input == "i")
                {
                    string item = ItemFinder.itemList[j].itemName;
                    foundItem = true;
                    Inventory(rndItem);
                    Console.Title = "Tutorial";
                    Console.WriteLine("This is where certain quest items and tools will appear\nYou only have 20 spaces - use them well!\nYou can also hover over an item and press 'Q' to drop it\n(WARNING: Once you have done this you cannot pick it back up)");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    correct = 1;
                }
                else
                {
                    correct = 0;
                    Console.Clear();
                }
            } while (correct == 0);
            correct = 0;
            //movement tut
            Console.Clear();
            Console.WriteLine("You can move in the directions North, East, South and West by typing 'Go [DIRECTION]'");
            Console.ReadKey();
            do
            {
                //combat system tutorial
                Console.Clear();
                Console.WriteLine("Now Im going to drop you into a fight\nIf you die - don't worry! It's just a test");
                Console.ReadKey();
                Combat(tutorial, map);
                Console.Title = "Tutorial";
                Console.WriteLine("That wasnt so bad was it?");
                Console.ReadKey();
                Console.Clear();
                do
                {
                    Console.Write("Now you can check your bestiary for your new encounter\n>> ");
                    input = Console.ReadLine();
                    if (input == "b")
                    {
                        Bestiary();
                        correct = 1;
                    }
                    else
                    {
                        Console.Clear();
                        correct = 0;
                    }
                } while (correct == 0);
                Console.WriteLine("I think you're ready! Be careful...");
                Console.ReadKey();
                //!!dont forget to add health regen!!//
                Difficulty();
                Parallel.Invoke(() => ManaAndHealthRegen(), () => Start());


            } while (correct == 0);
            correct = 0;
        }
        //bestiary containing all known monsters and demons

        public static void Difficulty()
        {
            correct = 0;
            Console.Title = "Difficulty";
            Console.Clear();
            //num of unique enemies from 1 - 4
            do
            {
                string selectedDifficulty = DrawMenu(difficulty);
                if (selectedDifficulty == difficulty[0])
                {
                    numOfEnemies = 1;
                    correct = 1;
                }
                else if (selectedDifficulty == difficulty[1])
                {
                    numOfEnemies = 2;
                    correct = 1;
                }
                else if (selectedDifficulty == difficulty[2])
                {
                    numOfEnemies = 3;
                    correct = 1;
                }
                else if (selectedDifficulty == difficulty[3])
                {
                    numOfEnemies = 4;
                    correct = 1;
                }


            } while (correct == 0);
            correct = 0;
            Console.Clear();
            Console.Title = "The Ember Isles";
        }
        public static void Bestiary()
        {
            try
            {
                correct = 0;
                Console.Title = "-=-BESTIARY-=-";
                Console.Clear();
                do
                {
                    string selectedMenuItem = DrawMenu(bestiary);
                    if (selectedMenuItem == bestiary[0])
                    {
                        Console.Clear();
                        correct = 1;
                    }
                    else if (selectedMenuItem == bestiary[index])
                    {
                        Console.Clear();
                        Console.WriteLine("Maximum damage/hit: " + MonsterFinder.monsters[bestiaryIndex[index - 1]].enemyMaxDmg);
                        Console.WriteLine("Health: " + MonsterFinder.monsters[bestiaryIndex[index - 1]].defaultEnemyHP);
                        Console.WriteLine("Description: " + MonsterFinder.monsters[bestiaryIndex[index - 1]].enemyDesc);
                        Console.WriteLine("\n\nPress any key to go back...");
                        Console.ReadKey();
                    }
                } while (correct == 0);
                correct = 0;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Title = "The Ember Isles";
            }
            catch
            {
                if (bestiary.Count > 1)
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
                Bestiary();
            }
            
        }
        //inventory of player
        public static void Inventory(int rndItem)
        {
            try
            {
                correct = 0;
                Console.Title = "-=-INVENTORY-=-";
                Console.Clear();
                do
                {
                    string selectedMenuItem = DrawMenu(inventory);
                    if (selectedMenuItem == inventory[0])
                    {
                        Console.Clear();
                        correct = 1;
                    }
                    else if (selectedMenuItem == inventory[index])
                    {
                        Console.Clear();
                        Console.WriteLine("Sword damage increase: " + ItemFinder.itemList[inventoryIndex[index - 1]].swordIncrease);
                        Console.WriteLine("Ranged damage increase: " + ItemFinder.itemList[inventoryIndex[index - 1]].rangedIncrease);
                        Console.WriteLine("Description: " + ItemFinder.itemList[inventoryIndex[index - 1]].itemDesc);
                        Console.WriteLine("\n\nPress any key to go back...");
                        Console.ReadKey();

                    }
                } while (correct == 0);
                correct = 0;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Title = "The Ember Isles";
            }
            catch
            {
                if (inventory.Count > 1)
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
                Inventory(rndItem);
            }
           
        }
        //mana regen over time
        public static void ManaAndHealthRegen()
        {
            do
            {
                if (playerHP < 200)
                {
                    Thread.Sleep(30000);
                    playerHP += 10;
                }
                else if (playerHP > 200)
                {
                    playerHP = 200;
                }
                if (playerMana < 120)
                {
                    Thread.Sleep(30000);
                    playerMana += 20;
                }
                else if (playerMana > 120)
                {
                    playerMana = 120;
                }

            } while (true);
        }
        public static void PositionDecider()
        {
            Random rnd = new Random();

            //player = 1
            //paths = ???
            //enemies = 2-7
            //loot = 8
            //fake loot = 9
            Console.Title = "The Ember Isles";
            Console.Clear();
            Console.WriteLine(story[0]);
            //Randomly generates enemy positions
            for (j = 2; j <= 7; j++)
            {
                for (int i = 0; i < numOfEnemies; i++)
                {
                    while (true)
                    {
                        int yofj = rnd.Next(0, 9);
                        int xofj = rnd.Next(0, 9);


                        if (!new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(map[yofj, xofj]))
                        {
                            map[yofj, xofj] = j;
                            Console.Write(yofj + ", ");
                            Console.WriteLine(xofj);
                            break;
                        }
                    }
                    
                }
            }
            for (int k = 0; k <= 8; k++)
            {
                int yofk = rnd.Next(0, 9);
                int xofk = rnd.Next(0, 9);
                if (!new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(map[yofk, xofk]))
                {
                    map[yofk, xofk] = 8;
                }
            }
        }
        //start of game
        public static void Start()
        {
            Random rnd1 = new Random();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.Black;
            if (escapeSucess == false)
            {
                //declares 1s position
                int xof1 = rnd1.Next(0, 10);
                int yof1 = rnd1.Next(0, 10);
                map[yof1, xof1] = 1;
                PositionDecider();
            }
            do
            {
                Console.Write(">> ");
                input = Console.ReadLine().ToLower();
                Match movement = new Regex(@"(go |head |move |travel |walk |reposition )").Match(input);
                if (movement.Success)
                {
                    string secondWord = input.Split(' ').Skip(1).FirstOrDefault().ToLower();
                    switch (secondWord)
                    {
                        case "north":
                            if (yof1 == 0)
                            {
                                Console.WriteLine("Can't go");
                            }
                            else
                            {
                                Console.WriteLine("Moved north");
                                yof1--;
                                if (new int[] { 1, 2, 3, 4, 5, 6, 7 }.Contains(map[yof1, xof1]))
                                {
                                    j = map[yof1, xof1];
                                    Combat(tutorial, map);
                                }
                                if (new int[] { 8 }.Contains(map[yof1, xof1]))
                                {
                                    Random randomItem = new Random();
                                    Random randomLootSpot = new Random();
                                    List<string> lootSpots = new List<string> { "You come across a pile of leaves", "You find an old chest", "You find a wooden barrel", "You pass something buried in the ground" };
                                    int chosenSentence = randomLootSpot.Next(0, lootSpots.Count());
                                    Console.WriteLine(lootSpots[chosenSentence]);
                                    Console.Write(">> ");
                                    input = Console.ReadLine().ToLower();
                                    int rndItem = randomItem.Next(0, 6);
                                    Match inspect = new Regex(@"(( )?look( at )?|( )?inspect( )?|( )?open( )?|( )?search( )?|( )?see( )?)").Match(input);
                                    if (inspect.Success)
                                    {
                                        Console.WriteLine("You found a {0}!", ItemFinder.itemList[rndItem].itemName);
                                        while (true)
                                        {
                                            Console.Write(">> ");
                                            input = Console.ReadLine().ToLower();
                                            Match pickUp = new Regex(@"(( )?pick up( )?|( )?collect( )?|( )?grab( )?|( )?take( )?)").Match(input);
                                            if (pickUp.Success)
                                            {
                                                inventory.Add(ItemFinder.itemList[rndItem].itemName);
                                                inventoryIndex.Add(rndItem);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("I don't understand {0}", input);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Okay");
                                    }

                                }
                                map[yof1 + 1, xof1] = 0;
                            }
                            break;
                        case "south":
                            if (yof1 == 9)
                            {
                                Console.WriteLine("Can't go");
                            }
                            else
                            {
                                Console.WriteLine("Moved south");
                                yof1++;
                                if (new int[] { 1, 2, 3, 4, 5, 6, 7 }.Contains(map[yof1, xof1]))
                                {
                                    j = map[yof1, xof1];
                                    Combat(tutorial, map);
                                }
                                if (new int[] { 8 }.Contains(map[yof1, xof1]))
                                {
                                    Random randomItem = new Random();
                                    Random randomLootSpot = new Random();
                                    List<string> lootSpots = new List<string> { "You come across a pile of leaves", "You find an old chest", "You find a wooden barrel", "You pass something buried in the ground" };
                                    int chosenSentence = randomLootSpot.Next(0, lootSpots.Count());
                                    Console.WriteLine(lootSpots[chosenSentence]);
                                    Console.Write(">> ");
                                    input = Console.ReadLine().ToLower();
                                    Match inspect = new Regex(@"(( )?look( at )?|( )?inspect( )?|( )?see( )?|( )?search( )?|( )?open( )?)").Match(input);
                                    int rndItem = randomItem.Next(0, 6);
                                    if (inspect.Success)
                                    {
                                        Console.WriteLine("You found a {0}!", ItemFinder.itemList[rndItem].itemName);
                                        while (true)
                                        {
                                            Console.Write(">> ");
                                            input = Console.ReadLine().ToLower();
                                            Match pickUp = new Regex(@"(( )?pick up( )?|( )?collect( )?|( )?grab( )?|( )?take( )?)").Match(input);
                                            if (pickUp.Success)
                                            {
                                                inventory.Add(ItemFinder.itemList[rndItem].itemName);
                                                inventoryIndex.Add(rndItem);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("I don't understand {0}", input);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ooooof");
                                    }
                                }
                                map[yof1 - 1, xof1] = 0;
                            }
                            break;
                        case "east":
                            if (xof1 == 9)
                            {
                                Console.WriteLine("Can't go");
                            }
                            else
                            {
                                Console.WriteLine("Moved east");
                                xof1++;
                                if (new int[] { 1, 2, 3, 4, 5, 6, 7 }.Contains(map[yof1, xof1]))
                                {
                                    j = map[yof1, xof1];
                                    Combat(tutorial, map);
                                }
                                if (new int[] { 8 }.Contains(map[yof1, xof1]))
                                {
                                    Random randomItem = new Random();
                                    Random randomLootSpot = new Random();
                                    List<string> lootSpots = new List<string> { "You come across a pile of leaves", "You find an old chest", "You find a wooden barrel", "You pass something buried in the ground" };
                                    int chosenSentence = randomLootSpot.Next(0, lootSpots.Count());
                                    Console.WriteLine(lootSpots[chosenSentence]);
                                    Console.Write(">> ");
                                    input = Console.ReadLine().ToLower();
                                    Match inspect = new Regex(@"(( )?look( at )?|( )?inspect( )?|( )?see( )?|( )?search( )?|( )?open( )?)").Match(input);
                                    int rndItem = randomItem.Next(0, 6);
                                    if (inspect.Success)
                                    {
                                        Console.WriteLine("You found a {0}!", ItemFinder.itemList[rndItem].itemName);
                                        while (true)
                                        {
                                            Console.Write(">> ");
                                            input = Console.ReadLine().ToLower();
                                            Match pickUp = new Regex(@"(( )?pick up( )?|( )?collect( )?|( )?grab( )?|( )?take( )?)").Match(input);
                                            if (pickUp.Success)
                                            {
                                                inventory.Add(ItemFinder.itemList[rndItem].itemName);
                                                inventoryIndex.Add(rndItem);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("I don't understand {0}", input);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Okay");
                                    }

                                }
                                map[yof1, xof1 - 1] = 0;
                            }
                            break;
                        case "west":
                            if (xof1 == 0)
                            {
                                Console.WriteLine("Can't go");
                            }
                            else
                            {
                                Console.WriteLine("Moved west");
                                xof1--;
                                if (new int[] { 1, 2, 3, 4, 5, 6, 7 }.Contains(map[yof1, xof1]))
                                {
                                    j = map[yof1, xof1];
                                    Combat(tutorial, map);
                                }
                                if (new int[] { 8 }.Contains(map[yof1, xof1]))
                                {
                                    Random randomItem = new Random();
                                    Random randomLootSpot = new Random();
                                    List<string> lootSpots = new List<string> { "You come across a pile of leaves", "You find an old chest", "You find a wooden barrel", "You pass something buried in the ground" };
                                    int chosenSentence = randomLootSpot.Next(0, lootSpots.Count());
                                    Console.WriteLine(lootSpots[chosenSentence]);
                                    Console.Write(">> ");
                                    input = Console.ReadLine().ToLower();
                                    Match inspect = new Regex(@"(( )?look( at )?|( )?inspect( )?|( )?see( )?|( )?search( )?|( )?open( )?)").Match(input);
                                    int rndItem = randomItem.Next(0, 6);
                                    if (inspect.Success)
                                    {
                                        Console.WriteLine("You found a {0}!", ItemFinder.itemList[rndItem].itemName);
                                        while (true)
                                        {
                                            Console.Write(">> ");
                                            input = Console.ReadLine().ToLower();
                                            Match pickUp = new Regex(@"(( )?pick up( )?|( )?collect( )?|( )?grab( )?|( )?take( )?)").Match(input);
                                            if (pickUp.Success)
                                            {
                                                inventory.Add(ItemFinder.itemList[rndItem].itemName);
                                                inventoryIndex.Add(rndItem);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("I don't understand {0}", input);
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        Console.WriteLine("Okay");
                                    }
                                }
                                map[yof1, xof1 + 1] = 0;
                            }
                            break;
                        default:
                            Console.WriteLine("Direction not recognised");
                            break;
                    }
                    map[yof1, xof1] = 1;
                    int rowLength1 = map.GetLength(0);
                    int colLength1 = map.GetLength(1);
                    for (int x = 0; x < rowLength1; x++)
                    {
                        for (int y = 0; y < colLength1; y++)
                        {
                            Console.Write(string.Format("{0}\t", map[x, y]));
                        }
                        Console.Write("\n\n");
                    }
                }
                else
                {
                    if (input == "quit")
                    {
                        Console.WriteLine("Exiting");
                    }
                    else if (input == "b")
                    {
                        Bestiary();
                    }
                    else if (input == "i")
                    {
                        Inventory(rndItem);
                    }
                    else
                    {
                        Console.WriteLine("I don't understand " + input.Split(' ')[0]);
                    }


                }
            } while (input != "quit");

            //prints grid
            int rowLength = map.GetLength(0);
            int colLength = map.GetLength(1);
            for (int x = 0; x < rowLength; x++)
            {
                for (int y = 0; y < colLength; y++)
                {
                    Console.Write(string.Format("{0}\t", map[x, y]));
                }
                Console.Write("\n\n");
            }

            Console.ReadKey();
        }
        public static void Ending()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
            string text1 = ("As your sword lies thrust through flesh and earth, you feel strange - as if some invisble power is dragging your lifeforce from you.\nYou fall to your knees, unable to do anything" +
                " but clutch your chest weakly.\n\nAll of this trial, and for what?\n");
            Thread.Sleep(2000);
            Console.Clear();
            Thread.Sleep(500);
            SlowPrint.SlowPrinting(text1);
            Thread.Sleep(3000);
            string text2 = ("You collapse, exausted, and dying.\nThere's no way out now, no way back to the light. Death surrounds you.\nAll you know is this: your life served to birth a new age of man\n" +
                "One that did not have to face the horrors that you were forced to live through.\n");
            Thread.Sleep(2000);
            Console.Clear();
            Thread.Sleep(500);
            SlowPrint.SlowPrinting(text2);
            Thread.Sleep(2000);
            Console.Clear();
            Thread.Sleep(500);
            string text3 = ("Rest, hero.\nYou completed your journey, completed what you set out to do\n");
            SlowPrint.SlowPrinting(text3);
            Thread.Sleep(2000);
            string text4 = ("\n\n\nSleep now...");
            SlowPrint.SlowPrinting(text4);
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + ("Thanks for playing".Length / 2)) + "}", "Thanks for playing"));
            Console.ReadKey();
            Environment.Exit(0);
        }
        //sets everything up
        public static void Main2(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "The Ember Isles";
            Console.WindowHeight = 40;
            Console.WindowWidth = 165;
            Console.SetBufferSize(165, 40);
            Console.CursorVisible = false;
            Ending();
            CharacterCreation();
        }
    }
}
