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
            // ranks[i] == 0 means unset
            var ranks = new int[items.Count];
            var usedRanks = new Dictionary<int, int>(); // rank -> index

            void PrintAssignments()
            {
                Console.Clear();
                Console.WriteLine("Current assignments (blank = unset):\n");
                for (int i = 0; i < items.Count; i++)
                {
                    string assigned = ranks[i] == 0 ? "-" : ranks[i].ToString();
                    Console.WriteLine("{0,2}. {1,-45} => {2}", i + 1, items[i].Name, assigned);
                }
                Console.WriteLine();
                Console.WriteLine("Commands: <item#> to set/change a rank for that item");
                Console.WriteLine("          done   - finish (requires all items ranked)");
                Console.WriteLine("          clear  - clear a rank by item number (e.g. \"clear 3\")");
                Console.WriteLine("When assigning a rank already in use you will be offered to swap/override.\n");
            }

            while (true)
            {
                PrintAssignments();

                // If all assigned, prompt to finish or edit
                if (ranks.All(r => r > 0))
                {
                    Console.Write("All items ranked. Type 'done' to finish or enter an item number to change: ");
                }
                else
                {
                    Console.Write("Enter item number to set/change (1-{0}), or 'done' to finish later: ", items.Count);
                }

                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input))
                    continue;

                if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    if (ranks.All(r => r > 0))
                        break;
                    Console.WriteLine("Not all items have been ranked yet. Continue assigning.\nPress any key...");
                    Console.ReadKey();
                    continue;
                }

                // clear command: "clear 3"
                if (input.StartsWith("clear ", StringComparison.OrdinalIgnoreCase))
                {
                    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && int.TryParse(parts[1], out int clearIndex) && clearIndex >= 1 && clearIndex <= items.Count)
                    {
                        int idx = clearIndex - 1;
                        if (ranks[idx] > 0)
                        {
                            usedRanks.Remove(ranks[idx]);
                            ranks[idx] = 0;
                            Console.WriteLine("Cleared rank for item {0}.", clearIndex);
                        }
                        else
                        {
                            Console.WriteLine("Item {0} was already unset.", clearIndex);
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    }
                }

                if (!int.TryParse(input, out int itemNumber) || itemNumber < 1 || itemNumber > items.Count)
                {
                    Console.WriteLine("Invalid command or item number. Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                int index = itemNumber - 1;
                Console.Write("Enter rank for '{0}' (1-{1}) or leave blank to cancel: ", items[index].Name, items.Count);
                var rankInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(rankInput))
                {
                    continue;
                }

                if (!int.TryParse(rankInput, out int rank) || rank < 1 || rank > items.Count)
                {
                    Console.WriteLine("Invalid rank. Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (usedRanks.TryGetValue(rank, out int occupyingIndex) && occupyingIndex != index)
                {
                    Console.WriteLine("Rank {0} is currently assigned to item {1}: '{2}'", rank, occupyingIndex + 1, items[occupyingIndex].Name);
                    Console.Write("Type 'swap' to swap ranks, 'override' to take rank and unset previous, anything else to cancel: ");
                    var choice = Console.ReadLine()?.Trim();
                    if (choice != null && choice.Equals("swap", StringComparison.OrdinalIgnoreCase))
                    {
                        // swap
                        int previousRank = ranks[index];
                        ranks[occupyingIndex] = previousRank;
                        if (previousRank > 0)
                        {
                            usedRanks[previousRank] = occupyingIndex;
                        }
                        else
                        {
                            usedRanks.Remove(previousRank);
                        }

                        ranks[index] = rank;
                        usedRanks[rank] = index;
                        Console.WriteLine("Swapped ranks between item {0} and item {1}.", index + 1, occupyingIndex + 1);
                    }
                    else if (choice != null && choice.Equals("override", StringComparison.OrdinalIgnoreCase))
                    {
                        // override: unset previous
                        ranks[occupyingIndex] = 0;
                        usedRanks.Remove(rank);

                        // assign new
                        if (ranks[index] > 0)
                            usedRanks.Remove(ranks[index]);

                        ranks[index] = rank;
                        usedRanks[rank] = index;
                        Console.WriteLine("Rank {0} moved to item {1}; previous item {2} is now unset.", rank, index + 1, occupyingIndex + 1);
                    }
                    else
                    {
                        Console.WriteLine("Assignment cancelled.");
                    }

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                // rank not in use or assigning same rank to same item
                if (ranks[index] > 0)
                {
                    // freeing previous rank
                    usedRanks.Remove(ranks[index]);
                }

                ranks[index] = rank;
                usedRanks[rank] = index;
            }

            // build dictionary result
            var result = new Dictionary<int, int>(items.Count);
            for (int i = 0; i < items.Count; i++)
                result[i] = ranks[i];

            return result;
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
