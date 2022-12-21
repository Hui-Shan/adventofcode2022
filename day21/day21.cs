class Day21
{
    class Monkey
    {
        public string Name { get; private set; }
        public string Job { get; private set; }
        public long? Value { get; set; }

        public Monkey(string line)
        {
            string[] parts = line.Split(": ");
            Name = parts[0];
            Job = parts[1];

            if (Job.All(char.IsDigit))
            {
                Value = long.Parse(Job);
            }
        }

        public override string ToString()
        {
            return $"Monkey {Name} has Job {Job}, value: {Value}";
        }
    }

    class Puzzle
    {
        List<Monkey> monkeys = new List<Monkey>();

        public Puzzle(string[] input)
        {
            foreach (string line in input)
            {
                Monkey newMonkey = new Monkey(line);
                monkeys.Add(newMonkey);
            }
        }
        
        public List<Monkey> getMonkeys()
        {
            return monkeys;
        }

        public Monkey getMonkey(string name)
        {
            Monkey rightMonkey = getMonkeys().Where(monkey => monkey.Name == name).First();
            
            return rightMonkey;
        }

        public long? getMonkeyValue(string name)
        {
            return getMonkey(name).Value;
        }

        public string getPartValues(Monkey monkey)
        {
            string res = monkey.Value.ToString();

            if (!monkey.Job.All(char.IsDigit))
            {
                string first = monkey.Job.Split(" ")[0];
                string second = monkey.Job.Split(" ")[2];

                res += ": " + getMonkeyValue(first).ToString() + ", " + getMonkeyValue(second).ToString();
            }

            return res;

        }


        public long? computeMonkeyValue(Monkey monkey)
        {
            long? result = null;             
            
            if (!monkey.Job.All(char.IsDigit))
            {
                // determine operator and two parts
                string[] jobParts = monkey.Job.Split(" ");
                string name1 = jobParts[0];
                string name2 = jobParts[2];
                string op = jobParts[1];

                long? value1 = getMonkeyValue(name1);
                long? value2 = getMonkeyValue(name2);
                
                if (value1 is not null && value2 is not null)
                {
                    switch (op)
                    {
                        case "+":
                            result = value1 + value2; 
                            break;
                        case "-":
                            result = value1 - value2; 
                            break;
                        case "*":
                            result = value1 * value2; 
                            break;
                        case "/":
                            result = value1 / value2; 
                            break;
                    }
                }
            }

            return result; 
        }

        public long getMissingValue(Monkey monkeyIn, string missingName)
        {
            string[] parts = monkeyIn.Job.Split(" ");
            long result = monkeyIn.Value ?? throw new InvalidOperationException("Result should be set");

            string name1 = parts[0];
            string op = parts[1];
            string name2 = parts[2]; 
            string knownName = name1;

            if (missingName == name1)
            {
                knownName = name2;
            }

            long missingValue = 0;
            long knownValue = getMonkeyValue(knownName) ?? throw new InvalidOperationException($"Value for known {knownName} should be set");            

            switch(op)
            {
                case "+":
                    missingValue = result - (long) getMonkeyValue(knownName);
                    break;
                case "-":
                    if (missingName == name1)
                    {
                        missingValue = result + knownValue;
                    }
                    else 
                    {
                        missingValue = knownValue - result;
                    }
                    break;
                case "*":
                    missingValue = result / (long) getMonkeyValue(knownName);
                    break;
                case "/":
                    if (missingName == name1)
                    {
                        missingValue = result * knownValue;
                    }
                    else 
                    {
                        missingValue = result / knownValue;
                    }
                    break;
            }
            
            return missingValue;
        }        

        public long solvePart1()
        {
            List<Monkey> unValuedMonkeys = getMonkeys().Where(monkey => monkey.Value is null).ToList();
            while (unValuedMonkeys.Count > 0)
            {
                foreach (Monkey monkey in unValuedMonkeys)
                {
                    long? res = computeMonkeyValue(monkey);
                    if (res is not null)
                    {
                        monkey.Value = res;
                    }
                }
                unValuedMonkeys = getMonkeys().Where(monkey => monkey.Value is null).ToList();
            }

            return (long) getMonkeyValue("root");
        }

        public List<Monkey> monkeysToProcess()
        {
            List<Monkey> res = new List<Monkey>();
            foreach(Monkey monkey in getMonkeys().Where(monkey => !monkey.Job.All(char.IsDigit)).ToList())
            {
                
                string name1 = monkey.Job.Split(" ")[0];
                string name2 = monkey.Job.Split(" ")[2];

                long? val1 = getMonkeyValue(name1);
                long? val2 = getMonkeyValue(name2);
                
                if ((val1 is null ^ val2 is null)  && monkey.Value is not null)
                {
                    res.Add(monkey);
                }
            }
            return res;
        }

        public long solvePart2()
        {            
            // Reset Monkeys with operation to null value
            List<Monkey> operatingMonkeys = getMonkeys().Where(monkey => !monkey.Job.All(char.IsDigit) || (monkey.Name == "humn")).ToList();
            foreach (Monkey operatingMonkey in operatingMonkeys)
            {
                operatingMonkey.Value = null;
            }            
            
            List<Monkey> unValuedMonkeys = getMonkeys().Where(monkey => monkey.Value is null).ToList();
            int oldCount = unValuedMonkeys.Count;            
            int newCount = oldCount;
            
            do 
            {
                foreach (Monkey monkey in unValuedMonkeys)
                {                    
                    long? res = computeMonkeyValue(monkey);
                    if (res is not null)
                    {
                        monkey.Value = res;
                    }                  
                }
                oldCount = newCount;
                unValuedMonkeys = getMonkeys().Where(monkey => monkey.Value is null).ToList();
                newCount = unValuedMonkeys.Count;
            }
            while (newCount < oldCount);

            Monkey rootMonkey = getMonkeys().Where(monkeys => monkeys.Name == "root").ToList().First();
            string rootJob = rootMonkey.Job;            

            // Set value of root part containing humn's value
            string rootPart1 = rootJob.Split(" ")[0];
            string rootPart2 = rootJob.Split(" ")[2];
    
            if (getMonkeyValue(rootPart1) is null)
            {                
                getMonkey(rootPart1).Value = getMonkeyValue(rootPart2);
            }
            else {
                getMonkey(rootPart2).Value = getMonkeyValue(rootPart1);
            }

            do
            {
                foreach(Monkey valopMonkey in monkeysToProcess())
                {                                    
                    string name1 = valopMonkey.Job.Split(" ")[0];
                    string name2 = valopMonkey.Job.Split(" ")[2];

                    long? val1 = getMonkeyValue(name1);
                    long? val2 = getMonkeyValue(name2);

                    if ((val1 is null) && (val2 is not null))
                    {                        
                        getMonkey(name1).Value = getMissingValue(valopMonkey, name1);                                                
                    }
                    else if ((val1 is not null) && (val2 is null))
                    {
                        getMonkey(name2).Value = getMissingValue(valopMonkey, name2);                                                
                    }
                } 
            }
            while(getMonkeyValue("humn") is null);             

            long? humnValue = getMonkeyValue("humn");
            if (humnValue is not null)
            {
                return (long) humnValue;
            }
            else
            {
                return 0;
            }
        }
    }

    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");

        string[] input = real; //test;

        Puzzle myPuzzle = new Puzzle(input);
        Console.WriteLine(myPuzzle.getMonkeys().Count + " monkeys are yelling");

        long res1 = (long) myPuzzle.solvePart1();
        Console.WriteLine("Res 1: " + res1);

        long res2 = myPuzzle.solvePart2();
        Console.WriteLine("Res 2: " + res2);

    }
}