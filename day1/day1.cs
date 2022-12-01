class ReadFromFile
{
    static void Main()
    {
        string infile = "input.txt";        
       
        // // Read each line of the file into a string array. Each element
        // // of the array is one line of the file.
        string[] lines = System.IO.File.ReadAllLines(infile);

        List<int> calories = new List<int>();
        int sum = 0;
        foreach (string line in lines)
        {
            if (line == "")
            {
                calories.Add(sum);
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }

        int res1 = calories.Max();
        Console.WriteLine("Part 1: " + res1);

        calories.Sort();
        calories.Reverse();
        
        int res2 = 0;
        for (int ii = 0; ii < 3; ii++)
        {
            res2 += calories[ii];
        }
        
        Console.WriteLine("Part 2: " + res2);

        // Keep the console window open in debug mode.
        Console.WriteLine("Press any key to exit.");
        System.Console.ReadKey();
    }
}