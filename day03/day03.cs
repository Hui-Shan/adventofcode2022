class Day3
{
    public static int getPriority(char c)
    {
        /*
        Returns the priority associated with a char (a -> 1, b -> 2, c -> 3, ..., X -> 50, Y -> 51, Z -> 52)
        */
        
        string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";        
        return alphabet.IndexOf(c) + 1;
    }

    public static void Main()
    {
        string infile = "input.txt";                
        string[] real = System.IO.File.ReadAllLines(infile);
        
        string[] test = new string[6]
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg",
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw"
        };

        var input = real; 

        // Part 1: Sum priorities of common element in the two rucksacks of an Elf
        int nElves = input.Length;
        int[] priorities = new int[nElves];

        for (int nn = 0; nn < nElves; nn++)
        {
            string line = input[nn];
            int rucksackSize = line.Length / 2;            
            for (int ii = 0; ii < rucksackSize; ii++)
            {
                char item = line[ii];                                
                for (int jj = 0; jj < rucksackSize; jj++)
                {
                    char otherItem = line[rucksackSize + jj];
                    if (item == otherItem)
                    {                                                             
                        priorities[nn] = getPriority(item);
                    }
                }                                
            }
        }

        int res1 = priorities.Sum();    
        Console.WriteLine("Part 1: " + res1);

        // Part 3: Sum badges of common element in Elf groups of three
        int nGroups = input.Length / 3;
        int totalBadgePriority = 0;
        for (int kk = 0; kk < nGroups; kk++)
        {            
            List<char> firstRucksack = input[3 * kk].ToList();
            List<char> secondRucksack = input[3 * kk + 1].ToList();
            List<char> thirdRucksack = input[3 * kk + 2].ToList();

            List<char> badge = firstRucksack.Intersect(secondRucksack).Intersect(thirdRucksack).ToList();
            int badgePriority = getPriority(badge.First());
            totalBadgePriority += badgePriority;            
        }

        int res2 = totalBadgePriority;    
        Console.WriteLine("Part 2: " + res2);
    }
}