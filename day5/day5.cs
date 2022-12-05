class Day5
{
    class CrateStacks
    {        
        private string[] operations { get; }
        private List<Stack<char>> stacks = new List<Stack<char>>();

        public static int getSeparatorIndex(string[] input)
        {
            // Returns index of empty line separating crate arrangement and instructions
            
            int ii = 0;
            foreach (string line in input)
            {                
                if (line == "")    
                {                    
                    break;
                }
                ii = ii + 1;
            }
            
            return ii;
        }

        public CrateStacks(string[] input)
        {
            // Creates CrateStack object from <input>

            int separator = getSeparatorIndex(input);
            string[] state = input[0..(separator - 1)];
            int nStacks = (state[0].Length + 1)/4;
            
            for (int n = 0; n < nStacks; n++)
            {
                stacks.Add(new Stack<char>());
            }            
            
             // build stacks from the bottom up
            foreach (string line in state.Reverse())
            {                
                for (int ii = 0; ii < nStacks; ii++)
                {                    
                    char item = line[4*ii + 1];
                    if (item != ' ')
                    {
                        stacks[ii].Push(item);
                    }                    
                }
            }

            operations = input[(separator + 1)..input.Length];                
        }

        public void doOperations9000()
        {
            // Perform operations, moving crates one by one

            foreach(string op in operations)
            {
                string[] parts = op.Split(' ');
                int nToPop = int.Parse(parts[1]);
                int originalStack = int.Parse(parts[3]);
                int newStack = int.Parse(parts[5]);
                
                for (int kk = 0; kk < nToPop; kk++)
                {
                    var itemToMove = stacks[originalStack - 1].Pop();                    
                    stacks[newStack - 1].Push(itemToMove);
                }
            }
        }

        
        public void doOperations9001()
        {
            // Perform operations, moving crates keeping original order

            foreach(string op in operations)
            {
                string[] parts = op.Split(' ');
                int nToPop = int.Parse(parts[1]);
                int originalStack = int.Parse(parts[3]);
                int newStack = int.Parse(parts[5]);
                
                List<char> items = new List<char>();
                for (int kk = 0; kk < nToPop; kk++)
                {
                    char itemToMove = stacks[originalStack - 1].Pop();
                    items.Add(itemToMove);
                }
                items.Reverse();  // to keep original order
                foreach (char item in items)
                {
                    stacks[newStack - 1].Push(item);
                }                
            }
        }

        public string getTopElements()
        {
            string res = "";
            foreach(Stack<char> stack in stacks)
            {
                res += stack.Pop();
            }

            return res;
        }

        public override string ToString()
        {            
            // Print visual representation of stacks

            int number = 1;
            string res = number.ToString() + ": ";
            foreach (Stack<char> stack in stacks)
            {
                foreach (char c in stack.Reverse())
                {
                    res += c;
                }
                number++;
                if (number < (stacks.Count + 1))
                {
                    res = res + "\n" + number.ToString() + ": ";
                }                
            }
            return res;
        }
    }
    
    public static void Main()
    {
        string infile = "input.txt";                
        string[] real = System.IO.File.ReadAllLines(infile);

        string[] test = new string[9]
        {
            "    [D]    ",
            "[N] [C]    ",
            "[Z] [M] [P]",
            " 1   2   3 ",
            "",
            "move 1 from 2 to 1",
            "move 3 from 1 to 3",
            "move 2 from 2 to 1",
            "move 1 from 1 to 2"
        }; 

        string[] input = real;
        
        CrateStacks pile = new CrateStacks(input);
        pile.doOperations9000();        
        string res1 = pile.getTopElements();
        Console.WriteLine("Part 1: " + res1);

        CrateStacks pile2 = new CrateStacks(input);
        pile2.doOperations9001();        
        string res2 = pile2.getTopElements();
        Console.WriteLine("Part 2: " + res2);
    }
}