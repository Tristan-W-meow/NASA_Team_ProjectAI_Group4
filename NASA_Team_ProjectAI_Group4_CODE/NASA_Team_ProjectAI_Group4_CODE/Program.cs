namespace NASA_Team_ProjectAI_Group4_CODE
{
    internal class Program
    {
        private sealed record Item(string Name, int NasaRank, string NasaReason);

        static void Main(string[] args)
        {
            var items = new List<Item>
                {
                    new Item("Box of matches", 15, "Virtually worthless – no oxygen to sustain combustion."),
                    new Item("Food concentrate", 4, "Efficient means of supplying energy requirements."),
                    new Item("50 feet of nylon rope", 6, "Useful in scaling cliffs and tying injured together."),
                    new Item("Parachute silk", 8, "Protection from the sun’s rays."),
                    new Item("Portable heating unit", 13, "Not needed unless on the dark side."),
                    new Item("Two .45 caliber pistols", 11, "Possible means of self-propulsion."),
                    new Item("One case of dehydrated milk", 12, "Bulky duplication of food concentrate."),
                    new Item("Two 100 lb. tanks of oxygen", 1, "Most pressing survival need – oxygen supply."),
                    new Item("Stellar map", 3, "Primary means of navigation – star patterns visible."),
                    new Item("Self-inflating life raft", 9, "CO2 bottle can be used for propulsion."),
                    new Item("Magnetic compass", 14, "Worthless – the Moon’s magnetic field isn’t polarized."),
                    new Item("20 liters of water", 2, "Needed to replace fluid loss on light side."),
                    new Item("Signal flares", 10, "For distress signaling when mother ship is sighted."),
                    new Item("First aid kit with injection needle", 7, "For treating injuries and medical needs."),
                    new Item("Solar-powered FM receiver-transmitter", 5, "For short-range communication with mother ship.")
                };

            Console.WriteLine("Hello — welcome to the NASA moon survival test.");
            Console.WriteLine("Please assign a unique rank 1-{0} to each item (1 = most important).\n", items.Count);

            var userRanks = GetUserRanks(items);

            Console.Clear();
            PrintComparison(items, userRanks);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static Dictionary<int, int> GetUserRanks(IReadOnlyList<Item> items)
        {
            var ranks = new Dictionary<int, int>(items.Count);
            var usedRanks = new HashSet<int>();

            for (int i = 0; i < items.Count; i++)
            {
                while (true)
                {
                    Console.WriteLine("{0}. {1}", i + 1, items[i].Name);
                    Console.Write("Enter rank for this item (1-{0}): ", items.Count);
                    var input = Console.ReadLine();

                    if (!int.TryParse(input, out int rank))
                    {
                        Console.WriteLine("Invalid number. Try again.\n");
                        continue;
                    }

                    if (rank < 1 || rank > items.Count)
                    {
                        Console.WriteLine("Rank out of range. Try again.\n");
                        continue;
                    }

                    if (usedRanks.Contains(rank))
                    {
                        Console.WriteLine("Rank already used. Choose a different rank.\n");
                        continue;
                    }

                    usedRanks.Add(rank);
                    ranks[i] = rank;
                    Console.WriteLine("Recorded: {0} => {1}\n", items[i].Name, rank);
                    break;
                }
            }

            return ranks;
        }

        private static void PrintComparison(IReadOnlyList<Item> items, IReadOnlyDictionary<int, int> userRanks)
        {
            Console.WriteLine("Your rankings vs NASA's rankings:\n");
            Console.WriteLine("{0,-3} {1,-45} {2,6} {3,11} {4,12}", "#", "Item", "You", "NASA", "Difference");
            Console.WriteLine(new string('-', 80));

            int totalDifference = 0;

            for (int i = 0; i < items.Count; i++)
            {
                int your = userRanks[i];
                int nasa = items[i].NasaRank;
                int diff = Math.Abs(your - nasa);
                totalDifference += diff;

                Console.WriteLine("{0,-3} {1,-45} {2,6} {3,11} {4,12}",
                    i + 1,
                    items[i].Name,
                    your,
                    nasa,
                    diff);
            }

            Console.WriteLine("\nTotal difference score: {0}", totalDifference);
            Console.WriteLine("Lower score means closer to NASA's ranking.\n");

            Console.WriteLine("NASA reasoning (item -> reason):\n");
            foreach (var item in items)
            {
                Console.WriteLine("- {0}: {1}", item.Name, item.NasaReason);
            }
        }
    }
}
