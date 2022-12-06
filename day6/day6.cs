class Day6
{

    public static int getMarkerEnd(string datastream, int packetSize)
    {
        // Returns index of last character of marker
                
        int endIndex = -1;
        
        for (int nn = packetSize; nn < datastream.Length; nn++)
        {                
            char[] sequence = datastream[(nn-packetSize)..nn].ToCharArray();
            int n_unique = sequence.Distinct<char>().Count();  
            
            if (n_unique == packetSize)
            {                
                endIndex = nn;                 
                break;
            }
            
        }
        
        return endIndex;
    }

    public static int getStartOfPacketEnd(string datastream, int packetSize=4)
    {
        return getMarkerEnd(datastream, packetSize);
    }

    public static int getStartOfMessageEnd(string datastream, int packetSize=14)
    {
        return getMarkerEnd(datastream, packetSize);
    }

    public static void Main()
    {
        string infile = "input.txt";                
        string[] real = System.IO.File.ReadAllLines(infile);

        string[] test = new string[5]
        {
            "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
            "bvwbjplbgvbhsrlpgdmjqwftvncz",  // first marker after character 5
            "nppdvjthqldpwncqszvftbrmjlhg",  // first marker after character 6
            "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",  // first marker after character 10
            "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" // first marker after character 11
        }; 

        string[] input = real;

        int res1 = -1;
        int res2 = -1; 
        foreach (string line in input)
        {
            res1 = getStartOfPacketEnd(line);                        
            res2 = getStartOfMessageEnd(line);            
        }
        
        Console.WriteLine("Part 1: " + res1);        
        Console.WriteLine("Part 2: " + res2);
    }
}