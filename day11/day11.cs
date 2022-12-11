class Day11
{
    class Monkey
    {
        public int Idx;
        public List <long> Items;
        public int Divider;        
        public int TrueMonkeyIdx;
        public int FalseMonkeyIdx;
        public int numberOfInspections;

        Func<long, long> Operation;
        
        public Monkey(int idx, List<long> items, int divider, Func<long, long> operation, int trueMonkeyIdx, int falseMonkeyIdx)
        {
            Idx = idx;
            Items = items;
            Divider = divider;
            TrueMonkeyIdx = trueMonkeyIdx;
            FalseMonkeyIdx = falseMonkeyIdx;
            Operation = operation;
            numberOfInspections = 0;
        }

        public long inspectItem(long itemIn, bool reduceWorryLevel = true)
        {            
            long itemOut = Operation(itemIn);                
            if (reduceWorryLevel)
            {
                itemOut = itemOut / 3;
            }
            numberOfInspections += 1;
            return itemOut;
        }    

        public override string ToString()
        {
            return "Monkey " + Idx + " Div: " + Divider;
        }
    }

    class Barrel
    {
        private List<Monkey> monkeys = new List<Monkey>();
        private long dividerProduct = 1; 

        public Barrel(string input)
        {
            // Initialize list of Monkeys with initial items, operations etc.
            string[] inputPerMonkey = input.Split("\n\n");
            foreach(string item in inputPerMonkey)
            {
                string[] lines = item.Split("\n");
                int idx = getMonkeyId(lines[0]);
                List<long> items = getItems(lines[1]);
                Func<long, long> operation = v => v * v;
                
                if (lines[2].Contains("old * ") && !lines[2].Contains("old * old"))
                {                
                    long factor = long.Parse(lines[2].Split("old * ")[1]);
                    operation = v => v * factor;                    
                } else if (lines[2].Contains(" + "))
                {
                    long delta = long.Parse(lines[2].Split("old + ")[1]);
                    operation = v => v + delta;
                }
                 
                int divisor = getDivisor(lines[3]);
                int trueMonkeyIdx = getThrowingMonkeyIdx(lines[4]); 
                int falseMonkeyIdx = getThrowingMonkeyIdx(lines[5]);
                
                Monkey currentMonkey = new Monkey(idx, items, divisor, operation, trueMonkeyIdx, falseMonkeyIdx);                
                monkeys.Add(currentMonkey);                

                dividerProduct *= divisor;
            }            
        }

        public void doRounds(int numberOfRounds, bool reduceWorryLevel = true)
        {
            // Do one round of each Monkey inspecting their items and throwing them to the next Monkey
            // Order: monkey 0 ... monkey N
            for (int nn = 0; nn < numberOfRounds; nn++)
            {
                foreach(Monkey monkey in monkeys)
                {
                    foreach(long item in monkey.Items)
                    {
                        long itemOut = monkey.inspectItem(item, reduceWorryLevel);
                        if (!reduceWorryLevel)
                        {
                            itemOut = itemOut % dividerProduct;
                        }                        
                        Monkey newMonkey = getMonkey(monkey.FalseMonkeyIdx);
                        if ((itemOut % monkey.Divider) == 0)
                        {
                            newMonkey = getMonkey(monkey.TrueMonkeyIdx);
                        }                                                
                        newMonkey.Items.Add(itemOut);
                    }
                    monkey.Items = new List<long>(); // Clear own Monkey list (all items are thrown away)
                }
            }            
        }

        public Monkey getMonkey(int monkeyId)
        {
            // return the monkey with the given MonkeyId
            return monkeys.Where(monkey => monkey.Idx == monkeyId).First();
        }
        

        public int getMonkeyId(string line)
        {
            // Get the monkeyIdx from the string input
            return int.Parse(line.Split("Monkey ")[1].Split(":")[0]);
        }

        public List<long> getItems(string line)
        {
            // Get the items from the string input
            List<long> intItems = new List<long>();
            string[] stringItems = line.Split("Starting items: ")[1].Split(", ");            
            foreach (string str in stringItems)
            {
                intItems.Add(long.Parse(str));
            }

            return intItems;
        }

        public int getDivisor(string line)
        {
            // Get the integer to use for the divisible check from the string input
            return int.Parse(line.Split("divisible by ")[1]);
        }

        public int getThrowingMonkeyIdx(string line)
        {
            // Get the MonkeyIdx of the monkey to throw to
            return int.Parse(line.Split("throw to monkey ")[1]);
        }

        public long getMonkeyBusiness(int n = 2)
        {
            // Return the product of the two (n) monkeys with the highest number of inspections

            List<int> inspections = new List<int>();
            foreach(Monkey monkey in monkeys)
            {
                inspections.Add(monkey.numberOfInspections);
            }
            inspections.Sort();
            inspections.Reverse();

            long res = 1; 
            for (int ii = 0; ii < n; ii++)
            {
                // Console.WriteLine(inspections[ii]);
                res *= inspections[ii];
            }                
            return res;
        }
    }

    public static void Main()
    {
        string real = System.IO.File.ReadAllText("input.txt");
        string test = System.IO.File.ReadAllText("testinput.txt");

        var input = real; // test;

        Barrel myBarrel = new Barrel(input);
        myBarrel.doRounds(20, true);

        long res1 = myBarrel.getMonkeyBusiness();
        Console.WriteLine("Part 1: " + res1);

        Barrel myBarrel2 = new Barrel(input);
        myBarrel2.doRounds(10000, false);

        long res2 = myBarrel2.getMonkeyBusiness();
        Console.WriteLine("Part 2: " + res2);
        
    }
}